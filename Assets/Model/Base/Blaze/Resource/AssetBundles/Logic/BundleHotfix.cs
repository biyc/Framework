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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blaze.Common;
using Blaze.Core;
using Blaze.Manage.Data;
using Blaze.Manage.Download;
using Blaze.Manage.Progress;
using Blaze.Resource.AssetBundles.Data;
using Blaze.Resource.AssetBundles.Logic;
using Blaze.Resource.AssetBundles.Logic.StreamingAssets;
using Blaze.Resource.Common;
using Blaze.Resource.Poco;
using Blaze.Utility;
using Blaze.Utility.Base;
using Blaze.Utility.Extend;
using Blaze.Utility.Helper;
using ETModel;
using Model.Module.Blaze.Resource.AssetBundles.Data;
using UniRx;
using UniRx.WebRequest;
using UnityEngine;
using PathHelper = Blaze.Utility.Helper.PathHelper;

namespace Blaze.Resource.AssetBundles
{
    /// <summary>
    /// 包管理器
    /// </summary>
    public sealed class BundleHotfix : Singeton<BundleHotfix>
    {
        //// 资源存储路径
        // .../Bundle/iOS
        private string _resBasePath;

        public string ResBasePath => _resBasePath;

        /// 网络基础路径
        /// $"http://192.168.8.199:8088/iOS"
        private string _netBasePath;

        public string NetBasePath => _netBasePath;

        private LocalHotfixVersion _hotfixVersion;

        /// streamAsset 文件夹中带的资源描述文件
        private ManifestInfo _saMf = new ManifestInfo();

        public ManifestInfo StreamMf => _saMf;

        /// streamAsset 本次启动最终使用的 mf 描述文件
        private ManifestInfo _startMf;

        // 本次启动，运行的目标平台
        private EnumRuntimeTarget _runtimeTarget;

        public LocalHotfixVersion GetVersion() => _hotfixVersion;


        /// <summary>
        /// 获取可用的装配文件
        /// </summary>
        /// <returns></returns>
        public ManifestInfo GetManifestInfo() => _startMf;

        /// 当前mf下载的基础路径
        public string GetBasePath()
        {
            // Bundle/iOS/1.1/
            return PathHelper.Combine(_resBasePath, _hotfixVersion.AbleVersion.Version());
        }


        ///  查找可用的网络连接
        private Task<string> SelectUri(string target)
        {
            var task = new TaskCompletionSource<string>();
            int total = DefaultRuntime.ServerURIList.Count;
            int errCount = 0;
            var reqs = new List<IDisposable>();
            DefaultRuntime.ServerURIList.ForEach(delegate(string serverUri)
            {
                var req = ObservableWebRequest.Get(PathHelper.Combine(serverUri, target, "VersionCheck.json"))
                    .Subscribe(delegate(string s)
                    {
                        // 最先回来的 uri
                        task.SetResult(serverUri);
                        // 终止释放所有的连接
                        reqs.ForEach(delegate(IDisposable disposable) { disposable.Dispose(); });
                    }, delegate(Exception exception)
                    {
                        errCount++;
                        // 当所有的网络连接都错误时，抛出异常
                        if (total == errCount)
                            task.SetException(exception);
                    });
                reqs.Add(req);
            });
            return task.Task;
        }

