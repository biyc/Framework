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
using Blaze.Manage.Csv.Enum;
using Blaze.Utility.Helper;
using UnityEngine.UI;

namespace Blaze.Common
{

    /// <summary>
    /// App应用的默认配置
    /// </summary>
    public static class DefaultApp
    {
        /// <summary>
        /// 游戏名字,一般会出现在调试信息中
        /// </summary>
        public static string AppName = "DonopoGame";

        /// 应用程序默认语言
        public static LocaleType LocaleType = LocaleType.zh;

        /// 下面三个数字共同形成唯一的版本号: 1.0.0, 第一个是大版本升级, 第二个是开发里程碑, 第三个每次的Build编号,一般是5位以下
        public static int VersionCode = 1;

        public static int VersionDev = 0;

        public static int VersionBuild = 0;

        private static VersionInfo version;

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns>字符串版本号</returns>
        public static string GetVersionStr()
        {
            return version.FullVersion();
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns>字符串版本对象</returns>
        public static VersionInfo GetVersion()
        {
            return version;
        }

        /// <summary>
        /// 设置版本
        /// </summary>
        /// <param name="vers"></param>
        public static void SetVersion(string vers)
        {
            version = VersionHelper.Parse(vers);
        }

        /// <summary>
        /// 获取当前的版本信息,决定热更资源的比对
        /// </summary>
        public static VersionInfo Info = new VersionInfo(VersionCode, VersionDev, VersionBuild);

        /// <summary>
        /// 是否为发行版,发行版的自动打包程序会自动将这个变量设置为true
        /// </summary>
        public static bool IsReleaseVersion = false;

        /// <summary>
        /// 如果IsReleaseVersion为true,Debug自动失效
        /// </summary>
        public static bool IsDebugVersion = true;


    }

    /// <summary>
    /// Sound音乐音效配置
    /// </summary>
    public static class DefaultSound
    {
        public static decimal VolumeBgmMax = 1;
        public static decimal VolumeEfxMax = 1;
        public static decimal VolumeSndMax = 1;
        public static decimal VolumeVicMax = 1;

        /// <summary>
        /// 默认的按钮音效
        /// </summary>
        public static string DefaultButtonEfx = "";

        /// <summary>
        /// 默认的菜单背景音乐
        /// </summary>
        public static string DefaultMenuBgm = "";

        /// <summary>
        /// 默认的主游戏背景音乐
        /// </summary>
        public static string DefaultCoreBgm = "";
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

        /// <summary>
        /// 显示Debug面板
        /// </summary>
        public static bool EnableDebugPanel = false;

        /// <summary>
        /// 关闭CSV数据缓存
        /// </summary>
        public static bool DisableCsvDataCache = false;

        /// <summary>
        /// 显示状态信息
        /// </summary>
        public static bool EnableStatusInfo = false;

        /// <summary>
        /// 显示FPS信息
        /// </summary>
        public static bool EnableFpsInfo = false;
    }


}