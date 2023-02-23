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
using Blaze.Utility;
using Blaze.Utility.Helper;

namespace Blaze.Core
{
    /// <summary>
    /// 可命名的基类
    /// </summary>
    [Serializable]
    public class Namedable : INamed
    {
        [field: NonSerialized] protected string Name { get; set; }

        /// <summary>
        /// 获取命名
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            if (null == Name)
            {
                try
                {
                    var meta = AttrHelper.GetAttribute<NamedAttribute>(GetType());
                    Name = meta.Name;
                }
                catch (Exception e)
                {
                    Tuner.Error("Get NameAttribute Err : {0}", e);
                    return "";
                }
            }

            return Name;
        }

        public void SetName(string name)
        {
            Name = name;
        }
    }
}