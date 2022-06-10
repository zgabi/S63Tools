using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace S63Tools;

public class S63Tools
{
    public static string CreateUserPermit(byte[] hwId, byte[] key, ushort mId)
    {
        if (hwId.Length != 5)
        {
            throw new Exception("Invalid HW ID length.");
        }

        Span<byte> hwIdBlock = stackalloc byte[8];
        hwId.CopyTo(hwIdBlock);
        hwIdBlock[5] = 3;
        hwIdBlock[6] = 3;
        hwIdBlock[7] = 3;

        var blow = new BlowFish(key);
        var enc = blow.EncryptECB(hwIdBlock);

        var permit = new byte[28];
        for (int i = 0; i < enc.Length; i++)
        {
            Utf8Formatter.TryFormat(enc[i], permit.AsSpan(2 * i), out _, new StandardFormat('X', 2));
        }

        uint crc = Crc32.Compute(permit.AsSpan(0, 16));
        Utf8Formatter.TryFormat(crc, permit.AsSpan(16), out _, new StandardFormat('X', 8));
        Utf8Formatter.TryFormat(mId, permit.AsSpan(24), out _, new StandardFormat('X', 4));

        return Encoding.ASCII.GetString(permit);
    }

    public static byte[] DecryptUserPermit(string userPermit, byte[] key, out ushort mId)
    {
        var permit = Encoding.ASCII.GetBytes(userPermit);
        uint crc = Crc32.Compute(permit.AsSpan(0, 16));

        Utf8Parser.TryParse(permit.AsSpan(16, 8), out uint crc2, out _, 'X');

        if (crc != crc2)
        {
            throw new Exception("Invalid CRC.");
        }

        Utf8Parser.TryParse(permit.AsSpan(24, 4), out mId, out _, 'X');

        var blow = new BlowFish(key);
        Span<byte> hwIdBlock = stackalloc byte[8];
        for (int i = 0; i < hwIdBlock.Length; i++)
        {
            Utf8Parser.TryParse(permit.AsSpan(i * 2, 2), out byte b, out _, 'X');
            hwIdBlock[i] = b;
        }

        hwIdBlock = blow.DecryptCBC(hwIdBlock);

        if (hwIdBlock.Length != 8)
        {
            throw new Exception("Invalid HW ID length.");
        }

        if (hwIdBlock[5] != 3 || hwIdBlock[6] != 3 || hwIdBlock[7] != 3)
        {
            throw new Exception("Invalid HW ID.");
        }

        return hwIdBlock.Slice(0, 5).ToArray();
    }

    public static string CreateCellPermit(byte[] hwId, string cellName, DateTime expiryDate, byte[] ck1, byte[] ck2)
    {
        if (hwId.Length != 5)
        {
            throw new Exception("Invalid HW ID length.");
        }

        if (ck1.Length != 5)
        {
            throw new Exception("Invalid Cell Key 1 length.");
        }

        if (ck2.Length != 5)
        {
            throw new Exception("Invalid Cell Key 2 length.");
        }
            
        var hwId6 = new byte[6];
        hwId.AsSpan().CopyTo(hwId6);
        hwId6[5] = hwId[0];

        var permit = new byte[64];
        Encoding.ASCII.GetBytes(cellName, permit.AsSpan());
        Utf8Formatter.TryFormat(expiryDate.Year, permit.AsSpan(8), out _, new StandardFormat('D', 4));
        Utf8Formatter.TryFormat(expiryDate.Month, permit.AsSpan(12), out _, new StandardFormat('D', 2));
        Utf8Formatter.TryFormat(expiryDate.Day, permit.AsSpan(14), out _, new StandardFormat('D', 2));

        Span<byte> block = stackalloc byte[8];
        var blow = new BlowFish(hwId6);

        ck1.CopyTo(block);
        block[5] = 3;
        block[6] = 3;
        block[7] = 3;

        var eck = blow.EncryptECB(block);
        for (int i = 0; i < eck.Length; i++)
        {
            Utf8Formatter.TryFormat(eck[i], permit.AsSpan(16 + 2 * i), out _, new StandardFormat('X', 2));
        }

        ck2.CopyTo(block);
        block[5] = 3;
        block[6] = 3;
        block[7] = 3;

        eck = blow.EncryptECB(block);
        for (int i = 0; i < eck.Length; i++)
        {
            Utf8Formatter.TryFormat(eck[i], permit.AsSpan(32 + 2 * i), out _, new StandardFormat('X', 2));
        }

        uint crc = Crc32.Compute(permit.AsSpan(0, 48));

        BinaryPrimitives.TryWriteUInt32BigEndian(block, crc);
        block[4] = 4;
        block[5] = 4;
        block[6] = 4;
        block[7] = 4;

        var encHash = blow.EncryptECB(block);
        for (int i = 0; i < encHash.Length; i++)
        {
            Utf8Formatter.TryFormat(encHash[i], permit.AsSpan(48 + 2 * i), out _, new StandardFormat('X', 2));
        }

        return Encoding.ASCII.GetString(permit);
    }

