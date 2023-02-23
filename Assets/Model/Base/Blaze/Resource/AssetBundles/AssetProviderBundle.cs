using System;
using System.Collections.Generic;
using Blaze.Manage.Data;
using Blaze.Resource.Asset;
using Blaze.Resource.AssetBundles.Asset;
using Blaze.Resource.AssetBundles.Bundle;
using Blaze.Resource.Common;
using Blaze.Utility;
using Sirenix.Utilities;
using UnityEngine;

namespace Blaze.Resource.AssetBundles
{
    /// <summary>
    /// 资源提供管理器
    /// </summary>
    public class AssetProviderBundle : IAssetProvider, IAssetBundleProvider
    {
        #region Properties

        // 资产
        private readonly Dictionary<string, IUAsset> _assets = new Dictionary<string, IUAsset>();

        // 对象
        private readonly Dictionary<string, UniBundle> _bundles = new Dictionary<string, UniBundle>();

        // 销毁标记
        private bool _disposed;

        // 当前资源提供器
        private readonly ICompleted<bool> _providerCompleted = new DataWatch<bool>();

        #endregion

        #region AssetProvider 基础实现

        /// <summary>
        /// 启动（初始化资源提供器）
        /// </summary>
        public void Open()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            // 资源提供器准备完毕
            _providerCompleted.Complet(true);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            // 释放资源
            if (_disposed) return;
            _disposed = true;

            // 释放 assets
            _assets.Values.ForEach(delegate(IUAsset asset)
            {
                asset.OnDispose = null;
                asset.Dispose();
            });
            _assets.Clear();

            // 释放 bundle
            foreach (var bundle in _bundles.Values)
            {
                bundle.OnDispose = null;
                bundle.Release();
            }

            _bundles.Clear();

            GC.Collect();
        }


        ICompleted<bool> IAssetProvider.Ready()
        {
            return _providerCompleted;
        }

        /// <summary>
        /// 获取UAsset资源
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="type"></param>
        /// <param name="isLoadAll">加载图集时使用</param>
        /// <returns></returns>
        public IUAsset GetAsset(string assetPath, Type type, bool isLoadAll = false)
        {
            // 缓存中有，并且没有被销毁
            if (_assets.TryGetValue(assetPath, out var assetRef) && !assetRef.IsAlreadyDispose())
            {
                // Tuner.Log("命中asset缓存:" + assetPath);
                Res.GetAnalyzer().OnAssetAccess(assetPath);
                return assetRef;
            }

            // string bundleName;
            if (BundleManager._.AssetPath2Bundle(assetPath, out var bundleName))
            {
                var bundle = GetBundle(bundleName);
                if (bundle == null)
                {
                    Tuner.Error($"加载{assetPath}的 bundle 出错");
                    return new UAssetFailure(assetPath);
                }

                var asset = new UAsset(bundle, assetPath, type, isLoadAll);
                // 对象销毁时，从缓存中移除
                asset.OnDispose += delegate(IUAsset iuAsset)
                {
                    // Debug.Log("移除ass缓存:" + assetPath);
                    _assets.Remove(assetPath);
                };
                // 不缓存 zip 产生的 asset
                if (!bundle.IsZip())
                    _assets[assetPath] = asset;
                return asset;
            }
            else
            {
                Tuner.Error($"没有找到{assetPath}的 bundleName 信息");
            }


            // 实例化资源失败
            return new UAssetFailure(assetPath);
        }

        public IUAsset GetAssetSync(string assetPath, Type type, bool isLoadAll = false)
        {
            // 缓存中有，并且已经被加载成功，并且没有被销毁
            if (_assets.TryGetValue(assetPath, out var assetRef) && assetRef.IsLoadAsset() &&
                !assetRef.IsAlreadyDispose())
            {
                // Tuner.Log("命中asset缓存:" + assetPath);
                Res.GetAnalyzer().OnAssetAccess(assetPath);
                return assetRef;
            }

            if (BundleManager._.AssetPath2Bundle(assetPath, out var bundleName))
            {
                var bundle = GetBundleSync(bundleName);
                if (bundle == null)
                {
                    Tuner.Error($"加载{assetPath}的 bundle 出错");
                    return new UAssetFailure(assetPath);
                }

                var asset = new UAsset(bundle, assetPath, type, isLoadAll, isSync: true);
                // 对象销毁时，从缓存中移除
                asset.OnDispose += delegate(IUAsset iuAsset)
                {
                    // Debug.Log("移除ass缓存" + assetPath);
                    _assets.Remove(assetPath);
                };
                // 不缓存 zip 产生的 asset
                if (!bundle.IsZip())
                    _assets[assetPath] = asset;
                return asset;
            }
            else
            {
                // Tuner.Error($"没有找到{assetPath}的 bundleName 信息");
            }

            // 实例化资源失败
            return new UAssetFailure(assetPath);
        }


