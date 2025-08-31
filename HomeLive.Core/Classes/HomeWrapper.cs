using HomeLive.Core.Legacy;
using System.Buffers.Binary;
using PKHeX.Core;

namespace HomeLive.Core;

public class HomeWrapper
{
    public PKM? PKM { get; protected set; }

    public ushort DataVersion { get => GetDataVersion(); }
    public Span<byte> Data { get => GetDecryptedData(); }
    public byte[] EncryptedData { get => GetEncryptedData(); }
    public ulong Tracker { get => GetTracker(); }
    public bool IsValid { get => GetIsValid(); }

    public HomeWrapper(byte[] data)
    {
        PKM = null;
        var version = BinaryPrimitives.ReadUInt16LittleEndian(data);

        switch (version)
        {
            case 1:
                PKM = new PH1(data);
                break;
            case 2:
                PKM = new PH2(data);
                break;
            case 3:
                PKM = new PKH(data);
                break;
        }

        if (PKM is null || !GetIsValid(PKM))
            PKM = EntityFormat.GetFromBytes(data);
    }

    public HomeWrapper(PKM? pkm) => PKM = pkm;

    private Span<byte> GetDecryptedData()
    {
        if (PKM is PH1 or PH2 or PKH)
            return PKM.Data;
        else if (PKM is not null)
            return PKH.ConvertFromPKM(PKM).Data;

        throw new ArgumentNullException("PKM is null");
    }

    private byte[] GetEncryptedData()
    {
        if (PKM is PH1 or PH2 or PKH)
            return HomeCrypto.Encrypt(Data);
        else if (PKM is not null)
            return HomeCrypto.Encrypt(PKH.ConvertFromPKM(PKM).Data);

        throw new ArgumentNullException("PKM is null");
    }

    private ulong GetTracker() => PKM switch
    {
        IHomeTrack track => track.Tracker,
        _ => 0
    };

    public bool HasPB7() => PKM switch
    {
        PH1 ph1 => ph1.DataPB7 is not null,
        PH2 ph2 => ph2.DataPB7 is not null,
        PKH pkh => pkh.DataPB7 is not null,
        PB7 => true,
        _ => false
    };

    public PB7? ConvertToPB7() => PKM switch
    {
        PH1 ph1 => ph1.ConvertToPB7(),
        PH2 ph2 => ph2.ConvertToPB7(),
        PKH pkh => pkh.ConvertToPB7(),
        PB7 pb7 => pb7,
        _ => null
    };

    public bool HasPK8() => PKM switch
    {
        PH1 ph1 => ph1.DataPK8 is not null,
        PH2 ph2 => ph2.DataPK8 is not null,
        PKH pkh => pkh.DataPK8 is not null,
        PK8 => true,
        _ => false
    };

    public PK8? ConvertToPK8() => PKM switch
    {
        PH1 ph1 => ph1.ConvertToPK8(),
        PH2 ph2 => ph2.ConvertToPK8(),
        PKH pkh => pkh.ConvertToPK8(),
        PK8 pk8 => pk8,
        _ => EntityConverter.ConvertToType(PKM!, typeof(PK8), out _) as PK8,
    };

    public bool HasPB8() => PKM switch
    {
        PH1 ph1 => ph1.DataPB8 is not null,
        PH2 ph2 => ph2.DataPB8 is not null,
        PKH pkh => pkh.DataPB8 is not null,
        PB8 => true,
        _ => false
    };

    public PB8? ConvertToPB8() => PKM switch
    {
        PH1 ph1 => ph1.ConvertToPB8(),
        PH2 ph2 => ph2.ConvertToPB8(),
        PKH pkh => pkh.ConvertToPB8(),
        PB8 pb8 => pb8,
        _ => EntityConverter.ConvertToType(PKM!, typeof(PB8), out _) as PB8,
    };

    public bool HasPA8() => PKM switch
    {
        PH1 ph1 => ph1.DataPA8 is not null,
        PH2 ph2 => ph2.DataPA8 is not null,
        PKH pkh => pkh.DataPA8 is not null,
        PA8 => true,
        _ => false
    };

    public PA8? ConvertToPA8() => PKM switch
    {
        PH1 ph1 => ph1.ConvertToPA8(),
        PH2 ph2 => ph2.ConvertToPA8(),
        PKH pkh => pkh.ConvertToPA8(),
        PA8 pa8 => pa8,
        _ => EntityConverter.ConvertToType(PKM!, typeof(PA8), out _) as PA8,
    };

    public bool HasPK9() => PKM switch
    {
        PH2 ph2 => ph2.DataPK9 is not null,
        PKH pkh => pkh.DataPK9 is not null,
        PK9 => true,
        _ => false
    };

    public PK9? ConvertToPK9() => PKM switch
    {
        PH2 ph2 => ph2.ConvertToPK9(),
        PKH pkh => pkh.ConvertToPK9(),
        PK9 pk9 => pk9,
        _ => EntityConverter.ConvertToType(PKM!, typeof(PK9), out _) as PK9,
    };

    private bool GetIsValid() => GetIsValid(PKM);

    private static bool GetIsValid(PKM? pkm)
    {
        if (pkm is null)
            return false;

        if (pkm is PH1 ph1 && ph1.Species > (ushort)Species.None && ph1.Species <= (ushort)Species.Enamorus)
            return true;
        else if (pkm is PH2 ph2 && ph2.Species > (ushort)Species.None && ph2.Species <= (ushort)Species.IronLeaves)
            return true;
        else if (pkm is PKH ph3 && ph3.Species > (ushort)Species.None && ph3.Species < (ushort)Species.MAX_COUNT)
            return true;

        return pkm.ChecksumValid;
    }

    private ushort GetDataVersion() => PKM switch
    {
        PH1 ph1 => ph1.DataVersion,
        PH2 ph2 => ph2.DataVersion,
        PKH pkh => pkh.DataVersion,
        _ => 0
    };
}
