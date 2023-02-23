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
using UnityEngine;

namespace Blaze.Utility.Helper
{
    /// <summary>
    /// 数组工具
    /// </summary>
    static public class ArrayHelper
    {
        /// <summary>
        /// 添加数字元素
        /// </summary>
        /// <param name="array"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public T AddArrayElement<T>(ref T[] array) where T : new()
        {
            return AddArrayElement<T>(ref array, new T());
        }

        /// <summary>
        /// 添加一个数组元素
        /// </summary>
        /// <param name="array"></param>
        /// <param name="elToAdd"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public T AddArrayElement<T>(ref T[] array, T elToAdd)
        {
            if (array == null)
            {
                array = new T[1];
                array[0] = elToAdd;
                return elToAdd;
            }

            var newArray = new T[array.Length + 1];
            array.CopyTo(newArray, 0);
            newArray[array.Length] = elToAdd;
            array = newArray;
            return elToAdd;
        }

        /// <summary>
        /// 删除数组的数据
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <typeparam name="T"></typeparam>
        static public void DeleteArrayElement<T>(ref T[] array, int index)
        {
            if (index >= array.Length || index < 0)
            {
                Debug.LogWarning("invalid index in DeleteArrayElement: " + index);
                return;
            }

            var newArray = new T[array.Length - 1];
            int i;
            for (i = 0; i < index; i++)
            {
                newArray[i] = array[i];
            }

            for (i = index + 1; i < array.Length; i++)
            {
                newArray[i - 1] = array[i];
            }

            array = newArray;
        }
    }
}