        /// <summary>
        /// 查看存在
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public bool IsAssetExists(string assetPath) => Find(assetPath) != null;

        #endregion

        #region BundleProvider 专用接口实现

        /// <summary>
        /// 获取资源包 Completed 监听是否加载成功
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public UniBundle GetBundle(string bundleName, bool isSub = false)
        {
            // 缓存中有，并且没有被销毁
            if (_bundles.TryGetValue(bundleName, out var bundle) && !bundle.IsDispose())
            {
                // Tuner.Log("命中bundle缓存:" + bundleName);
                return bundle;
            }

            if (BundleManager._.BundleName2Data(bundleName, out var bundleManifest))
            {
                try
                {
                    bundle = new UniBundle(this, bundleManifest, isSub: isSub, isSync: true);
                    bundle.OnDispose = delegate(UniBundle uniBundle) { _bundles.Remove(bundleName); };
                    // 同步加载AB包或zip包
                    // todo 写错了？
                    // bundle.LoadSync();
                    bundle.Load();
                    _bundles[bundleName] = bundle;
                }
                catch (Exception e)
                {
                    Tuner.Error(bundleName);
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                Tuner.Error($"没有找到{bundleName}的ManifestData信息");
            }

            return bundle;
        }


        /// <summary>
        /// 同步获取资源包
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public UniBundle GetBundleSync(string bundleName, bool isSub = false)
        {
            // 缓存中有，并且已经被加载成功，并且没有被销毁
            if (_bundles.TryGetValue(bundleName, out var bundle) && bundle.IsLoad() && !bundle.IsDispose())
            {
                // Tuner.Log("命中bundle缓存:" + bundleName);
                return bundle;
            }

            if (BundleManager._.BundleName2Data(bundleName, out var bundleManifest))
            {
                try
                {
                    bundle = new UniBundle(this, bundleManifest, isSub: isSub, isSync: true);

                    bundle.OnDispose = delegate(UniBundle uniBundle) { _bundles.Remove(bundleName); };
                    // 同步加载AB包或zip包
                    bundle.LoadSync();
                    _bundles[bundleName] = bundle;
                }
                catch (Exception e)
                {
                    Tuner.Error(bundleName);
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                Tuner.Error($"没有找到{bundleName}的ManifestData信息");
            }

            return bundle;
        }


        /// <summary>
        ///  查找资源 assetPath 对应的 bundle.name
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public string Find(string assetPath) => BundleManager._.AssetPath2Bundle(assetPath);

        #endregion


        #region Private

        /// <summary>
        /// 清除 asset 缓存
        /// </summary>
        public void DevCleanAsset()
        {
            Tuner.Log(_assets.Count + " 缓存数量 ");

            _assets.ForEach(delegate(KeyValuePair<string, IUAsset> pair)
            {
                try
                {
                    Tuner.Log("count:" + pair.Value.RefCount() + "  " + pair.Key);
                    pair.Value.OnDispose = null;
                    pair.Value?.Dispose();
                }
                catch (Exception e)
                {
                    Tuner.Error(pair.Key);
                }
            });
            _assets.Clear();
            Tuner.Log(" 已清空 ");
        }

        /// <summary>
        /// 统计缓存中的数据
        /// </summary>
        public void DevAssetShow()
        {
            Tuner.Log(_assets.Count + " 缓存数量 ");
            _assets.ForEach(delegate(KeyValuePair<string, IUAsset> pair)
            {
                Tuner.Log("count:" + pair.Value.RefCount() + "  " + pair.Key);
            });
        }

        #endregion
    }
}