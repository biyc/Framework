//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/24 | Initialize core skeleton |
*/

using System;
using Blaze.Core;

namespace Blaze.Manage.Data
{
    /// <summary>
    /// 数据实现, 使用数据端可以根据数据名称来Switch
    /// </summary>
    public class DataObj<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        protected T _data;

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get()
        {
            return _data;
        }

        /// <summary>
        /// 放入
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        public void Put(T obj)
        {
            _data = obj;
        }
    }
}