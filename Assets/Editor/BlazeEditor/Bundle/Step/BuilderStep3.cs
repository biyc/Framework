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
using System.Threading;
using System.Threading.Tasks;
using Blaze.Core;
using Blaze.Resource.AssetBundles.Data;
using Blaze.Utility;
using Blaze.Utility.Helper;
using Sirenix.Utilities;
using UnityEngine;
using ManifestInfo = Blaze.Resource.AssetBundles.Data.ManifestInfo;

namespace Blaze.Bundle.Step
{
    public class BuilderStep3 : Singeton<BuilderStep3>
    {
        // static ThorPreferences _cfg = ThorPreferences._;


        /// <summary>
        /// 执行主程序
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> Execution()
        {
            return Publish();
        }


        /// <summary>
        /// 发布所有打包文件
        /// </summary>
        Task<bool> Publish()
        {
            PathHelper.CheckOrCreate(BuilderStep1._.GetPublishStreamingPath());
            // 删除 StreamingAsset
            Directory.Delete(PathHelper.GetStreamingPath(), true);
            // 删除发布版本目录
            Directory.Delete(BuilderStep2._.VersionPublishPath(), true);
            // 重新创建发布版本目录
            PathHelper.CheckOrCreate(PathHelper.GetStreamingPath());
            PathHelper.CheckOrCreate(BuilderStep2._.VersionPublishPath());

            var cachePath = BuilderStep2._.VersionCachePath();

            ManifestInfo _manifest = BuilderStep1._.GetManifest();
            _manifest.Version = BuilderStep1._.Version();

            // 建立 md5 数据
            Dictionary<string, string> fileToMd5 = new Dictionary<string, string>();
            _manifest.ManifestList.ForEach(delegate(ManifestData data)
            {
                if (!fileToMd5.ContainsKey(data.ABName))
                {
                    var filePath = PathHelper.Combine(cachePath, data.ABName);
                    data.Md5 = CryptoHelper.FileMD5(filePath);
                    fileToMd5.Add(data.ABName, data.Md5);
                }
                else
                {
                    // 找到已创建的哈希
                    data.Md5 = fileToMd5[data.ABName];
                }
            });


            // 建立文件大小数据
            Dictionary<string, long> fileToSize = new Dictionary<string, long>();
            _manifest.ManifestList.ForEach(delegate(ManifestData data)
            {
                if (!fileToSize.ContainsKey(data.ABName))
                {
                    var filePath = PathHelper.Combine(cachePath, data.ABName);
                    data.Size = new FileInfo(filePath).Length;
                    fileToSize.Add(data.ABName, data.Size);
                }
                else
                {
                    // 找到已创建的哈希
                    data.Size = fileToSize[data.ABName];
                }
            });

            // long total = 0;
            // _manifest.ManifestList.ForEach(delegate(ManifestData data)
            // {
            //     if (data.AssetPath.Contains("Assets/Projects/UI/Chapter/Chapter5"))
            //     {
            //         total += data.Size;
            //     }
            //
            //     if (data.AssetPath.Contains("Assets/Projects/Audio/Dubbing/voice_ch01"))
            //     {
            //         total += data.Size;
            //     }
            // });
            //
            // Debug.Log("容量：" + total / 1024);


            // 为文件创建哈希
            // File -> Hash
            Dictionary<string, string> fileToHash = new Dictionary<string, string>();
            _manifest.ManifestList.ForEach(delegate(ManifestData data)
            {
                if (!fileToHash.ContainsKey(data.ABName))
                {
                    // 给文件创建全新哈希 (文件名 + MD5)
                    data.Hash = CryptoHelper.MD5Encode(data.ABName + data.Md5) + ".ab";
                    // data.Hash = CryptoHelper.ShortId(data.File + data.Md5, true) + ".ab";
                    fileToHash.Add(data.ABName, data.Hash);
                }
                else
                {
                    // 找到已创建的哈希
                    data.Hash = fileToHash[data.ABName];
                }
            });
            // 保存装配信息到 版本目录
            var publishPath = BuilderStep2._.VersionPublishPath();
            _manifest.Config(publishPath);
            _manifest.Save();

            if (BuilderStep1._.GetBundleBuilderConf().IsPublishStreaming)
            {
                // 保存装配信息到 SA目录
                PathHelper.CheckOrCreate(BuilderStep1._.GetPublishStreamingPath());
                _manifest.Config(PathHelper.Combine(PathHelper.GetStreamingPath(), BuilderStep1._.GetTargetMode()));
                _manifest.Save();
            }

            var task = new TaskCompletionSource<bool>();


            var publishSAPath = BuilderStep1._.GetPublishStreamingPath();
            new Thread(new ThreadStart(delegate
                {
                    // 从原始 cache 目录到 具体的版本目录
                    foreach (var pair in fileToHash)
                    {
                        // 从源文件到哈希文件
                        File.Copy(
                            PathHelper.Combine(cachePath, pair.Key),
                            PathHelper.Combine(publishPath, pair.Value),
                            true
                        );
                    }

                    if (BuilderStep1._.GetBundleBuilderConf().IsPublishStreaming)
                    {
                        // 从版本目录复制到 sa 目录
                        Debug.Log("开始复制文件到 AS 文件夹");
                        foreach (var pair in fileToHash)
                        {
                            // 从哈希文件到哈希文件
                            File.Copy(
                                PathHelper.Combine(publishPath, pair.Value),
                                PathHelper.Combine(publishSAPath, pair.Value),
                                true
                            );
                        }
                    }

                    task.SetResult(true);

                    Debug.Log("bundle 文件生成成功");
                }
            )).Start();

            return task.Task;
        }
    }
}