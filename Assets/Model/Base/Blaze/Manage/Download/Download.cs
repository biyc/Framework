using System;
using UnityEngine;

namespace Blaze.Manage.Download
{
    public static class Download
    {
        /// <summary>
        /// 添加下载任务
        /// </summary>
        /// <param name="url">下载链接</param>
        /// <param name="savePath">目标存存储目录</param>
        /// <param name="force">强制下载，不进行断点续传</param>
        /// <returns></returns>
        public static DownTaskInfo Add(string url, string savePath, bool force = false,
            Action<DownTaskInfo> OnCompleted = null, Action<Exception> OnError = null)
        {
            var temp = DownloadManager._.AddDownloadTask(url, savePath, force);
            if (OnCompleted != null)
                temp.OnCompleted += OnCompleted;
            if (OnError != null)
                temp.OnError += OnError;
            return temp;
        }


        /// <summary>
        /// 停止所有下载任务
        /// </summary>
        public static void StopAll()
        {
            DownloadManager._.Abort();
        }


        /// <summary>
        /// 请求图片
        /// </summary>
        /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
        /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
        /// <returns></returns>
        public static void GetTexture(string path, Action<Texture2D> actionResult)
        {
            DownloadManager._.GetTexture(path, actionResult);
        }

        /// <summary>
        /// 清空已下载任务，重新统计
        /// </summary>
        public static void CleanSpeedCount()
        {
            DownloadManager._.CleanSpeedCount();
        }

        public static void StartSpeedCount()
        {
            DownloadManager._.StartSpeedCount();
        }

        public static void StopSpeedCount()
        {
            DownloadManager._.StopSpeedCount();
        }

        /// <summary>
        /// 添加兴趣者 第一次不通知数据，只在变化时调用
        /// </summary>
        public static event Action<long,long> OnSpeed
        {
            add { DownloadManager._.WatchSpeed += value; }

            remove { DownloadManager._.WatchSpeed -= value; }
        }

        // public static Action<long> WatchSpeed()
        // {
        //     return ;
        // }


        // public static void Demo()
        // {
        //     string url = "https://dldir1.qq.com/weixin/Windows/WeChatSetup.exe";
        //     string path = PathHelper.GetPersistentDnpPath() + "/WeChatSetup.exe";
        //
        //     var task = Download.Add(url, path);
        //     // 下载进度监控
        //     task.OnIneresting += delegate(DownTaskInfo info) { Tuner.Log(info.Url + "  " + info.GetProgress()); };
        //     // 下载完成通知
        //     task.OnCompleted += delegate(DownTaskInfo info) { Tuner.Log("下载完成" + info.Url); };
        // }
    }
}