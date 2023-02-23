using System.Collections.Generic;
using Blaze.Common;
using Blaze.Core;

namespace Model.Module.Blaze.Resource.AssetBundles.Data
{
    /// <summary>
    /// 本地热更记录
    /// </summary>
    [Named("LocalHotfixVersion")]
    [Persist(CamelFileName = true)]
    public class LocalHotfixVersion : Persistable<LocalHotfixVersion>
    {

        /// 本地 StreamAsset 中携带的资源版本
        public VersionInfo BaseVersion = new VersionInfo();

        /// 可用版本
        public VersionInfo AbleVersion = new VersionInfo();

        /// 正在下载中的版本
        public VersionInfo DownVersion = new VersionInfo();

        /// 历史版本
        public List<string> HistoryVersion = new List<string>();
    }
}