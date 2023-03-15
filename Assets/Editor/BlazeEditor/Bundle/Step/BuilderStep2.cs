//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/18 | Initialize core skeleton |
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Blaze.Bundle.Data;
using Blaze.Common;
using Blaze.Core;
using Blaze.Resource.AssetBundles.Data;
using Blaze.Resource.AssetBundles.Logic;
using Blaze.Resource.Poco;
using Blaze.Utility;
using Blaze.Utility.Helper;
using ICSharpCode.SharpZipLib.Zip;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Blaze.Bundle.Step
{
    public class BuilderStep2 : Singeton<BuilderStep2>
    {
        // Publish/iOS/_Cache/1.1
        private string versionCachePath;

        // Publish/iOS/1.1.1
        private string versionPublishPath;

        private readonly Dictionary<string, string> _originPath = new Dictionary<string, string>();


        public void Execution()
        {
            _originPath.Clear();
            CheckVersionPath();
            // 根目录 Data 文件夹中的子文件压缩成 ZIP 包
            ProcessZip();
            ProcessAssetBundle();
            // 自定义打包规则目录
            // ProcessCustomAssetBundle();
            BundleAssetPackage();
        }

        /// <summary>
        /// 版本缓存路径
        /// </summary>
        /// <returns></returns>
        public string VersionCachePath()
        {
            return versionCachePath;
        }

        /// <summary>
        /// 版本发布路径
        /// </summary>
        /// <returns></returns>
        public string VersionPublishPath()
        {
            return versionPublishPath;
        }

        /// <summary>
        /// 检查是否忽略
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        bool CheckIgnore(string filename)
        {
            if (filename.EndsWith(".DS_Store")
                || filename.EndsWith(".meta")
                || filename.EndsWith(".MD")
               ) return true;
            return false;
        }


        /// <summary>
        /// Zip Package执行
        /// </summary>
        /// <param name="group"></param>
        /// <param name="path"></param>
        void ProcessZip()
        {
            string rootPath = PathHelper.Combine(PathHelper.GetCurrentPath(), "Data");

            PathHelper.FindSubPath(rootPath).ToList().ForEach(delegate(DirectoryInfo dir)
            {
                switch (dir.Name)
                {
                    case "XlsxData":
                    case "LuaScripts":
                    case "Dev":
                        return;
                }

                // 最后的目标文件
                var zipName = PathHelper.Combine(versionCachePath, dir.Name + ".ab");

                using (var zip = new ZipOutputStream(File.Create(zipName)))
                {
                    zip.IsStreamOwner = true;
                    // zip 设置密码
                    zip.Password = "he#3t;ZwcqF2E)DAgNVztuK";
                    var files = PathHelper.FindFiles(dir.FullName, true);
                    foreach (var file in files)
                    {
                        var mf = BuilderStep1._.GetManifest();
                        var data = new ManifestData();
                        data.AssetPath = PathHelper.Combine("Data", dir.Name, file.Name);
                        data.Type = BundleType.Zip;
                        data.ABName = dir.Name + ".ab";
                        mf.ManifestList.Add(data);

                        var name = ZipEntry.CleanName(file.Name);
                        var entry = new ZipEntry(name);
                        entry.DateTime = file.LastWriteTimeUtc;
                        entry.Size = file.Length;
                        zip.PutNextEntry(entry);
                        using (var fs = file.OpenRead())
                        {
                            var buf = new byte[1024 * 128];
                            do
                            {
                                var read = fs.Read(buf, 0, buf.Length);
                                if (read <= 0)
                                {
                                    break;
                                }

                                zip.Write(buf, 0, read);
                            } while (true);
                        }
                    }

                    zip.Close();
                }
            });
        }


        /// <summary>
        /// 处理AB包
        /// </summary>
        /// <param name="item"></param>
        private void ProcessAssetBundle()
        {
            var files = AssetDatabase.GetAllAssetPaths().ToList()
                .FindAll(path =>
                    path.StartsWith("Assets/Projects") && !Directory.Exists(path));
            files.ForEach(delegate(string file)
            {
                // 需要忽略的文件
                if (CheckIgnore(file)) return;

                // todo 跳过自定义打包规则
                if (file.Contains("Assets/Projects/Prefabs")) return;

                _originPath[file.ToLower()] = file;

                var packageName = file.Replace(".", "/");
                try
                {
                    var fileName = packageName.Replace("/", "-") + ".ab";
                    AssetImporter.GetAtPath(file).assetBundleName = fileName;
                }
                catch (Exception e)
                {
                    Tuner.Log(e.Message);
                }
            });
        }

        private void ProcessCustomAssetBundle()
        {
            var fguiPath = PathHelper.Combine(PathHelper.GetProjectPath(), "FGui");

            foreach (var dirInfo in PathHelper.FindSubPath(fguiPath))
            {
                var dir = dirInfo.Name;
                string abName = dir.Replace("/", "-").Replace(" ", "-") + ".ab";

                var files = PathHelper.FindFiles(dirInfo.FullName);

                // var files = AssetDatabase.GetAllAssetPaths().ToList()
                // .FindAll(path => path.StartsWith(dir) && !Directory.Exists(path));
                files.ToList().ForEach(delegate(FileInfo file)
                {
                    // 需要忽略的文件
                    if (CheckIgnore(file.Name)) return;
                    // originPath[file.ToLower()] = file;
                    var asp = PathHelper.GetAssetsRelative(file.FullName, PathHelper.GetAssetsPath());
                    AssetImporter.GetAtPath(asp).assetBundleName = abName;
                });
            }

            var subDir = AssetDatabase.GetAllAssetPaths().ToList()
                .FindAll(path => path.StartsWith("Assets/Projects/FGui") && Directory.Exists(path));
            subDir.ForEach(delegate(string dir) { });
        }

        /// <summary>
        /// 1.0 检查版本目录是否存在
        /// </summary>
        private void CheckVersionPath()
        {
            // Publish/iOS/_Cache/1.1
            versionCachePath = PathHelper.Combine(
                BuilderStep1._.GetCachePath(),
                BuilderStep1._.VersionStr());
            PathHelper.CheckOrCreate(versionCachePath);

            // Publish/iOS/1.1.1
            versionPublishPath = PathHelper.Combine(
                BuilderStep1._.GetPublishPath(),
                BuilderStep1._.Version().FullVersion());
            PathHelper.CheckOrCreate(versionPublishPath);
        }

        private void BundleAssetPackage()
        {
            BuildTarget buildTarget =
                EnumConvert.RuntimeTargetToBuildTarget(BuilderStep1._.GetBundleBuilderConf().TargetMode);
            Tuner.Log($"[>>] Build Target {versionCachePath} - {buildTarget}");

            var abmf = BuildPipeline.BuildAssetBundles(
                versionCachePath,
                BuildAssetBundleOptions.None,
                buildTarget);

            Generation(abmf);

            AssetDatabase.GetAllAssetBundleNames().ForEach(abName => AssetDatabase.RemoveAssetBundleName(abName, true));
        }

        /// <summary>
        /// 形成装配文件
        /// </summary>
        void Generation(AssetBundleManifest abmf)
        {
            // Publish/iOS/_Cache/1.1/ 1.1    文件
            // var path = PathHelper.Combine(
            //     PathHelper.GetCurrentPath(),
            //     BuilderStep2._.VersionCachePath(),
            //     BuilderStep1._.VersionStr());
            // Debug.Log(path);
            // AssetBundle.UnloadAllAssetBundles(false);
            // AssetBundle ab = AssetBundle.LoadFromFile(path); // 加载总ManifestAssetBundle
            // AssetBundleManifest abmf = (AssetBundleManifest) ab.LoadAsset("AssetBundleManifest");

            // 根据官方装配文件，生成自己的装配文件
            var mf = BuilderStep1._.GetManifest();
            abmf.GetAllAssetBundles().ToList().ForEach(delegate(string abName)
            {
                // Publish/iOS/_Cache/1.1/1.1.manifest
                var subMfPath =
                    PathHelper.Combine(PathHelper.GetCurrentPath(), VersionCachePath(), abName + ".manifest");

                var desc = new ManifestDesc(File.ReadAllText(subMfPath));
                desc.Assets.ForEach(delegate(string asstePath)
                {
                    var data = new ManifestData();
                    data.AssetPath = asstePath;
                    data.Type = BundleType.AssetBundle;
                    data.ABName = abName;
                    data.Checksum = desc.CRC.ToString();
                    data.Dependencies = abmf.GetAllDependencies(abName).ToList();
                    if (IsContent(data))
                        mf.ManifestList.Add(data);
                });
            });
        }


        private bool IsContent(ManifestData data)
        {
            // int filterStart = 10;
            // // 第七章之后的内容不打包
            // if (data.AssetPath.Contains("Assets/Projects/UI/Chapter/Chapter"))
            // {
            //     for (int i = filterStart; i < 20; i++)
            //     {
            //         if (data.AssetPath.Contains($"Assets/Projects/UI/Chapter/Chapter{i}"))
            //             return false;
            //     }
            // }
            //
            // // "Assets/Projects/Audio/Dubbing/voice_ch10_104_01.wav"
            // if (data.AssetPath.Contains("Assets/Projects/Audio/Dubbing/voice"))
            // {
            //     // for (int i = filterStart; i < 10; i++)
            //     // {
            //     //     if (data.AssetPath.Contains($"Assets/Projects/Audio/Dubbing/voice_ch0{i}"))
            //     //         return false;
            //     // }
            //
            //     // for (int i = filterStart; filterStart >= 10 && i < 15; i++)
            //     // {
            //         // if (data.AssetPath.Contains($"Assets/Projects/Audio/Dubbing/voice_ch{i}"))
            //         // 开放前9章音频
            //         if (data.AssetPath.Contains($"Assets/Projects/Audio/Dubbing/voice_ch1"))
            //             return false;
            //     // }
            // }

            return true;
        }
    }
}