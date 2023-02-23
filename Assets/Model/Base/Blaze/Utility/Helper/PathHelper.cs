//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | Tim    | 2019/10/18 | Initialize core skeleton |
*/

using System;
using System.Collections.Generic;
using System.IO;
using Blaze.Common;
using Blaze.Utility.Extend;
using UnityEngine;

namespace Blaze.Utility.Helper
{
    /// <summary>
    /// 所有和路径相关的方法
    /// iOS:
    /// Application.dataPath
    ///             /var/containers/Bundle/Application/app sandbox/xxx.app/Data
    /// Application.streamingAssetsPath
    ///             /var/containers/Bundle/Application/app sandbox/test.app/Data/Raw
    /// Application.temporaryCachePath
    ///             /var/mobile/Containers/Data/Application/app sandbox/Library/Caches
    /// Application.persistentDataPath
    ///             /var/mobile/Containers/Data/Application/app sandbox/Documents
    ///
    /// Android:
    /// Application.dataPath
    ///             /data/app/package name-1/base.apk
    /// Application.streamingAssetsPath
    ///             jar:file:///data/app/package name-1/base.apk!/assets
    /// Application.temporaryCachePath
    ///             /storage/emulated/0/Android/data/package name/cache
    /// Application.persistentDataPath
    ///             /storage/emulated/0/Android/data/package name/files
    ///
    /// Editor(MacOS):
    /// Application.dataPath
    ///             /Users/???/Projects/jethummer/Assets
    /// Application.streamingAssetsPath
    ///             /Users/???/Projects/jethummer/Assets/StreamingAssets
    /// Application.temporaryCachePath
    ///             /Users/???/Library/Application Support/jethummer/firebolt-unity
    /// Application.persistentDataPath
    ///             /var/folders/37/???/T/jethummer/thoruni
    /// 
    /// </summary>
    public static class PathHelper
    {
        public static void PrintPath()
        {
            Debug.Log($"[>>] PersistentPath : {GetPersistentPath()}");
            Debug.Log($"[>>] GetCurrentPath : {GetCurrentPath()}");
            Debug.Log($"[>>] GetAssetsPath : {GetAssetsPath()}");
            Debug.Log($"[>>] GetResourcePath : {GetResourcePath()}");
            Debug.Log($"[>>] GetProjectPath : {GetProjectPath()}");
            Debug.Log($"[>>] GetConfigPath : {GetConfigPath()}");
            Debug.Log($"[>>] GetStreamingPath : {GetStreamingPath()}");
            Debug.Log($"[>>] GetTemporaryPath : {GetTemporaryPath()}");
        }

        /// <summary>
        /// 获取当前程序的运行目录
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentPath()
        {
            return Environment.CurrentDirectory;
        }


        /// <summary>
        /// TODO: 要实测是否在运行期有区别
        /// 获取Assets内打包的资源所在路径 (打包后,运行期)
        /// </summary>
        /// <returns></returns>
        public static string GetAssetsPath()
        {
            return Application.dataPath;
        }

        /// <summary>
        /// 获取Assets内Resources文件夹的资源所在路径
        /// </summary>
        /// <returns></returns>
        public static string GetResourcePath()
        {
            return Combine(Application.dataPath, "Resources");
        }

        /// <summary>
        /// 获取Assets内Resources文件夹的资源所在路径
        /// </summary>
        /// <returns></returns>
        public static string GetProjectPath()
        {
            return Combine(Application.dataPath, "Projects");
        }

        /// <summary>
        /// 获取Assets内Config文件夹的资源所在路径
        /// </summary>
        /// <returns></returns>
        public static string GetConfigPath()
        {
            return Combine(Application.dataPath, "Configs");
        }

        /// <summary>
        /// 获取Assets/StreamingAsset流式数据目录
        /// </summary>
        /// <returns></returns>
        public static string GetStreamingPath()
        {
            return Application.streamingAssetsPath;
        }

        /// <summary>
        /// 获取用户持久化数据目录
        /// </summary>
        /// <returns></returns>
        public static string GetPersistentPath()
        {
            return Application.persistentDataPath;
        }


