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

        /// <summary>
        /// 包的类型
        /// </summary>
        private EnumPackageType PackageType;

        /// <summary>
        /// 目标平台
        /// </summary>
        private EnumRuntimeTarget _targetPlatform;

        private string _name;

        public string Name => _name;

        public EnumRuntimeTarget TargetPlatform => _targetPlatform;

        private ManifestInfo _manifestInfo;
        public ManifestInfo ManifestInfo => _manifestInfo;

        public void Execution(EnumPackageType packageType, EnumRuntimeTarget targetPlatform, string name)
        {
            _manifestInfo = new ManifestInfo();
            _name = name;
            _targetPlatform = targetPlatform;
            PackageType = packageType;
            PUBLISH_ROOT_PATH = PathHelper.Combine("Publish", packageType.ToString(), targetPlatform.ToString(),
                "ModelBundles");
            Start();
        }

        void Start()
        {
            //Debug.LogError(PUBLISH_ROOT_PATH);
            if (Directory.Exists(GetPublishPath()))
                Directory.Delete(GetPublishPath());
            PathHelper.CheckOrCreate(GetPublishPath());
            PathHelper.CheckOrCreate(GetCachePath());
        }

        /// <summary>
        /// 获取原始ab包的路径
        /// </summary>
        /// <returns></returns>
        public string GetCachePath() => PathHelper.Combine(PUBLISH_ROOT_PATH, "_Cache", _name);

        /// <summary>
        /// 发布路径  Publish+ 包的类型+目标平台
        /// </summary>
        public string GetPublishPath() => PathHelper.Combine(PUBLISH_ROOT_PATH, _name);
    }
}