//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/18 | Initialize core skeleton |
*/

using Blaze.Common;
using Blaze.Core;

namespace Blaze.Resource.Poco
{
    /// <summary>
    /// 版本检查
    /// </summary>
    [Named("Version Check")]
    [Persist(CamelFileName = true)]
    public class VersionCheck : Persistable<VersionCheck>
    {

        // 版本号
        public VersionInfo Version = new VersionInfo();

        // 构建次数
        public int Build;

        // 上次更新
        public long LastUpdate;

        // 上次操作者
        public string LastOperator;

    }
}