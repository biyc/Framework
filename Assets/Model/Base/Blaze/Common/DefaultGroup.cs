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
using System.Collections.Generic;

namespace Blaze.Common
{
    /// <summary>
    /// Target Local服务器默认配置
    /// </summary>
    public static class DefaultRuntime
    {
        /// 默认的Runtime的工作模式,开发期 AssetDatabase ，发行期是 HotfixRun
        public static EnumEnvMode RuntimeEnvMode = EnumEnvMode.AssetDatabase;

        /// <summary>
        /// 核心下载服务器
        /// </summary>
        // public static string ServerURI = $"http://192.168.8.199:8088/";
        public static string ServerURI = $"http://127.0.0.1:8089/EditorWin64Dev";

        public static List<string> ServerURIList = new List<string>()
        {
            "http://127.0.0.1:8089/EditorWin64Dev", "http://192.168.1.6:8089/EditorWin64Dev",
            "http://192.168.8.6:8089/EditorWin64Dev"
        };


        /// <summary>
        /// 公共服务器列表
        /// 用于检查手机是否联网，获取公共时间等
        /// </summary>
        public static List<string> PublicServerURIList = new List<string>()
        {
            "https://www.baidu.com",
            "https://www.taobao.com",
            "https://www.sohu.com",
            "https://blog.csdn.net",
            "https://www.jianshu.com",
            "https://github.com",
            "http://iyloft.com",
        };

        /// <summary>
        /// 存档服务器地址
        /// </summary>
        // public static string SlotServerURI = $"http://114.116.27.95:14432";
        public static string SlotServerURI = $"http://api.iyloft.com:14432";
        // 更换存档服务器地址
        // public static string SlotServerURI = $"http://114.116.27.95:14429";

        /// <summary>
        /// 核心下载装配文件位置
        /// </summary>
        public static string ServerManifest = "BundleManifest.json";


        /// <summary>
        /// 下载失败重试
        /// </summary>
        public static int DownloadRetry = 3;

        /// <summary>
        /// 并行下载任务
        /// </summary>
        public static int ConcurrentTask = 8;

        /// <summary>
        /// 下载的超时时间
        /// </summary>
        public static int DownloadTimeout = 3;

        /// <summary>
        /// 是否强制检查版本
        /// </summary>
        public static bool IsForceCheckVersion = false;
    }


    /// <summary>
    /// Debug调试信息的配置部分
    /// </summary>
    public static class DefaultDebug
    {
        /// <summary>
        /// 允许调试信息
        /// </summary>
        public static bool AllowBundleInfo = false;

        /// <summary>
        /// 允许调试信息
        /// </summary>
        public static bool AllowDebugInfo = false;

        /// <summary>
        /// 允许详细信息
        /// </summary>
        public static bool AllowVerboseInfo = false;

        /// <summary>
        /// 显示资源加载信息
        /// </summary>
        public static bool ShowLoadResource = false;
    }

    /// <summary>
    /// 加密相关的参数
    /// </summary>
    public static class DefaultCrypto
    {
        public static string Md5SaltPre = "DOnoP0@";
        public static string Md5SaltPost = "C0Ol*1973";
        public static string XxteaPre = "JPG-PNG-MPA";
        public static string XxteaSalt = "WhAzC00l%2o48";
        public static UInt32 XxteaSeed = 0x9E3779B9;
        public static int XxteaMXMagic = 3;
        public static uint MemorySalt = 19730806;
        public static int MomoryShift = 5;
        public static string AES128IV = "iA0MGrSF7l9XPswk";
        public static string AES128Key = "th0r1973o8O6F7l@";
    }
}