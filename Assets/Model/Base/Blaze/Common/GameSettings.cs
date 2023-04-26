using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blaze.Common
{
    public class GameSettings : ScriptableObject
    {
        /// <summary>
        /// 获取设置
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static GameSettings GetSetting(string target)
        {
            return Resources.Load<GameSettings>($"Settings/{target}");
        }


        /// <summary>
        /// 获取指定平台的文件路径
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetFullPath(string target)
        {
            return $"Assets/Resources/Settings/{target}.asset";
        }


        [SerializeField]
        // 资源服务器地址
        public List<string> ResServerList;

        [SerializeField]
        // 渠道
        public string Channel;

        [SerializeField]
        // 母包版本
        public int MotherVersion;

        [SerializeField]
        // 母包修订版本
        public int FixVersion;

        [SerializeField]
        // 母包构建次数
        public int BuildNum;

        [SerializeField]
        // 是否强制检查版本
        public bool IsForceCheckVersion;

        [SerializeField]
        // 是否使用ILRuntime
        public bool UseILRuntime;

        [SerializeField]
        // 是否开启DEV面板
        public bool UseDev;

        [SerializeField]
        // 是否使用AssetBundle
        public bool UseAssetBundle;


        [SerializeField]
        // 母包中是否包含AB包
        public bool IsContentAssetBundle;


        [SerializeField]
        // 应用名称
        public string ProductName;

        [SerializeField]
        //是否导出安卓包
        public bool IsExportProject;


        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns>AndroidDev_1.0.1</returns>
        public string GetVersion()
        {
            return $"{Channel}_{MotherVersion}.{FixVersion}.{BuildNum}";
        }

        public string GetVersionIos()
        {
            return $"{MotherVersion}.{FixVersion}.{BuildNum}";
        }
    }
}