// //  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
// //  Copyright (c) DONOPO and contributors
//
// //  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
// //  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.
//
// /*
// | VER | AUTHOR | DATE                  | DESCRIPTION              |
// |
// | 1.0 | Eric   | 2020-01-03   04:10:15 | Des:  读取CSV配置文件方法   |
// */
//
// using System;
// using System.Collections.Generic;
// using Blaze.Core;
// using Blaze.Manage.Spring;
// using Blaze.Utility;
// using UnityEngine;
//
//
// namespace Blaze.Manage.Csv
// {
//     /// <summary>
//     /// CSV管理器
//     /// </summary>
//     public sealed class CsvManager : Singeton<CsvManager>
//     {
//         /// <summary>
//         /// 读取器
//         /// </summary>
//         private Dictionary<string, ICsvBaseTable> readers = new Dictionary<string, ICsvBaseTable>();
//
//         /// <summary>
//         /// 类缓存
//         /// </summary>
//         private Dictionary<string, Type> typers = new Dictionary<string, Type>();
//
//         /// <summary>
//         /// 数据源
//         /// </summary>
//         private ICsvSource csvSource;
//
//
//         public void Initialize()
//         {
//             // 加载CSV处理器
//             SpringManager._.ScanBeans(new CsvHolder());
//         }
//
//         /// <summary>
//         /// 注册CSV对象
//         /// </summary>
//         /// <param name="csvEnum"></param>
//         /// <param name="csvBaseTable"></param>
//         public void RegisterCsv(string csvEnum, ICsvBaseTable csvBaseTable, Type type)
//         {
//             if (readers.ContainsKey(csvEnum))
//             {
//                 readers.Remove(csvEnum);
//                 typers.Remove(csvEnum);
//             }
//
//             readers.Add(csvEnum, csvBaseTable);
//             typers.Add(csvEnum, type);
//         }
//
//
//         /// <summary>
//         /// 获取CSV对象
//         /// </summary>
//         /// <param name="csvEnum"></param>
//         /// <returns></returns>
//         public ICsvBaseTable GetCsv(string csvEnum)
//         {
//             return readers[csvEnum];
//         }
//
//         /// <summary>
//         /// 获取一个全新的Csv实例
//         /// </summary>
//         /// <param name="csvEnum"></param>
//         /// <returns></returns>
//         public ICsvBaseTable NewCsv(string csvEnum)
//         {
//             return (ICsvBaseTable) Activator.CreateInstance(typers[csvEnum]);
//         }
//
//         /// <summary>
//         /// 获取一个全新的配置
//         /// </summary>
//         /// <param name="csvEnum"></param>
//         /// <param name="sn"></param>
//         /// <returns></returns>
//         public ICsvBaseTable NewCsv(string csvEnum, int sn)
//         {
//             var obj = (ICsvBaseTable) Activator.CreateInstance(typers[csvEnum]);
//             obj.SetSN(sn);
//             obj.Active();
//             return obj;
//         }
//
//         /// <summary>
//         /// 检查是否存在
//         /// </summary>
//         /// <param name="csvEnum"></param>
//         /// <returns></returns>
//         public bool HasTable(string csvEnum)
//         {
//             return readers.ContainsKey(csvEnum);
//         }
//
//         /// <summary>
//         /// 获取ho
//         /// </summary>
//         /// <param name="csvName"></param>
//         /// <typeparam name="T"></typeparam>
//         /// <returns></returns>
//         public T GetCsv<T>(string csvName) where T : ICsvBaseTable
//         {
//             return (T) readers[csvName];
//         }
//
//
//         /// <summary>
//         /// 注册数据源
//         /// </summary>
//         /// <param name="csvSource"></param>
//         public void RegisterSource(ICsvSource csvSource)
//         {
//             this.csvSource = csvSource;
//         }
//
//
//         /// <summary>
//         /// 获取CSV数据源
//         /// </summary>
//         /// <returns></returns>
//         public ICsvSource GetCsvSource()
//         {
//             return csvSource;
//         }
//     }
// }