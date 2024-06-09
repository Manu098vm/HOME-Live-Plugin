using HomeLive.Core.Legacy;
using System.Buffers.Binary;
using PKHeX.Core;

namespace HomeLive.Core
{
    public class HomeWrapper
    {
        public PKM? PKM { get; protected set; }

        public ushort DataVersion { get => GetDataVersion(); }
        public byte[] Data { get => GetDecryptedData(); }
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

        private byte[] GetDecryptedData()
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

        private ulong GetTracker()
        {
            if (PKM is PH1 or PH2 or PKH)
                return ((dynamic)PKM).Tracker;

            return 0;
        }

        public PKM? GetHomeEntity(out int DataVersion)
        {
            if (PKM is PH1 ph1)
            {
                DataVersion = 1;
                return ph1;
            }
            else if (PKM is PH2 ph2)
            {
                DataVersion = 2;
                return ph2;
            }
            else if (PKM is PKH pkh)
            {
                DataVersion = 3;
                return pkh;
            }
            else if (PKM is not null)
            {
                DataVersion = 2;
                return PKH.ConvertFromPKM(PKM);
            }

            DataVersion = 0;
            return null;
        }


        public bool HasPB7()
        {
            if (PKM is PH1 or PH2 or PKH)
                return ((dynamic)PKM).DataPB7 is not null;
            else if (PKM is PB7)
                return true;

            return false;
        }

        public PB7? ConvertToPB7()
        {
            if (PKM is PH1 or PH2 or PKH)
                return ((dynamic)PKM).ConvertToPB7();
            else if (PKM is PB7 pb7)
                return pb7;

            return null;
        }

        public bool HasPK8()
        {
            if (PKM is PH1 or PH2 or PKH)
                return ((dynamic)PKM).DataPK8 is not null;
            else if (PKM is PK8)
                return true;

            return false;
        }

        public PK8? ConvertToPK8()
        {
            if (PKM is PH1 or PH2 or PKH)
                return ((dynamic)PKM).ConvertToPK8();
            else if (PKM is PK8 pk8)
                return pk8;
            else if (PKM is not null)
            {
                var pkm = EntityConverter.ConvertToType(PKM, typeof(PK8), out var res);
                if ((int)res < 4 && pkm is PK8 pk)
                    return pk;
            }

            return null;
        }

        public bool HasPB8()
        {
            if (PKM is PH1 or PH2 or PKH)
                return ((dynamic)PKM).DataPB8 is not null;
            else if (PKM is PB8)
                return true;

            return false;
        }

        public PB8? ConvertToPB8()
        {
            if (PKM is PH1 or PH2 or PKH)
                return ((dynamic)PKM).ConvertToPB8();
            else if (PKM is PB8 pb8)
                return pb8;
            else if (PKM is not null)
            {
                var pk = EntityConverter.ConvertToType(PKM, typeof(PB8), out var res);
                if ((int)res < 4 && pk is PB8 pb)
                    return pb;
            }

            return null;
        }

        public bool HasPA8()
        {
            if (PKM is PH1 or PH2 or PKH)
                return ((dynamic)PKM).DataPA8 is not null;
            else if (PKM is PA8)
                return true;

            return false;
        }

        public PA8? ConvertToPA8()
        {
            if (PKM is PH1 or PH2 or PKH)
                return ((dynamic)PKM).ConvertToPA8();
            else if (PKM is PA8 pa8)
                return pa8;
            else if (PKM is not null)
            {
                var pk = EntityConverter.ConvertToType(PKM, typeof(PA8), out var res);
                if ((int)res < 4 && pk is PA8 pa)
                    return pa;
            }

            return null;
        }

        public bool HasPK9()
        {
            if (PKM is PH2 or PKH)
                return ((dynamic)PKM).DataPK9 is not null;
            else if (PKM is PK9)
                return true;

            return false;
        }

        public PK9? ConvertToPK9()
        {
            if (PKM is PH1 or PH2 or PKH)
                return ((dynamic)PKM).ConvertToPK9();
            else if (PKM is PK9 pk9)
                return pk9;
            else if (PKM is not null)
            {
                var pkm = EntityConverter.ConvertToType(PKM, typeof(PK9), out var res);
                if ((int)res < 4 && pkm is PK9 pk)
                    return pk;
            }

            return null;
        }

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

        private ushort GetDataVersion()
        {
            if (PKM is PH1)
                return 1;
            else if (PKM is PH2)
                return 2;
            else
                return 3;
        }
    }
}
