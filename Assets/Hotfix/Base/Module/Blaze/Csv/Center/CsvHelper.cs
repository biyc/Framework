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
// using Blaze.Utility.Helper;
//
// namespace Blaze.Manage.Csv.Poco
// {
//     /// <summary>
//     /// Csv辅助类
//     /// </summary>
//     public static partial class CsvHelper
//     {
//         /// <summary>
//         /// 返回泛型
//         /// </summary>
//         /// <param name="name"></param>
//         /// <typeparam name="T"></typeparam>
//         /// <returns></returns>
//         public static T Get<T>() where T : ICsvBaseTable
//         {
//             var attr = AttrHelper.GetAttribute<CsvTable>(typeof(T));
//             return (T) CsvManager._.GetCsv(attr.Value);
//         }
//
//         /// <summary>
//         /// 获得一个新的实例
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         /// <returns></returns>
//         public static T New<T>(int sn) where T : ICsvBaseTable
//         {
//             var attr = AttrHelper.GetAttribute<CsvTable>(typeof(T));
//             return (T) CsvManager._.NewCsv(attr.Value, sn);
//         }
//     }
// }