using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blaze.Utility;
using Blaze.Utility.Helper;
using Newtonsoft.Json;

namespace Model.Base.Blaze.Manage.Archive
{
    /// <summary>
    /// 帮助开发运维读取  Data/Dev/Archive 中存放的存档，方便数据分析
    /// </summary>
    public class DevArchive
    {
        #region Singleton

        private static readonly DevArchive _instance = new DevArchive();

        public static DevArchive _ => _instance;

        #endregion

        private readonly string _dirPath = PathHelper.Combine(PathHelper.GetCurrentPath(), "Data", "Dev", "Archive");


        // private readonly Dictionary<string, ArchiveData> _cache = new Dictionary<string, ArchiveData>();



        string GetSavePath(string slotName)
        {
            PathHelper.CheckOrCreate(_dirPath);
            return PathHelper.Combine(_dirPath, $"{slotName}.json");
        }

        /// <summary>
        /// 获取所有的存档名字
        /// </summary>
        /// <returns></returns>
        public List<string> GetArchiveNames()
        {
            PathHelper.CheckOrCreate(_dirPath);

            return new DirectoryInfo(_dirPath).GetFiles().ToList()
                .FindAll(info => info.Extension.Contains(".json"))
                .ConvertAll(input => input.Name);
        }

        // /// <summary>
        // /// 处理数据完毕后，移除缓存
        // /// </summary>
        // /// <param name="archiveName"></param>
        // public void RemoveCache(string archiveName)
        // {
        //     _cache.Remove(archiveName);
        // }


        /// <summary>
        /// 根据存档名称，从本地加载存档
        /// </summary>
        /// <param name="archiveName"></param>
        /// <returns></returns>
        public ArchiveData LoadLocal(string archiveName)
        {
            // if (_cache.ContainsKey(archiveName))
            // {
            //     return _cache[archiveName];
            // }
            var path = GetSavePath(archiveName);
            if (!File.Exists(path))
            {
                Tuner.Error("读取存档失败" + path);
                return null;
            }

            try
            {
                var content = File.ReadAllText(path);
                var decodeContent = CryptoHelper.XxteaDecryptByString(content);
                // 存档信息读取到当前存档节点
                var data= JsonConvert.DeserializeObject<ArchiveData>(decodeContent);
                // // 放入缓存方便后面的插槽直接读取
                // _cache[archiveName] = data;
                return data;
            }
            catch (Exception e)
            {
                Tuner.Error("解析存档失败" + archiveName);
            }

            return null;
        }
    }
}