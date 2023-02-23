using System;
using Blaze.Resource.Asset;
using Blaze.Resource.AssetBundles.Bundle;
using Blaze.Resource.AssetBundles.Data;
using Blaze.Resource.Task;
using Blaze.Utility;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Blaze.Resource.AssetBundles.Asset
{
    /// <summary>
    /// 从 AssetBundle 资源包载入
    /// </summary>
    public class UAsset : IUAsset
    {
        /// <summary>
        /// 关联的Bundle
        /// </summary>
        private UniBundle _bundle;

        private Type _type;

        private bool _isLoadSub;


        public UAsset(UniBundle bundle, string assetPath, Type type, bool isLoadAll = false, bool isSync = false)
            : base(assetPath)
        {
            _isLoadSub = isLoadAll;
            _type = type;
            _bundle = bundle;
            _bundle.AddRef();
            if (isSync)
                // 同步加载数据
                BundleLoadedSync();
            else
                // 异步加载数据后的回调
                _bundle.Completed += OnBundleLoaded;
        }


        protected override void Complete(Object data)
        {
            // 资源加载完成，减少BUNDLE引用计数
            _bundle.RemoveRef();
            // 释放 UAsset 指向bundle的强引用
            if (_bundle.GetBundleType() == BundleType.AssetBundle)
                _bundle = null;
            base.Complete(data);
        }


        public override byte[] ReadAllBytes()
        {
            return _bundle.ReadAllBytes(_assetPath);
        }

        public override string ReadAllText()
        {
            return _bundle.ReadAllText(_assetPath);
        }


        // Bundle 包准备好后，加载资源
        private void BundleLoadedSync()
        {
            if (_disposed)
            {
                Complete(null);
                return;
            }

            var assetBundle = _bundle.GetAssetBundle();
            if (assetBundle != null)
            {
                if (_isLoadSub)
                {
                    var ass = assetBundle.LoadAssetWithSubAssets(_assetPath);
                    Res.GetAnalyzer().OnAssetOpen(_assetPath);
                    CompleteAll(ass);
                }
                else
                {
                    var request = _type != null
                        ? assetBundle.LoadAsset(_assetPath, _type)
                        : assetBundle.LoadAsset(_assetPath);

                    Res.GetAnalyzer().OnAssetOpen(_assetPath);
                    Complete(request);
                }
            }
            else
            {
                Complete(null); // failed
            }
        }

        // Bundle 包准备好后，加载资源
        private void OnBundleLoaded(UniBundle bundle)
        {
            if (_disposed)
            {
                Complete(null);
                return;
            }

            var assetBundle = _bundle.GetAssetBundle();
            if (assetBundle != null)
            {
                if (_isLoadSub)
                {
                    assetBundle.LoadAssetWithSubAssetsAsync(_assetPath).completed += delegate(AsyncOperation op)
                    {
                        Res.GetAnalyzer().OnAssetOpen(_assetPath);
                        CompleteAll((op as AssetBundleRequest)?.allAssets);
                    };
                }
                else
                {
                    var request = _type != null
                        ? assetBundle.LoadAssetAsync(_assetPath, _type)
                        : assetBundle.LoadAssetAsync(_assetPath);
                    request.completed += delegate(AsyncOperation op)
                    {
                        Res.GetAnalyzer().OnAssetOpen(_assetPath);
                        Complete((op as AssetBundleRequest)?.asset);
                    };
                }
            }
            else
            {
                Complete(null); // failed
            }
        }
    }
}