namespace HOME
{
    public enum ConnectionType : int
    {
        WiFi = 1,
        USB = 2,
    }

    public enum DumpTarget : int
    {
        B1S1 = 1,
        TargetAll = 2,
    }

    public enum DumpFormat : int
    {
        Encrypted = 1,
        Decrypted = 2,
        EncAndDec = 3,
    }
}