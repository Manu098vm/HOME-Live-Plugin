namespace HOME
{
    public enum ConnectionType : int
    {
        WiFi = 1,
        USB = 2,
    }

    public enum DumpTarget : int
    {
        TargetBox = 1,
        TargetSlot = 2,
        TargetAll = 3,
    }

    public enum DumpFormat : int
    {
        Encrypted = 1,
        Decrypted = 2,
        EncAndDec = 3,
    }
}