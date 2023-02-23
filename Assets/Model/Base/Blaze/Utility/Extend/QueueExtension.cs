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
    public static class QueueExtension
    {
        /// <summary>
        /// 添加不重复元素
        /// </summary>
        public static bool AddValue<T>(this Queue<T> queue, T value)
        {
            var result = queue.Contains(value);
            if (!result)
                queue.Enqueue(value);
            return result;
        }

        /// <summary>
        /// 添加不重复元素并报错
        /// </summary>
        public static void AddValueAndLogError<T>(this Queue<T> queue, T value)
        {
            var result = queue.AddValue(value);
            if (result)
                Tuner.Error($"存储队列出现重复添加情况，重复元素：{value}，请检查。");
        }

        /// <summary>
        /// 转换成列表形式
        /// </summary>
        public static List<T> ToList<T>(this Queue<T> queue)
        {
            var list = new List<T>();
            while (queue.Count > 0)
                list.Add(queue.Dequeue());

            return list;
        }

        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="target"></param>
        /// <typeparam name="T"></typeparam>
        public static void Merge<T>(this Queue<T> queue, Queue<T> target)
        {
            while (target.Count > 0)
            {
                queue.Enqueue(target.Dequeue());
            }
        }
    }
}