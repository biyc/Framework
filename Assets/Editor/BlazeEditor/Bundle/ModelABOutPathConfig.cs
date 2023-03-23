using System.Collections.Generic;
using Blaze.Ci;
using Blaze.Common;
using Blaze.Utility.Helper;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blaze.Bundle
{
    public class ModelABBuildConfig
    {
        /// <summary>
        /// 模型ab包输出路径
        /// </summary>
        public string ExtraOutPath;

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

    public class ModelABOutPathConfig : ScriptableObject
    {
        /// <summary>
        /// 模型ab包输出路径
        /// </summary>
        public List<string> OutPaths;

        public static string GetFullPath => "Assets/Configs/ModelABOutPathConfig.asset";

        public static ModelABOutPathConfig GetModelConfig()
        {
            var config =
                AssetDatabase.LoadAssetAtPath<ModelABOutPathConfig>("Assets/Configs/ModelABOutPathConfig.asset");
            if (config == null)
            {
                config = CreateInstance<ModelABOutPathConfig>();
                config.OutPaths = new List<string>();
                AssetDatabase.CreateAsset(config, GetFullPath);
            }

            return config;
        }
    }
}