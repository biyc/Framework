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
using System.Collections.Generic;
using System.IO;
using Blaze.Utility.Helper;
using Newtonsoft.Json;

namespace Blaze.Utility.Base
{
    /// <summary>
    /// 用户存储区存档,默认都将加密存储
    /// </summary>
    public static class Storage
    {
        private static Dictionary<string, string> _storage;

        private static readonly string Path = PathHelper.Combine(PathHelper.GetPersistentPath(), "slots.json");

        /// <summary>
        /// 保存一个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void Save(string key, string val)
        {
            Load();
            _storage[key] = val;
            Save();
        }

        /// <summary>
        /// 删除对应存档
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void Del(string key)
        {
            if (_storage.ContainsKey(key))
            {
                _storage.Remove(key);
                Save();
            }
        }

        /// <summary>
        /// 获取一个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            Load();
            if (_storage.ContainsKey(key))
                return _storage[key];
            return null;
        }


        // 保存存档信息到文件中
        private static void Save()
        {
            var jsonContent = ConvertJsonString( JsonConvert.SerializeObject(_storage));
           // jsonContent = ConvertJsonString(jsonContent);
            if (!File.Exists(Path))
                File.Create(Path).Dispose();
            // File.WriteAllText(Path, CryptoHelper.XxteaEncryptToString(jsonContent));
            File.WriteAllText(Path, jsonContent);
        }

        // 检查是否已加载数据
        private static void Load()
        {
            if (_storage != null)
                return;

            if (_storage == null && File.Exists(Path))
            {
                try
                {
                    var content = File.ReadAllText(Path);
                    // var decodeContent = CryptoHelper.XxteaDecryptByString(content);
                    var decodeContent = content;
                    _storage = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodeContent);
                }
                catch (Exception e)
                {
                    // 存档解析失败，备份并重新创建存档
                    if (File.Exists(Path))
                        File.Move(Path, Path + ".bk");
                    _storage = null;
                }
            }

            if (_storage == null)
                _storage = new Dictionary<string, string>();
        }

        /// <summary>
        /// 存档是否存在
        /// google apple 游戏中心使用
        /// </summary>
        /// <returns></returns>
        public static bool IsExists() => File.Exists(Path);

        /// <summary>
        /// 从硬盘中读取源存档
        /// </summary>
        /// <returns></returns>
        public static string ReadOrigin() => File.ReadAllText(Path);

        /// <summary>
        /// 从写入源存档到硬盘
        /// google apple 游戏中心使用
        /// </summary>
        /// <returns></returns>
        public static void WriteOriginToFile(string origin)
        {
            if (!File.Exists(Path))
                File.Create(Path).Dispose();
            File.WriteAllText(Path, origin);
        }
        public static string ConvertJsonString(string str)
        {
            //格式化json字符串  
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }
    }

}