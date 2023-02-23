//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/06 | Initialize core skeleton |
*/

using Blaze.Common;
using Blaze.Utility.Extend;
using UnityEngine;

namespace Blaze.Utility.Helper
{
    /// <summary>
    /// 版本工具
    /// </summary>
    public static class VersionHelper

    {
        /// <summary>
        /// 获取Unity的版本号
        /// </summary>
        /// <returns></returns>
        public static VersionInfo UnityVersion()
        {
            return Parse(Application.unityVersion);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsureLoaded()
        {
        }

        /// <summary>
        /// 判断版本大于
        /// </summary>
        /// <param name="info"></param>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <returns></returns>
        public static bool IsGreater(this VersionInfo info, int major, int minor)
        {
            return info.Major > major || (info.Major == major && info.Minor > minor);
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="info"></param>
        /// <param name="build"></param>
        /// <returns></returns>
        public static bool IsGreater(this VersionInfo info, int build)
        {
            return info.Build > build;
        }

        /// <summary>
        /// 解析版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static VersionInfo Parse(string version)
        {
            var vers = version.ToIntArray('.');
            if (vers.Length == 3)
                return new VersionInfo(vers[0], vers[1], vers[2]);
            if (vers.Length == 2)
                return new VersionInfo(vers[0], vers[1]);
            return new VersionInfo(version.ToInt(), 0);
        }
    }
}