        public async void Init(Action<bool> callback)
        {
            _runtimeTarget = EnumConvert.PlatformToRuntimeTarget(Application.platform);
            // 初始化资源存储路径  .../Bundle/iOS
            _resBasePath = PathHelper.Combine(PathHelper.GetPersistentBundlePath(), _runtimeTarget.ToString());
            try
            {
                //  查找可用的网络连接
                DefaultRuntime.ServerURI = await SelectUri(_runtimeTarget.ToString());
            }
            catch (Exception e)
            {
                Tuner.Log("服务器不可用，选择线路失败");
            }

            DefaultRuntime.ServerURI = "http://192.168.8.6:8088/EditorWin64Dev";
            // http://192.168.8.199:8088/iOS/
            _netBasePath = PathHelper.Combine(DefaultRuntime.ServerURI, _runtimeTarget.ToString());
            Tuner.Log(_netBasePath);
            PathHelper.CheckOrCreate(_resBasePath);

            // 加载本地热更信息
            _hotfixVersion = LocalHotfixVersion.Load(_resBasePath);
            // 第一次安装游戏，或热更文件夹被删除
            if (_hotfixVersion == null)
            {
                _hotfixVersion = new LocalHotfixVersion();
                _hotfixVersion.Config(_resBasePath);
                _hotfixVersion.Save();
            }


            // 即将进入的游戏版本
            // 上一次的可用版本
            VersionInfo gameVersion = _hotfixVersion.AbleVersion;

            // 访问远程服务器的 VersionCheck.json 获取最新的版本信息
            var lastVersion = await LastVersion();
            // 远端最新版本，比本地当前可用版本 新
            if (lastVersion != null && lastVersion.Version.IsNew(_hotfixVersion.AbleVersion))
            {
                gameVersion = lastVersion.Version;
            }


            bool isNeedDown = true;
            // 从 StreamAsset 中读取 mf 文件 , 记录SA中包含的最新资版本源到 LocalHotfixVersion 版本管理文件中
            try
            {
                var text = await StreamingAssetsLoader._.Load(DefaultRuntime.ServerManifest);
                if (text != null)
                {
                    _saMf = PersistHelper.FromJson<ManifestInfo>(text);
                    _hotfixVersion.BaseVersion = _saMf.Version;
                    _hotfixVersion.Save();

                    // 母包中的版本比网络上的版本更新（当本地母包更新时，就会出现这种情况）
                    if (_saMf.Version.IsNew(gameVersion) || _saMf.Version.IsEqu(gameVersion))
                    {
                        gameVersion = _saMf.Version;
                        _hotfixVersion.AbleVersion = _hotfixVersion.BaseVersion;
                        _hotfixVersion.Save();
                        _startMf = _saMf;
                        // 母包中包含所有最新资源，不用下载最新资源
                        isNeedDown = false;
                    }
                }
            }
            catch (Exception e)
            {
                // 从本地 SA 中读取加载 mf 文件失败，可能是 sa 中没有文件
                Tuner.Log("StreamingAssets 中没有加载到装配文件，母包中不含资源");
            }

            // 下载全新版本(老版本做文件的查缺补漏)
            if (isNeedDown)
                await DownNewVersion(gameVersion);

            callback?.Invoke(true);
        }

        // 访问远程服务器的 VersionCheck.json 获取最新的版本信息
        private async Task<VersionCheck> LastVersion()
        {
            var versionCheckTask = new TaskCompletionSource<VersionCheck>();
            try
            {
                // "http://127.0.0.1:8088/iOS/VersionCheck.json";
                var task = new TaskCompletionSource<string>();
                ObservableWebRequest.Get(PathHelper.Combine(_netBasePath, "VersionCheck.json"))
                    .Subscribe(task.SetResult, task.SetException);
                var vc = PersistHelper.FromJson<VersionCheck>(await task.Task);
                versionCheckTask.SetResult(vc);
            }
            catch (Exception e)
            {
                // 是否强制检查版本
                if (DefaultRuntime.IsForceCheckVersion)
                {
                    _tip.Invoke("获取版本信息失败，请检查网络连接", async delegate { versionCheckTask.SetResult(await LastVersion()); },
                        async delegate { versionCheckTask.SetResult(await LastVersion()); });
                }
                else
                {
                    versionCheckTask.SetResult(null);
                }
            }

            return await versionCheckTask.Task;
        }


