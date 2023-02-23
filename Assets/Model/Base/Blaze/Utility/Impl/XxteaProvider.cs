//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | Tim    | 2019/10/18 | Initialize core skeleton |
*/

using System;
using System.Text;
using Blaze.Common;

namespace Blaze.Utility.Impl
{
    public sealed class XxteaProvider
    {
        private static readonly UTF8Encoding _utf8 = new UTF8Encoding();
        private static readonly UInt32 _delta = DefaultCrypto.XxteaSeed;
        private static readonly int _magic = DefaultCrypto.XxteaMXMagic;
        private static readonly byte[] _preMagic = _utf8.GetBytes(DefaultCrypto.XxteaPre);

        private static UInt32 MX(UInt32 sum, UInt32 y, UInt32 z, Int32 p, UInt32 e, UInt32[] k)
        {
            return (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & _magic ^ e] ^ z);
        }

        private XxteaProvider()
        {
        }

        public static Byte[] Encrypt(Byte[] data, Byte[] key)
        {
            if (data.Length == 0)
            {
                return data;
            }

            byte[] result = ToByteArray(Encrypt(ToUInt32Array(data, true), ToUInt32Array(FixKey(key), false)), false);
            result = Combine(_preMagic, result);
            return result;
        }

        public static Byte[] Encrypt(String data, Byte[] key)
        {
            return Encrypt(_utf8.GetBytes(data), key);
        }

        public static Byte[] Encrypt(Byte[] data, String key)
        {
            return Encrypt(data, _utf8.GetBytes(key));
        }

        public static Byte[] Encrypt(String data, String key)
        {
            return Encrypt(_utf8.GetBytes(data), _utf8.GetBytes(key));
        }

        public static String EncryptToBase64String(Byte[] data, Byte[] key)
        {
            return Convert.ToBase64String(Encrypt(data, key));
        }

        public static String EncryptToBase64String(String data, Byte[] key)
        {
            return Convert.ToBase64String(Encrypt(data, key));
        }

        public static String EncryptToBase64String(Byte[] data, String key)
        {
            return Convert.ToBase64String(Encrypt(data, key));
        }

        public static String EncryptToBase64String(String data, String key)
        {
            return Convert.ToBase64String(Encrypt(data, key));
        }

        public static Byte[] Decrypt(Byte[] data, Byte[] key)
        {
            if (data.Length == 0)
            {
                return data;
            }

            data = Subtract(data, _preMagic);

            return ToByteArray(Decrypt(ToUInt32Array(data, false), ToUInt32Array(FixKey(key), false)), true);
        }

        public static Byte[] Decrypt(Byte[] data, String key)
        {
            return Decrypt(data, _utf8.GetBytes(key));
        }

        public static Byte[] DecryptBase64String(String data, Byte[] key)
        {
            return Decrypt(Convert.FromBase64String(data), key);
        }

        public static Byte[] DecryptBase64String(String data, String key)
        {
            return Decrypt(Convert.FromBase64String(data), key);
        }

        public static String DecryptToString(Byte[] data, Byte[] key)
        {
            try
            {
                return _utf8.GetString(Decrypt(data, key));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static String DecryptToString(Byte[] data, String key)
        {
            try
            {
                return _utf8.GetString(Decrypt(data, key));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static String DecryptBase64StringToString(String data, Byte[] key)
        {
            try
            {
                return _utf8.GetString(DecryptBase64String(data, key));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static String DecryptBase64StringToString(String data, String key)
        {
            try
            {
                return _utf8.GetString(DecryptBase64String(data, key));
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static UInt32[] Encrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }

            UInt32 z = v[n], y, sum = 0, e;
            Int32 p, q = 6 + 52 / (n + 1);
            unchecked
            {
                while (0 < q--)
                {
                    sum += _delta;
                    e = sum >> 2 & 3;
                    for (p = 0; p < n; p++)
                    {
                        y = v[p + 1];
                        z = v[p] += MX(sum, y, z, p, e, k);
                    }

                    y = v[0];
                    z = v[n] += MX(sum, y, z, p, e, k);
                }
            }

            return v;
        }

        private static UInt32[] Decrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }

            UInt32 z, y = v[0], sum, e;
            Int32 p, q = 6 + 52 / (n + 1);
            unchecked
            {
                sum = (UInt32) (q * _delta);
                while (sum != 0)
                {
                    e = sum >> 2 & 3;
                    for (p = n; p > 0; p--)
                    {
                        z = v[p - 1];
                        y = v[p] -= MX(sum, y, z, p, e, k);
                    }

                    z = v[n];
                    y = v[0] -= MX(sum, y, z, p, e, k);
                    sum -= _delta;
                }
            }

            return v;
        }

        private static Byte[] FixKey(Byte[] key)
        {
            if (key.Length == 16) return key;
            Byte[] fixedkey = new Byte[16];
            if (key.Length < 16)
            {
                key.CopyTo(fixedkey, 0);
            }
            else
            {
                Array.Copy(key, 0, fixedkey, 0, 16);
            }

            return fixedkey;
        }

        private static UInt32[] ToUInt32Array(Byte[] data, Boolean includeLength)
        {
            Int32 length = data.Length;
            Int32 n = (((length & 3) == 0) ? (length >> 2) : ((length >> 2) + 1));
            UInt32[] result;
            if (includeLength)
            {
                result = new UInt32[n + 1];
                result[n] = (UInt32) length;
            }
            else
            {
                result = new UInt32[n];
            }

            for (Int32 i = 0; i < length; i++)
            {
                result[i >> 2] |= (UInt32) data[i] << ((i & 3) << 3);
            }

            return result;
        }

        private static Byte[] ToByteArray(UInt32[] data, Boolean includeLength)
        {
            Int32 n = data.Length << 2;
            if (includeLength)
            {
                Int32 m = (Int32) data[data.Length - 1];
                n -= 4;
                if ((m < n - 3) || (m > n))
                {
                    return null;
                }

                n = m;
            }

            Byte[] result = new Byte[n];
            for (Int32 i = 0; i < n; i++)
            {
                result[i] = (Byte) (data[i >> 2] >> ((i & 3) << 3));
            }

            return result;
        }

        /// <summary>
        /// 合并字节数组
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static Byte[] Combine(Byte[] prefix, Byte[] source)
        {
            byte[] result = new byte[prefix.Length + source.Length];
            Buffer.BlockCopy(prefix, 0, result, 0, prefix.Length);
            Buffer.BlockCopy(source, 0, result, prefix.Length, source.Length);
            return result;
        }

        /// <summary>
        /// 减除字节数组
        /// </summary>
        /// <param name="source"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private static Byte[] Subtract(Byte[] source, Byte[] prefix)
        {
            byte[] result = new byte[source.Length - prefix.Length];
            Buffer.BlockCopy(source, prefix.Length, result, 0, source.Length - prefix.Length);
            return result;
        }

        /// <summary>
        /// 检查字节流是否是xxtea加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool CheckXxtea(byte[] source)
        {
            for (int i = _preMagic.Length - 1; i >= 0; i--)
            {
                if (source[i] != _preMagic[i]) return false;
            }

            return true;
        }
    }
}