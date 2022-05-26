using System.Collections.Generic;
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
        /// Removes the virtual controller from the bot. Allows physical controllers to control manually.
        /// </summary>
        /// <param name="addrn">Encoding selector. Default "true" for sys-botbase.</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] DetachController(bool addrn = true) => Encode("detachController", addrn);

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

        /// <summary>
        /// Sends the Bot <see cref="data"/> to be written to <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Address of the data</param>
        /// <param name="data">Data to write</param>
        /// <param name="addrn">Encoding selector. Default "true" for sys-botbase.</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] Poke(ulong offset, byte[] data, bool addrn = true) => Encode($"poke 0x{offset:X16} 0x{string.Concat(data.Select(z => $"{z:X2}"))}", addrn);

        /// <summary>
        /// Requests the Bot to send <see cref="count"/> bytes from absolute <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Absolute address of the data</param>
        /// <param name="count">Amount of bytes</param>
        /// <param name="addrn">Encoding selector. Default "true" for sys-botbase.</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] PeekAbsolute(ulong offset, int count, bool addrn = true) => Encode($"peekAbsolute 0x{offset:X16} {count}", addrn);

        /// <summary>
        /// Requests the Bot to send concatenated byte array of multiple pointer reads
        /// </summary>
        /// <param name="offsets">Dictionary of offsets and counts</param>
        /// <param name="addrn">Encoding selector. Default "true" for sys-botbase.</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] PeekAbsoluteMulti(Dictionary<ulong, int> offsets, bool addrn = true) => Encode($"peekAbsoluteMulti {string.Join(" ", offsets.Select(z => $"{z.Key} {z.Value}"))}", addrn);

        /// <summary>
        /// Sends the Bot <see cref="data"/> to be written to absolute <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Absolute address of the data</param>
        /// <param name="data">Data to write</param>
        /// <param name="addrn">Encoding selector. Default "true" for sys-botbase.</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] PokeAbsolute(ulong offset, byte[] data, bool addrn = true) => Encode($"pokeAbsolute 0x{offset:X16} 0x{string.Concat(data.Select(z => $"{z:X2}"))}", addrn);

        /// <summary>
        /// Requests the Bot to send <see cref="count"/> bytes from main <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Address of the data relative to main</param>
        /// <param name="count">Amount of bytes</param>
        /// <param name="addrn">Encoding selector. Default "true" for sys-botbase.</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] PeekMain(ulong offset, int count, bool addrn = true) => Encode($"peekMain 0x{offset:X16} {count}", addrn);

        /// <summary>
        /// Sends the Bot <see cref="data"/> to be written to main <see cref="offset"/>.
        /// </summary>
        /// <param name="offset">Address of the data relative to main</param>
        /// <param name="data">Data to write</param>
        /// <param name="addrn">Encoding selector. Default "true" for sys-botbase.</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] PokeMain(ulong offset, byte[] data, bool addrn = true) => Encode($"pokeMain 0x{offset:X16} 0x{string.Concat(data.Select(z => $"{z:X2}"))}", addrn);

        /// <summary>
        /// Requests the Bot to send the address of the Heap base.
        /// </summary>
        /// <param name="addrn">Encoding selector. Default "true" for sys-botbase.</param>
        /// <returns>Encoded command bytes</returns>
        public static byte[] GetHeapBase(bool addrn = true) => Encode("getHeapBase", addrn);
    }
}