        /// <summary>
        /// 获取用户持久化 AB 包数据目录
        /// </summary>
        /// <returns></returns>
        public static string GetPersistentBundlePath()
        {
            var path = Combine(Application.persistentDataPath, "Bundle");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// 获取用户持久化 cache 目录
        /// </summary>
        /// <returns></returns>
        public static string GetPersistentCachePath()
        {
            var path = Combine(Application.persistentDataPath, "cache");
            CheckOrCreate(path);
            return path;
        }


        /// <summary>
        /// 获取用户持久化 自定义子目录
        /// </summary>
        /// <returns></returns>
        public static string GetPersistentCustomPath(string subPath)
        {
            var path = Combine(Application.persistentDataPath, "subPath");
            CheckOrCreate(path);
            return path;
        }


        /// <summary>
        /// 获取临时区数据目录
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryPath()
        {
            return Application.temporaryCachePath;
        }

        /// <summary>
        /// 删除目录下面的空目录
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool RemoveEmpty(string pathName)
        {
            if (string.IsNullOrEmpty(pathName))
            {
                throw new Exception("Directory name is invalid.");
            }

            try
            {
                if (!Directory.Exists(pathName))
                {
                    return false;
                }

                // 不使用 SearchOption.AllDirectories，以便于在可能产生异常的环境下删除尽可能多的目录
                string[] subDirectoryNames = Directory.GetDirectories(pathName, "*");
                int subDirectoryCount = subDirectoryNames.Length;
                foreach (string subDirectoryName in subDirectoryNames)
                {
                    if (RemoveEmpty(subDirectoryName))
                    {
                        subDirectoryCount--;
                    }
                }

                if (subDirectoryCount > 0)
                {
                    return false;
                }

                if (Directory.GetFiles(pathName, "*").Length > 0)
                {
                    return false;
                }

                Directory.Delete(pathName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件夹的所有文件，包括子文件夹 不包含.meta文件
        /// </summary>
        /// <returns>The files.</returns>
        /// <param name="pathName">Path.</param>
        /// <param name="deep">是否递归查找</param>
        /// <param name="ext">文件后缀</param>
        public static FileInfo[] FindFiles(string pathName, bool deep = false, string ext = null)
        {
            DirectoryInfo folder = new DirectoryInfo(pathName);

            DirectoryInfo[] subFolders = folder.GetDirectories();
            List<FileInfo> filesList = new List<FileInfo>();

            if (deep)
            {
                foreach (DirectoryInfo subFolder in subFolders)
                {
                    filesList.AddRange(FindFiles(subFolder.FullName, deep, ext));
                }
            }

            FileInfo[] files = folder.GetFiles();
            foreach (FileInfo file in files)
            {
                if (ext != null)
                {
                    if (file.Extension.Contains(ext))
                    {
                        filesList.Add(file);
                    }
                }
                else
                {
                    filesList.Add(file);
                }
            }

            return filesList.ToArray();
        }


        /// <summary>
        /// 获取文件夹的所有文件夹，包括子文件夹文件
        /// </summary>
        /// <returns>The files.</returns>
        /// <param name="pathName">Path.</param>
        /// <param name="deep">是否递归查找</param>
        public static DirectoryInfo[] FindDirs(string pathName, bool deep = false)
        {
            DirectoryInfo folder = new DirectoryInfo(pathName);

            DirectoryInfo[] subFolders = folder.GetDirectories();
            List<DirectoryInfo> dirsList = new List<DirectoryInfo>();

            if (deep)
            {
                foreach (DirectoryInfo subFolder in subFolders)
                {
                    dirsList.AddRange(FindDirs(subFolder.FullName, deep));
                }
            }

            DirectoryInfo[] dirs = folder.GetDirectories();
            foreach (DirectoryInfo file in dirs)
            {
                dirsList.Add(file);
            }

            return dirsList.ToArray();
        }

        /// <summary>
        /// 获取文件夹的所有文件夹
        /// </summary>
        /// <returns>The files.</returns>
        /// <param name="path">Path.</param>
        public static DirectoryInfo[] FindSubPath(string path)
        {
            DirectoryInfo folder = new DirectoryInfo(path);

            DirectoryInfo[] subFolders = folder.GetDirectories();
            return subFolders;
        }

        /// <summary>
        /// 检查目录是否存在不存在则创建
        /// </summary>
        /// <param name="url"></param>
        public static void CheckOrCreate(string url)
        {
            try
            {
                if (!Directory.Exists(url)) //如果不存在就创建file文件夹　　             　　              
                    Directory.CreateDirectory(url); //创建该文件夹　　            
            }
            catch (Exception)
            {
                // ignore
            }
        }

        /// <summary>
        /// //将绝对路径转成工作空间内的相对路径
        /// TODO: 这里是有问题的
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetAssetsRelative(string fullPath, string root)
        {
            return fullPath.ReplaceFirst(root, "Assets");
        }


        /// <summary>
        /// 将Windows的路径归一化到Linux格式
        /// </summary>
        public static string NormalizePathName(string pathName)
        {
            return Path.GetDirectoryName(pathName).Replace("\\", "/");
        }

        /// <summary>
        /// 检查是否有子目录
        /// </summary>
        /// <param name="parentDir">The parent directory.</param>
        /// <param name="subDir">The sub directory.</param>
        public static bool HasSubDirectory(this DirectoryInfo parentDir, DirectoryInfo subDir)
        {
            string str = parentDir.FullName.TrimEnd('\\', '/');
            for (; subDir != null; subDir = subDir.Parent)
            {
                if (subDir.FullName.TrimEnd('\\', '/') == str)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 检查父目录的名字是否吻合
        /// </summary>
        public static DirectoryInfo FindParentWithName(
            this DirectoryInfo dir,
            string folderName)
        {
            if (dir.Parent == null)
                return (DirectoryInfo) null;
            return string.Equals(dir.Name, folderName, StringComparison.InvariantCultureIgnoreCase)
                ? dir
                : dir.Parent.FindParentWithName(folderName);
        }


        /// <summary>
        /// 拼合路径字符串
        /// </summary>
        public static string Combine(string a, string b)
        {
            a = a.Replace("\\", "/").TrimEnd('/');
            b = b.Replace("\\", "/").TrimStart('/');
            return a + "/" + b;
        }

        public static string Combine(string a, string b, string c)
        {
            a = a.Replace("\\", "/").TrimEnd('/');
            b = b.Replace("\\", "/").TrimStart('/');
            c = c.Replace("\\", "/").TrimStart('/');
            return a + "/" + b + "/" + c;
        }

        public static string Combine(string a, string b, string c, string d)
        {
            a = a.Replace("\\", "/").TrimEnd('/');
            b = b.Replace("\\", "/").TrimStart('/');
            c = c.Replace("\\", "/").TrimStart('/');
            d = d.Replace("\\", "/").TrimStart('/');
            return a + "/" + b + "/" + c+ "/" + d;
        }
    }
}