//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/15 | Initialize core skeleton |
*/

using System.IO;
using Blaze.Common;
using Blaze.Core;
using Blaze.Manage.Data;
using Blaze.Resource.AssetBundles;
using Blaze.Resource.AssetDatabas;
using Blaze.Resource.Common;
using Blaze.Utility;
using Blaze.Utility.Helper;
using UnityEngine;

namespace Blaze.Resource
{
    /// <summary>
    /// Tfs环境控制,对Debug等也会产生作用,以及默认配置项有微弱影响
    /// </summary>
    // public sealed class ResManager : Singeton<ResManager>
    public sealed class ResManager
    {
        #region Singleton

        private static readonly ResManager _instance = new ResManager();
        public static ResManager _ => _instance;

        #endregion


        private bool _debugStep = false;

        /// <summary>
        /// 当前的环境
        /// </summary>
        public EnumEnvMode CurrentEnv;

        /// <summary>
        /// 资源加载器
        /// </summary>
        private IAssetProvider _assetProvider;

        public ICompleted<IAssetProvider> WatchProvidComplete = new DataWatch<IAssetProvider>();


        /// <summary>
        /// 初始化资源系统
        /// </summary>
        public void Initialize()
        {
            // 根据默认配置设置当前运行的模式
            CurrentEnv = DefaultRuntime.RuntimeEnvMode;
            Tuner.Log(Co._($"[>>] Runtime environment : ; [{CurrentEnv.ToString().ToUpper()}] :lightred:b;"));


            switch (CurrentEnv)
            {
                // 初始化资源提供器
                case EnumEnvMode.AssetDatabase:
                    // 初始化 AssetDatabase 资源资源提供器
                    InitProvider();
                    break;
                case EnumEnvMode.HotfixRun:
                    // 对比下载资源更新等
                    BundleHotfix._.Init(delegate(bool b)
                    {
                        // 启动 AB 包管理器
                        BundleManager._.Initialize();
                        // 初始化 ab 资源资源提供器
                        InitProvider();
                    });
                    break;
            }
        }

        /// <summary>
        /// 前置初始化准备完成，正式初始化 AssetProvider
        /// </summary>
        void InitProvider()
        {
            // 建立快速索引
            if (CurrentEnv == EnumEnvMode.AssetDatabase)
            {
                AssetIndex._.BuildAssetDatabaseIndex();
                _assetProvider = new AssetProviderDatabase();
            }
            else
            {
                AssetIndex._.Build(BundleManager._.GetManifestInfo());
                _assetProvider = new AssetProviderBundle();
            }

            // 初始化资源提供器
            _assetProvider.Open();
            _assetProvider.Ready().OnCompleted += delegate(bool b)
            {
                // 准备完毕，启动其它游戏模块
                WatchProvidComplete.Complet(_assetProvider);
            };
        }


        /// <summary>
        /// 设置资产提供器
        /// </summary>
        /// <returns></returns>
        public IAssetProvider GetAssetProvider()
        {
            return _assetProvider;
        }
    }
}