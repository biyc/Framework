//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE                  | DESCRIPTION              |
|
| 1.0 | Eric   | 2020-01-14   10:10:54 | Des: Filter configs with condition. |
*/

using System.Collections.Generic;

namespace Blaze.Utility.Extend
{
    public static class ListExtension
    {
        public static void Merge<T>(this List<T> list, List<T> beMergeList)
        {
            if (beMergeList == null || beMergeList.Count == 0)
                return;
            if (list == null)
                list = new List<T>();

            foreach (var item in beMergeList)
            {
                list.Add(item);
            }
        }

        public static Queue<T> ToQueue<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                return null;

            var queue = new Queue<T>();
            for (int i = 0; i < list.Count; i++)
                queue.Enqueue(list[i]);

            return queue;
        }

        public static bool AddValue<T>(this List<T> list, T value)
        {
            if (list == null)
            {
                Tuner.Error("需添加元素的列表为空，请检查。");
                return false;
            }

            var result = list.Contains(value);
            if (result)
                return false;

            list.Add(value);
            return true;
        }

        public static void AddValueAndLogError<T>(this List<T> list, T value)
        {
            var result = list.AddValue(value);
            if (!result)
                Tuner.Error("添加元素不成功，可能出现列表为空或元素重复的情况。");
        }
    }
}