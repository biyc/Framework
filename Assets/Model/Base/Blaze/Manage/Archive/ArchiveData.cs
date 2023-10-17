using System;
using System.Collections.Generic;
using System.IO;
using Blaze.Utility.Helper;
using Newtonsoft.Json;
using UnityEngine;

namespace Model.Base.Blaze.Manage.Archive
{
    /// <summary>
    /// 底层存档数据结构
    /// </summary>
    public class ArchiveData
    {
        /// 存档创建日期
        public long CreateDate;

        /// 存档版本 (最后时间戳)
        public long SlotVersion;

        /// 存档名称
        public string ArchiveName;

        /// <summary>
        /// 存档数据
        /// </summary>
        public Dictionary<string, string> Storage;

        [JsonIgnore] private bool _isLoad;

        /// <summary>
        /// 获取存储路径
        /// </summary>
        /// <returns></returns>
        public static string GetSavePath(string slotName)
        {
            PathHelper.CheckOrCreate(PathHelper.Combine(PathHelper.GetPersistentPath(), "slot"));
            return PathHelper.Combine(PathHelper.GetPersistentPath(), "slot", $"{slotName}.json");
        }

        /// <summary>
        /// 创建全新存档
        /// </summary>
        /// <returns></returns>
        public static ArchiveData Create(string archiveName)
        {
            var archiveData = new ArchiveData();
            archiveData.CreateDate = TimeNow();
            archiveData.SlotVersion = TimeNow();
            archiveData.ArchiveName = archiveName;
            archiveData.Storage = new Dictionary<string, string>();
            archiveData._isLoad = true;
            return archiveData;
        }


        /// <summary>
        /// 根据存档名称，从本地加载存档
        /// </summary>
        /// <param name="slotName"></param>
        /// <returns></returns>
        public static ArchiveData LoadLocal(string slotName)
        {
            var path = GetSavePath(slotName);
            if (!File.Exists(path)) return null;
            try
            {
                var content = File.ReadAllText(path);
                var decodeContent = CryptoHelper.XxteaDecryptByString(content);
                // 存档信息读取到当前存档节点
                var data = JsonConvert.DeserializeObject<ArchiveData>(decodeContent);
                data._isLoad = true;
                return data;
            }
            catch (Exception e)
            {
                // 存档解析失败，备份并重新创建存档
                if (File.Exists(path))
                {
                    try
                    {
                        File.Move(path, path + "_pre.bk");
                    }
                    catch (Exception exception)
                    {
                    }
                }
            }

            return null;
        }
        
        /// <summary>
        /// 保存一个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Save(string key, string val)
        {
            // Load();
            Storage[key] = val;
            Save();
        }

        /// <summary>
        /// 删除对应存档
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Del(string key)
        {
            if (Storage.ContainsKey(key))
            {
                Storage.Remove(key);
                Save();
            }
        }

        /// <summary>
        /// 获取一个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            if (Storage.ContainsKey(key))
                return Storage[key];
            return null;
        }


        // 保存存档信息到文件中
        public void Save()
        {
            // 更新存档版本
            SlotVersion = TimeNow();
            var path = GetSavePath(ArchiveName);
            if (!File.Exists(path))
                File.Create(path).Dispose();
            var jsonContent = ReadOrigin();
            File.WriteAllText(path, CryptoHelper.XxteaEncryptToString(jsonContent));
            // File.WriteAllText(path, jsonContent);
        }
        private static long TimeNow()
        {
            long time = TimeHelper.ClientNow();
            try
            {
                time = TimeHelper.CurrentMillis();
            }
            catch (Exception e)
            {
                time = TimeHelper.ClientNow();
            }
            return time;
        }


        /// <summary>
        /// 读取存档源JSON
        /// </summary>
        /// <returns></returns>
        public string ReadOrigin()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}