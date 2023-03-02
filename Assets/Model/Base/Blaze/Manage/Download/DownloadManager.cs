using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blaze.Common;
using Blaze.Core;
using Blaze.Resource.Task;
using Blaze.Utility;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Blaze.Manage.Download
{
    public class DownloadManager : Singeton<DownloadManager>
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        public void Initialize()
        {
            MAX_TASKS = Math.Max(1, Math.Min(DefaultRuntime.ConcurrentTask, 16)); // 并发下载任务数量
            RETRY = DefaultRuntime.DownloadRetry;
            TIMEOUT = DefaultRuntime.DownloadTimeout;
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void OnDestroy()
        {
            Abort();
        }

        // 所有任务
        private List<IEnumerator> _tasks = new List<IEnumerator>();

        // 正在下载中的任务
        private List<DownTaskInfo> _runing = new List<DownTaskInfo>();


        // 最大并行下载任务
        private static int MAX_TASKS = 0;

        // 正在下载中的任务
        private int _curTasks = 0;

        // 重试次数
        private static int RETRY;

        // 超时
        private static int TIMEOUT;

        #region 统计下载速度

        private int _downSeq = 0;

        // 任务ID -> 已下载字节数量
        Dictionary<int, long> _downTotalSize = new Dictionary<int, long>();
        private long preSize;
        public Action<long, long> WatchSpeed;
        private IDisposable _speedTask;

        /// <summary>
        /// 清空已下载任务，重新统计
        /// </summary>
        public void CleanSpeedCount()
        {
            _downTotalSize.Clear();
        }


        /// <summary>
        /// 开启下载速度统计任务
        /// </summary>
        public void StartSpeedCount()
        {
            preSize = _downTotalSize.Values.Sum();
            _speedTask?.Dispose();
            _speedTask = Observable.Interval(TimeSpan.FromSeconds(1f)).Subscribe(delegate(long l)
            {
                var curSize = _downTotalSize.Values.Sum();
                WatchSpeed?.Invoke(curSize - preSize, curSize);
                preSize = curSize;
            });
        }

        /// <summary>
        /// 关闭下载速度统计任务
        /// </summary>
        public void StopSpeedCount()
        {
            _downTotalSize.Clear();
            _speedTask?.Dispose();
            _speedTask = null;
            WatchSpeed = null;
            preSize = 0;
        }

        #endregion


        /// <summary>
        /// 添加下载任务
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <param name="force">强制下载，不进行断点续传</param>
        /// <returns></returns>
        public DownTaskInfo AddDownloadTask(string url, string filePath, bool force = false)
        {
            _downSeq++;
            var taskInfo = new DownTaskInfo();
            taskInfo.Id = _downSeq;
            IEnumerator task = null;
            if (force)
                task = Download(url, filePath, taskInfo);
            else
                task = DownloadContin(url, filePath, taskInfo);

            taskInfo.OnCompleted += delegate(DownTaskInfo info)
            {
                _curTasks--;
                // 开始等待的下载任务
                StartWaitTask();
            };

            taskInfo.OnError += delegate(Exception exception)
            {
                _curTasks--;
                // 开始等待的下载任务
                StartWaitTask();
            };


            // 添加下载任务
            lock (_tasks)
            {
                _tasks.Add(task);
            }

            // 开始等待的下载任务
            StartWaitTask();

            return taskInfo;
        }

        /// <summary>
        /// 开始等待中的任务
        /// </summary>
        private void StartWaitTask()
        {
            if (_curTasks >= MAX_TASKS)
                return;

            lock (_tasks)
            {
                if (_tasks.Count > 0)
                {
                    var task = _tasks[0];
                    _tasks.Remove(task);
                    _curTasks++;
                    MonoScheduler.DispatchCoroutine(task);
                }
            }
        }


        /// <summary>
        /// 终止所有任务
        /// </summary>
        public void Abort()
        {
            _curTasks = 0;
            // 清除等待中的任务
            _tasks.Clear();
            // 终止下载中的任务
            _runing.ForEach(task => task.Abort());
        }


        /// <summary>
        /// 支持断电续传的下载
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private IEnumerator DownloadContin(string url, string filePath, DownTaskInfo taskInfo)
        {
            taskInfo.Url = url;
            taskInfo.Path = filePath;
            UnityWebRequest huwr = UnityWebRequest.Head(url); //Head方法可以获取到文件的全部长度
            yield return huwr.SendWebRequest();
            if (huwr.isNetworkError || huwr.isHttpError) //如果出错
            {
                taskInfo.OnError?.Invoke(new Exception(url + "  " + huwr.error));
                // Debug.Log(url+"  "+huwr.error); //输出 错误信息
            }
            else
            {
                long totalLength = long.Parse(huwr.GetResponseHeader("Content-Length")); //首先拿到文件的全部长度
                taskInfo.Size = totalLength;
                string dirPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dirPath)) //判断路径是否存在
                    Directory.CreateDirectory(dirPath);

                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    long nowFileLength = fs.Length; //当前文件长度
                    if (nowFileLength < totalLength)
                    {
                        var uri = new Uri(url);
                         UnityWebRequest downloader = UnityWebRequest.Get(uri);
                        taskInfo.downloader = downloader;
                        _runing.Add(taskInfo);
                        downloader.SetRequestHeader("Range", "bytes=" + nowFileLength + "-" + totalLength);
                        downloader.downloadHandler = new DownloadHandlerFile(filePath, true); //直接将文件下载到外存
                        downloader.disposeDownloadHandlerOnDispose = true;
                        // downloader.timeout = TIMEOUT;
                        downloader.SendWebRequest();
                        Tuner.Log("开始下载");
                        taskInfo.IsRunning = true;

                        while (!downloader.isDone)
                        {
                            taskInfo.Progress = downloader.downloadProgress;
                            taskInfo.DownSize = downloader.downloadedBytes;
                            _downTotalSize[taskInfo.Id] = (long) taskInfo.DownSize;
                            taskInfo.ChangeNotify();

                            if (downloader.error != null)
                            {
                                taskInfo.OnError?.Invoke(new Exception(downloader.error));
                            }

                            yield return null;
                        }

                        taskInfo.Progress = downloader.downloadProgress;
                        taskInfo.DownSize = downloader.downloadedBytes;
                        _downTotalSize[taskInfo.Id] = (long) taskInfo.DownSize;
                        taskInfo.ChangeNotify();

                        _runing.Remove(taskInfo);
                        if (downloader.error != null)
                        {
                            taskInfo.IsRunning = false;
                            taskInfo.isDone = true;
                            taskInfo.OnError?.Invoke(new Exception(downloader.error));
                            // taskInfo.Complet(taskInfo);
                            // Debug.LogError(downloader.error);
                        }
                        else
                        {
                            taskInfo.IsRunning = false;
                            taskInfo.isDone = true;
                            taskInfo.Complet(taskInfo);
                            Tuner.Log("下载结束");
                        }
                    }
                    else
                    {
                        taskInfo.isDone = true;
                        taskInfo.Complet(taskInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 下载全新的文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private IEnumerator Download(string url, string filePath, DownTaskInfo taskInfo)
        {
            taskInfo.Url = url;
            taskInfo.Path = filePath;
             UnityWebRequest downloader = UnityWebRequest.Get(new Uri(url));
            taskInfo.downloader = downloader;
            _runing.Add(taskInfo);
            downloader.downloadHandler = new DownloadHandlerFile(filePath); //直接将文件下载到外存
            downloader.disposeDownloadHandlerOnDispose = true;
            // downloader.timeout = TIMEOUT;
            downloader.SendWebRequest();

            // Tuner.Log("开始下载");
            taskInfo.IsRunning = true;
            while (!downloader.isDone)
            {
                // Tuner.Log("同步进度条");
                taskInfo.Progress = downloader.downloadProgress;
                taskInfo.DownSize = downloader.downloadedBytes;
                _downTotalSize[taskInfo.Id] = (long) taskInfo.DownSize;
                taskInfo.ChangeNotify();
                yield return null;
            }

            taskInfo.Progress = downloader.downloadProgress;
            taskInfo.DownSize = downloader.downloadedBytes;
            _downTotalSize[taskInfo.Id] = (long) taskInfo.DownSize;
            taskInfo.ChangeNotify();

            _runing.Remove(taskInfo);


            if (downloader.error != null)
            {
                taskInfo.IsRunning = false;
                taskInfo.isDone = false;
                taskInfo.OnError?.Invoke(new Exception(downloader.error));
                yield return null;
            }
            else
            {
                taskInfo.IsRunning = false;
                taskInfo.isDone = true;
                taskInfo.Complet(taskInfo);
                // Tuner.Log("下载结束");
            }
        }


        /// <summary>
        /// 请求图片
        /// </summary>
        /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
        /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
        /// <returns></returns>
        public void GetTexture(string path, Action<Texture2D> actionResult)
        {
            MonoScheduler.DispatchCoroutine(Texture(path, actionResult));
        }

        private IEnumerator Texture(string url, Action<Texture2D> actionResult)
        {
            UnityWebRequest uwr = new UnityWebRequest(url);
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            uwr.downloadHandler = downloadTexture;
            yield return uwr.SendWebRequest();
            Texture2D t = null;
            if (!(uwr.isNetworkError || uwr.isHttpError))
            {
                t = downloadTexture.texture;
            }

            actionResult?.Invoke(t);
        }
    }
}