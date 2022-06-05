//Taken from Live Hex

using System;
using System.Threading;
using LibUsbDotNet;
using LibUsbDotNet.Main;

namespace HOME
{
    public class UsbBotMini : ICommunicator
    {
        public string IP = string.Empty;
        public int Port;

        private UsbDevice? SwDevice;
        private UsbEndpointReader? reader;
        private UsbEndpointWriter? writer;

        public bool Connected;

        private readonly object _sync = new object();

        bool ICommunicator.Connected { get => Connected; set => Connected = value; }
        int ICommunicator.Port { get => Port; set => Port = value; }
        string ICommunicator.IP { get => IP; set => IP = value; }

        /// <summary>
        /// Soft connect USB reader and writer, no persistent connection will be active due to limitations of USB-Botbase.
        /// </summary>
        public void Connect()
        {
            lock (_sync)
            {
                // Find and open the usb device.
                foreach (UsbRegistry ur in UsbDevice.AllDevices)
                {
                    ur.DeviceProperties.TryGetValue("Address", out object port);
                    if (ur.Vid == 1406 && ur.Pid == 12288 && Port == (int)port)
                        SwDevice = ur.Device;
                }

                // If the device is open and ready
                if (SwDevice == null)
                    throw new Exception("USB device not found.");

                IUsbDevice? usb = SwDevice as IUsbDevice;
                if (SwDevice == null)
                    throw new Exception("Device is using a WinUSB driver. Use libusbK and create a filter.");
                if (usb!.UsbRegistryInfo.IsAlive)
                    usb.ResetDevice();

                if (SwDevice.IsOpen)
                    SwDevice.Close();
                SwDevice.Open();

                if (SwDevice is IUsbDevice wholeUsbDevice)
                {
                    // This is a "whole" USB device. Before it can be used, 
                    // the desired configuration and interface must be selected.

                    // Select config #1
                    wholeUsbDevice.SetConfiguration(1);

                    // Claim interface #0.
                    bool resagain = wholeUsbDevice.ClaimInterface(0);
                    if (!resagain)
                    {
                        wholeUsbDevice.ReleaseInterface(0);
                        wholeUsbDevice.ClaimInterface(0);
                    }
                }
                else
                {
                    Disconnect();
                    throw new Exception("Device is using WinUSB driver. Use libusbK and create a filter");
                }

                // open read write endpoints 1.
                reader = SwDevice.OpenEndpointReader(ReadEndpointID.Ep01);
                writer = SwDevice.OpenEndpointWriter(WriteEndpointID.Ep01);

                Connected = true;
            }
        }

        public void Disconnect()
        {
            lock (_sync)
            {
                if (SwDevice is { IsOpen: true })
                {
                    if (SwDevice is IUsbDevice wholeUsbDevice)
                        wholeUsbDevice.ReleaseInterface(0);
                    SwDevice.Close();
                }

                reader?.Dispose();
                writer?.Dispose();
                Connected = false;
            }
        }

        public byte[] ReadBytes(ulong offset, int length) => ReadBytesUSB(offset, length);

        public byte[] ReadBytesUSB(ulong offset, int length)
        {
            lock (_sync)
            {
                SendInternal(SwitchCommand.Peek(offset, length, false));
                return ReadBulkUSB();
            }
        }

        private int SendInternal(byte[] buffer)
        {
            if (writer == null)
                throw new Exception("USB writer is null, you may have disconnected the device during previous function");

            uint pack = (uint)buffer.Length + 2;
            var ec = writer.Write(BitConverter.GetBytes(pack), 2000, out _);
            if (ec != ErrorCode.None)
            {
                Disconnect();
                throw new Exception(UsbDevice.LastErrorString);
            }
            ec = writer.Write(buffer, 2000, out var l);
            if (ec != ErrorCode.None)
            {
                Disconnect();
                throw new Exception(UsbDevice.LastErrorString);
            }
            return l;
        }

        private byte[] ReadBulkUSB()
        {
            // Give it time to push back.
            Thread.Sleep(1);

            if (reader == null)
                throw new Exception("USB device not found or not connected.");

            // Let usb-botbase tell us the response size.
            byte[] sizeOfReturn = new byte[4];
            reader.Read(sizeOfReturn, 5000, out _);

            int size = BitConverter.ToInt32(sizeOfReturn, 0);
            byte[] buffer = new byte[size];

            // Loop until we have read everything.
            int transfSize = 0;
            while (transfSize < size)
            {
                Thread.Sleep(1);
                var ec = reader.Read(buffer, transfSize, Math.Min(reader.ReadBufferSize, size - transfSize), 5000, out int lenVal);
                if (ec != ErrorCode.None)
                {
                    Disconnect();
                    throw new Exception(UsbDevice.LastErrorString);
                }
                transfSize += lenVal;
            }
            return buffer;
        }
    }
}