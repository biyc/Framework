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
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Blaze.Common;
using Blaze.Manage.Data;
using Blaze.Resource.AssetBundles.Data;
using Blaze.Resource.AssetBundles.Logic.StreamingAssets;
using Blaze.Resource.Common;
using Blaze.Utility;
using Blaze.Utility.Base;
using Blaze.Utility.Extend;
using Blaze.Utility.Helper;
using ETModel;
using ICSharpCode.SharpZipLib.Zip;
using UniRx;
using UniRx.WebRequest;
using UnityEngine;

namespace Blaze.Resource.AssetBundles.Bundle
{
    /// <summary>
    /// 资源包抽象
    /// </summary>
    public class UniBundle : IRef
    {
        /// 释放时候执行回调
        public Action<UniBundle> OnDispose;

        /// <summary>
        /// 打包信息
        /// </summary>
        private ManifestData _manifest;

        /// <summary>
        /// 依赖关系
        /// </summary>
        private List<UniBundle> Denpendencies = new List<UniBundle>();

        // 是否加载完成  加载后的信息等
        private ICompleted<UniBundle> Bundle = new DataWatch<UniBundle>();


        private AssetProviderBundle _provider;

        // 已加载的ZIP包引用
        private ZipFile _zipFile;

        // 已加载的 AB 包引用
        private AssetBundle _assetBundle;

        private readonly TaskQueue subLoadQueue = new TaskQueue();

        // bundle 是否已经加载
        // private bool _isLoad;

        /// 是否释放
        private bool _isDisposed;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bundleManifest"></param>
        public UniBundle(AssetProviderBundle provider, ManifestData bundleManifest, bool isSub = false,
            bool isSync = false)
        {
            _provider = provider;
            _manifest = bundleManifest;

            // 加载本模块需要的其它依赖
            if (!isSub)
            {
                if (isSync)
                {
                    // 同步加载依赖
                    bundleManifest.Dependencies?.ForEach(delegate(string dependBundle)
                    {
                        var subBundle = _provider.GetBundleSync(dependBundle, true);
                        subBundle.AddRef();
                        Denpendencies.Add(subBundle);
                    });
                }
                else
                {
                    // 异步加载依赖
                    bundleManifest.Dependencies?.ForEach(delegate(string dependBundle)
                    {
                        var subBundle = _provider.GetBundle(dependBundle, true);
                        subBundle.AddRef();
                        Denpendencies.Add(subBundle);
                        subLoadQueue.AddTask(delegate(Action action)
                        {
                            subBundle.Bundle.OnCompleted += delegate(UniBundle bundle) { action.Invoke(); };
                        });
                    });
                }
            }
        }


        /// <summary>
        /// 主线程回调
        /// </summary>
        public event Action<UniBundle> Completed
        {
            add { Bundle.OnCompleted += value; }

            remove { Bundle.OnCompleted -= value; }
        }

        #region geter

