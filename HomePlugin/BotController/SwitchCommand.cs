//Taken from Live Hex

namespace PKHeX.Core.Injection
{
    /// <summary>
    /// Encodes commands for a <see cref="SysBotMini"/> to be sent as a <see cref="byte"/> array.
    /// </summary>
    public static class SwitchCommand
    {
        private static readonly System.Text.Encoding Encoder = System.Text.Encoding.UTF8;
        private static byte[] Encode(string command, bool addrn = true) => Encoder.GetBytes(addrn ? command + "\r\n" : command);

        /* 
         *
         * Memory I/O Commands
         *
         */

        /// <summary>
        /// Requests the Bot to send <see cref="count"/> bytes from <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Address of the data</param>
        /// <param name="count">Amount of bytes</param>
        /// <param name="addrn">Encoding selector. Default "true" for sys-botbase.</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] Peek(ulong offset, int count, bool addrn = true) => Encode($"peek 0x{offset:X16} {count}", addrn);
    }
}
