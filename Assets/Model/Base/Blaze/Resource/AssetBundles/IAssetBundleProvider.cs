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
using Blaze.Resource.AssetBundles.Bundle;
using Blaze.Resource.Poco;

namespace Blaze.Resource.AssetBundles
{
    /// <summary>
    /// 资源提供接口  AB专有
    /// </summary>
    public interface IAssetBundleProvider
    {
        // /// <summary>
        // /// 加载批量的装配
        // /// </summary>
        // /// <param name="manifests"></param>
        // /// <param name="callback"></param>
        // void LoadBundleManifests(ManifestGroup[] manifests, Action<int, string, UniBundle> callback,
        //     Action loadSuccessCb = null);

        /// <summary>
        /// 获取UniBundle句柄
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        UniBundle GetBundle(string bundleName, bool isSub = false);

        /// <summary>
        /// 获取UniBundle句柄
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        UniBundle GetBundleSync(string bundleName, bool isSub = false);

        /// <summary>
        /// 查找资源, 返回其所在的包
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        string Find(string assetPath);
    }
}