namespace PKHeX.Core.Injection
{
    public class PokeSysBotMini
    {
        public readonly int BoxStart;
        public const int SlotSize = 344;
        public const int SlotCount = 30;
        public readonly int page;
        public readonly SysBotMini sys;

        public PokeSysBotMini(int lv)
        {
            page = lv;
            sys = new SysBotMini();
            BoxStart = 0x1EC73028 + (30 * 32 * 344 * lv);
        }

        private uint GetBoxOffset(int box) => (uint)BoxStart + (uint)(SlotSize * SlotCount * box);
        private uint GetSlotOffset(int box, int slot) => GetBoxOffset(box) + (uint)(SlotSize * slot);
        public byte[] ReadBox(int box, int len) =>  sys.ReadBytes(GetBoxOffset(box), len);
        public byte[] ReadSlot(int box, int slot) => sys.ReadBytes(GetSlotOffset(box, slot), SlotSize);
    }
}