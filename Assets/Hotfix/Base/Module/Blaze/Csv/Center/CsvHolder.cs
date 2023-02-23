// //  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
// //  Copyright (c) DONOPO and contributors
//
// //  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
// //  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.
//
// /*
// | VER | AUTHOR | DATE       | DESCRIPTION              |
// |
// | 1.0 | tim    | 2020/04/01 | Initialize core skeleton |
// */
//
// using System;
// using Blaze.Manage.Spring;
// using Blaze.Utility;
//
// namespace Blaze.Manage.Csv
// {
//     /// <summary>
//     /// Csv Spring 扫描器
//     /// </summary>
//     public class CsvHolder : SpringHolder
//     {
//         /// <summary>
//         /// 扫描
//         /// </summary>
//         /// <returns></returns>
//         public Type GetRegisterType()
//         {
//             return typeof(CsvTable);
//         }
//
//         /// <summary>
//         /// 处理
//         /// </summary>
//         /// <param name="attr"></param>
//         /// <param name="type"></param>
//         public void Process(Attribute attr, Type type)
//         {
//             CsvTable table = (CsvTable) attr;
//             ICsvBaseTable obj = (ICsvBaseTable) Activator.CreateInstance(type);
//             if (table.Extend || !CsvManager._.HasTable(table.Value))
//             {
//                 CsvManager._.RegisterCsv(table.Value, obj, type);
//             }
//         }
//     }
// }