//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE                  | DESCRIPTION              |
|
| 1.0 | Eric   | 2019-11-22   03:19:26 | Des:  |
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blaze.Utility.Extend
{
    /// <summary>
    /// 字符串扩展方法
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 如果为空获取默认值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Default(this string str, string defaultValue)
        {
            if (null == str)
            {
                return defaultValue;
            }

            return str;
        }

        /// <summary>
        /// 替换第一个出现的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string str, string oldValue, string newValue)
        {
            int idx = str.IndexOf(oldValue);
            str = str.Remove(idx, oldValue.Length);
            str = str.Insert(idx, newValue);
            return str;
        }

        /// <summary>
        /// Split字符串成为int数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="middleSymbol"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this string str, char middleSymbol)
        {
            return Array.ConvertAll(str.Trim().Split(middleSymbol), int.Parse);
        }

        /// <summary>
        /// Split字符串成为float数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="middleSymbol"></param>
        /// <returns></returns>
        public static float[] ToFloatArray(this string str, char middleSymbol)
        {
            return Array.ConvertAll(str.Trim().Split(middleSymbol), float.Parse);
        }

        /// <summary>
        /// 转化为整形
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }

        /// <summary>
        /// 转化为浮点
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
        {
            return float.Parse(str);
        }

        /// <summary>
        /// 转化为布尔
        /// </summary>
        /// <param name="str">1,0 || true,false etc</param>
        /// <returns></returns>
        public static bool ToBool(this string str)
        {
            return bool.Parse(str);
        }

        /// <summary>
        /// 转化为Vector2
        /// </summary>
        /// <param name="str"></param>
        /// <param name="middleSymbol"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this string str, char middleSymbol)
        {
            if (string.IsNullOrEmpty(str))
                return Vector2.zero;
            var parts = str.ToFloatArray(middleSymbol);
            return new Vector2(parts[0], parts[1]);
        }

        /// <summary>
        /// 字符串处理回调函数句柄
        /// </summary>
        /// <param name="str"></param>
        public delegate void Callback(string str);

        /// <summary>
        /// 字符串处理回调函数句柄
        /// </summary>
        /// <param name="str"></param>
        public delegate string CallbackReturn(string str);

        /// <summary>
        /// 字符串条件前置
        /// </summary>
        /// <param name="str"></param>
        public delegate bool CallbackIf(string str);

        /// <summary>
        /// 检查条件为真时执行回调
        /// 例子:
        /// var hi = "Hello".CheckDo(str => { return str.StartsWith("H"); },
        ///                          str => { return str.ReplaceFirst("ll", "gg"); });
        /// 结果: hi = Heggo
        /// </summary>
        /// <param name="str"></param>
        /// <param name="callbackIf"></param>
        /// <param name="callbackReturn"></param>
        /// <returns></returns>
        public static string CheckDo(this string str, CallbackIf callbackIf, CallbackReturn callbackReturn)
        {
            if (callbackIf(str))
            {
                return callbackReturn(str);
            }

            return str;
        }

        /// <summary>
        /// 当长度大于执行
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="callback"></param>
        public static void CheckLengthGreaterThanValue(this string str, int length, Callback callback)
        {
            if (str.Length > length)
                callback(str);
        }

        /// <summary>
        /// 当长度小于执行
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="callback"></param>
        public static void CheckLengthLessThanValue(this string str, int length, Callback callback)
        {
            if (str.Length < length)
                callback(str);
        }


        /// <summary>
        /// 将文字的颜色转化为Color对象
        /// Example: "#ff000099".ToColor() red with alpha ~50%
        /// Example: "ffffffff".ToColor() white with alpha 100%
        /// Example: "00ff00".ToColor() green with alpha 100%
        /// Example: "0000ff00".ToColor() blue with alpha 0%        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToColor(this string color)
        {
            if (color.StartsWith("#", StringComparison.InvariantCulture))
            {
                color = color.Substring(1); // strip #
            }

            if (color.Length == 6)
            {
                color += "FF"; // add alpha if missing
            }

            var hex = Convert.ToUInt32(color, 16);
            var r = ((hex & 0xff000000) >> 0x18) / 255f;
            var g = ((hex & 0xff0000) >> 0x10) / 255f;
            var b = ((hex & 0xff00) >> 8) / 255f;
            var a = ((hex & 0xff)) / 255f;

            return new Color(r, g, b, a);
        }

        public static List<int[]> ToListIntArray(this string str, char splitSymbol = '|', char middleSymbol = ':')
        {
            var all = str.Trim().Split(splitSymbol);
            var list = new List<int[]>();
            foreach (var parts in all)
            {
                var part = parts.ToIntArray(middleSymbol);
                list.Add(part);
            }

            return list;
        }

        public static Color ToRGBColor(this string str)
        {
            var array = str.ToFloatArray('|');
            if (array.Length == 3)
            {
                var r = array[0] / 255f;
                var g = array[1] / 255f;
                var b = array[2] / 255f;
                return new Color(r, g, b);
            }

            if (array.Length == 4)
            {
                var r = array[0] / 255f;
                var g = array[1] / 255f;
                var b = array[2] / 255f;
                var a = array[3] / 100f;
                return new Color(r, g, b, a);
            }
            Tuner.Warn($"[>>] ToRGBColor Error : {str}");
            return Color.magenta;
        }
    }
}