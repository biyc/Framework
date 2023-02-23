//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | Tim    | 2019/10/18 | Initialize core skeleton |
*/

using System.Threading;
using Blaze.Common;
using UnityEngine;

namespace Blaze.Utility
{
    /// <summary>
    /// 调试器方便开发期调试信息用
    /// </summary>
    public static class Tuner
    {
        /// <summary>
        /// 普通的文本输出,支持格式化
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Log(string format, params object[] args)
        {
// #if DEBUG
            if (DefaultDebug.AllowDebugInfo)
                Debug.unityLogger.LogFormat(LogType.Log, format, args);
// #endif
        }

        public static void Echo(string format, params object[] args)
        {
// #if DEBUG
            if (DefaultDebug.AllowDebugInfo)
                Debug.unityLogger.LogFormat(LogType.Log, Co._($"{format}:blue:b;"), args);
// #endif
        }


        /// <summary>
        /// 详细调试信息输出
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Verbose(string format, params object[] args)
        {
// #if DEBUG
            if (DefaultDebug.AllowDebugInfo && DefaultDebug.AllowVerboseInfo)
                Debug.unityLogger.LogFormat(LogType.Log, format, args);
// #endif
        }

        /// <summary>
        /// 警告信息,不建议使用,因为Unity的运行期警告过多
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Warn(string format, params object[] args)
        {
// #if DEBUG
            if (DefaultDebug.AllowDebugInfo)
                Debug.unityLogger.LogFormat(LogType.Warning, format, args);
// #endif
        }

        /// <summary>
        /// 调试期间出现重大问题建议使用,防止出现运营事故
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Error(string format, params object[] args)
        {
            Debug.unityLogger.LogFormat(LogType.Error, format, args);
        }

        /// <summary>
        /// 调试器的语法糖,方便生成可以识别的输出区域
        /// </summary>
        /// <param name="info"></param>
        public static void Inco(string info)
        {
// #if DEBUG
            Debug.unityLogger.LogFormat(LogType.Log, ("+----------------------------------------+"));
            Debug.unityLogger.LogFormat(LogType.Log, "|" + Co._("{0}:green:b;"),
                info.PadLeft(20 + info.Length / 2).PadRight(40));
            Debug.unityLogger.LogFormat(LogType.Log, ("+----------------------------------------+"));
// #endif
        }

        /// <summary>
        /// 调试器的语法糖,方便生成可以识别的输出区域
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
// #if DEBUG
            Debug.unityLogger.LogFormat(LogType.Log, ("+----------------------------------------+"));
            Debug.unityLogger.LogFormat(LogType.Log, "|" + "{0}",
                info.PadLeft(20 + info.Length / 2).PadRight(40));
            Debug.unityLogger.LogFormat(LogType.Log, ("+----------------------------------------+"));
// #endif
        }


        public static void PrintThread()
        {
// #if DEBUG
// Thread
            Debug.unityLogger.Log("当前线程ID:" + Thread.CurrentThread.ManagedThreadId);
// #endif
        }
    }
}