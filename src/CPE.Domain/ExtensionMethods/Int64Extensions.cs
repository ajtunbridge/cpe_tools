﻿public static class Int64Extensions
{
    public static ulong RotateLeft(this ulong original, int bits)
    {
        return (original << bits) | (original >> (64 - bits));
    }

    public static ulong RotateRight(this ulong original, int bits)
    {
        return (original >> bits) | (original << (64 - bits));
    }

    public static unsafe ulong GetUInt64(this byte[] bb, int pos)
    {
        // we only read aligned longs, so a simple casting is enough
        fixed (byte* pbyte = &bb[pos]) {
            return *((ulong*) pbyte);
        }
    }
}