        // 下载文件，如果失败则弹出提示框，点确定后重新开始下载
        public async Task<bool> DownBundleMust(List<ManifestData> waitDown, string baseUri, string basePath)
        {
            var task = new TaskCompletionSource<bool>();
            try
            {
                task.SetResult(await DownBundle(waitDown, baseUri, basePath));
            }
            catch (Exception e)
            {
                Debug.LogError("无法连接到网络服务器，重新连接" + "--" + baseUri);
                // task.SetResult(await DownBundleMust(waitDown, baseUri, basePath));
                // _tip.Invoke("无法连接到网络服务器，是否尝试重新连接？",
                //     async delegate { task.SetResult(await DownBundleMust(waitDown, baseUri, basePath)); },
                //     async delegate { task.SetResult(await DownBundleMust(waitDown, baseUri, basePath)); });
            }

            return await task.Task;
        }

        // 下载全新版本
        private async System.Threading.Tasks.Task DownNewVersion(VersionInfo version)
        {
            // 记录正在下载中的版本
            _hotfixVersion.DownVersion = version;
            _hotfixVersion.Save();

            // 根据版本信息，下载 mf 描述文件到本地

            // http://127.0.0.1:8088/iOS/1.1.13
            var netVersionPath = PathHelper.Combine(_netBasePath, version.FullVersion());

            // Bundle/iOS/1.1.13/BundleManifest.json
            var filePath = PathHelper.Combine(_resBasePath, version.FullVersion(), "BundleManifest.json");

            // 报告进度：开始检查版本
            ProgressManager._.ReportStart(ProgressPoint.CheckVersion);

            ManifestInfo mf = null;
            try
            {
                // 尝试读取 mf 文件
                var preRead = new TaskCompletionSource<string>();
                ObservableWebRequest.Get("file://" + filePath).Subscribe(preRead.SetResult, preRead.SetException);
                mf = PersistHelper.FromJson<ManifestInfo>(await preRead.Task);
            }
            catch (Exception e)
            {
                Tuner.Log($"从磁盘没有加载到指定版本的装配文件 {filePath}");
            }

            // 装配文件不存在，或者之前下载的装配文件损坏，需要重新下载装配文件
            if (mf == null)
            {
                var downTask = new TaskCompletionSource<DownTaskInfo>();
                // 开始下载文件
                // http://127.0.0.1:8088/iOS/1.1.13/BundleManifest.json
                Download.Add(PathHelper.Combine(netVersionPath, "BundleManifest.json"), filePath, true,
                    downTask.SetResult, downTask.SetException);
                // 等待文件下载完成
                await downTask.Task;

                // 读取加载刚下载的 mf 文件
                var task = new TaskCompletionSource<string>();
                ObservableWebRequest.Get("file://" + filePath).Subscribe(task.SetResult, task.SetException);
                mf = PersistHelper.FromJson<ManifestInfo>(await task.Task);
            }

            _startMf = mf;

            var saFiles = _saMf.ManifestList.ConvertAll(input => input.Hash);

            // Bundle/iOS/1.1  存放 bundle 的文件夹
            var downPath = PathHelper.Combine(_resBasePath, version.Version());

            // 已检查数量
            var checkSuccessNum = 0;
            var totalCheck = mf.ManifestList.Count;
            var waitDownTask = new TaskCompletionSource<List<ManifestData>>();

            // 加载本地 MD5 效验信息
            var passFileInfo = PassFileInfo.Load(_resBasePath);
            // MD5 效验信息 不存在时，创建效验信息
            if (passFileInfo == null)
            {
                passFileInfo = new PassFileInfo();
                passFileInfo.Config(_resBasePath);
                passFileInfo.Save();
            }

            // 在独立的线程中计算一下文件MD5
            new Thread(new ThreadStart(delegate
            {
                var passFileHash = new HashSet<string>();
                passFileInfo.FilesHash.ForEach(delegate(string s) { passFileHash.Add(s); });

                var isNeedRefreshPassInfo = false;

                var waitHash = new HashSet<string>();
                // 等待下载的资源
                var waitDown2 = mf.ManifestList.FindAll(delegate(ManifestData data)
                {
                    checkSuccessNum++;
                    //  该路径下的资源不需要下载，用到的时候再动态下载
                    if (data.AssetPath.StartsWith("Assets/Projects/Prefabs/")) return false;

                    // streamAsset静态文件中有，不下载
                    if (saFiles.Contains(data.Hash))
                        return false;
                    // 已经在下载队列中等待的，不在重复添加任务
                    if (waitHash.Contains(data.Hash)) return false;

                    // [补丁代码] 以前在根目录的文件移动到子目录中
                    // var targetRoot = PathHelper.Combine(downPath, data.Hash);
                    // if (File.Exists(targetRoot))
                    // {
                    //     // 检查并创建二级目录
                    //     data.CheckSubPath(downPath);
                    //     var targetSub = PathHelper.Combine(downPath, data.GetSaveSubPath());
                    //     File.Move(targetRoot, targetSub);
                    // }

                    // 文件存储路径修改
                    var target = PathHelper.Combine(downPath, data.GetSaveSubPath());
                    // 已经下载并且可以通过效验,
                    if (File.Exists(target))
                    {
                        // 曾经通过效验，不下载
                        if (passFileHash.Contains(data.Md5))
                        {
                            return false;
                        }

                        // 通过效验，不下载
                        if (data.Md5 == CryptoHelper.FileMD5(target))
                        {
                            passFileHash.Add(data.Md5);
                            passFileInfo.FilesHash.Add(data.Md5);
                            isNeedRefreshPassInfo = true;
                            return false;
                        }
                    }

                    //  Debug.Log(data.AssetPath);
                    // 将文件加入等待下载的队列中
                    waitHash.Add(data.Hash);
                    return true;
                });

                // 保存MD5效验信息
                if (isNeedRefreshPassInfo)
                {
                    passFileInfo.Save();
                }

                passFileHash.Clear();

                // 整理完全部信息，返回给上层下载器，开始下载
                waitDownTask.SetResult(waitDown2);
            })).Start();

            // 开启进度条报告任务，刷新检查文件版本的进度条任务
            var checkTask = Observable.Interval(TimeSpan.FromMilliseconds(50f)).Subscribe(delegate(long l)
            {
                ProgressManager._.ReportSub(ProgressPoint.CheckVersion, totalCheck, checkSuccessNum);
            });
            var waitDown = await waitDownTask.Task;
            // 检查完成，关闭检查任务
            checkTask?.Dispose();
            ProgressManager._.ReportFinish(ProgressPoint.CheckVersion);

            try
            {
                // 下载 bundle
                await DownBundleMust(waitDown, netVersionPath, downPath);
            }
            catch (Exception e)
            {
                Tuner.Log("下载发生异常" + e.StackTrace);
                return;
            }

            // 更新版本信息
            _hotfixVersion.AbleVersion = _hotfixVersion.DownVersion;
            if (!_hotfixVersion.HistoryVersion.Contains(_hotfixVersion.DownVersion.FullVersion()))
                _hotfixVersion.HistoryVersion.Add(_hotfixVersion.DownVersion.FullVersion());
            _hotfixVersion.DownVersion = new VersionInfo();
            _hotfixVersion.Save();
        }

