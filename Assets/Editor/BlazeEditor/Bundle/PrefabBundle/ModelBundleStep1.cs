using System;
using System.Collections.Generic;
using Blaze.Ci;
using Blaze.Common;
using Blaze.Core;
using Blaze.Resource.AssetBundles.Data;
using Blaze.Utility.Helper;
using UnityEngine;
using UnityEngine.Windows;

namespace Blaze.Bundle.PrefabBundle
{
    public class ModelBundleStep1 : Singeton<ModelBundleStep1>
    {
        /// <summary>
        /// 发布路径
        /// </summary>
        private string PUBLISH_ROOT_PATH = "Publish";

        public string Name => _modelAbBuildConfig.Name;

        public EnumRuntimeTarget TargetPlatform => _modelAbBuildConfig.RuntimeTarget;

        private ManifestInfo _manifestInfo;
        public ManifestInfo ManifestInfo => _manifestInfo;

        private ModelABBuildConfig _modelAbBuildConfig;

        public void Execution(ModelABBuildConfig modelAbBuildConfig)
        {
            _manifestInfo = new ManifestInfo();
            _modelAbBuildConfig = modelAbBuildConfig;
            PUBLISH_ROOT_PATH = PathHelper.Combine("Publish", modelAbBuildConfig.PackageType.ToString(),
                modelAbBuildConfig.RuntimeTarget.ToString(),
                "ModelBundles");
            Start();
        }

        void Start()
        {
            //Debug.LogError(PUBLISH_ROOT_PATH);
            if (Directory.Exists(GetPublishPath()))
                Directory.Delete(GetPublishPath());
            if (!String.IsNullOrEmpty(GetExtraOutPath()))
            {
                if (Directory.Exists(GetExtraOutPath()))
                    Directory.Delete(GetExtraOutPath());
                PathHelper.CheckOrCreate(GetExtraOutPath());
            }

            PathHelper.CheckOrCreate(GetPublishPath());
            PathHelper.CheckOrCreate(GetCachePath());
        }

        /// <summary>
        /// 获取原始ab包的路径
        /// </summary>
        /// <returns></returns>
        public string GetCachePath() => PathHelper.Combine(PUBLISH_ROOT_PATH, "_Cache", _modelAbBuildConfig.Name);

        /// <summary>
        /// 发布路径  Publish+ 包的类型+目标平台
        /// </summary>
        public string GetPublishPath() => PathHelper.Combine(PUBLISH_ROOT_PATH, _modelAbBuildConfig.Name);

        public string GetExtraOutPath() => String.IsNullOrEmpty(_modelAbBuildConfig.ExtraOutPath)
            ? String.Empty
            : PathHelper.Combine(_modelAbBuildConfig.ExtraOutPath, _modelAbBuildConfig.Name);
    }
}