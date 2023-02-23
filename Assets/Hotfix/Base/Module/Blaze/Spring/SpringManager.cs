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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Blaze.Core;
using Blaze.Utility;

namespace Blaze.Manage.Spring
{
    /// <summary>
    /// 注入式管理器
    /// </summary>
    public sealed class SpringManager : Singeton<SpringManager>
    {

        private List<Type> _types;

        /// <summary>
        /// 初始化模块
        /// </summary>
        public void Initialize()
        {
            _types = ETModel.Game.Hotfix.GetHotfixTypes();
            if (_types == null)
                _types = Assembly.GetExecutingAssembly().GetTypes().ToList();
        }



        /// <summary>
        /// 注册并立刻执行单个装配任务
        /// </summary>
        public void ScanBeans(SpringHolder holder)
        {
            try
            {
                foreach (Type clazz in _types)
                {
                    Object[] attrs = clazz.GetCustomAttributes(holder.GetRegisterType(), false);
                    if (attrs != null && attrs.Length > 0)
                    {
                        holder.Process((Attribute) attrs[0], clazz);
                    }
                }
            }
            catch (Exception e)
            {
                Tuner.Log($"[>>] {e}, 扫描Bean注解类出错！");
            }
        }
    }
}