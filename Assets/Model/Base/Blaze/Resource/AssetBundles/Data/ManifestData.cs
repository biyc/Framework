using System;
using System.Collections.Generic;
using Blaze.Common;
using Blaze.Core;
using Blaze.Utility.Helper;
using UnityEngine.Serialization;

namespace Blaze.Resource.AssetBundles.Data
{
    public enum BundleType
    {
        AssetBundle,
        Zip,
    }

    [Serializable]
    public class ManifestData
    {
        /// <summary>
        /// 资源路径(在整个资源系统中的路径)
        /// </summary>
        /// <returns></returns>
        public string AssetPath;

        /// 资源包类型
        public BundleType Type;

        /// 资源版本
        public int Version;

        /// 资源所属ab包名
        public string ABName;

        /// 网络上下载到的HASH文件名
        public string Hash;

        /// 大小
        public long Size;

        /// 文件MD5
        public string Md5;

        /// CRC校验
        public string Checksum;

        /// 依赖的源文件组
        public List<string> Dependencies;


        // 检查并创建子目录
        public void CheckSubPath(string bathPath)
        {
            var subPath = Hash.Length >= 2 ? Hash.Substring(0, 2) : "sort";
            PathHelper.CheckOrCreate(PathHelper.Combine(bathPath, subPath));
        }

        // 获取下载到基础目录后的二级目录全部索引
        public string GetSaveSubPath()
        {
            return PathHelper.Combine(Hash.Length >= 2 ? Hash.Substring(0, 2) : "sort", Hash);
        }
    }

    /// <summary>
    /// 核心装配文件
    /// </summary>
    [Named("Bundle Manifest")]
    [Persist(CamelFileName = true)]
    public class ManifestInfo : Persistable<ManifestInfo>
    {
        /// <summary>
        /// 当前配置文件版本
        /// </summary>
        public VersionInfo Version = new VersionInfo();

        public List<ManifestData> ManifestList = new List<ManifestData>();
    }
}