        // 根据要求下载 ab 包
        private async Task<bool> DownBundle(List<ManifestData> waitDown, string baseUri, string basePath)
        {
            ProgressManager._.ReportStart(ProgressPoint.DownLoadAb);

            var downCompletion = new TaskCompletionSource<bool>();
            var task = new TaskQueue();
            task.OnFinish += delegate
            {
                downCompletion.SetResult(true);
                // 下载AB完成
                ProgressManager._.ReportFinish(ProgressPoint.DownLoadAb);
                // 停止统计网速
                Download.StopSpeedCount();
                // Download.StopAll();
            };

            var onError = new DataWatch<Exception>();
            // 如果发生错误，只执行一次
            onError.OnCompleted += delegate(Exception exception)
            {
                // 停止所有下载任务
                task.Clear();
                Download.StopAll();
                // 停止统计网速
                Download.StopSpeedCount();
                // 下载出错
                downCompletion.SetException(exception);
            };

            long secDownSize = 0;
            //  总量与已下载量
            long total = 0;
            long downSize = 0;

            // 开始统计网速
            Download.CleanSpeedCount();
            Download.StartSpeedCount();
            Download.OnSpeed += delegate(long secSize, long downSize)
            {
                secDownSize = secSize;

                var txt =
                    $"{Math.Round(secDownSize / 1024f)} KB/S （{Math.Round(downSize / 1024f / 1024f, 2)} MB / {Math.Round(total / 1024f / 1024f, 2)} MB）";
                Debug.Log(txt);
                // 更新下载进度条
                ProgressManager._.ReportSub(ProgressPoint.DownLoadAb, total, downSize, txt);
            };


            waitDown.ToList().ForEach(delegate(ManifestData data) { total += data.Size; });
            // 弹出下载提示框
            if (waitDown.Count > 0)
                await IsDown(total);
            waitDown.ForEach(delegate(ManifestData data)
            {
                // Log.Info("下载文件" + data.Hash);
                // Bundle/iOS/1.1/Hash
                data.CheckSubPath(basePath);
                var savePath = PathHelper.Combine(basePath, data.GetSaveSubPath());
                var url = PathHelper.Combine(baseUri, data.Hash);
                // 添加到下载队列开始下载
                var down = Download.Add(url, savePath, true,
                    OnCompleted: delegate(DownTaskInfo info) { waitDown.Remove(data); },
                    OnError: delegate(Exception exception) { onError.ChangeNotify(exception); });
                task.AddTask(delegate(Action action)
                {
                    down.OnCompleted += delegate(DownTaskInfo info)
                    {
                        // Debug.Log("下载ab文件完成：" + data.AssetPath);
                        action?.Invoke();
                    };
                });
            });

            Log.Info("开始下载AB文件:" + waitDown.Count);
            task.Start();
            return await downCompletion.Task;
        }

