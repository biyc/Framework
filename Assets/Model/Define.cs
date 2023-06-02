using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blaze.Common;

namespace ETModel
{
    public static class Define
    {
        /// <summary>
        /// 上线后用来存放变更参数
        /// </summary>
        public static Dictionary<string, string> CommonConf = new Dictionary<string, string>();


        public static string deviceId;

        /// <summary>
        /// 下载buddle,Editor也用ab.
        /// </summary>
        public static bool UseAB = false;

        /// 是否为开发模式
        public static bool IsDev;

        /// <summary>
        /// 是否是export打包
        /// </summary>
        public static bool IsExportProject;

        /// <summary>
        /// 游戏配置
        /// </summary>
        public static GameSettings GameSettings;

        /// <summary>
        /// AB包版本
        /// </summary>
        public static string AssetBundleVersion;
    }
}