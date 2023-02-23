//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/09 | Initialize core skeleton |
*/

using System;
using Blaze.Utility.Impl;


namespace Blaze.Utility.Helper
{
    public static class RandomHelper
    {
        private static Random _random = new Random();

        /// <summary>
        /// 获取下一个随机数
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Next(int max)
        {
            return _random.Next(max);
        }

        /// <summary>
        /// 获取一个固定随机数发生器
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static FixRandom GetFixRandom(uint seed)
        {
            FixRandom fixRandom = new FixRandom(seed);
            return fixRandom;
        }
    }
}