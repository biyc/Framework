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

namespace Blaze.Core
{
    /// <summary>
    /// 可命名的类属性,一般用来通过名字来检索的情况.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class NamedAttribute : Attribute
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        public NamedAttribute(string name)
        {
            Name = name;
        }
    }
}