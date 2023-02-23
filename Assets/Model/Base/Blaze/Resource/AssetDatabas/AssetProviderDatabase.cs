//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/18 | Initialize core skeleton |
*/

using System;
using System.Collections.Generic;
using System.IO;
using Blaze.Manage.Data;
using Blaze.Resource.Asset;
using Blaze.Resource.AssetDatabas.Asset;
using Blaze.Resource.Common;
using Blaze.Utility;
using Blaze.Utility.Helper;

namespace Blaze.Resource.AssetDatabas
{
    /// <summary>
    /// 开发期快速访问提供接口
    /// </summary>
    public class AssetProviderDatabase : IAssetProvider
    {
        private readonly ICompleted<bool> _providerCompleted = new DataWatch<bool>();

        public ICompleted<bool> Ready() => _providerCompleted;

        public IUAsset GetAsset(string assetPath, Type type, bool isLoadAll = false)
        {
            // 模拟异步加载
            return GetAsset(assetPath, type, isLoadAll, false);
        }

        public IUAsset GetAssetSync(string assetPath, Type type, bool isLoadAll = false)
        {
            // 使用同步加载
            return GetAsset(assetPath, type, isLoadAll, isSync: true);
        }


        private IUAsset GetAsset(string assetPath, Type type, bool isLoadAll, bool isSync)
        {
            IUAsset asset;

            // 存放开发数据的外部文件地址
            if (File.Exists(assetPath))
            {
                // 模拟异步加载
                asset = new UAssetDatabase(assetPath, type, isLoadAll, isSync: isSync);
            }
            else
            {
                asset = new UAssetFailure(assetPath);
            }

            // _assets[assetPath] = asset;
            return asset;
        }


        public bool IsAssetExists(string assetPath)
        {
            return File.Exists(assetPath);
        }


        public void Open()
        {
            _providerCompleted.Complet(true);
        }

        public void Close()
        {
            // _assets.Clear();
        }
    }
}