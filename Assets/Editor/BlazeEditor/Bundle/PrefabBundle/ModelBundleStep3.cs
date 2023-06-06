﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blaze.Bundle.Data;
using Blaze.Ci;
using Blaze.Common;
using Blaze.Core;
using Blaze.Resource.AssetBundles.Data;
using Blaze.Utility.Helper;
using ETModel;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Blaze.Bundle.PrefabBundle
{
    public class ModelBundleStep3 : Singeton<ModelBundleStep3>
    {
        public void Execution()
        {
            ModelBundleStep1._.TargetPlatform.ForEach(m => { Publish(m); });
            //return Publish();
        }

        private Task<bool> Publish(EnumRuntimeTarget target)
        {
            var manifest = ModelBundleStep1._.GetManifestInfo(target);

            Dictionary<string, string> fileToMd5 = new Dictionary<string, string>();
            manifest.ManifestList.ForEach(data =>
            {
                if (!fileToMd5.ContainsKey(data.ABName))
                {
                    var filePath = PathHelper.Combine(ModelBundleStep1._.GetCachePath(target), data.ABName);
                    data.Md5 = CryptoHelper.FileMD5(filePath);
                    fileToMd5.Add(data.ABName, data.Md5);
                }
                else
                {
                    data.Md5 = fileToMd5[data.ABName];
                }
            });
            Dictionary<string, long> fileToSize = new Dictionary<string, long>();

            manifest.ManifestList.ForEach(data =>
            {
                if (!fileToSize.ContainsKey(data.ABName))
                {
                    var filePath = PathHelper.Combine(ModelBundleStep1._.GetCachePath(target), data.ABName);
                    data.Size = new FileInfo(filePath).Length;
                    fileToSize.Add(data.ABName, data.Size);
                }
                else
                {
                    data.Size = fileToSize[data.ABName];
                }
            });

            Dictionary<string, string> fileToHash = new Dictionary<string, string>();
            manifest.ManifestList.ForEach(data =>
            {
                if (!fileToHash.ContainsKey(data.ABName))
                {
                    // data.Hash = CryptoHelper.MD5Encode(data.ABName + data.Md5) + ".ab";
                    data.Hash = CryptoHelper.MD5Encode(data.AssetPath) + ".ab";
                    fileToHash.Add(data.ABName, data.Hash);
                }
                else
                {
                    data.Hash = fileToHash[data.ABName];
                }
            });
            // Debug.Log(ModelBundleStep1._.Name);
            manifest.Config(ModelBundleStep1._.GetPublishPath(target));
            manifest.Save();
            PathHelper.CheckOrCreate(ModelBundleStep1._.GetPublishPath(target));

            var task = new TaskCompletionSource<bool>();
            new Thread(new ThreadStart(() =>
                {
                    foreach (var pair in fileToHash)
                    {
                        File.Copy(PathHelper.Combine(ModelBundleStep1._.GetCachePath(target), pair.Key),
                            PathHelper.Combine(ModelBundleStep1._.GetPublishPath(target), pair.Value),
                            true);
                        CryptoHelper.ABOffsetEncrypt(
                            PathHelper.Combine(ModelBundleStep1._.GetPublishPath(target), pair.Value),
                            pair.Value);
                    }

                    if (!String.IsNullOrEmpty(ModelBundleStep1._.GetExtraOutPath(target)))
                    {
                        var files = new DirectoryInfo(ModelBundleStep1._.GetPublishPath(target)).GetFiles();
                        files.ForEach(v =>
                        {
                            File.Copy(v.FullName,
                                PathHelper.Combine(ModelBundleStep1._.GetExtraOutPath(target), v.Name),
                                true);
                        });
                    }

                    task.SetResult(true);
                    Debug.Log($"{ModelBundleStep1._.Name} bundle 文件生成成功");
                }))
                .Start();
            return task.Task;
        }
    }
}