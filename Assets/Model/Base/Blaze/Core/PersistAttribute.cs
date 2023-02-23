//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | Tim    | 2019/10/18 | Initialize core skeleton |
*/

using System;
using Blaze.Common;
using Blaze.Utility.Helper;

namespace Blaze.Core
{
    /// <summary>
    /// 可命名的类属性,一般用来通过名字来检索的情况.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PersistAttribute : Attribute
    {
        /// <summary>
        /// 持久化类型
        /// </summary>
        public EnumPersistType Type
        {
            get => type;
            set
            {
                type = value;
                Ext = Type.ToString();
            }
        }

        /// <summary>
        /// 自定义扩展名
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// 持久化目录
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 驼峰文件名
        /// </summary>
        public bool CamelFileName { get; set; }

        private EnumPersistType type;

        public PersistAttribute()
        {
            CamelFileName = true;
            Type = EnumPersistType.JSON;
            Ext = EnumPersistType.JSON.ToString();
            Path = PathHelper.GetPersistentPath();
        }
    }
}