using System.Collections.Generic;

namespace PKHeX.Core.Injection
{
    public interface ICommunicator
    {
        void Connect();
        void Disconnect();
        byte[] ReadBytes(ulong offset, int length);
        bool Connected { get; set; }
        int Port { get; set; }
        string IP { get; set; }
    }
}
