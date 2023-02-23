//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/18 | Initialize core skeleton |
*/

using System.Collections.Generic;
using Blaze.Common;
using Blaze.Core;
using Blaze.Resource.AssetBundles.Data;
using Blaze.Resource.Poco;
using Blaze.Utility.Helper;
using ManifestInfo = Blaze.Resource.AssetBundles.Data.ManifestInfo;

namespace Blaze.Bundle.Step
{
    /// <summary>
    /// 这是构建工具的核心工具类,减小主体程序的代码量
    /// </summary>
    public class BuilderStep1 : Singeton<BuilderStep1>
    {
        private BundleBuilderConf _builderConf;
        private const string CACHE_PATH = "_Cache";

        private string PUBLISH_PATH = "Publish/Default";
        private VersionCheck _curVersion;
        private ManifestInfo _manifestInfo = new ManifestInfo();

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <returns></returns>
        public BundleBuilderConf GetBundleBuilderConf()
        {
            return _builderConf;
        }

        public void Execution(BundleBuilderConf conf)
        {
            _builderConf = conf;
            PUBLISH_PATH = _builderConf.PUBLISH_PATH + "/" + _builderConf.Channel;
            // 重新生成装配文件
            _manifestInfo = new ManifestInfo();
            Start();
            CheckPathValid();
            CheckVersion();
        }


        void Start()
        {
            PathHelper.CheckOrCreate(GetPublishPath());
            // 加载上次的版本信息
            _curVersion = VersionCheck.Load(GetPublishPath());
            // 如果版本信息不存在，则创建全新版本
            if (null == _curVersion)
            {
                _curVersion = new VersionCheck();
                _curVersion.Config(GetPublishPath());
                _curVersion.Save();
            }
        }


        public string VersionStr()
        {
            return _curVersion.Version.Version();
        }

        public VersionInfo Version()
        {
            return _curVersion.Version;
        }


        /// <summary>
        /// 检查目录结构的完备性
        /// </summary>
        void CheckPathValid() => PathHelper.CheckOrCreate(GetCachePath());

        /// <summary>
        /// 获取上一次的版本
        /// </summary>
        /// <returns></returns>
        void CheckVersion()
        {
            if (_curVersion != null)
            {
                // 如果原来的主次版本号与目标配置主次版本不一致，则 build 版本从头开始计数，同时修正主次版本到目标值
                if (_curVersion.Version.Major != _builderConf.Major ||
                    _curVersion.Version.Minor != _builderConf.Minor)
                {
                    _curVersion.Version.Major = _builderConf.Major;
                    _curVersion.Version.Minor = _builderConf.Minor;
                    _curVersion.Version.Build = 0;
                }

                // 构建子版本号自增
                _curVersion.Version.Build++;
                // 渠道信息现使用 Operator 作为区分
                _curVersion.Version.Channel = _builderConf.BuildOperator;
            }

            // 总构建次数自增
            _curVersion.Build++;
            _curVersion.LastUpdate = TimeHelper.ClientNow();
            _curVersion.LastOperator = _builderConf.BuildOperator;
            _curVersion.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ManifestInfo GetManifest() => _manifestInfo;

        /// <summary>
        /// 原始AB包缓存目录 Publish/iOS/_Cache
        /// </summary>
        /// <returns></returns>
        public string GetCachePath() =>
            PathHelper.Combine(PUBLISH_PATH, _builderConf.TargetMode.ToString(), CACHE_PATH);

        /// <summary>
        /// 发布目录 + 目标平台目录  Publish/iOS
        /// </summary>
        /// <returns></returns>
        public string GetPublishPath() => PathHelper.Combine(PUBLISH_PATH, _builderConf.TargetMode.ToString());

        /// <summary>
        /// StreamingAsset/iOS
        /// </summary>
        /// <returns></returns>
        public string GetPublishStreamingPath() =>
            PathHelper.Combine(PathHelper.GetStreamingPath(), _builderConf.TargetMode.ToString());

        public string GetTargetMode()
        {
            return _builderConf.TargetMode.ToString();
        }
    }
}