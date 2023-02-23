//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/14 | Initialize core skeleton |
*/

using System;
using System.Reflection;

namespace Blaze.Utility.Helper
{
    /// <summary>
    /// 属性的工具类 
    /// </summary>
    public static class AttrHelper
    {
        /// <summary>
        /// 获取单一属性的方法
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAttribute<T>(Type type) where T : Attribute
        {
            var metas = Attribute.GetCustomAttributes(type);

            foreach (var meta in metas)
            {
                if (meta.GetType().Equals(typeof(T)))
                {
                    return (T) meta;
                }
            }

            return null;
        }


        /// <summary>
        /// 获取指定方法的属性
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetMethodAttribute<T>(MethodInfo methodInfo) where T : Attribute
        {
            var metas = methodInfo.GetCustomAttributes(true);
            // var attributes = GetType().GetMethod("OnMessage").GetCustomAttributes(true);

            foreach (var meta in metas)
            {
                if (meta.GetType().Equals(typeof(T)))
                {
                    return (T) meta;
                }
            }

            return null;
        }
    }
}