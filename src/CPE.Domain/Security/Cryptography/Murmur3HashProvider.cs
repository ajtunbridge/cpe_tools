﻿#region Using directives

using System;

#endregion

namespace CPE.Domain.Security.Cryptography
{
    /// <summary>
    /// Very fast hashing algorithm for hashing documents
    /// </summary>
    public class Murmur3HashProvider : IHashProvider
    {
        private const ulong ReadSize = 16;
        private const ulong C1 = 0x87c37b91114253d5L;
        private const ulong C2 = 0x4cf5ad432745937fL;
        private readonly uint _seed; // if want to start with a seed, create a constructor

        private ulong _h1;
        private ulong _h2;
        private ulong _length;

        public Murmur3HashProvider()
        {
        }

        public Murmur3HashProvider(uint seed)
        {
            _seed = seed;
        }

        private byte[] Hash
        {
            get
            {
                _h1 ^= _length;
                _h2 ^= _length;

                _h1 += _h2;
                _h2 += _h1;

                _h1 = MixFinal(_h1);
                _h2 = MixFinal(_h2);

                _h1 += _h2;
                _h2 += _h1;

                var hash = new byte[ReadSize];

                Array.Copy(BitConverter.GetBytes(_h1), 0, hash, 0, 8);
                Array.Copy(BitConverter.GetBytes(_h2), 0, hash, 8, 8);

                return hash;
            }
        }

        public byte[] ComputeHashToBytes(byte[] bytes)
        {
            ProcessBytes(bytes);
            return Hash;
        }

        public string ComputeHashToString(byte[] bytes)
        {
            var hash = ComputeHashToBytes(bytes);

            string hex = BitConverter.ToString(hash);

            return hex.Replace("-", "");
        }

        private void MixBody(ulong k1, ulong k2)
        {
            _h1 ^= MixKey1(k1);

            _h1 = _h1.RotateLeft(27);
            _h1 += _h2;
            _h1 = _h1*5 + 0x52dce729;

            _h2 ^= MixKey2(k2);

            _h2 = _h2.RotateLeft(31);
            _h2 += _h1;
            _h2 = _h2*5 + 0x38495ab5;
        }

        private static ulong MixKey1(ulong k1)
        {
            k1 *= C1;
            k1 = k1.RotateLeft(31);
            k1 *= C2;
            return k1;
        }

        private static ulong MixKey2(ulong k2)
        {
            k2 *= C2;
            k2 = k2.RotateLeft(33);
            k2 *= C1;
            return k2;
        }

        private static ulong MixFinal(ulong k)
        {
            // avalanche bits

            k ^= k >> 33;
            k *= 0xff51afd7ed558ccdL;
            k ^= k >> 33;
            k *= 0xc4ceb9fe1a85ec53L;
            k ^= k >> 33;
            return k;
        }

        private void ProcessBytes(byte[] bytes)
        {
            _h1 = _seed;
            _length = 0L;

            int pos = 0;
            var remaining = (ulong) bytes.Length;

            // read 128 bits, 16 bytes, 2 longs in eacy cycle
            while (remaining >= ReadSize) {
                ulong k1 = bytes.GetUInt64(pos);
                pos += 8;

                ulong k2 = bytes.GetUInt64(pos);
                pos += 8;

                _length += ReadSize;
                remaining -= ReadSize;

                MixBody(k1, k2);
            }

            // if the input MOD 16 != 0
            if (remaining > 0) {
                ProcessBytesRemaining(bytes, remaining, pos);
            }
        }

        private void ProcessBytesRemaining(byte[] bytes, ulong remaining, int pos)
        {
            ulong k1 = 0;
            ulong k2 = 0;
            _length += remaining;

            // little endian (x86) processing
            switch (remaining) {
                case 15:
                    k2 ^= (ulong) bytes[pos + 14] << 48; // fall through
                    goto case 14;
                case 14:
                    k2 ^= (ulong) bytes[pos + 13] << 40; // fall through
                    goto case 13;
                case 13:
                    k2 ^= (ulong) bytes[pos + 12] << 32; // fall through
                    goto case 12;
                case 12:
                    k2 ^= (ulong) bytes[pos + 11] << 24; // fall through
                    goto case 11;
                case 11:
                    k2 ^= (ulong) bytes[pos + 10] << 16; // fall through
                    goto case 10;
                case 10:
                    k2 ^= (ulong) bytes[pos + 9] << 8; // fall through
                    goto case 9;
                case 9:
                    k2 ^= (ulong) bytes[pos + 8]; // fall through
                    goto case 8;
                case 8:
                    k1 ^= bytes.GetUInt64(pos);
                    break;
                case 7:
                    k1 ^= (ulong) bytes[pos + 6] << 48; // fall through
                    goto case 6;
                case 6:
                    k1 ^= (ulong) bytes[pos + 5] << 40; // fall through
                    goto case 5;
                case 5:
                    k1 ^= (ulong) bytes[pos + 4] << 32; // fall through
                    goto case 4;
                case 4:
                    k1 ^= (ulong) bytes[pos + 3] << 24; // fall through
                    goto case 3;
                case 3:
                    k1 ^= (ulong) bytes[pos + 2] << 16; // fall through
                    goto case 2;
                case 2:
                    k1 ^= (ulong) bytes[pos + 1] << 8; // fall through
                    goto case 1;
                case 1:
                    k1 ^= (ulong) bytes[pos]; // fall through
                    break;
                default:
                    throw new Exception("Something went wrong with remaining bytes calculation.");
            }

            _h1 ^= MixKey1(k1);
            _h2 ^= MixKey2(k2);
        }
    }
}