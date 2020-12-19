using System.Linq;
using System.Text;

namespace PKHeX.Core.Injection
{
    /// <summary>
    /// Encodes commands for a <see cref="SysBotMini"/> to be sent as a <see cref="byte"/> array.
    /// </summary>
    public static class SwitchCommand
    {
        private static readonly Encoding Encoder = Encoding.UTF8;
        private static byte[] Encode(string command, bool addrn = true) => Encoder.GetBytes(addrn ? command + "\r\n" : command);
        /// <summary>
        /// Requests the Bot to send <see cref="count"/> bytes from <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Address of the data</param>
        /// <param name="count">Amount of bytes</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] Peek(uint offset, int count) => Encode($"peek 0x{offset:X8} {count}");

        /// <summary>
        /// Sends the Bot <see cref="data"/> to be written to <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Address of the data</param>
        /// <param name="data">Data to write</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] Poke(uint offset, byte[] data) => Encode($"poke 0x{offset:X8} 0x{string.Concat(data.Select(z => $"{z:X2}"))}");

        /// <summary>
        /// Requests the Bot to send <see cref="count"/> bytes from absolute <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Absolute address of the data</param>
        /// <param name="count">Amount of bytes</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] PeekAbsolute(ulong offset, int count) => Encode($"peekAbsolute 0x{offset:X16} {count}");

        /// <summary>
        /// Sends the Bot <see cref="data"/> to be written to absolute <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Absolute address of the data</param>
        /// <param name="data">Data to write</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] PokeAbsolute(ulong offset, byte[] data) => Encode($"pokeAbsolute 0x{offset:X16} 0x{string.Concat(data.Select(z => $"{z:X2}"))}");

        /// <summary>
        /// Requests the Bot to send <see cref="count"/> bytes from main <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Address of the data relative to main</param>
        /// <param name="count">Amount of bytes</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] PeekMain(ulong offset, int count) => Encode($"peekMain 0x{offset:X16} {count}");

        /// <summary>
        /// Sends the Bot <see cref="data"/> to be written to main <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Address of the data relative to main</param>
        /// <param name="data">Data to write</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] PokeMain(ulong offset, byte[] data) => Encode($"pokeMain 0x{offset:X16} 0x{string.Concat(data.Select(z => $"{z:X2}"))}");
    }
}
