//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/21 | Initialize core skeleton |
*/

namespace Blaze.Resource.Common
{
    /// <summary>
    /// 引用计数器
    /// </summary>
    public interface IRef
    {
        /// <summary>
        /// 添加一次
        /// </summary>
        void AddRef();

        /// <summary>
        /// 减少一次
        /// </summary>
        void RemoveRef();

        /// <summary>
        /// 返回当前引用数量
        /// </summary>
        /// <returns></returns>
        int RefCount();

    }
}