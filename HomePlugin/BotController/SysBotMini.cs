﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace PKHeX.Core.Injection
{
    public enum RWMethod
    {
        Heap,
        Main,
        Absolute,
    }

    public class SysBotMini : ICommunicator
    {
        public string IP = "0.0.0.0";
        public int Port = 6000;

        public Socket Connection = new Socket(SocketType.Stream, ProtocolType.Tcp);

        public bool Connected;

        private readonly object _sync = new object();

        bool ICommunicator.Connected { get => Connected; set => Connected = value; }
        int ICommunicator.Port { get => Port; set => Port = value; }
        string ICommunicator.IP { get => IP; set => IP = value; }

        public void Connect()
        {
            lock (_sync)
            {
                Connection = new Socket(SocketType.Stream, ProtocolType.Tcp);
                Connection.Connect(IP, Port);
                Connected = true;
            }
        }

        public void Disconnect()
        {
            lock (_sync)
            {
                Connection.Disconnect(false);
                Connected = false;
            }
        }

        private int ReadInternal(byte[] buffer)
        {
            int br = Connection.Receive(buffer, 0, 1, SocketFlags.None);
            while (buffer[br - 1] != (byte)'\n')
                br += Connection.Receive(buffer, br, 1, SocketFlags.None);
            return br;
        }

        private int SendInternal(byte[] buffer) => Connection.Send(buffer);

        public int Read(byte[] buffer)
        {
            lock (_sync)
                return ReadInternal(buffer);
        }

        public byte[] ReadBytes(ulong offset, int length, RWMethod method)
        {
            lock (_sync)
            {
                var cmd = method switch
                {
                    RWMethod.Heap => SwitchCommand.Peek(offset, length),
                    RWMethod.Main => SwitchCommand.PeekMain(offset, length),
                    RWMethod.Absolute => SwitchCommand.PeekAbsolute(offset, length),
                    _ => SwitchCommand.Peek(offset, length),
                };

                SendInternal(cmd);

                // give it time to push data back
                Thread.Sleep((length / 256) + 100);
                var buffer = new byte[(length * 2) + 1];
                var _ = ReadInternal(buffer);
                return Decoder.ConvertHexByteStringToBytes(buffer);
            }
        }

        public byte[] ReadAbsoluteMulti(Dictionary<ulong, int> offsets)
        {
            lock (_sync)
            {
                var cmd = SwitchCommand.PeekAbsoluteMulti(offsets);
                SendInternal(cmd);

                // give it time to push data back
                var length = offsets.Values.ToArray().Sum();
                Thread.Sleep((length / 256) + 100);
                var buffer = new byte[(length * 2) + 1];
                var _ = ReadInternal(buffer);
                return Decoder.ConvertHexByteStringToBytes(buffer);
            }
        }

        public void WriteBytes(byte[] data, ulong offset, RWMethod method)
        {
            lock (_sync)
            {
                var cmd = method switch
                {
                    RWMethod.Heap => SwitchCommand.Poke(offset, data),
                    RWMethod.Main => SwitchCommand.PokeMain(offset, data),
                    RWMethod.Absolute => SwitchCommand.PokeAbsolute(offset, data),
                    _ => SwitchCommand.Poke(offset, data),
                };

                SendInternal(cmd);

                // give it time to push data back
                Thread.Sleep((data.Length / 256) + 100);
            }
        }



        public void WriteLargeBytes(byte[] data, ulong offset, RWMethod method)
        {
            const int maxlength = 344 * 30;
            if (data.Length <= maxlength)
            {
                WriteBytes(data, offset, method);
                return;
            }
            int i = 0;
            var split = data.GroupBy(_ => i++ / maxlength).Select(g => g.ToArray()).ToArray();
            foreach (var ba in split)
            {
                WriteBytes(ba, offset, method);
                offset += maxlength;
            }
        }

        public ulong GetHeapBase()
        {
            var cmd = SwitchCommand.GetHeapBase();
            SendInternal(cmd);
            var buffer = new byte[(8 * 2) + 1];
            var _ = ReadInternal(buffer);
            return Convert.ToUInt64(string.Concat(buffer.Select(z => (char)z)).Trim(), 16);
        }

        public byte[] ReadBytes(uint offset, int length) => ReadBytes(offset, length, RWMethod.Heap);
        public void WriteBytes(byte[] data, ulong offset) => WriteLargeBytes(data, offset, RWMethod.Heap);
        public byte[] ReadBytesMain(ulong offset, int length) => ReadBytes(offset, length, RWMethod.Main);
        public void WriteBytesMain(byte[] data, ulong offset) => WriteLargeBytes(data, offset, RWMethod.Main);
        public void WriteBytesAbsolute(byte[] data, ulong offset) => WriteLargeBytes(data, offset, RWMethod.Absolute);
        public byte[] ReadBytesAbsolute(ulong offset, int length) => ReadBytes(offset, length, RWMethod.Absolute);
        public byte[] ReadBytes(ulong offset, int length) => ReadBytes(offset, length, RWMethod.Heap);
        public byte[] ReadBytesAbsoluteMulti(Dictionary<ulong, int> offsets) => ReadAbsoluteMulti(offsets);
    }
}