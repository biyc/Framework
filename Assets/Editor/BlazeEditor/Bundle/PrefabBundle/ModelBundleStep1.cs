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

        public List<EnumRuntimeTarget> TargetPlatform => _modelAbBuildConfig.RuntimeTarget;

        public Dictionary<EnumRuntimeTarget, ManifestInfo> ManifestInfos;

        private ModelABBuildConfig _modelAbBuildConfig;


        public void Execution(ModelABBuildConfig modelAbBuildConfig)
        {
            ManifestInfos = new Dictionary<EnumRuntimeTarget, ManifestInfo>();
            _modelAbBuildConfig = modelAbBuildConfig;
            PUBLISH_ROOT_PATH = PathHelper.Combine("Publish", "ModelBundles");
            Start();
        }

        void Start()
        {
            _modelAbBuildConfig.RuntimeTarget.ForEach(m =>
            {
                if (!ManifestInfos.ContainsKey(m))
                    ManifestInfos.Add(m, new ManifestInfo());

                //Debug.LogError(PUBLISH_ROOT_PATH);
                if (Directory.Exists(GetPublishPath(m)))
                    Directory.Delete(GetPublishPath(m));
                if (!String.IsNullOrEmpty(GetExtraOutPath(m)))
                {
                    if (Directory.Exists(GetExtraOutPath(m)))
                        Directory.Delete(GetExtraOutPath(m));
                    PathHelper.CheckOrCreate(GetExtraOutPath(m));
                }

                PathHelper.CheckOrCreate(GetPublishPath(m));
                PathHelper.CheckOrCreate(GetCachePath(m));
            });
        }

        public ManifestInfo GetManifestInfo(EnumRuntimeTarget target) => ManifestInfos[target];

        /// <summary>
        /// 获取原始ab包的路径
        /// </summary>
        /// <returns></returns>
        public string GetCachePath(EnumRuntimeTarget target) => PathHelper.Combine(PUBLISH_ROOT_PATH, target.ToString(),
            "_Cache", _modelAbBuildConfig.Name);

        /// <summary>
        /// 发布路径  Publish+ 包的类型+目标平台
        /// </summary>
        public string GetPublishPath(EnumRuntimeTarget target) =>
            PathHelper.Combine(PUBLISH_ROOT_PATH, target.ToString(), _modelAbBuildConfig.Name);

        public string GetExtraOutPath(EnumRuntimeTarget target) =>
            String.IsNullOrEmpty(_modelAbBuildConfig.ExtraOutPath)
                ? String.Empty
                : PathHelper.Combine(_modelAbBuildConfig.ExtraOutPath, target.ToString(), _modelAbBuildConfig.Name);
    }
}