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
using System.Threading.Tasks;
using Blaze.Common;
using Blaze.Resource.Analyzer;
using Blaze.Resource.Asset;
using Blaze.Resource.AssetBundles;
using Blaze.Resource.AssetBundles.Bundle;
using Blaze.Resource.AssetBundles.Logic;
using Blaze.Resource.Common;
using Blaze.Resource.Poco;
using UnityEngine;

namespace Blaze.Resource
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public static class Res
    {
        /// <summary>
        /// 资源分析器
        /// </summary>
        static IAssetsAnalyzer _analyzer;

        public static void SetAnalyzer(IAssetsAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        public static IAssetsAnalyzer GetAnalyzer()
        {
            if (_analyzer == null)
            {
                _analyzer = new AssetsAnalyzerDummy();
            }

            return _analyzer;
        }

        /// <summary>
        /// 设置资产提供器
        /// </summary>
        /// <returns></returns>
        private static IAssetProvider GetAssetProvider()
        {
            return ResManager._.GetAssetProvider();
        }

        #region Base Helper

        /// <summary>
        /// 检查某资源是否存在
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static bool IsAssetExists(string assetPath)
        {
            return GetAssetProvider().IsAssetExists(assetPath);
        }

        /// <summary>
        /// 异步加载一个资源,是否成功要看OnComplete回调
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="callback"></param>
        /// <param name="type"></param>
        /// <param name="isLoadAll">加载Sprit等子资源时为true</param>
        /// <returns></returns>
        public static IUAsset LoadAsset(string assetPath, Action<IUAsset> callback = null, Type type = null,
            bool isLoadAll = false)
        {
            var asset = GetAssetProvider().GetAsset(assetPath, type, isLoadAll);
            if (callback != null)
            {
                asset.Completed += callback;
            }

            return asset;
        }

        /// <summary>
        /// 同步加载一个独立资源 (大对象不推荐使用)
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static IUAsset LoadAssetSync(string assetPath, Type type = null)
        {
            // assetPath = assetPath.ToLower();
            return GetAssetProvider().GetAssetSync(assetPath, type);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="loadCb"></param>
        /// <typeparam name="T"></typeparam>
        public static IUAsset LoadAsset<T>(string assetPath, Action<T> loadCb) where T : class
        {
            var uasset = LoadAsset(assetPath, type: typeof(T));
            uasset.Completed += delegate(IUAsset asset) { loadCb?.Invoke(asset.GetObject<T>()); };
            return uasset;
        }


        /// <summary>
        /// 直接实例化预制件
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static async Task<T> LoadAssetAsync<T>(string assetPath) where T : class
        {
            var task = new TaskCompletionSource<T>();
            LoadAsset(assetPath, delegate(T asset) { task.SetResult(asset); });
            return await task.Task;
        }

        /// <summary>
        /// 直接实例化预制件
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static async Task<IUAsset> LoadAssetAsync(string assetPath, Action<IUAsset> callback = null,
            Type type = null,
            bool isLoadAll = false)
        {
            var task = new TaskCompletionSource<IUAsset>();
            LoadAsset(assetPath, delegate(IUAsset asset)
            {
                callback?.Invoke(asset);
                task.SetResult(asset);
            }, type, isLoadAll);
            return await task.Task;
        }

        /// <summary>
        /// 加载资源(同步方法 不推荐)
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="loadCb"></param>
        /// <typeparam name="T"></typeparam>
        public static T LoadAssetSync<T>(string assetPath, Type type = null) where T : class
        {
            return LoadAssetSync(assetPath, type: type).GetObject<T>();
        }

        /// <summary>
        /// 直接实例化预制件
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static PrefabObject Instantiate(string assetPath, Transform parent = null,
            bool worldPositionStays = false)
        {
            return PrefabObject.Load(assetPath, parent, worldPositionStays);
        }

        /// <summary>
        /// 直接实例化预制件
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static PrefabObject InstantiateSync(string assetPath, Transform parent = null,
            bool worldPositionStays = false)
        {
            return PrefabObject.LoadSync(assetPath, parent, worldPositionStays);
        }

        /// <summary>
        /// 直接实例化预制件
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static async Task<PrefabObject> InstantiateAsync(string assetPath, Transform parent = null,
            bool worldPositionStays = false)
        {
            // var abDownTask = new TaskCompletionSource<bool>();
            // if (DefaultRuntime.RuntimeEnvMode == EnumEnvMode.HotfixRun &&
            //     assetPath.StartsWith("Assets/Projects/Prefabs/"))
            //     abDownTask.SetResult(await BundleHotfix._.LoadTarget(assetPath));
            // else
            //     abDownTask.SetResult(true);

            var task = new TaskCompletionSource<PrefabObject>();
            // if (!await abDownTask.Task)
            //     task.SetResult(null);
            // else
            // 成功回调
            PrefabObject.Load(assetPath, parent, worldPositionStays).Completed += task.SetResult;
            return await task.Task;
        }

        public static async Task<bool> DownLoadModelAsset( string name)
        {
            var abDownTask = new TaskCompletionSource<bool>();
            // if (DefaultRuntime.RuntimeEnvMode)
            //     abDownTask.SetResult(await BundleHotfix._.LoadModelAsset(assetPath));
            // else
            //     abDownTask.SetResult(true);
            abDownTask.SetResult(await BundleHotfix._.LoadModelAsset(name));
            return await abDownTask.Task;
        }

        #endregion

        #region Extension

        //  AB 模式下起作用的API ===============================================================================

        /// <summary>
        /// 通过资产名称查找到AB包
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static UniBundle GetBundle(string bundleName)
        {
            if (ResManager._.CurrentEnv != EnumEnvMode.AssetDatabase)
            {
                var provider = ResManager._.GetAssetProvider() as IAssetBundleProvider;
                return (UniBundle) provider.GetBundle(bundleName);
            }

            return null;
        }

        /// <summary>
        /// 通过资产名称查找到AB包
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static UniBundle GetBundleSync(string bundleName)
        {
            if (ResManager._.CurrentEnv != EnumEnvMode.AssetDatabase)
            {
                var provider = ResManager._.GetAssetProvider() as IAssetBundleProvider;
                return (UniBundle) provider.GetBundleSync(bundleName);
            }

            return null;
        }

        #endregion
    }
}