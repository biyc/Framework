//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | Tim    | 2019/10/18 | Initialize core skeleton |
*/

using System.IO;
using System.Text;

namespace Blaze.Utility.Helper
{
    public static class FileHelper
    {
        /// <summary>
        /// 将文件转换成byte[]数组
        /// </summary>
        /// <param name="filePathName">文件路径文件名称</param>
        /// <returns>byte[]数组</returns>
        public static byte[] ReadToByte(string filePathName)
        {
            try
            {
                using (FileStream fs = new FileStream(filePathName, FileMode.Open, FileAccess.Read))
                {
                    byte[] byteArray = new byte[fs.Length];
                    fs.Read(byteArray, 0, byteArray.Length);
                    return byteArray;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将文件转换成string
        /// </summary>
        /// <param name="filePathName">文件路径文件名称</param>
        /// <returns>文件内容字符串</returns>
        public static string ReadToString(string filePathName)
        {
            try
            {
                using (FileStream fs = new FileStream(filePathName, FileMode.Open, FileAccess.Read))
                {
                    byte[] byteArray = new byte[fs.Length];
                    fs.Read(byteArray, 0, byteArray.Length);
                    return Encoding.UTF8.GetString(byteArray);
                    ;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将byte数组写入文件
        /// </summary>
        /// <param name="filePathName"></param>
        /// <param name="bytes"></param>
        public static void WriteFile(string filePathName, byte[] bytes)
        {
            {
                if (File.Exists(filePathName))
                {
                    File.Delete(filePathName);
                }

                FileStream stream = new FileStream(filePathName, FileMode.Create);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();
            }
        }

        /// <summary>
        /// 将字符串写入文件
        /// </summary>
        /// <param name="filePathName"></param>
        /// <param name="content"></param>
        /// <param name="append">是否追加</param>
        public static void WriteFile(string filePathName, string content, bool append = false)
        {
            {
                StreamWriter sw = new StreamWriter(filePathName, append);
                sw.WriteLine(content);
                sw.Flush();
                sw.Close();
            }
        }

        /// <summary>
        /// 获得文件内容
        /// </summary>
        /// <param name="filePathName">文件名</param>
        /// <param name="isTrim">是否合并换行</param>
        /// <returns></returns>
        public static string GetContent(string filePathName, bool isTrim = false)
        {
            StringBuilder sb = new StringBuilder();

            StreamReader sr = new StreamReader(filePathName, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (isTrim)
                    sb.Append(line.Trim());
                else
                    sb.Append(line);
            }

            return sb.ToString();
        }
    }
}