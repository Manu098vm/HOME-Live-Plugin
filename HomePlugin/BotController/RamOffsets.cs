namespace PKHeX.Core.Injection
{
    public static class RamOffsets
    {
        public static LiveHeXVersion[] GetValidVersions(SaveFile sf)
        {
            return sf switch
            {
                SAV8SWSH _ => new[] {
                    LiveHeXVersion.BOX132, 
                    LiveHeXVersion.BOX3364, 
                    LiveHeXVersion.BOX6596, 
                    LiveHeXVersion.BOX97128, 
                    LiveHeXVersion.BOX129160, 
                    LiveHeXVersion.BOX161192, 
                    LiveHeXVersion.BOX168200 },
                _ => new[] { LiveHeXVersion.BOX132 }
            };
        }

        public static ICommunicator GetCommunicator(LiveHeXVersion lv, InjectorCommunicationType ict)
        {
            return lv switch
            {
                LiveHeXVersion.BOX132 => GetSwitchInterface(ict),
                LiveHeXVersion.BOX3364 => GetSwitchInterface(ict),
                LiveHeXVersion.BOX6596 => GetSwitchInterface(ict),
                LiveHeXVersion.BOX97128 => GetSwitchInterface(ict),
                LiveHeXVersion.BOX129160 => GetSwitchInterface(ict),
                LiveHeXVersion.BOX161192 => GetSwitchInterface(ict),
                LiveHeXVersion.BOX168200 => GetSwitchInterface(ict),
                _ => new SysBotMini()
            };
        }

        public static int GetB1S1Offset(LiveHeXVersion lv)
        {
            return lv switch
            {
                LiveHeXVersion.BOX132 => 0x1EC73028,
                LiveHeXVersion.BOX3364 => 0x1ECC3A28,
                LiveHeXVersion.BOX6596 => 0x1ED14428,
                LiveHeXVersion.BOX97128 => 0x1ED64E28,
                LiveHeXVersion.BOX129160 => 0x1EDB5828,
                LiveHeXVersion.BOX161192 => 0x1EE06228,
                LiveHeXVersion.BOX168200 => 0x1EE17C58,
                _ => 0x1E6393A8
            };
        }

        public static int GetSlotSize(LiveHeXVersion lv)
        {
            return lv switch
            {
                LiveHeXVersion.BOX132 => 344,
                LiveHeXVersion.BOX3364 => 344,
                LiveHeXVersion.BOX6596 => 344,
                LiveHeXVersion.BOX97128 => 344,
                LiveHeXVersion.BOX129160 => 344,
                LiveHeXVersion.BOX161192 => 344,
                LiveHeXVersion.BOX168200 => 344,
                _ => 344
            };
        }

        public static int GetGapSize(LiveHeXVersion lv)
        {
            return lv switch
            {
                LiveHeXVersion.BOX132 => 0,
                LiveHeXVersion.BOX3364 => 0,
                LiveHeXVersion.BOX6596 => 0,
                LiveHeXVersion.BOX97128 => 0,
                LiveHeXVersion.BOX129160 => 0,
                LiveHeXVersion.BOX161192 => 0,
                LiveHeXVersion.BOX168200 => 0,
                _ => 0
            };
        }

        public static int GetSlotCount(LiveHeXVersion lv)
        {
            return lv switch
            {
                LiveHeXVersion.BOX132 => 30,
                LiveHeXVersion.BOX3364 => 30,
                LiveHeXVersion.BOX6596 => 30,
                LiveHeXVersion.BOX97128 => 30,
                LiveHeXVersion.BOX129160 => 30,
                LiveHeXVersion.BOX161192 => 30,
                LiveHeXVersion.BOX168200 => 30,
                _ => 30
            };
        }

        public static int GetTrainerBlockSize(LiveHeXVersion lv)
        {
            return lv switch
            {
                LiveHeXVersion.BOX132 => 0x110,
                LiveHeXVersion.BOX3364 => 0x110,
                LiveHeXVersion.BOX6596 => 0x110,
                LiveHeXVersion.BOX97128 => 0x110,
                LiveHeXVersion.BOX129160 => 0x110,
                LiveHeXVersion.BOX161192 => 0x110,
                LiveHeXVersion.BOX168200 => 0x110,
                _ => 0x110
            };
        }

        public static uint GetTrainerBlockOffset(LiveHeXVersion lv)
        {
            return lv switch
            {
                //TODO
                LiveHeXVersion.BOX132 => 0x45068F18,
                LiveHeXVersion.BOX3364 => 0x45068F18,
                LiveHeXVersion.BOX6596 => 0x45068F18,
                LiveHeXVersion.BOX97128 => 0x45068F18,
                LiveHeXVersion.BOX129160 => 0x45068F18,
                LiveHeXVersion.BOX161192 => 0x45068F18,
                LiveHeXVersion.BOX168200 => 0x45068F18,
                _ => 0x45068F18
            };
        }

        public static bool WriteBoxData(LiveHeXVersion lv)
        {
            return lv switch
            {
                LiveHeXVersion.BOX132 => true,
                LiveHeXVersion.BOX3364 => true,
                LiveHeXVersion.BOX6596 => true,
                LiveHeXVersion.BOX97128 => true,
                LiveHeXVersion.BOX129160 => true,
                LiveHeXVersion.BOX161192 => true,
                LiveHeXVersion.BOX168200 => true,
                _ => false
            };
        }

        private static ICommunicator GetSwitchInterface(InjectorCommunicationType ict)
        {
            // No conditional expression possible
            return ict switch
            {
                InjectorCommunicationType.SocketNetwork => new SysBotMini(),
                _ => new SysBotMini()
            };
        }
    }
}
