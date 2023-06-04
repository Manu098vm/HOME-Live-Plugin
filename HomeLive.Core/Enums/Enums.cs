namespace HomeLive.Core;

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

public enum ConversionType : int
{
    SpecificData = 1,
    CompatibleData = 2,
    AnyData = 3,
}