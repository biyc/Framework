//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | Tim    | 2019/10/18 | Initialize core skeleton |
*/

using System;
using System.Diagnostics;
using Blaze.Manage.Data;
using Blaze.Resource.Asset;
using Blaze.Utility;
using UniRx;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Blaze.Resource.Common
{
    // 创建一个占位用 GameObject, 异步加载指定prefab资源, 并实例化挂载与此节点
    public class PrefabObject
    {
        // UAsset对象
        private IUAsset _asset;

        // 资源加载完成回调
        private ICompleted<PrefabObject> _load = new DataWatch<PrefabObject>();

        // 目标GameObject
        private GameObject _target;

        // 获取目标对象
        public GameObject Target
        {
            get { return _target; }
        }

        public event Action<PrefabObject> Completed
        {
            add { _load.OnCompleted += value; }
            remove { _load.OnCompleted -= value; }
        }

        /// <summary>
        /// 已经加载
        /// </summary>
        public bool IsLoaded => _load.IsLoad();

        /// <summary>
        /// 加载完成后若干秒销毁
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public PrefabObject DestroyAfter(long seconds, Action endCb = null)
        {
            _load.OnCompleted += delegate(PrefabObject o)
            {
                Observable.Timer(new TimeSpan(TimeSpan.TicksPerSecond * seconds)).Subscribe(delegate(long ls)
                {
                    Destroy();
                    endCb?.Invoke();
                });
            };
            return this;
        }


        /// <summary>
        /// 通过资源管理器加载
        /// </summary>
        /// <param name="assetPath"></param>
        private void LoadInternal(string assetPath, Transform parent, bool worldPositionStays)
        {

            _asset = Res.LoadAsset(assetPath);
            // 增加源计数
            _asset.AddRef();
            _asset.Completed += delegate(IUAsset asset)
            {
                InstObj(asset, parent, worldPositionStays);
            };
        }

        /// <summary>
        /// 通过资源管理器加载
        /// </summary>
        /// <param name="assetPath"></param>
        private void LoadInternalSync(string assetPath, Transform parent, bool worldPositionStays)
        {
            _asset = Res.LoadAssetSync(assetPath, typeof(GameObject));
            // 增加源计数
            _asset.AddRef();
            InstObj(_asset, parent, worldPositionStays);
        }


        /// <summary>
        /// 根据 IUAsset 实例化对象
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="parent"></param>
        private void InstObj(IUAsset asset, Transform parent = null, bool worldPositionStays = false)
        {
            try
            {
                if (asset.GetObject() == null)
                {
                    Tuner.Error("PrefabObject 加载出错：资源为空");
                }

                if (parent == null)
                    _target = GameObject.Instantiate(asset.GetObject() as GameObject);
                else
                    _target = GameObject.Instantiate(asset.GetObject() as GameObject, parent, worldPositionStays);

                _load.Complet(this);
            }
            catch (Exception e)
            {
                Tuner.Error("PrefabObject 加载出错" + e.StackTrace);
            }
        }


        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy()
        {
            // 释放当前对象
            if (_target != null)
                GameObject.Destroy(_target);
            // 对象销毁后，释放未引用资源
            Resources.UnloadUnusedAssets();
            _asset?.RemoveRef();
            _asset = null;
            _load = null;
        }


        #region 外部调用静态方法

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static PrefabObject Load(string assetPath, Transform parent = null, bool worldPositionStays = false)
        {
            var pr = new PrefabObject();
            ResManager._.WatchProvidComplete.OnCompleted += delegate(IAssetProvider provider)
            {
                pr.LoadInternal(assetPath, parent, worldPositionStays);
            };
            return pr;
        }


        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static PrefabObject LoadSync(string assetPath, Transform parent = null, bool worldPositionStays = false)
        {
            var pr = new PrefabObject();
            pr.LoadInternalSync(assetPath, parent, worldPositionStays);
            return pr;
        }

        #endregion
    }
}