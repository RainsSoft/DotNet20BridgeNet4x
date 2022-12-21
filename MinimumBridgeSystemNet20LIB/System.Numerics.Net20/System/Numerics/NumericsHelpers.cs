// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==

using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace System.Numerics
{

    [StructLayout(LayoutKind.Explicit)]
    internal struct DoubleUlong
    {
        [FieldOffset(0)]
        public double dbl;
        [FieldOffset(0)]
        public ulong uu;
    }


    internal static class NumericsHelpers
    {
        private const int kcbitUint = 32;

        public static void GetDoubleParts(double dbl, out int sign, out int exp, out ulong man, out bool fFinite) {
            Contract.Ensures(Contract.ValueAtReturn(out sign) == +1 || Contract.ValueAtReturn(out sign) == -1);

            DoubleUlong du;
            du.uu = 0;
            du.dbl = dbl;

            sign = 1 - ((int)(du.uu >> 62) & 2);
            man = du.uu & 0x000FFFFFFFFFFFFF;
            exp = (int)(du.uu >> 52) & 0x7FF;
            if (exp == 0) {
                // Denormalized number.
                fFinite = true;
                if (man != 0)
                    exp = -1074;
            }
            else if (exp == 0x7FF) {
                // NaN or Inifite.
                fFinite = false;
                exp = int.MaxValue;
            }
            else {
                fFinite = true;
                man |= 0x0010000000000000;
                exp -= 1075;
            }
        }

        public static double GetDoubleFromParts(int sign, int exp, ulong man) {
            DoubleUlong du;
            du.dbl = 0;

            if (man == 0)
                du.uu = 0;
            else {
                // Normalize so that 0x0010 0000 0000 0000 is the highest bit set.
                int cbitShift = CbitHighZero(man) - 11;
                if (cbitShift < 0)
                    man >>= -cbitShift;
                else
                    man <<= cbitShift;
                exp -= cbitShift;
                Contract.Assert((man & 0xFFF0000000000000) == 0x0010000000000000);

                // Move the point to just behind the leading 1: 0x001.0 0000 0000 0000
                // (52 bits) and skew the exponent (by 0x3FF == 1023).
                exp += 1075;

                if (exp >= 0x7FF) {
                    // Infinity.
                    du.uu = 0x7FF0000000000000;
                }
                else if (exp <= 0) {
                    // Denormalized.
                    exp--;
                    if (exp < -52) {
                        // Underflow to zero.
                        du.uu = 0;
                    }
                    else {
                        du.uu = man >> -exp;
                        Contract.Assert(du.uu != 0);
                    }
                }
                else {
                    // Mask off the implicit high bit.
                    du.uu = (man & 0x000FFFFFFFFFFFFF) | ((ulong)exp << 52);
                }
            }

            if (sign < 0)
                du.uu |= 0x8000000000000000;

            return du.dbl;
        }



        // Do an in-place twos complement of d and also return the result.
        // "Dangerous" because it causes a mutation and needs to be used
        // with care for immutable types
        public static uint[] DangerousMakeTwosComplement(uint[] d) {
            // first do complement and +1 as long as carry is needed
            int i = 0;
            uint v = 0;
            for (; i < d.Length; i++) {
                v = ~d[i] + 1;
                d[i] = v;
                if (v != 0) { i++; break; }
            }
            if (v != 0) {
                // now ones complement is sufficient
                for (; i < d.Length; i++) {
                    d[i] = ~d[i];
                }
            }
            else {
                //??? this is weird
                d = resize(d, d.Length + 1);
                d[d.Length - 1] = 1;
            }
            return d;
        }

        public static uint[] resize(uint[] v, int len) {
            if (v.Length == len) return v;
            uint[] ret = new uint[len];
            int n = System.Math.Min(v.Length, len);
            for (int i = 0; i < n; i++) {
                ret[i] = v[i];
            }
            return ret;
        }

        public static void Swap<T>(ref T a, ref T b) {
            T tmp = a;
            a = b;
            b = tmp;
        }

        public static uint GCD(uint u1, uint u2) {
            const int cvMax = 32;
            if (u1 < u2)
                goto LOther;
            LTop:
            Contract.Assert(u2 <= u1);
            if (u2 == 0)
                return u1;
            for (int cv = cvMax; ;) {
                u1 -= u2;
                if (u1 < u2)
                    break;
                if (--cv == 0) {
                    u1 %= u2;
                    break;
                }
            }
            LOther:
            Contract.Assert(u1 < u2);
            if (u1 == 0)
                return u2;
            for (int cv = cvMax; ;) {
                u2 -= u1;
                if (u2 < u1)
                    break;
                if (--cv == 0) {
                    u2 %= u1;
                    break;
                }
            }
            goto LTop;
        }

        public static ulong GCD(ulong uu1, ulong uu2) {
            const int cvMax = 32;
            if (uu1 < uu2)
                goto LOther;
            LTop:
            Contract.Assert(uu2 <= uu1);
            if (uu1 <= uint.MaxValue)
                goto LSmall;
            if (uu2 == 0)
                return uu1;
            for (int cv = cvMax; ;) {
                uu1 -= uu2;
                if (uu1 < uu2)
                    break;
                if (--cv == 0) {
                    uu1 %= uu2;
                    break;
                }
            }
            LOther:
            Contract.Assert(uu1 < uu2);
            if (uu2 <= uint.MaxValue)
                goto LSmall;
            if (uu1 == 0)
                return uu2;
            for (int cv = cvMax; ;) {
                uu2 -= uu1;
                if (uu2 < uu1)
                    break;
                if (--cv == 0) {
                    uu2 %= uu1;
                    break;
                }
            }
            goto LTop;

            LSmall:
            uint u1 = (uint)uu1;
            uint u2 = (uint)uu2;
            if (u1 < u2)
                goto LOtherSmall;
            LTopSmall:
            Contract.Assert(u2 <= u1);
            if (u2 == 0)
                return u1;
            for (int cv = cvMax; ;) {
                u1 -= u2;
                if (u1 < u2)
                    break;
                if (--cv == 0) {
                    u1 %= u2;
                    break;
                }
            }
            LOtherSmall:
            Contract.Assert(u1 < u2);
            if (u1 == 0)
                return u2;
            for (int cv = cvMax; ;) {
                u2 -= u1;
                if (u2 < u1)
                    break;
                if (--cv == 0) {
                    u2 %= u1;
                    break;
                }
            }
            goto LTopSmall;
        }

        public static ulong MakeUlong(uint uHi, uint uLo) {
            return ((ulong)uHi << kcbitUint) | uLo;
        }

        public static uint GetLo(ulong uu) {
            return (uint)uu;
        }

        public static uint GetHi(ulong uu) {
            return (uint)(uu >> kcbitUint);
        }

        public static uint Abs(int a) {
            uint mask = (uint)(a >> 31);
            return ((uint)a ^ mask) - mask;
        }

        //    public static ulong Abs(long a) {
        //      ulong mask = (ulong)(a >> 63);
        //      return ((ulong)a ^ mask) - mask;
        //    }

        public static uint CombineHash(uint u1, uint u2) {
            return ((u1 << 7) | (u1 >> 25)) ^ u2;
        }

        public static int CombineHash(int n1, int n2) {
            return (int)CombineHash((uint)n1, (uint)n2);
        }
        public static int CbitHighZero(uint u) {
            if (u == 0)
                return 32;

            int cbit = 0;
            if ((u & 0xFFFF0000) == 0) {
                cbit += 16;
                u <<= 16;
            }
            if ((u & 0xFF000000) == 0) {
                cbit += 8;
                u <<= 8;
            }
            if ((u & 0xF0000000) == 0) {
                cbit += 4;
                u <<= 4;
            }
            if ((u & 0xC0000000) == 0) {
                cbit += 2;
                u <<= 2;
            }
            if ((u & 0x80000000) == 0)
                cbit += 1;
            return cbit;
        }

        public static int CbitLowZero(uint u) {
            if (u == 0)
                return 32;

            int cbit = 0;
            if ((u & 0x0000FFFF) == 0) {
                cbit += 16;
                u >>= 16;
            }
            if ((u & 0x000000FF) == 0) {
                cbit += 8;
                u >>= 8;
            }
            if ((u & 0x0000000F) == 0) {
                cbit += 4;
                u >>= 4;
            }
            if ((u & 0x00000003) == 0) {
                cbit += 2;
                u >>= 2;
            }
            if ((u & 0x00000001) == 0)
                cbit += 1;
            return cbit;
        }

        public static int CbitHighZero(ulong uu) {
            if ((uu & 0xFFFFFFFF00000000) == 0)
                return 32 + CbitHighZero((uint)uu);
            return CbitHighZero((uint)(uu >> 32));
        }

        //    public static int CbitLowZero(ulong uu) {
        //      if ((uint)uu == 0)
        //        return 32 + CbitLowZero((uint)(uu >> 32));
        //      return CbitLowZero((uint)uu);
        //    }
        //
        //    public static int Cbit(uint u) {
        //      u = (u & 0x55555555) + ((u >> 1) & 0x55555555);
        //      u = (u & 0x33333333) + ((u >> 2) & 0x33333333);
        //      u = (u & 0x0F0F0F0F) + ((u >> 4) & 0x0F0F0F0F);
        //      u = (u & 0x00FF00FF) + ((u >> 8) & 0x00FF00FF);
        //      return (int)((ushort)u + (ushort)(u >> 16));
        //    }
        //
        //    static int Cbit(ulong uu) {
        //      uu = (uu & 0x5555555555555555) + ((uu >> 1) & 0x5555555555555555);
        //      uu = (uu & 0x3333333333333333) + ((uu >> 2) & 0x3333333333333333);
        //      uu = (uu & 0x0F0F0F0F0F0F0F0F) + ((uu >> 4) & 0x0F0F0F0F0F0F0F0F);
        //      uu = (uu & 0x00FF00FF00FF00FF) + ((uu >> 8) & 0x00FF00FF00FF00FF);
        //      uu = (uu & 0x0000FFFF0000FFFF) + ((uu >> 16) & 0x0000FFFF0000FFFF);
        //      return (int)((uint)uu + (uint)(uu >> 32));
        //    }

        //[DebuggerNonUserCode]
        public static int Log2(int number) {
            if (number < 0) {
                throw new ArgumentOutOfRangeException(nameof(number), "The logarithm of a negative number is imaginary.");
            }

            return Log2(unchecked((uint)number));
        }

        //[DebuggerNonUserCode]
        public static int Log2(long number) {
            if (number < 0) {
                throw new ArgumentOutOfRangeException(nameof(number), "The logarithm of a negative number is imaginary.");
            }

            return Log2(unchecked((ulong)number));
        }

        [CLSCompliant(false)]
        //[DebuggerNonUserCode]
        public static int Log2(uint number) {
            if (number == 0) {
                throw new ArgumentOutOfRangeException(nameof(number), "The logarithm of zero is not defined.");
            }

            number |= number >> 1;
            number |= number >> 2;
            number |= number >> 4;
            number |= number >> 8;
            number |= number >> 16;
            return PopulationCount(number >> 1);
        }

        [CLSCompliant(false)]
        //[DebuggerNonUserCode]
        public static int Log2(ulong number) {
            if (number == 0) {
                throw new ArgumentOutOfRangeException(nameof(number), "The logarithm of zero is not defined.");
            }

            number |= number >> 1;
            number |= number >> 2;
            number |= number >> 4;
            number |= number >> 8;
            number |= number >> 16;
            number |= number >> 32;
            return PopulationCount(number >> 1);
        }
        public static int PopulationCount(int value) {
            unchecked {
                return PopulationCount((uint)value);
            }
        }

        public static int PopulationCount(long value) {
            unchecked {
                return PopulationCount((ulong)value);
            }
        }

        // Gem from Hacker's Delight
        // Returns the number of bits set in @value
        [CLSCompliant(false)]
        public static int PopulationCount(uint value) {
            value -= (value >> 1) & 0x55555555;
            value = (value & 0x33333333) + ((value >> 2) & 0x33333333);
            value = (value + (value >> 4)) & 0x0F0F0F0F;
            value += value >> 8;
            value += value >> 16;
            return (int)(value & 0x0000003F);
        }

        // Based on code by Zilong Tan on Ulib released under MIT license
        // Returns the number of bits set in @x
        [CLSCompliant(false)]
        public static int PopulationCount(ulong value) {
            value -= (value >> 1) & 0x5555555555555555UL;
            value = (value & 0x3333333333333333UL) + ((value >> 2) & 0x3333333333333333UL);
            value = (value + (value >> 4)) & 0x0f0f0f0f0f0f0f0fUL;
            return (int)((value * 0x0101010101010101UL) >> 56);
        }

        public static int BinaryReverse( int value) {
            unchecked {
                return (int)BinaryReverse((uint)value);
            }
        }

        public static long BinaryReverse( long value) {
            unchecked {
                return (long)BinaryReverse((ulong)value);
            }
        }

        [CLSCompliant(false)]
        public static uint BinaryReverse( uint value) {
            value = ((value & 0xaaaaaaaa) >> 1) | ((value & 0x55555555) << 1);
            value = ((value & 0xcccccccc) >> 2) | ((value & 0x33333333) << 2);
            value = ((value & 0xf0f0f0f0) >> 4) | ((value & 0x0f0f0f0f) << 4);
            value = ((value & 0xff00ff00) >> 8) | ((value & 0x00ff00ff) << 8);
            return (value >> 16) | (value << 16);
        }

        [CLSCompliant(false)]
        public static ulong BinaryReverse( ulong value) {
            GetParts(value, out var lo, out var hi);
            return BuildUInt64(BinaryReverse(lo), BinaryReverse(hi));
        }

        [CLSCompliant(false)]
        public static ulong BuildUInt64(uint hi, uint lo) {
            return ((ulong)hi << 32) | lo;
        }
        public static int SingleAsInt32(float value) {
            return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
        }

        [CLSCompliant(false)]
        public static uint SingleAsUInt32(float value) {
            unchecked {
                return (uint)BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            }
        }
        #region
      

        public static void GetParts(double value, out int sign, out long mantissa, out int exponent, out bool finite) {
            GetDoubleParts(value, out sign, out exponent, out var tmpMantissa, out finite);
            mantissa = (long)tmpMantissa;
        }

        public static void GetParts(float value, out int sign, out int mantissa, out int exponent, out bool finite) {
            if (value.CompareTo(0.0f) == 0) {
                sign = 0;
                mantissa = 0;
                exponent = 1;
                finite = true;
            }
            else {
                var bits = SingleAsInt32(value);
                sign = bits < 0 ? -1 : 1;
                exponent = (bits >> 23) & 0xff;
                if (exponent == 2047) {
                    finite = false;
                    mantissa = 0;
                }
                else {
                    finite = true;
                    mantissa = bits & 0xffffff;
                    if (exponent == 0) {
                        // Subnormal numbers; exponent is effectively one higher,
                        // but there's no extra normalization bit in the mantissa
                        exponent = 1;
                    }
                    else {
                        // Normal numbers; leave exponent as it is but add extra
                        // bit to the front of the mantissa
                        mantissa |= 1 << 23;
                    }

                    // Bias the exponent. It's actually biased by 127, but we're
                    // treating the mantissa as m.0 rather than 0.m, so we need
                    // to subtract another 23 from it.
                    exponent -= 150;
                    if (mantissa == 0) {
                        return;
                    }

                    while ((mantissa & 1) == 0) {
                        mantissa >>= 1;
                        exponent++;
                    }
                }
            }
        }

        public static void GetParts(int value, out short lo, out short hi) {
            unchecked {
                lo = (short)value;
                hi = (short)(value >> 16);
            }
        }

        public static void GetParts(long value, out int lo, out int hi) {
            unchecked {
                lo = (int)value;
                hi = (int)((ulong)value >> 32);
            }
        }

        [CLSCompliant(false)]
        public static void GetParts(uint value, out ushort lo, out ushort hi) {
            unchecked {
                lo = (ushort)value;
                hi = (ushort)(value >> 16);
            }
        }

        [CLSCompliant(false)]
        public static void GetParts(ulong value, out uint lo, out uint hi) {
            unchecked {
                lo = (uint)value;
                hi = (uint)(value >> 32);
            }
        }
        #endregion
    }
}