    public static bool TryDecryptCellPermit(string cellPermit, byte[] hwId, out byte[] ck1, out byte[] ck2)
    {
        var permit = Encoding.ASCII.GetBytes(cellPermit);
        uint crc = Crc32.Compute(permit.AsSpan(0, 48));

        Span<byte> hwId6 = stackalloc byte[6];
        hwId.AsSpan().CopyTo(hwId6);
        hwId6[5] = hwId[0];

        Span<byte> block = stackalloc byte[8];
        var blow = new BlowFish(hwId6);

        for (int i = 0; i < 8; i++)
        {
            Utf8Parser.TryParse(permit.AsSpan(48 + i * 2, 2), out byte b, out _, 'X');
            block[i] = b;
        }

        var crcBlock = blow.DecryptCBC(block);
        if (crcBlock[4] != 4 || crcBlock[5] != 4 || crcBlock[6] != 4 || crcBlock[7] != 4)
        {
            // Invalid CRC.
            ck1 = null;
            ck2 = null;
            return false;
        }

        uint crc2 = BinaryPrimitives.ReadUInt32BigEndian(crcBlock);
        if (crc != crc2)
        {
            // Invalid CRC.
            ck1 = null;
            ck2 = null;
            return false;
        }

        for (int i = 0; i < 8; i++)
        {
            Utf8Parser.TryParse(permit.AsSpan(16 + i * 2, 2), out byte b, out _, 'X');
            block[i] = b;
        }

        var ck1Block = blow.DecryptCBC(block);
        if (ck1Block[5] != 3 || ck1Block[6] != 3 || ck1Block[7] != 3)
        {
            // Invalid Cell Key 1.
            ck1 = null;
            ck2 = null;
            return false;
        }

        ck1 = ck1Block.AsSpan(0, 5).ToArray();

        for (int i = 0; i < 8; i++)
        {
            Utf8Parser.TryParse(permit.AsSpan(32 + i * 2, 2), out byte b, out _, 'X');
            block[i] = b;
        }

        var ck2Block = blow.DecryptCBC(block);
        if (ck2Block[5] != 3 || ck2Block[6] != 3 || ck2Block[7] != 3)
        {
            // Invalid Cell Key 2.
            ck1 = null;
            ck2 = null;
            return false;
        }

        ck2 = ck2Block.AsSpan(0, 5).ToArray();
        return true;
    }

    public static byte[]? HackUserPermit(string userPermit, out ushort mId, out byte[]? keyBytes)
    {
        var permit = Encoding.ASCII.GetBytes(userPermit);
        uint crc = Crc32.Compute(permit.AsSpan(0, 16));

        Utf8Parser.TryParse(permit.AsSpan(16, 8), out uint crc2, out _, 'X');

        if (crc != crc2)
        {
            throw new Exception("Invalid CRC.");
        }

        Utf8Parser.TryParse(permit.AsSpan(24, 4), out mId, out _, 'X');

        Span<byte> hwIdBlockDef = stackalloc byte[8];
        for (int i = 0; i < hwIdBlockDef.Length; i++)
        {
            Utf8Parser.TryParse(permit.AsSpan(i * 2, 2), out byte b, out _, 'X');
            hwIdBlockDef[i] = b;
        }

        var keyFinder = new KeyFinder(hwIdBlockDef);
        keyFinder.Start();

        keyBytes = keyFinder.FoundKey;
        return keyFinder.FoundHwId;
    }

    private class KeyFinder
    {
        private readonly byte[] _hwIdBlockDef;
        private int _i;

        public volatile byte[]? FoundKey;
        public volatile byte[]? FoundHwId;

        private static readonly byte[] Hex = {
            (byte) '0', (byte) '1', (byte) '2', (byte) '3', (byte) '4', (byte) '5', (byte) '6', (byte) '7',
            (byte) '8', (byte) '9', (byte) 'A', (byte) 'B', (byte) 'C', (byte) 'D', (byte) 'E', (byte) 'F'
        };

        public KeyFinder(Span<byte> hwIdBlockDef)
        {
            _hwIdBlockDef = hwIdBlockDef.ToArray();
        }

        private void ThreadFunc()
        {
            var key = new byte[5];
            Span<byte> hwIdBlock = stackalloc byte[8];
            var blow = new BlowFish();

            while (true)
            {
                int i = Interlocked.Increment(ref _i) - 1;
                if (i >= 0x100000 || FoundKey != null)
                {
                    break;
                }

                key[4] = Hex[i & 0xf];
                key[3] = Hex[(i >> 4) & 0xf];
                key[2] = Hex[(i >> 8) & 0xf];
                key[1] = Hex[(i >> 12) & 0xf];
                key[0] = Hex[(i >> 16) & 0xf];

                blow.SetupKey5(key);

                _hwIdBlockDef.CopyTo(hwIdBlock);
                blow.DecryptCBC8(hwIdBlock);

                if (hwIdBlock[5] != 3 || hwIdBlock[6] != 3 || hwIdBlock[7] != 3)
                {
                    continue;
                }

                FoundKey = key;
                FoundHwId = hwIdBlock.Slice(0, 5).ToArray();
                break;
            }
        }

        public void Start()
        {
            var threads = new List<Thread>();
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var t = new Thread(ThreadFunc);
                threads.Add(t);
                t.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }

    public static void LoadPermit(string permitPath, Dictionary<string, (byte[], byte[])> permits, byte[][] hardwareIds)
    {
        Exception exception = null;
        foreach (var hardwareId in hardwareIds)
        {
            try
            {
                if (TryLoadPermit(permitPath, permits, hardwareId))
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        if (exception != null)
        {
            throw exception;
        }

        throw new Exception("Failed to load the permits.");
    }

    public static bool TryLoadPermit(string permitPath, Dictionary<string, (byte[], byte[])> permits, byte[] hardwareId)
    {
        var lines = File.ReadAllLines(permitPath);
        foreach (string line in lines)
        {
            if (line.StartsWith(':'))
            {
                continue;
            }

            var parts = line.Split(',');
            if (parts.Length > 2)
            {
                string permit = parts[0];
                if (!TryDecryptCellPermit(permit, hardwareId, out var ck1, out var ck2))
                {
                    return false;
                }

                string cellName = permit.Substring(0, 8);
                permits[cellName] = (ck1, ck2);
            }
        }

        return true;
    }
}
