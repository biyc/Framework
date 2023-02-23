//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/04/01 | Initialize core skeleton |
*/

using System;
using System.Diagnostics;

namespace Blaze.Manage.Spring
{
    /// <summary>
    /// 注入式管理基类
    /// </summary>
    public interface SpringHolder
    {
        /// <summary>
        /// 获取注册类型
        /// </summary>
        /// <returns></returns>
        Type GetRegisterType();

        /// <summary>
        /// 处理已经发现的对象
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="bean"></param>
        void Process(Attribute attr, Type bean);
    }
}