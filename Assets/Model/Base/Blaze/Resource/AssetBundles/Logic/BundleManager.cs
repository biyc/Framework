//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/21 | Initialize core skeleton |
*/

using System;
using System.Collections.Generic;
using Blaze.Core;
using Blaze.Resource.AssetBundles.Data;
using ETModel;

namespace Blaze.Resource.AssetBundles
{
    /// <summary>
    /// 包管理器
    /// </summary>
    public sealed class BundleManager : Singeton<BundleManager>
    {
        private bool _debugStep = false;

        // 初始化完成回调
        private Action _initDown;

        #region 包管理器

        // 存放 AssetName <--> Bundle的容器   assetPath -> bundleName
        // CsvData/Game.Common$Favorability.csv -> CsvData
        private readonly Dictionary<string, string> _bundleMap = new Dictionary<string, string>();


        // bundleName => 包详细信息
        private readonly Dictionary<string, ManifestData> _bundleData = new Dictionary<string, ManifestData>();


        private ManifestInfo _manifest;

        public void Initialize()
        {
            _manifest = BundleHotfix._.GetManifestInfo();

            Define.AssetBundleVersion = _manifest.Version.FullVersion();
            // 添加到快速查找
            // CsvData/Game.Common$Favorability.csv -> CsvData.ab
            _manifest.ManifestList.ForEach(delegate(ManifestData data)
            {
                _bundleData[data.ABName] = data;
                _bundleMap[data.AssetPath] = data.ABName;
            });
        }

        public void AddManifest(ManifestInfo info)
        {
            info.ManifestList.ForEach(data =>
            {
                _bundleData[data.ABName] = data;
                _bundleMap[data.AssetPath] = data.ABName;
            });
        }


        /// <summary>
        /// 获取Bundle的名字
        /// CsvData/Game.Common$Favorability.csv  -> CsvData
        /// </summary>
        /// <param name="assetPath"></param>
        public string AssetPath2Bundle(string assetPath)
        {
            if (_bundleMap.TryGetValue(assetPath, out var bundleName))
            {
                return bundleName;
            }

            return null;
        }

        public bool AssetPath2Bundle(string assetPath, out string bundleName)
        {
            return _bundleMap.TryGetValue(assetPath, out bundleName);
        }

        public ManifestData BundleName2Data(string bundleName) => _bundleData[bundleName];

        public bool BundleName2Data(string bundleName, out ManifestData bundleData)
        {
            return _bundleData.TryGetValue(bundleName, out bundleData);
        }

        #endregion


        /// 获取装配表
        public ManifestInfo GetManifestInfo() => _manifest;


        /// <summary>
        /// 返回给定包名的物理文件名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string FileHashName(string bunleName)
        {
            try
            {
                return _bundleData[bunleName].Hash;
            }
            catch (Exception e)
            {
            }

            return "";
        }
    }
}