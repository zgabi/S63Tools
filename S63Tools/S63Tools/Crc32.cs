using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

/// <summary>
///     Implements a 32-bit CRC hash algorithm compatible with Zip etc.
/// </summary>
/// <seealso cref="System.Security.Cryptography.HashAlgorithm" />
/// <remarks>
///     Crc32 should only be used for backward compatibility with older file formats
///     and algorithms. It is not secure enough for new applications.
///     If you need to call multiple times for the same data either use the HashAlgorithm
///     interface or remember that the result of one Compute call needs to be ~ (XOR) before
///     being passed in as the seed for the next Compute call.
/// </remarks>
public sealed class Crc32 : HashAlgorithm
{
    /// <summary>
    ///     The default polynomial
    /// </summary>
    private const uint DefaultPolynomial = 0xedb88320u;

    /// <summary>
    ///     The default seed
    /// </summary>
    private const uint DefaultSeed = 0xffffffffu;

    /// <summary>
    ///     The default table
    /// </summary>
    private static uint[] _defaultTable;

    /// <summary>
    ///     The seed
    /// </summary>
    private readonly uint _seed;

    /// <summary>
    ///     The table
    /// </summary>
    private readonly uint[] _table;

    /// <summary>
    ///     The hash
    /// </summary>
    private uint _hash;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Crc32" /> class.
    /// </summary>
    public Crc32() : this(DefaultPolynomial, DefaultSeed)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Crc32" /> class.
    /// </summary>
    /// <param name="polynomial">The polynomial.</param>
    /// <param name="seed">The seed.</param>
    public Crc32(uint polynomial, uint seed)
    {
        _table = InitializeTable(polynomial);
        _seed = _hash = seed;
    }

    /// <summary>
    ///     Initializes an implementation of the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> class.
    /// </summary>
    public override void Initialize()
    {
        _hash = _seed;
    }

    /// <summary>
    ///     When overridden in a derived class, routes data written to the object into the hash algorithm for computing the
    ///     hash.
    /// </summary>
    /// <param name="array">The input to compute the hash code for.</param>
    /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
    /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        _hash = CalculateHash(_table, _hash, array, ibStart, cbSize);
    }

    /// <summary>
    ///     When overridden in a derived class, finalizes the hash computation after the last data is processed by the
    ///     cryptographic stream object.
    /// </summary>
    /// <returns>
    ///     The computed hash code.
    /// </returns>
    protected override byte[] HashFinal()
    {
        var hashBuffer = UInt32ToBigEndianBytes(~_hash);
        HashValue = hashBuffer;
        return hashBuffer;
    }

    /// <summary>
    ///     Gets the size, in bits, of the computed hash code.
    /// </summary>
    public override int HashSize => 32;

    /// <summary>
    ///     Computes the specified buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <returns></returns>
    public static uint Compute(Span<byte> buffer)
    {
        return Compute(DefaultSeed, buffer);
    }

    /// <summary>
    ///     Computes the specified seed.
    /// </summary>
    /// <param name="seed">The seed.</param>
    /// <param name="buffer">The buffer.</param>
    /// <returns></returns>
    public static uint Compute(uint seed, Span<byte> buffer)
    {
        return Compute(DefaultPolynomial, seed, buffer);
    }

    /// <summary>
    ///     Computes the specified polynomial.
    /// </summary>
    /// <param name="polynomial">The polynomial.</param>
    /// <param name="seed">The seed.</param>
    /// <param name="buffer">The buffer.</param>
    /// <returns></returns>
    public static uint Compute(uint polynomial, uint seed, Span<byte> buffer)
    {
        return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
    }

    /// <summary>
    ///     Initializes the table.
    /// </summary>
    /// <param name="polynomial">The polynomial.</param>
    /// <returns></returns>
    private static uint[] InitializeTable(uint polynomial)
    {
        if (polynomial == DefaultPolynomial && _defaultTable != null) return _defaultTable;

        var createTable = new uint[256];
        for (int i = 0; i < 256; i++)
        {
            uint entry = (uint)i;
            for (int j = 0; j < 8; j++)
                if ((entry & 1) == 1)
                    entry = (entry >> 1) ^ polynomial;
                else
                    entry = entry >> 1;
            createTable[i] = entry;
        }

        if (polynomial == DefaultPolynomial) _defaultTable = createTable;

        return createTable;
    }

    private static uint CalculateHash(uint[] table, uint seed, Span<byte> buffer, int start, int size)
    {
        uint crc = seed;
        for (int i = start; i < size - start; i++) crc = (crc >> 8) ^ table[buffer[i] ^ (crc & 0xff)];
        return crc;
    }

    private static byte[] UInt32ToBigEndianBytes(uint uint32)
    {
        var result = BitConverter.GetBytes(uint32);

        if (BitConverter.IsLittleEndian) Array.Reverse(result);

        return result;
    }

    public string ComputeHashString(byte[] buffer)
    {
        var hash = ComputeHash(buffer);
        var sb = new StringBuilder(8);
        foreach (byte b in hash) sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
}