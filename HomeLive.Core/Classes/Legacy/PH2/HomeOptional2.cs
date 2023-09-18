/*Legacy code for PKH Data Version 2.
 * https://github.com/kwsch/PKHeX/blob/75ec6ca38dcd9ac50e781434e390c14d671ebae1/PKHeX.Core/PKM/HOME/HomeOptional1.cs
 * GPL v3 License
 * I claim no ownership of this code. Thanks to all the PKHeX contributors.*/

using PKHeX.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace HomeLive.Core.Legacy;

/// <summary>
/// Side game data base class for <see cref="PKM"/> data transferred into HOME.
/// </summary>
public abstract class HomeOptional2
{
    // Internal Attributes set on creation
    private readonly Memory<byte> Buffer; // Raw Storage
    protected Span<byte> Data => Buffer.Span;
    public int SerializedSize => HeaderSize + Buffer.Length;

    public const int HeaderSize = 3; // u8 format, u16 length(data[u8])
    protected abstract HomeGameDataFormat Format { get; }

    protected HomeOptional2(ushort size) => Buffer = new byte[size];
    protected HomeOptional2(Memory<byte> buffer) => Buffer = buffer;

    protected void EnsureSize(int size)
    {
        if (Buffer.Length != size)
            throw new ArgumentOutOfRangeException(nameof(size), size, $"Expected size {Buffer.Length} but received {size}.");
    }

    protected byte[] ToArray() => Data.ToArray();
    protected int WriteWithHeader(Span<byte> result)
    {
        result[0] = (byte)Format;
        WriteUInt16LittleEndian(result[1..], (ushort)Data.Length);
        return HeaderSize + WriteWithoutHeader(result[HeaderSize..]);
    }

    private int WriteWithoutHeader(Span<byte> result)
    {
        var span = Data;
        span.CopyTo(result);
        return span.Length;
    }
}
