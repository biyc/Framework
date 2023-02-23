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
using Blaze.Common;
using Blaze.Manage.Data;
using Blaze.Resource.Common;
using ETModel;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Blaze.Resource.Asset
{
    /// <summary>
    /// Unity 抽象资源对象
    /// </summary>
    public abstract class IUAsset : IRef, IDisposable
    {
        /// <summary>
        /// Asset路径
        /// </summary>
        protected string _assetPath;

        /// <summary>
        /// 是否释放
        /// </summary>
        protected bool _disposed;

        /// <summary>
        /// 是否已经加载资源
        /// </summary>
        private bool _isLoadAsset;

        /// <summary>
        /// 加载后的资产 (对象，是否已加载等信息)
        /// isLoadAll = false 结果放入 Asset
        /// </summary>
        protected ICompleted<Object> Asset = new DataWatch<Object>();

        /// isLoadAll = true 结果放入 AssetAll
        protected ICompleted<Object[]> AssetAll = new DataWatch<Object[]>();

        // 等待加载的回调们
        private Action<IUAsset> _callbacks;

        public bool IsUseGc = true;

        /// 释放时候执行回调
        public Action<IUAsset> OnDispose;

        /// <summary>
        /// 加载完成回调
        /// </summary>
        public event Action<IUAsset> Completed
        {
            add
            {
                if (_disposed)
                    Debug.LogError($"uasset already disposed ({_assetPath})");

                if (Asset.IsLoad())
                    value(this);
                else if (AssetAll.IsLoad())
                    value(this);
                else
                    _callbacks += value;
            }
            remove { _callbacks -= value; }
        }


        public Object GetObject()
        {
            if (_disposed)
            {
                Debug.LogError($"uasset already disposed ({_assetPath})");
                return null;
            }

            return Asset.Get();
        }

        public Object[] GetObjects()
        {
            if (_disposed)
            {
                Debug.LogError($"uasset already disposed ({_assetPath})");
                return null;
            }

            return AssetAll.Get();
        }

        /// <summary>
        /// 返回泛型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetObject<T>() where T : class
        {
            if (_disposed)
            {
                Debug.Log($"uasset already disposed ({_assetPath})");
                return null as T;
            }

            return Asset.Get() as T;
        }

        public T Get<T>() where T : class
        {
            return GetObject<T>();
        }


        /// <summary>
        /// 加载完成
        /// </summary>
        protected virtual void Complete(Object data)
        {
            if (_disposed) return;
            if (!Asset.IsLoad())
            {
                // Debug.Log($"asset loaded {_assetPath}");
                Asset.Complet(data);

                try
                {
                    _callbacks?.Invoke(this);
                    _callbacks = null;
                }
                catch (Exception e)
                {
                    Log.Error("加载回调执行出错：" + _assetPath + "\n" + e.StackTrace);
                }
            }

            _isLoadAsset = true;
        }


        /// <summary>
        /// 加载完成
        /// </summary>
        protected void CompleteAll(Object[] data)
        {
            if (_disposed) return;
            if (!AssetAll.IsLoad())
            {
                // Debug.Log($"asset loaded {_assetPath}");
                AssetAll.Complet(data);

                _callbacks?.Invoke(this);
                _callbacks = null;
            }
            _isLoadAsset = true;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="assetPath"></param>
        public IUAsset(string assetPath)
        {
            _assetPath = assetPath;
        }

        /// 对象是否已经释放
        public bool IsAlreadyDispose()
        {
            lock (this)
            {
                // 以正常释放
                if (_disposed) return true;
                // 资源丢失
                if (Asset.Get() == null && AssetAll.Get() == null) return true;

                return false;
            }
        }


        /// 是否已经加载资源
        public bool IsLoadAsset()
        {
            return _isLoadAsset;
            // return Asset.IsLoad() || AssetAll.IsLoad();
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (_disposed) return;
                _disposed = true;
                OnDispose?.Invoke(this);

                // ASSET DATABASE 模式下不真正的卸载资源，只在AB模型下卸载资源
                if (ResManager._.CurrentEnv == EnumEnvMode.AssetDatabase)
                {
                    // Debug.Log("AssetDatabase不销毁" + _assetPath);
                    return;
                }
                try
                {
                    if (Asset.Get() != null && Asset.Get() is GameObject)
                    {
                        Object.DestroyImmediate(Asset.Get(), true);
                        //Object.Destroy(Asset.Get());
                    }
                    else
                    {
                        if (Asset.Get() != null)
                            Resources.UnloadAsset(Asset.Get());
                        if (AssetAll.Get() != null)
                            foreach (var o in AssetAll.Get())
                            {
                                Resources.UnloadAsset(o);
                            }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("asset已销毁\n" + e.StackTrace);
                }

                Asset = null;
                AssetAll = null;
                OnDispose = null;
                Res.GetAnalyzer().OnAssetClose(_assetPath);
                if (IsUseGc)
                {
                    // GC.Collect();
                    Resources.UnloadUnusedAssets();
                }
            }
        }


        /// <summary>
        /// 资源是否有效
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsValid()
        {
            // 资源已经被加载，并且没有被释放
            return IsLoadAsset() && !IsAlreadyDispose();
        }

        // 读取二进制资源
        public abstract byte[] ReadAllBytes();

        // 读取外部文本资源
        public abstract string ReadAllText();


        #region 资源引用计数

        private int _refCount;

        public void AddRef()
        {
            lock (this)
                _refCount++;
        }

        public void RemoveRef()
        {
            lock (this)
                _refCount--;
            if (_refCount == 0)
                Dispose();
        }

        public int RefCount()
        {
            return _refCount;
        }

        #endregion
    }
}