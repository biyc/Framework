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
using Blaze.Manage.Data;
using Blaze.Utility;
using UnityEngine.Networking;

namespace Blaze.Manage.Download
{
    /// <summary>
    /// 下载状态 接口定义
    /// </summary>
    public class DownTaskInfo : DataWatch<DownTaskInfo>
    {
        public DownTaskInfo()
        {
            Put(this);
        }

        // 下载任务编号
        public int Id;

        /// <summary>
        /// 是否下载中
        /// </summary>
        public bool IsRunning;

        /// <summary>
        /// 下载百分比
        /// </summary>
        public float Progress;

        /// <summary>
        /// 已下载字节数量
        /// </summary>
        public ulong DownSize;

        /// <summary>
        /// 下载总大小
        /// </summary>
        public long Size;

        /// <summary>
        /// 下载路径
        /// </summary>
        public string Path;

        /// <summary>
        /// 下载Url
        /// </summary>
        public string Url;

        /// <summary>
        /// 是否已经下载完成
        /// </summary>
        public bool isDone;

        /// <summary>
        /// 是否手动终止
        /// </summary>
        public bool isAbort;

        /// <summary>
        /// 下载器的引用句柄
        /// </summary>
        public UnityWebRequest downloader;

        /// <summary>
        /// 只在加载完成后通知一次
        /// </summary>
        public Action<Exception> OnError;

        public string GetProgress()
        {
            return (Progress * 100).ToString("F2") + "%";
        }

        /// <summary>
        /// 重写消息变化通知
        /// </summary>
        public override void ChangeNotify()
        {
            try
            {
                _callbacks?.Invoke(_data);
            }
            catch (Exception e)
            {
                Tuner.Log($"[>>] {e}");
            }
        }

        /// <summary>
        /// 重写完成通知
        /// </summary>
        public override void Complet(DownTaskInfo data)
        {
            _loaded = true;
            _completCb?.Invoke(_data);
            _completCb = null;
        }

        // 终止当前下载任务
        public void Abort()
        {
            downloader?.Abort();
            isAbort = true;
        }
    }
}