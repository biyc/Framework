//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE                  | DESCRIPTION              |
|
| 1.0 | Eric   | 2020年3月14日 | Des:            |
*/

using System.Collections.Generic;

namespace Blaze.Utility.Extend
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// 非唯一性存储，若存在相同键返回值为False
        /// </summary>
        /// <returns>False为存储失败，存在相同的Key。</returns>
        public static bool AddValue<T, T1>(this Dictionary<T, T1> dict, T key, T1 value)
        {
            if (dict.ContainsKey(key))
                return false;

            dict.Add(key, value);
            return true;
        }

        /// <summary>
        /// 唯一性存储，如果存在相同的键则报错
        /// </summary>
        public static void AddValueAndLogError<T, T1>(this Dictionary<T, T1> dict, T key, T1 value)
        {
            bool temp = dict.AddValue(key, value);
            if (!temp)
                Tuner.Error($"字典存储失败，存在相同的键： {key}, 请检查。");
        }

        /// <summary>
        /// 获取元素，值有可能为空
        /// </summary>
        public static T1 GetValue<T, T1>(this Dictionary<T, T1> dict, T key) where T1 : class, new()
        {
            bool temp = dict.ContainsKey(key);
            if (!temp)
                return null;

            return dict[key];
        }

        /// <summary>
        /// 确定性获取元素，若不存在该元素则报错。
        /// </summary>
        public static T1 GetValueLogError<T, T1>(this Dictionary<T, T1> dict, T key) where T1 : class, new()
        {
            var temp = dict.GetValue(key);
            if (temp == null)
                Tuner.Error($"未找到对应键的值，键： {key}， 请检查。");
            return temp;
        }

        /// <summary>
        /// 添加元素到值为列表的字典中，若存在相同值，则返回False
        /// </summary>
        public static bool AddValueToList<T, T1>(this Dictionary<T, List<T1>> dict, T key, T1 value,
            bool allowSameValue = false)
        {
            var exist = dict.ContainsKey(key);
            if (!exist)
                dict[key] = new List<T1>();

            var list = dict[key];
            if (allowSameValue)
            {
                exist = list.Contains(value);
                if (exist)
                    return false;
            }

            list.Add(value);
            return true;
        }

        /// <summary>
        /// 唯一性存储，添加元素到值为列表的字典中，存在相同则报错
        /// </summary>
        public static void AddValueToListLogError<T, T1>(this Dictionary<T, List<T1>> dict, T key, T1 value,
            bool allowSameValue = false)
        {
            var temp = dict.AddValueToList(key, value, allowSameValue);
            if (!temp)
                Tuner.Error($"字典存储失败，存在相同的值： {value}, 请检查。");
        }

        public static void AddValueToListAndMerge<T, T1>(this Dictionary<T, List<T1>> dict, T key, List<T1> value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
            else
            {
                dict[key].Merge(value);
            }
        }
    }
}