//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/17 | Initialize core skeleton |
*/

using System.Threading.Tasks;
using Blaze.Bundle.Step;
using Blaze.Core;
using Blaze.Utility;
using UnityEditor;

namespace Blaze.Bundle
{
    /// <summary>
    /// 核心的打包管理器
    /// </summary>
    public class ThorBundleBuilder : Singeton<ThorBundleBuilder>
    {
        /// <summary>
        /// 执行构建
        /// </summary>
        public Task<bool> Execution(BundleBuilderConf conf)
        {
            // 1. 获取当前的版本号,创建版本目录,清除上一次的构建信息
            BuilderStep1._.Execution(conf);
            // 2. 扫描flat数据结构,形成第一手的临时构建
            BuilderStep2._.Execution();
            // 3. 发布到StreamingAssets目录
            return BuilderStep3._.Execution();
        }
    }
}