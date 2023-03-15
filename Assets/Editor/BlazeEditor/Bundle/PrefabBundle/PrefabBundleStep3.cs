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
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Blaze.Bundle.PrefabBundle
{
    public class PrefabBundleStep3 : Singeton<PrefabBundleStep3>
    {
        public Task<bool> Execution()
        {
            return Publish();
        }

        private Task<bool> Publish()
        {
            var manifest = PrefabBundleStep1._.ManifestInfo;

            Dictionary<string, string> fileToMd5 = new Dictionary<string, string>();
            manifest.ManifestList.ForEach(data =>
            {
                if (!fileToMd5.ContainsKey(data.ABName))
                {
                    var filePath = PathHelper.Combine(PrefabBundleStep1._.GetCachePath(), data.ABName);
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
                    var filePath = PathHelper.Combine(PrefabBundleStep1._.GetCachePath(), data.ABName);
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
                    data.Hash = CryptoHelper.MD5Encode(data.ABName + data.Md5) + ".ab";
                    fileToHash.Add(data.ABName, data.Hash);
                }
                else
                {
                    data.Hash = fileToHash[data.ABName];
                }
            });
            manifest.Config(PrefabBundleStep1._.GetPublishPath());
            manifest.Save();

            var task = new TaskCompletionSource<bool>();
            new Thread(new ThreadStart(() =>
                {
                    foreach (var pair in fileToHash)
                    {
                        File.Copy(PathHelper.Combine(PrefabBundleStep1._.GetCachePath(), pair.Key),
                            PathHelper.Combine(PrefabBundleStep1._.GetPublishPath(), pair.Value),
                            true);
                    }

                    task.SetResult(true);
                    Debug.Log($"{PrefabBundleStep1._.Name} bundle 文件生成成功");
                }))
                .Start();
            return task.Task;
        }
    }
}