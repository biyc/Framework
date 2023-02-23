//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/13 | Initialize core skeleton |
*/

using System;

namespace Blaze.Common
{
    /// <summary>
    /// 版本
    /// </summary>
    [Serializable]
    public class VersionInfo
    {
        // 主版本号，与母包版本进行匹配
        public int Major { get; set; }

        // 次版本号，升级时相对上一个资源版本相当于全量更新（_Cache 目录中的AB包开始全量编译）
        public int Minor { get; set; }

        // 构建版本，日常热更升级版本
        public int Build { get; set; }

        // 渠道名称
        public string Channel { get; set; }

        public VersionInfo()
        {
            Major = 0;
            Minor = 0;
            Build = 0;
        }

        public VersionInfo(int major, int minor, int build = 0)
        {
            Major = major;
            Minor = minor;
            Build = build;
        }

        /// <summary>
        /// 获取 1.1
        /// </summary>
        /// <returns></returns>
        public string Version()
        {
            return $"{Major}.{Minor}";
        }

        /// <summary>
        /// 获取 1.1.10000
        /// </summary>
        /// <returns></returns>
        public string FullVersion()
        {
            return $"{Major}.{Minor}.{Build}";
        }

        public override string ToString()
        {
            return $"Version {Channel} {Major}.{Minor}.{Build}";
        }

        /// <summary>
        /// 当前自身版本  是否比 输入的版本 更新
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool IsNew(VersionInfo info)
        {
            if (info.Major < Major)
                return true;

            if (info.Major == Major && info.Minor < Minor)
                return true;

            // 比较 build 版本的前提一定是 主 副 版本一致，否则会出问题
            if (info.Major == Major && info.Minor == Minor && info.Build < Build)
                return true;
            return false;
        }

        /// <summary>
        /// 版本一致
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool IsEqu(VersionInfo info)
        {
            if (info.Major == Major && info.Minor == Minor && info.Build == Build)
                return true;
            return false;
        }
    }
}