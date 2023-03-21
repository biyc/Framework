using Blaze.Ci;
using Blaze.Common;
using Blaze.Utility.Helper;
using UnityEditor;
using UnityEngine;

namespace Blaze.Bundle
{
    public class ModelABBuildConfig
    {
        /// <summary>
        /// 模型ab包输出路径
        /// </summary>
        public string OutPath;

        /// <summary>
        /// 模型的名字
        /// </summary>
        public string Name;

        /// <summary>
        /// 打包的类型
        /// </summary>
        public EnumPackageType PackageType;

        /// <summary>
        /// 打包平台
        /// </summary>
        public EnumRuntimeTarget RuntimeTarget;
    }

    public class ModelABConfig : ScriptableObject
    {
        /// <summary>
        /// 模型ab包输出路径
        /// </summary>
        public string OutPath;

        public static string GetFullPath => "Assets/Configs/ModelABConfig.asset";

        public static string GetDefaultPath(EnumPackageType packageType, EnumRuntimeTarget targetPlatform,
            string modelName)
        {
            var root = PathHelper.Combine("Publish", packageType.ToString(), targetPlatform.ToString(),
                "ModelBundles");
            return PathHelper.Combine(root, modelName);
        }

        public static ModelABConfig GetModelConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath<ModelABConfig>("Assets/Configs/ModelABConfig");
            if (config == null)
            {
                config = CreateInstance<ModelABConfig>();
                AssetDatabase.CreateAsset(config, GetFullPath);
            }

            return config;
        }
    }
}