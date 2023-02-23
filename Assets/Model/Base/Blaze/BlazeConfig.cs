//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/24 | Initialize core skeleton |
*/

using System.Diagnostics;
using Blaze.Common;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 应用配置重载
    /// </summary>
    public static class BlazeConfig
    {
        public static void Config()
        {
            SectionSystem();
            SectionDebug();
            SectionGameSettings();
        }

        private static void SectionGameSettings()
        {
            if (Define.GameSettings == null) return;
            // 是否强制检查版本
            DefaultRuntime.IsForceCheckVersion = Define.GameSettings.IsForceCheckVersion;
            // 下载服务器地址
            if (Define.GameSettings.ResServerList.Count > 0)
            {
                DefaultRuntime.ServerURI = Define.GameSettings.ResServerList[0];
                DefaultRuntime.ServerURIList = Define.GameSettings.ResServerList;
            }
        }

        /// <summary>
        /// 重载系统参数
        /// </summary>
        private static void SectionSystem()
        {
            // 开发期根据启动参数选择模式
            if (Define.UseAB)
                DefaultRuntime.RuntimeEnvMode = EnumEnvMode.HotfixRun;
            else
                DefaultRuntime.RuntimeEnvMode = EnumEnvMode.AssetDatabase;

            // 运行在正式模式，强制使用 AB模式
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                case RuntimePlatform.WebGLPlayer:
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXPlayer:
                    DefaultRuntime.RuntimeEnvMode = EnumEnvMode.HotfixRun;
                    break;
            }

            Application.targetFrameRate = 60;
            Application.runInBackground = true;
        }

        /// <summary>
        /// 重载调试参数
        /// </summary>
        private static void SectionDebug()
        {
            DefaultDebug.AllowDebugInfo = true;
            DefaultDebug.AllowVerboseInfo = false;
            DefaultDebug.ShowLoadResource = false;
            DefaultDebug.AllowBundleInfo = false;
        }
    }
}