using UnityEngine;

namespace Blaze.Common
{
    public class CommonSetting : ScriptableObject
    {
        /// <summary>
        /// 获取设置
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static CommonSetting GetSetting()
        {
            return Resources.Load<CommonSetting>(AssetLocal());
        }


        /// <summary>
        /// 获取指定平台的文件路径
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetFullPath()
        {
            return $"Assets/Resources/{AssetLocal()}.asset";
        }

        private static string AssetLocal()
        {
            return $"Settings/CommonSetting";
        }


        [SerializeField]
        // 游戏要启动的设置文件名称
        public string GameSetting;
    }
}