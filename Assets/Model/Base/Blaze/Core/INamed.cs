//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/27 | Initialize core skeleton |
*/

namespace Blaze.Core
{
    /// <summary>
    /// 可命名的接口
    /// </summary>
    public interface INamed
    {
        /// <summary>
        /// 获取名字
        /// </summary>
        /// <returns></returns>
        string GetName();
    }
}