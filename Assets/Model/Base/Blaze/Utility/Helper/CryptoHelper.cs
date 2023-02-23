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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Blaze.Common;
using Blaze.Utility.Extend;
using Blaze.Utility.Impl;

namespace Blaze.Utility.Helper
{
    /// <summary>
    /// 包装好的加密相关的算法/ MD5 / RSA(待完成) / XXTEA / 内存混淆
    /// </summary>
    public static class CryptoHelper
    {
        #region MD5

        public static string FileMD5(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    int len = (int) fs.Length;
                    byte[] bytes = new byte[len];
                    fs.Read(bytes, 0, len);
                    fs.Close();
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] result = md5.ComputeHash(bytes);
                    string fileMD5 = "";
                    foreach (byte b in result)
                    {
                        fileMD5 += Convert.ToString(b, 16);
                    }

                    return fileMD5;
                }
            }
            catch (FileNotFoundException e)
            {
                return null;
            }
        }

        /// <summary>
        /// MD5的全局句柄
        /// </summary>
        private static readonly MD5 MD5Provider = MD5.Create();

        /// <summary>
        /// 获取Md5值,每次Md5的加权已经加了Salt,外部无法验证通过
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="hasSalt">是否进行加密化处理</param>
        /// <param name="preSalt">前导盐</param>
        /// <param name="postSalt">后置盐</param>
        /// <returns>输出</returns>
        public static string MD5Encode(string input,
            bool hasSalt = false,
            string preSalt = null,
            string postSalt = null)
        {
            byte[] data;

            if (hasSalt)
            {
                data = MD5Provider.ComputeHash(
                    Encoding.UTF8.GetBytes(preSalt.Default(DefaultCrypto.Md5SaltPre) + input +
                                           postSalt.Default(DefaultCrypto.Md5SaltPost)));
            }
            else
            {
                data = MD5Provider.ComputeHash(Encoding.UTF8.GetBytes(input));
            }

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// 验证Md5的加盐Salt哈希
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="md5">需要验证的Md5串</param>
        /// <returns></returns>
        public static bool MD5Verify(string input,
            string md5,
            bool hasSalt = false,
            string preSalt = null,
            string postSalt = null)
        {
            string hashOfInput = MD5Encode(input, hasSalt, preSalt, postSalt);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, md5))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region XXTEA

        /// <summary>
        /// XXTEA加密
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>字节数组</returns>
        public static Byte[] XxteaEncryptToByte(string input, string salt = null)
        {
            return XxteaProvider.Encrypt(input, salt.Default(DefaultCrypto.XxteaSalt));
        }

        /// <summary>
        /// XXTEA加密
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>Base64字符串</returns>
        public static string XxteaEncryptToString(string input, string salt = null)
        {
            return XxteaProvider.EncryptToBase64String(input, salt.Default(DefaultCrypto.XxteaSalt));
        }

        /// <summary>
        /// XXTEA解密
        /// </summary>
        /// <param name="input">字节数组</param>
        /// <returns>字符串</returns>
        public static string XxteaDecryptByByte(Byte[] input, string salt = null)
        {
            return XxteaProvider.DecryptToString(input, salt.Default(DefaultCrypto.XxteaSalt));
        }

        /// <summary>
        /// XXTEA解密
        /// </summary>
        /// <param name="input">Base64字符串</param>
        /// <returns>字符串</returns>
        public static string XxteaDecryptByString(string input, string salt = null)
        {
            return XxteaProvider.DecryptBase64StringToString(input, salt.Default(DefaultCrypto.XxteaSalt));
        }

        #endregion

        #region BASE64

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="plainText">字符串</param>
        /// <returns>字符串</returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="base64EncodedData">字符串</param>
        /// <returns>字符串</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        #endregion

        #region SHORTID

        /// <summary>
        /// 短编码生成器
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ShortId(string input, bool fix = false)
        {
            Random rd = new Random();
//            string key = "TheBestD@nopo";
            string[] chars =
            {
                "a", "b", "c", "d", "e", "f", "g", "h",
                "i", "j", "k", "l", "m", "n", "o", "p",
                "q", "r", "s", "t", "u", "v", "w", "x",
                "y", "z", "0", "1", "2", "3", "4", "5",
                "6", "7", "8", "9", "A", "B", "C", "D",
                "E", "F", "G", "H", "I", "J", "K", "L",
                "M", "N", "O", "P", "Q", "R", "S", "T",
                "U", "V", "W", "X", "Y", "Z"
            };
            string hex = MD5Encode(input);
            //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
            int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(rd.Next(0, 4) * 8, 8), 16);
            if (fix) hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(8, 8), 16);
            string outChars = string.Empty;
            for (int j = 0; j < 6; j++)
            {
                //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                int index = 0x0000003D & hexint;
                //把取得的字符相加
                outChars += chars[index];
                //每次循环按位右移5位
                hexint = hexint >> 5;
            }

            return outChars;
        }

        #endregion

        #region MEMCRPT

        /// <summary>
        /// 循环移位
        /// </summary>
        /// <param name="val">输入的数字</param>
        /// <param name="iShiftBit">要移几位</param>
        /// <param name="isLeft">移位的方向</param>
        private static uint CycleShift(uint val, int iShiftBit, bool isLeft)
        {
            uint temp = 0;
            uint result = 0;
            temp |= val;
            if (isLeft)
            {
                val <<= iShiftBit;
                temp >>= (32 - iShiftBit);
                result = val | temp;
            }
            else
            {
                val >>= iShiftBit;
                temp <<= (32 - iShiftBit);
                result = val | temp;
            }

            return result;
        }

        /// <summary>
        /// 获取一个加密后的值
        /// </summary>
        /// <param name="orignal"></param>
        /// <returns></returns>
        public static uint MemoryEncrypt(uint orignal)
        {
            return CycleShift(orignal, DefaultCrypto.MomoryShift, true) ^ DefaultCrypto.MemorySalt;
        }

        /// <summary>
        /// 获取一个解密后的值
        /// </summary>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public static uint MemoryDecrypt(uint encrypt)
        {
            return CycleShift(encrypt ^ DefaultCrypto.MemorySalt, DefaultCrypto.MomoryShift, false);
        }

        /// <summary>
        /// 内存直接设置数字并加密
        /// </summary>
        /// <param name="encrypt"></param>
        /// <param name="value"></param>
        public static void MemorySet(ref uint encrypt, uint value)
        {
            encrypt = MemoryEncrypt(value);
        }

        /// <summary>
        /// 对内存中的数字进行加操作
        /// </summary>
        /// <param name="encrypt"></param>
        /// <param name="number"></param>
        public static void MemoryPlus(ref uint encrypt, uint number)
        {
            encrypt = MemoryEncrypt(MemoryDecrypt(encrypt) + (uint) number);
        }

        /// <summary>
        /// 对内存的数字进行+1操作
        /// </summary>
        /// <param name="encrypt"></param>
        public static void MemoryIncrease(ref uint encrypt)
        {
            encrypt = MemoryEncrypt(MemoryDecrypt(encrypt) + 1);
        }

        /// <summary>
        /// 对内存中的数字进行减操作
        /// </summary>
        /// <param name="encrypt"></param>
        /// <param name="number"></param>
        public static void MemoryMinus(ref uint encrypt, uint number)
        {
            encrypt = MemoryEncrypt(MemoryDecrypt(encrypt) - (uint) number);
        }

        /// <summary>
        /// 对内存中的数字进行-1操作
        /// </summary>
        /// <param name="encrypt"></param>
        public static void MemoryDecrease(ref uint encrypt)
        {
            encrypt = MemoryEncrypt(MemoryDecrypt(encrypt) - 1);
        }

        #endregion

        #region AES128CTR

        private static AesProvider AesCryptor =
            new AesProvider(
                Encoding.ASCII.GetBytes(DefaultCrypto.AES128Key),
                Encoding.ASCII.GetBytes(DefaultCrypto.AES128IV));

        /// <summary>
        /// 对内容进行加密,因为是核心加密不再提供string的获取方法
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static byte[] EncryptByByte(byte[] Content)
        {
            return AesCryptor.Encrypt(Content);
        }

        /// <summary>
        /// 对内容进行解密,因为是核心加密不再提供string的获取方法
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static byte[] DecryptByByte(byte[] Content)
        {
            return AesCryptor.Decrypt(Content);
        }

        #endregion
    }
}