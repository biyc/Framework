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
using Blaze.Utility.Helper;
using UnityEngine;

namespace Blaze.Utility.Base
{
    /// <summary>
    /// 用户数据存取工具类,默认都将加密存储
    /// （windows 存储在注册表中，mac 存储在plist文件中，不推荐使用）
    /// </summary>
    public static class Local
    {
        /// <summary>
        /// 保存一个浮点数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void Save(string key, float val)
        {
            PlayerPrefs.SetString(key, CryptoHelper.XxteaEncryptToString(val.ToString()));
        }

        /// <summary>
        /// 保存一个整形
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void Save(string key, int val)
        {
            PlayerPrefs.SetString(key, CryptoHelper.XxteaEncryptToString(val.ToString()));
        }

        /// <summary>
        /// 保存一个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void Save(string key, string val)
        {
            PlayerPrefs.SetString(key, CryptoHelper.XxteaEncryptToString(val));
        }

        /// <summary>
        /// 删除对应存档
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void Del(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }


        /// <summary>
        /// 获取一个浮点数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static float Float(string key)
        {
            try
            {
                return float.Parse(CryptoHelper.XxteaDecryptByString(PlayerPrefs.GetString(key)));
            }
            catch (Exception)
            {
                Tuner.Warn("Decrypt Error!");
                return 0;
            }
        }

        /// <summary>
        /// 获取一个整形
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int Int(string key)
        {
            try
            {
                return int.Parse(CryptoHelper.XxteaDecryptByString(PlayerPrefs.GetString(key)));
            }
            catch (Exception)
            {
                Tuner.Warn("Decrypt Error!");
                return 0;
            }
        }

        /// <summary>
        /// 获取一个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string String(string key)
        {
            return CryptoHelper.XxteaDecryptByString(PlayerPrefs.GetString(key));
        }
    }
}