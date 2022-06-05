using System;
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
        public readonly ICommunicator sys;

        public PokeSysBotMini(ConnectionType con)
        {
            sys = con switch {
                ConnectionType.WiFi => new SysBotMini(),
                ConnectionType.USB => new UsbBotMini(),
                _ => new SysBotMini(),
            };
        }

        public uint GetB1S1Offset() => BoxStart;
        public uint GetSlotSize() => SlotSize;
        public int GetSlotCount() => SlotCount;
        public int GetBoxCount() => BoxCount;
        public uint GetBoxOffset(int box) => BoxStart + (uint)(SlotSize * SlotCount * box);
        public uint GetSlotOffset(int box, int slot) => GetBoxOffset(box) + (uint)(SlotSize * slot);

        public byte[]? ReadBox(int box, int boxSize = (int)SlotSize)
        {
            var offset = GetB1S1Offset() + box * boxSize;
            return sys.ReadBytes((ulong)offset, boxSize);
        }
        public byte[]? ReadSlot(int box, int slot) 
        {
            var offset = GetSlotOffset(box, slot);
            return ReadBytesPKH(offset);
        }
        public byte[]? ReadBytesPKH(uint offset)
        {
            var data = sys.ReadBytes(offset, 0x10);

            //TBD check if proper enc data
            if (ReadUInt64LittleEndian(data.AsSpan(0x02)) == 0)
                return null;

            var EncodedDataSize = ReadUInt16LittleEndian(data.AsSpan(0x0E));
            return sys.ReadBytes(offset, 0x10 + EncodedDataSize);
        }
    }
}