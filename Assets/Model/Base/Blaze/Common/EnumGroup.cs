//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | Tim    | 2019/10/18 | Initialize core skeleton |
*/

using Blaze.Utility.Helper;
using UnityEditor;
using UnityEngine;

namespace Blaze.Common
{
    /// <summary>
    /// 持久化文件的类型
    /// </summary>
    public enum EnumPersistType
    {
        JSON,
        YAML,
    }

    /// <summary>
    /// 资源的加载模式
    /// </summary>
    public enum EnumEnvMode
    {
        AssetDatabase, // AssetDatabase模式
        HotfixRun // 远程Hotfix包模式
    }

    /// <summary>
    /// 程序的运行平台
    /// </summary>
    public enum EnumRuntimeTarget
    {
        EditorOSX, // 编辑器 MacOS
        EditorWin64, // 编辑器 Win64
        IOS, // iOS
        Android, // Android
        WebGL, // H5
        Linux,
        MacOS, // MacOS
        Win64, // Win64
    }

    public static class EnumConvert
    {
        public static EnumRuntimeTarget PlatformToRuntimeTarget(RuntimePlatform platform)
        {
            EnumRuntimeTarget target = EnumRuntimeTarget.Android;
            switch (platform)
            {
                case RuntimePlatform.Android:
                    target = EnumRuntimeTarget.Android;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    target = EnumRuntimeTarget.IOS;
                    break;
                case RuntimePlatform.OSXEditor:
                    target = EnumRuntimeTarget.EditorOSX;
                    break;
                case RuntimePlatform.WindowsEditor:
                    target = EnumRuntimeTarget.EditorWin64;
                    break;
                case RuntimePlatform.WebGLPlayer:
                    target = EnumRuntimeTarget.WebGL;
                    break;
                case RuntimePlatform.LinuxPlayer:
                    target = EnumRuntimeTarget.Linux;
                    break;
                case RuntimePlatform.OSXPlayer:
                    target = EnumRuntimeTarget.MacOS;
                    break;
                case RuntimePlatform.WindowsPlayer:
                    target = EnumRuntimeTarget.Win64;
                    break;
            }

            return target;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 转换为打包的目标平台枚举
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static BuildTarget RuntimeTargetToBuildTarget(EnumRuntimeTarget target)
        {
            BuildTarget platform = BuildTarget.Android;

            switch (target)
            {
                case EnumRuntimeTarget.Android:
                    platform = BuildTarget.Android;
                    break;
                case EnumRuntimeTarget.IOS:
                    platform = BuildTarget.iOS;
                    break;
                case EnumRuntimeTarget.EditorOSX:
                    platform = BuildTarget.StandaloneOSX;
                    break;
                case EnumRuntimeTarget.EditorWin64:
                    platform = BuildTarget.StandaloneWindows64;
                    break;
                case EnumRuntimeTarget.WebGL:
                    platform = BuildTarget.WebGL;
                    break;
                case EnumRuntimeTarget.Linux:
                    platform = BuildTarget.StandaloneLinux64;
                    break;
                case EnumRuntimeTarget.MacOS:
                    platform = BuildTarget.StandaloneOSX;
                    break;
                case EnumRuntimeTarget.Win64:
                    platform = BuildTarget.StandaloneWindows64;
                    break;
            }

            return platform;
        }
#endif
    }
}