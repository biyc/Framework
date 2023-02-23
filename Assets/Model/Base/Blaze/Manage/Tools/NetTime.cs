using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Blaze.Common;
using Blaze.Resource.Task;
using UnityEngine.Networking;

/// <summary>
/// 通过服务器获取时间
/// </summary>
public class NetTime
{
    #region Singleton

    private static readonly NetTime _instance = new NetTime();

    public static NetTime _ => _instance;

    #endregion

    /// <summary>
    /// 缓存是否过期
    /// </summary>
    private bool CacheIsExpired = true;

    /// 向服务器同步数据时的时间
    private DateTime LocalStartTime;

    /// 服务器返回的时间点
    private DateTime CacheServerTime;


    /// <summary>
    /// 获取时间，通过本地缓存计算获得
    /// </summary>
    /// <returns></returns>
    public DateTime GetTime()
    {
        if (CacheIsExpired)
        {
            Debug.Log("不可用，使用同步方法初始化");
            CacheServerTime = GetServerTimeSync();
            LocalStartTime = DateTime.Now;
            CacheIsExpired = false;
        }

        // 缓存时间可用，直接返回
        // Debug.Log("缓存时间可用，直接返回");
        return CacheServerTime + (DateTime.Now - LocalStartTime);
    }

    /// <summary>
    /// 设置缓存时间作废
    /// </summary>
    public void Expired()
    {
        CacheIsExpired = true;
    }

    /// <summary>
    /// 通过服务器异步同步时间
    /// </summary>
    public async void Sync()
    {
        try
        {
            Debug.Log("开始同步服务器时间");
            CacheServerTime = await GetServerTime();
            LocalStartTime = DateTime.Now;
            CacheIsExpired = false;
        }
        catch (Exception e)
        {
            // 时间同步错误
        }
    }


    /// <summary>
    /// 同步从服务器获取时间
    /// </summary>
    /// <returns></returns>
    public DateTime GetServerTimeSync()
    {
        foreach (var url in DefaultRuntime.PublicServerURIList)
        {
            WWW www = new WWW(url);
            while (!www.isDone)
            {
            }

            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                Dictionary<string, string> resHeaders = www.responseHeaders;
                string key = "DATE";
                string value = null;
                if (resHeaders != null && resHeaders.ContainsKey(key))
                {
                    resHeaders.TryGetValue(key, out value);
                }

                if (value == null)
                {
                    return DateTime.MinValue;
                }

                return GMT2Local(value);
            }
        }

        return DateTime.MinValue;
    }


    /// <summary>
    /// 获得服务器时间
    /// </summary>
    /// <returns></returns>
    public Task<DateTime> GetServerTime()
    {
        var task = new TaskCompletionSource<DateTime>();
        int total = DefaultRuntime.PublicServerURIList.Count;
        int errCount = 0;
        var reqs = new List<IEnumerator>();
        DefaultRuntime.PublicServerURIList.ForEach(delegate(string serverUri)
        {
            var co = ServerTime(serverUri, delegate(DateTime time)
            {
                task.SetResult(time);
                Debug.Log(serverUri + "   " + time);
                reqs.ForEach(delegate(IEnumerator enumerator) { MonoScheduler.Stop(enumerator); });
            }, delegate(Exception exception)
            {
                errCount++;
                // 当所有的网络连接都错误时，抛出异常
                if (total == errCount)
                    task.SetException(exception);
            });
            // 添加任务
            reqs.Add(co);
        });
        // 开始执行
        reqs.ForEach(delegate(IEnumerator enumerator) { MonoScheduler.DispatchCoroutine(enumerator); });

        return task.Task;
    }


    /// <summary>
    /// 从服务器请求头任务
    /// </summary>
    /// <param name="url"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onFaile"></param>
    /// <returns></returns>
    private IEnumerator ServerTime(string url, Action<DateTime> onSuccess, Action<Exception> onFaile)
    {
        UnityWebRequest huwr = UnityWebRequest.Head(url); //Head方法可以获取到文件的全部长度
        yield return huwr.SendWebRequest();
        if (huwr.isNetworkError || huwr.isHttpError) //如果出错
        {
            onFaile?.Invoke(new Exception("server error"));
            yield break;
        }

        var resHeaders = huwr.GetResponseHeaders();
        var key = "DATE";
        string value = null;
        if (resHeaders != null && resHeaders.ContainsKey(key))
        {
            resHeaders.TryGetValue(key, out value);
        }

        if (value == null)
        {
            onFaile?.Invoke(new Exception("server error"));
            yield break;
        }

        onSuccess.Invoke(GMT2Local(value));
    }

    /// <summary>
    /// GMT时间转成本地时间
    /// </summary>
    /// <param name="gmt">字符串形式的GMT时间</param>
    /// <returns></returns>
    private DateTime GMT2Local(string gmt)
    {
        DateTime dt = DateTime.MinValue;
        try
        {
            string pattern = "";
            if (gmt.IndexOf("+0") != -1)
            {
                gmt = gmt.Replace("GMT", "");
                pattern = "ddd, dd MMM yyyy HH':'mm':'ss zzz";
            }

            if (gmt.ToUpper().IndexOf("GMT") != -1)
            {
                pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
            }

            if (pattern != "")
            {
                dt = DateTime.ParseExact(gmt, pattern, System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AdjustToUniversal);
                dt = dt.ToLocalTime();
            }
            else
            {
                dt = Convert.ToDateTime(gmt);
            }
        }
        catch
        {
            // ignored
        }

        return dt;
    }
}