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
using System.Collections;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Blaze.Common;
using Blaze.Core;
using Blaze.Manage.Download;
using Blaze.Resource.AssetBundles.Data;
using Blaze.Resource.Task;
using Blaze.Utility;
using Blaze.Utility.Base;
using Blaze.Utility.Helper;
using UniRx;
using UniRx.WebRequest;
using UnityEngine;
using UnityEngine.Networking;

namespace Blaze.Resource.AssetBundles.Logic.StreamingAssets
{
    /// <summary>
    /// 流式资产加载器
    /// </summary>
    public class StreamingAssetsLoader : Singeton<StreamingAssetsLoader>
    {
        /// 流式读取的根目录
        /// StreamAsset/IOS/...
        private string _streamingAssetsPathRoot;

        /// <summary>
        /// 初始化流式加载器
        /// </summary>
        /// <param name="manifest"></param>
        public StreamingAssetsLoader()
        {
            _streamingAssetsPathRoot = PathHelper.Combine(Application.streamingAssetsPath,
                EnumConvert.PlatformToRuntimeTarget(Application.platform).ToString());

            // 根据平台，添加 file:// 前缀
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                    // _streamingAssetsPathRoot = PathHelper.Combine("file://", _streamingAssetsPathRoot);
                    _streamingAssetsPathRoot = "file://" + _streamingAssetsPathRoot;
                    break;
            }

            Tuner.Log("StreamingAssets Path" + _streamingAssetsPathRoot);
        }

        /// <summary>
        /// 读取AB包中的文本文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<string> Load(string fileName)
        {
            var loadPath = PathHelper.Combine(_streamingAssetsPathRoot, fileName);
            var task = new TaskCompletionSource<string>();
            ObservableWebRequest.Get(loadPath).Subscribe(task.SetResult, task.SetException);
            return task.Task;
        }

        /// <summary>
        /// 根据类型加载ZIP包或AB包
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="bundleType"></param>
        /// <param name="callback"></param>
        /// <param name="abCallback"></param>
        public void Load(ManifestData mf, Action<Stream> callback,
            Action<AssetBundle> abCallback)
        {
            MonoScheduler.DispatchCoroutine(LoadBundle(mf, callback, abCallback, null));
        }


        private IEnumerator LoadBundle(ManifestData mf, Action<Stream> callback,
            Action<AssetBundle> abCallback, Action endCb)
        {
            var uri = PathHelper.Combine(_streamingAssetsPathRoot, mf.Hash);

            // Tuner.Log(uri);

            var startTime = TimeHelper.ClientNowSeconds();
            // Debug.Log($"LoadBundle From StreamingAssets Start -------------- {uri} {bundleType}");

            if (mf.Type == BundleType.AssetBundle)
            {
                uint crc = 0;
                // 转换失败使用默认值0
                if (!uint.TryParse(mf.Checksum, out crc)) crc = 0;
                UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(uri, crc);
                {
                    yield return uwr.SendWebRequest();
                    // 下一帧加载成功后，手动释放下载器
                    Observable.NextFrame().Subscribe(delegate(Unit unit)
                    {
                        if (uwr.error == null && uwr.responseCode == 200)
                        {
                            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                            abCallback(bundle);
                        }
                        else
                        {
                            Debug.Log($"LoadBundle From StreamingAssets Error {mf.File} {uwr.error}");
                            callback(null);
                        }

                        uwr.Dispose();
                    });
                }


                endCb?.Invoke();
            }
            else if (mf.Type == BundleType.Zip)
            {
                UnityWebRequest uwr = new UnityWebRequest(uri);
                DownloadHandlerBuffer handler = new DownloadHandlerBuffer();
                uwr.downloadHandler = handler;
                yield return uwr.SendWebRequest();
                endCb?.Invoke();
                MemoryStream stream = null;
                if (uwr.error == null && uwr.responseCode == 200)
                {
                    var dtTime = TimeHelper.ClientNowSeconds() - startTime;
                    Debug.Log($"LoadZip From StreamingAssets Success time: {dtTime}s  path: {uri}");
                    var bytes = handler.data;
                    stream = new MemoryStream(bytes);
                    callback(stream);
                }
                else
                {
                    Debug.Log($"LoadZip From StreamingAssets Error {mf.File} {uwr.error}");
                    callback(null);
                    endCb?.Invoke();
                }
            }
        }

        /// 同步加载文本文件
        public string LoadSync(string fileName)
        {
            var uri = PathHelper.Combine(_streamingAssetsPathRoot, fileName);
            var loadDB = new WWW(uri);
            while (!loadDB.isDone) ;
            return loadDB.text;
        }

        /// <summary>
        /// 根据类型加载ZIP包或AB包
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="bundleType"></param>
        /// <param name="callback"></param>
        /// <param name="abCallback"></param>
        public Stream LoadSyncStream(string fileName)
        {
            var uri = PathHelper.Combine(_streamingAssetsPathRoot, fileName);
            var loadDB = new WWW(uri);
            while (!loadDB.isDone) ;
            return new MemoryStream(loadDB.bytes);
        }

        public AssetBundle LoadSyncAssetBundle(string fileName)
        {
            var uri = PathHelper.Combine(_streamingAssetsPathRoot, fileName);
            WWW loadDB = new WWW(uri);
            while (!loadDB.isDone)
            {
            }

            return loadDB.assetBundle;
        }
    }
}