        private bool _isAgreeDown = false;


        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="size"></param>
        private async Task<bool> IsDown(long size)
        {
            if (_isAgreeDown) return true;

            var task = new TaskCompletionSource<bool>();
            string netEvn = "移动数据网络";
            if (NetHelper.NetworkIsWifi())
                netEvn = "WIFI环境";
            _isAgreeDown = true;
            task.SetResult(true);
            // _tip.Invoke($"检测到{Math.Round(size / 1024f / 1024f, 2)}MB资源更新，当前为{netEvn}，是否更新资源？",
            //     async delegate
            //     {
            //         _isAgreeDown = true;
            //         task.SetResult(true);
            //     },
            //     async delegate
            //     {
            //         // 不同意在提示一遍，直到同意为止
            //         task.SetResult(await IsDown(size));
            //     });
            return await task.Task;
        }

        #region 提示监听

        /// <summary>
        /// 添加兴趣者 第一次不通知数据，只在变化时调用
        /// </summary>
        public delegate void Tip(string tip, Action okCb, Action failCb);

        private Tip _tip;

        public event Tip OnTip
        {
            add => _tip += value;
            remove => _tip -= value;
        }

        #endregion


        public async Task<bool> LoadTarget(string assetPath)
        {
            var p = assetPath.Replace(".fbx", "").Split('/');
            var name = p[p.Length - 1];
            var netPath = "http://192.168.8.6:808/EditorWin64Dev/EditorWin64/PrefabBundles/" + name;
            var downCompletion = new TaskCompletionSource<bool>();

            var localManifestPath = PathHelper.Combine(_resBasePath, name, "BundleManifest.json");
            Debug.Log(localManifestPath);
            var downRemoteManifest = new TaskCompletionSource<string>();
            ObservableWebRequest.Get(PathHelper.Combine(netPath, "BundleManifest.json"))
                .Subscribe(s => downRemoteManifest.SetResult(s), exception =>
                {
                    Debug.Log("远程装配文件下载失败,加载本地装配文件");
                    var info = ManifestInfo.Load(localManifestPath);
                    if (info == null)
                    {
                        Debug.LogError("本地装配文件不存在");
                        downCompletion.SetResult(false);
                    }
                    else
                    {
                        BundleManager._.AddManifest(info);
                        downCompletion.SetResult(true);
                    }
                    // ObservableWebRequest.Get("file://" + localManifestPath).Subscribe(
                    //     s =>
                    //     {
                    //         BundleManager._.AddManifest(PersistHelper.FromJson<ManifestInfo>(s));
                    //         downCompletion.SetResult(true);
                    //     }, exception =>
                    //     {
                    //         downCompletion.SetResult(false);
                    //         Debug.LogError($"资源：{name} 远程，本地加载均出现错误，请检查！");
                    //     });
                });

            //远端装配文件
            var remoteManifest = PersistHelper.FromJson<ManifestInfo>(await downRemoteManifest.Task);
            BundleManager._.AddManifest(remoteManifest);
            // PathHelper.CheckOrCreate(localManifestPath);
            remoteManifest.Config(localManifestPath);
            remoteManifest.Save();

            var saFiles = StreamMf.ManifestList.ConvertAll(input => input.Hash);

            // // Bundle/iOS/1.1  存放 bundle 的文件夹
            var downPath = PathHelper.Combine(ResBasePath, GetVersion().AbleVersion.Version());

            var waitDownTask = new TaskCompletionSource<List<ManifestData>>();
            // 加载本地 MD5 效验信息
            var passFileInfo = PassFileInfo.Load(ResBasePath);
            // // 在独立的线程中计算一下文件MD5
            new Thread(new ThreadStart(delegate
            {
                var passFileHash = new HashSet<string>();
                passFileInfo.FilesHash.ForEach(delegate(string s) { passFileHash.Add(s); });

                var isNeedRefreshPassInfo = false;

                var waitHash = new HashSet<string>();
                // 等待下载的资源
                var waitDown = remoteManifest.ManifestList.FindAll(delegate(ManifestData data)
                {
                    // streamAsset静态文件中有，不下载
                    if (saFiles.Contains(data.Hash))
                        return false;
                    // 已经在下载队列中等待的，不在重复添加任务
                    if (waitHash.Contains(data.Hash)) return false;

                    // 文件存储路径修改
                    var target = PathHelper.Combine(downPath, data.GetSaveSubPath());
                    // 已经下载并且可以通过效验,
                    if (File.Exists(target))
                    {
                        // 曾经通过效验，不下载
                        if (passFileHash.Contains(data.Md5))
                        {
                            return false;
                        }

                        // 通过效验，不下载
                        if (data.Md5 == CryptoHelper.FileMD5(target))
                        {
                            passFileHash.Add(data.Md5);
                            passFileInfo.FilesHash.Add(data.Md5);
                            isNeedRefreshPassInfo = true;
                            return false;
                        }
                    }

                    // 将文件加入等待下载的队列中
                    waitHash.Add(data.Hash);
                    return true;
                });

                // 保存MD5效验信息
                if (isNeedRefreshPassInfo)
                {
                    passFileInfo.Save();
                }

                passFileHash.Clear();

                // 整理完全部信息，返回给上层下载器，开始下载
                waitDownTask.SetResult(waitDown);
            })).Start();

            var waitDown = await waitDownTask.Task;
            try
            {
                // 下载 bundle
                downCompletion.SetResult(await DownBundleMust(waitDown, netPath, downPath));
                //return true;
            }
            catch (Exception e)
            {
                Tuner.Log("下载发生异常,使用本地版本" + e.StackTrace);
                downCompletion.SetResult(true);
                // downCompletion.SetResult(false);
                //return false;
            }

            //   await downCompletion.Task;
            return await downCompletion.Task;
        }
    }
}