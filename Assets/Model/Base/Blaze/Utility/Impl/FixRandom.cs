//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/09 | Initialize core skeleton |
*/

namespace Blaze.Utility.Impl
{
    /// <summary>
    /// 梅森螺旋随机数实现
    /// </summary>
    public class FixRandom
    {
        private const ulong N = 312;
        private const ulong M = 156;
        private const ulong MATRIX_A = 0xB5026F5AA96619E9L;
        private const ulong UPPER_MASK = 0xFFFFFFFF80000000;
        private const ulong LOWER_MASK = 0X7FFFFFFFUL;
        private static ulong[] mt = new ulong[N + 1];
        private static ulong mti = N + 1;

        /// <summary>
        /// 构造函数,可以直接指定随机种子
        /// </summary>
        /// <param name="seed">随机种子的起点</param>
        public FixRandom(ulong seed)
        {
            Seed(seed);
        }

        /// <summary>
        /// 设置随机种子
        /// </summary>
        /// <param name="seed">随机种子的起点</param>
        public void Seed(ulong seed)
        {
            mt[0] = seed;
            for (mti = 1; mti < N; mti++)
            {
                mt[mti] = 6364136223846793005L * (mt[mti - 1] ^ (mt[mti - 1] >> 62)) + mti;
            }
        }

        /// <summary>
        /// 基础实现,下一个无符号长整形
        /// </summary>
        /// <returns>无符号长整形随机数0x0FFFFFFFFFFFFFFF</returns>
        public ulong NextLong()
        {
            ulong x = 0;
            ulong[] mag01 = new ulong[2] {0x0UL, MATRIX_A};

            if (mti >= N)
            {
                ulong kk;
                if (mti == N + 1)
                {
                    Seed(5489UL);
                }

                for (kk = 0; kk < (N - M); kk++)
                {
                    x = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (x >> 1) ^ mag01[x & 0x1UL];
                }

                for (; kk < N - 1; kk++)
                {
                    x = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk - M] ^ (x >> 1) ^ mag01[x & 0x1UL];
                }

                x = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (x >> 1) ^ mag01[x & 0x1UL];

                mti = 0;
            }

            x = mt[mti++];
            x ^= (x >> 29) & 0x5555555555555555L;
            x ^= (x << 17) & 0x71D67FFFEDA60000L;
            x ^= (x << 37) & 0xFFF7EEE000000000L;
            x ^= (x >> 43);
            return x;
        }

        /// <summary>
        /// 下一个长整形
        /// </summary>
        /// <param name="value">最大范围</param>
        /// <returns>无符号长整形随机数</returns>
        public ulong NextLong(ulong value)
        {
            return NextLong() % value;
        }

        /// <summary>
        /// 下一个范围长整形
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>无符号长整形随机数</returns>
        public ulong NextLong(ulong min, ulong max)
        {
            return min + NextLong() % (max - min);
        }

        /// <summary>
        /// 下一个整形
        /// </summary>
        /// <returns>无符号长整形随机数0x0FFFFFFF</returns>
        public uint NextInt()
        {
            return (uint) NextLong();
        }

        /// <summary>
        /// 下一个整形
        /// </summary>
        /// <param name="value">最大值</param>
        /// <returns>无符号整形随机数</returns>
        public uint NextInt(uint value)
        {
            return (uint) NextLong() % value;
        }

        /// <summary>
        /// 下一个范围整形
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>无符号整形随机数</returns>
        public uint NextInt(uint min, uint max)
        {
            return min + (uint) NextLong() % (max - min);
        }
    }
}