        /// <summary>
        /// Bundle信息
        /// </summary>
        public ManifestData ManifestData() => _manifest;


        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return _manifest.ABName; }
        }

        #endregion


        #region 资源引用计数

        /// <summary>
        /// 资源引用计数
        /// </summary>
        private int _refCount;

        private IDisposable _timer;

        /// <summary>
        /// 添加引用
        /// </summary>
        public void AddRef()
        {
            // 全局包,不进行引用计数分析,如果没有价值,直接调用Release函数.
            // if (_package.IsGlobal) return;
            // 打断可能准备释放的资源
            // _timer?.Dispose();
            _refCount++;
        }

        /// <summary>
        /// 删除引用
        /// </summary>
        public void RemoveRef()
        {
            // 全局包,不进行引用计数分析,如果没有价值,直接调用Release函数.
            // if (_package.IsGlobal) return;
            _refCount--;
            if (_refCount == 0)
            {
                // 延后释放资源
                // _timer = Observable.Timer(new TimeSpan(TimeSpan.TicksPerMillisecond * 500)).Subscribe(delegate(long l)
                // {
                Release();
                // });
            }
        }

        public int RefCount()
        {
            return _refCount;
        }

        #endregion

        /// <summary>
        /// 释放
        /// </summary>
        public void Release()
        {
            if (DefaultDebug.AllowBundleInfo)
                Tuner.Log(Co._($"[(=ﾟωﾟ=)] - [[{Name}]]  ;love:green:b;"));

            // Tuner.Log("释放 bundle:" + _manifest.AssetPath);


            // 不释放ZIP包
            if (_manifest.Type == BundleType.Zip) return;

            lock (this)
            {
                if (_isDisposed) return;
                _isDisposed = true;
            }

            // 从资源提供器中卸载当前 UniBundle
            if (_provider != null)
            {
                // _provider.RemoveBundle(this);
                // _provider.RemoveBundle(this);
                OnDispose?.Invoke(this);
                OnDispose = null;
                _provider = null;
            }

            // 卸载AB包
            if (_assetBundle != null)
            {
                _assetBundle.Unload(false);
                _assetBundle = null;
            }

            // 卸载依赖AB包
            Denpendencies.ForEach(delegate(UniBundle subBundle) { subBundle.RemoveRef(); });
            Denpendencies.Clear();

            // 卸载zip包
            if (_zipFile != null)
            {
                _zipFile.Close();
                _zipFile = null;
            }

            Bundle = null;
        }


        /// <summary>
        /// 调用所有监听回调
        /// </summary>
        private void OnLoaded()
        {
            if (DefaultDebug.AllowBundleInfo)
                Tuner.Log(Co._($"[(●ﾟωﾟ●)] - [[{Name}]]  ;love:red:b;"));

            // _isLoad = true;

            // BundleManager._.setValid(Name);
            subLoadQueue.OnFinish = delegate
            {
                // 通知所有回调,加载完成
                Bundle.Complet(this);

                // _isLoad = true;
            };
            subLoadQueue.Start();
        }


        /// 是否已经加载 bundle
        public bool IsLoad()
        {
            return Bundle.IsLoad();
        }


        /// bundle 是否已经释放
        public bool IsDispose()
        {
            lock (this)
            {
                // 以正常释放
                return _isDisposed;
            }
        }


        public BundleType GetBundleType() => _manifest.Type;


        #region Loader 资源加载器 加载ab包或zip包

        /// <summary>
        /// 加载AB包或ZIP包
        /// </summary>
        /// <returns></returns>
        public UniBundle Load()
        {
            // var filePath = Path.Combine(ResManager.ResPath, _manifest.Hash);
            // Tuner.Log("加载bundle:" + _manifest.AssetPath);

            // Bundle/iOS/1.1/Hash
            var filePath = GetAssetLocalPath();

            if (File.Exists(filePath))
            {
                // 1. 从本地文件中直接加载AB
                Load(filePath);
            }
            else
            {
                // 从SA文件中加载
                StreamingAssetsLoader._.Load(_manifest,
                    delegate(Stream stream) { Load(stream); },
                    delegate(AssetBundle assetBundle) { Load(assetBundle); });
            }

            return this;
        }

        public string GetAssetLocalPath()
        {
            var isModel = _manifest.IsModelAsset();
            var splits = _manifest.AssetPath.Split('/');
            var filePath = isModel
                ? PathHelper.Combine(BundleHotfix._.ResBasePath, "ModelBundle", splits[4],
                    _manifest.Hash)
                : Path.Combine(
                    BundleHotfix._.GetBasePath(), _manifest.GetSaveSubPath());
            return filePath;
        }

        public UniBundle LoadSync()
        {
            // Tuner.Log("加载bundle:" + _manifest.AssetPath);
            // Bundle/iOS/1.1/Hash

            var filePath = GetAssetLocalPath();
            if (File.Exists(filePath))
            {
                // Tuner.Log("从文件中加载资源   " + filePath);
                LoadSync(filePath);
            }
            else
            {
                switch (_manifest.Type)
                {
                    case BundleType.Zip:
                        var stream = StreamingAssetsLoader._.LoadSyncStream(_manifest.Hash);
                        Load(stream);
                        break;
                    case BundleType.AssetBundle:
                        var assetBundle = StreamingAssetsLoader._.LoadSyncAssetBundle(_manifest.Hash);
                        Load(assetBundle);
                        break;
                }
            }

            return this;
        }


        // /// <summary>
        // /// 载入数据流
        // /// </summary>
        // /// <param name="stream"></param>
        // public abstract void Load(Stream stream, Action<IUniBundle> onLoad = null);
        // public abstract void Load(AssetBundle assetBundle, Action<IUniBundle> onLoad = null);
        // public abstract void Load(string path, Action<IUniBundle> onLoad = null);
        // public abstract void LoadSync(string path, Action<IUniBundle> onLoad = null);

        private AssetBundleCreateRequest _assetBundleCreateRequest;

        // 异步加载函数
        public UniBundle Load(string path)
        {
            try
            {
                if (_manifest.Type == BundleType.AssetBundle)
                {
                    _assetBundleCreateRequest = _manifest.IsModelAsset()
                        ? AssetBundle.LoadFromFileAsync(path, 0, (ulong) CryptoHelper.GetABOffestNum(_manifest.Hash))
                        : AssetBundle.LoadFromFileAsync(path);
                    _assetBundleCreateRequest.completed += delegate(AsyncOperation operation)
                    {
                        // OnAssetBundleLoaded(operation);
                        _assetBundle = (operation as AssetBundleCreateRequest)?.assetBundle;
                        OnLoaded();
                    };
                }
                else if (_manifest.Type == BundleType.Zip)
                {
                    ObservableWebRequest.GetAndGetBytes("file://" + path).Subscribe(delegate(byte[] bytes)
                    {
                        Encoding utf = Encoding.GetEncoding("UTF-8");
                        ZipConstants.DefaultCodePage = utf.CodePage;
                        _zipFile = new ZipFile(new MemoryStream(bytes));
                        OnLoaded();
                    }, delegate(Exception exception)
                    {
                        Tuner.Error($"[>>] {Name} 读取zip 失败!");
                        OnLoaded();
                    });
                    // Encoding utf = Encoding.GetEncoding("UTF-8");
                    // ZipConstants.DefaultCodePage = utf.CodePage;
                    // _zipFile = new ZipFile(path);
                    // _zipFile.IsStreamOwner = true;
                    // OnLoaded();
                }
            }
            catch (Exception)
            {
                Tuner.Error($"[>>] {Name} Loaded Error!");
            }

            return this;
        }

        // 同步加载函数
        public UniBundle LoadSync(string path)
        {
            try
            {
                if (_manifest.Type == BundleType.AssetBundle)
                {
                    // 如果异步也在加载该资源，需要先卸载异步加载的资源
                    _assetBundleCreateRequest?.assetBundle.Unload(true);
                    _assetBundle = _manifest.IsModelAsset()
                        ? AssetBundle.LoadFromFile(path, 0, (ulong) CryptoHelper.GetABOffestNum(_manifest.Hash))
                        : AssetBundle.LoadFromFile(path);
                    OnLoaded();
                }
                else if (_manifest.Type == BundleType.Zip)
                {
                    Encoding utf = Encoding.GetEncoding("UTF-8");
                    ZipConstants.DefaultCodePage = utf.CodePage;
                    _zipFile = new ZipFile(path);
                    _zipFile.IsStreamOwner = true;
                    OnLoaded();
                }
            }
            catch (Exception)
            {
                Tuner.Error($"[>>] {Name} Loaded Error!");
            }

            return this;
        }


        public void Load(Stream stream)
        {
            try
            {
                if (_manifest.Type == BundleType.Zip)
                {
                    try
                    {
                        Encoding utf = Encoding.GetEncoding("UTF-8");
                        ZipConstants.DefaultCodePage = utf.CodePage;
                        _zipFile = new ZipFile(stream);
                        _zipFile.IsStreamOwner = true;
                    }
                    catch (Exception e)
                    {
                        Tuner.Log($"[>>] {Name} zip 加载出现小问题");
                    }

                    OnLoaded();
                }
            }
            catch (Exception)
            {
                // Tuner.Error($"[>>] {Name} Loaded Error!");
                // Tuner.Log($"[>>] {Name} zip 加载出现小问题");
            }
        }

        public void Load(AssetBundle assetBundle)
        {
            try
            {
                if (_manifest.Type == BundleType.AssetBundle)
                {
                    _assetBundle = assetBundle;
                    OnLoaded();
                }
            }
            catch (Exception)
            {
                Tuner.Error($"[>>] {Name} Loaded Error!");
            }
        }

        #endregion

        #region Reader 从资源包中读取指定资源

        public AssetBundle GetAssetBundle()
        {
            return _assetBundle;
        }

        public ZipFile GetZip()
        {
            return _zipFile;
        }

        /// <summary>
        /// 此 bundle 是否为zip
        /// </summary>
        /// <returns></returns>
        public bool IsZip()
        {
            return _manifest.Type == BundleType.Zip;
        }

        public byte[] ReadAllBytes(string assetPath)
        {
            if (_manifest.Type == BundleType.AssetBundle)
            {
                return GetAssetBundle()?.LoadAsset<TextAsset>(assetPath)?.bytes;
            }
            else if (_manifest.Type == BundleType.Zip)
            {
                if (_zipFile != null)
                {
                    _zipFile.Password = "he#3t;ZwcqF2E)DAgNVztuK";
                    string fileName = Path.GetFileName(assetPath);
                    var entry = _zipFile.GetEntry(fileName);
                    if (entry != null)
                    {
                        using (var stream = _zipFile.GetInputStream(entry))
                        {
                            var buffer = new byte[entry.Size];
                            stream.Read(buffer, 0, buffer.Length);
                            return buffer;
                        }
                    }
                }
            }

            return null;
        }

        public string ReadAllText(string assetPath)
        {
            if (_manifest.Type == BundleType.AssetBundle)
            {
                return GetAssetBundle()?.LoadAsset<TextAsset>(assetPath)?.text;
            }
            else if (_manifest.Type == BundleType.Zip)
            {
                if (_zipFile != null)
                {
                    _zipFile.Password = "he#3t;ZwcqF2E)DAgNVztuK";
                    string fileName = Path.GetFileName(assetPath);
                    var entry = _zipFile.GetEntry(fileName);
                    if (entry != null)
                    {
                        using (var stream = _zipFile.GetInputStream(entry))
                            return new StreamReader(stream).ReadToEnd();
                    }
                }
            }

            return null;
        }

        #endregion
    }
}