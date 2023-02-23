//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Blaze.Common;
using Blaze.Resource;
using Blaze.Utility;
using Blaze.Utility.Helper;
using UnityEngine;

namespace Blaze.Manage.Csv
{
    public class CsvReader
    {
        /// <summary>
        /// 路径读取文件操作 editor gen
        /// </summary>
        /// <param name="fileFullPath">文件全路径带格式后缀</param>
        /// <returns></returns>
        public static string ReadCsvWithPath(string fileFullPath)
        {
            return File.ReadAllText(fileFullPath, Encoding.Default);
        }


        /// <summary>
        /// 转换为字符串列表，并去除头部指定行数 editor gen
        /// </summary>
        /// <param name="csvContent"></param>
        /// <returns></returns>
        public static List<string> ToListWithDeleteFirstLines(string csvContent, int deleteLineCount = 0)
        {
            if (csvContent == null)
                return null;
            var tempStrs = csvContent.Split('\n');
            if (tempStrs.Length <= deleteLineCount)
            {
                Tuner.Error("操作失败，数据行数不足 {0} 行，请检查。 传入字符串： {1}", deleteLineCount, csvContent);
                return null;
            }

            List<string> list = new List<string>();

            for (int i = deleteLineCount; i < tempStrs.Length; i++)
            {
                if (tempStrs[i].Trim().Length > 0)
                {
                    list.Add(tempStrs[i].Trim());
                }
            }

            return list;
        }


        /// <summary>
        /// 转换为字符串列表，只返回头部 editor gen
        /// </summary>
        /// <param name="csvContent"></param>
        /// <returns></returns>
        private static List<string> ToListWithFirstLines(string csvContent, int readLineCount = 3)
        {
            var tempStrs = csvContent.Split('\n');
            if (tempStrs.Length <= readLineCount)
            {
                Tuner.Error("操作失败，数据行数不足 {0} 行，请检查。 传入字符串： {1}", readLineCount, csvContent);
                return null;
            }

            List<string> list = new List<string>();

            for (int i = 0; i < readLineCount; i++)
            {
                if (tempStrs[i].Trim().Length > 0)
                {
                    list.Add(tempStrs[i].Trim());
                }
            }

            return list;
        }


        /// <summary>
        /// 获取CSV文件的所有的行描述信息 editor gen
        /// </summary>
        /// <param name="csvContent">CSV文件的内容整体</param>
        /// <returns></returns>
        public static List<CsvRowDesc> GetRowDesc(string csvContent)
        {
            // 读取CSV前三行并且拆解数据
            var headLine = CsvReader.ToListWithFirstLines(csvContent, 3);
            var keys = headLine[0].Split(',');
            var types = headLine[1].Split(',');
            var descs = headLine[2].Split(',');
            // csv完整描述
            var csvDesc = new List<CsvRowDesc>();
            for (int i = 0; i < keys.Length; i++)
            {
                // 创建单列描述
                var csvRowInfo = new CsvRowDesc();
                csvRowInfo.Index = i;
                csvRowInfo.Key = keys[i];
                csvRowInfo.Type = types[i];
                csvRowInfo.Desc = descs[i];
                // 当键不为空时候加载结构
                if (!keys[i].Trim().Equals(""))
                {
                    csvDesc.Add(csvRowInfo);
                }
            }

            return csvDesc;
        }


        /// <summary>
        /// 路径读取文件操作 runtime
        /// </summary>
        /// <param name="fileFullName">文件全名带格式后缀</param>
        /// <param name="directoryPath">所在文件夹路径</param>
        /// <returns></returns>
        public static string ReadLocalCsv(string fileFullName)
        {
            // 从AB包中加载数据
            return Res.LoadAssetSync("Data/CsvData/" + fileFullName).ReadAllText();
        }


        /// <summary>
        /// 获取CSV头
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<CsvRowDesc> GetHead(string fileName)
        {
            // 本地CSV
            return GetRowDesc(ReadLocalCsv(fileName));
        }

        /// <summary>
        /// 获取文件内容 runtime
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string> GetBody(string fileName)
        {
            return ToListWithDeleteFirstLines(ReadLocalCsv(fileName), 3);
        }
    }
}