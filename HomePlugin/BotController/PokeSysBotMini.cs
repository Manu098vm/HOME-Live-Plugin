﻿using System;
using HOME;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PKHeX.Core.Injection
{
    public class PokeSysBotMini
    {
        public const uint BoxStart = 0x36C000;
        public const uint SlotSize = 0x248;
        public const int SlotCount = 30;
        public const int BoxCount = 200;
        public readonly int page;
        public readonly ICommunicator sys;

        public PokeSysBotMini(ConnectionType con, int lv)
        {
            page = lv;
            sys = con switch {
                ConnectionType.WiFi => new SysBotMini(),
                ConnectionType.USB => new UsbBotMini(),
                _ => new SysBotMini(),
            };
        }

        public uint GetB1S1Offset() => BoxStart;
        public uint GetSlotSize() => SlotSize;
        public uint GetBoxOffset(int box) => BoxStart + (uint)(SlotSize * SlotCount * box);
        public uint GetSlotOffset(int box, int slot) => GetBoxOffset(box) + (uint)(SlotSize * slot);
        public byte[] ReadBox(int box, int len=(int)SlotSize*SlotCount) => sys.ReadBytes(GetBoxOffset(box), len);
        public byte[]? ReadSlot(int box, int slot) 
        {
            var offset = GetSlotOffset(box, slot);
            return ReadBytesPKH(offset);
        }
        public byte[]? ReadBytesPKH(uint offset)
        {
            var data = sys.ReadBytes(offset, 0x10);

            if (ReadUInt64LittleEndian(data.AsSpan(0x02)) == 0)
                return null;

            var EncodedDataSize = ReadUInt16LittleEndian(data.AsSpan(0x0E));
            return sys.ReadBytes(offset, 0x10 + EncodedDataSize);
        }
    }
}