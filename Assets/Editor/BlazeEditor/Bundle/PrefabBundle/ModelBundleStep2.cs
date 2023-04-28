using System.IO;
using System.Linq;
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
    public class ModelBundleStep2 : Singeton<ModelBundleStep2>
    {
        public void Execution()
        {
            ProcessAssetBundle();
            BundleAssetPackage();
        }

        private void ProcessAssetBundle()
        {
            AssetDatabase.GetAllAssetBundleNames().ForEach(abName => AssetDatabase.RemoveAssetBundleName(abName, true));
            // var files = AssetDatabase.GetAllAssetPaths().ToList()
            //     .FindAll(path =>
            //         path.StartsWith("Assets/Projects/Models/" + ModelBundleStep1._.Name) && !Directory.Exists(path));
            //  var packageName = "Assets/Projects/Models/" + ModelBundleStep1._.Name;
            var guids = AssetDatabase.FindAssets("t:Model",
                new[] {"Assets/Projects/3d/Models/" + ModelBundleStep1._.Name});

            guids.ForEach(uid =>
            {
                var file = AssetDatabase.GUIDToAssetPath(uid);
                if (CheckIgnore(file)) return;
                var packageName = file.Replace('.', '/');
                var abName = packageName.Replace("/", "-") + ".ab";
                AssetImporter.GetAtPath(file).assetBundleName = abName;
            });

            bool CheckIgnore(string fileName)
            {
                if (fileName.EndsWith(".DS_Store")
                    || fileName.EndsWith(".meta")
                    || fileName.EndsWith(".MD")
                   ) return true;
                return false;
            }
        }

        private void BundleAssetPackage()
        {
            ModelBundleStep1._.TargetPlatform.ForEach(m =>
            {
                BuildTarget buildTarget = EnumConvert.RuntimeTargetToBuildTarget(m);
                var abmf = BuildPipeline.BuildAssetBundles(
                    ModelBundleStep1._.GetCachePath(m),
                    BuildAssetBundleOptions.None,
                    buildTarget
                );
                Generation(abmf, m);
            });
            AssetDatabase.GetAllAssetBundleNames().ForEach(abName => AssetDatabase.RemoveAssetBundleName(abName, true));
        }

        private void Generation(AssetBundleManifest abmf, EnumRuntimeTarget target)
        {
            var mf = ModelBundleStep1._.GetManifestInfo(target);
            abmf.GetAllAssetBundles().ToList().ForEach(abName =>
            {
                var subMfPath = PathHelper.Combine(PathHelper.GetCurrentPath(), ModelBundleStep1._.GetCachePath(target),
                    abName + ".manifest");
                var des = new ManifestDesc(File.ReadAllText(subMfPath));
                des.Assets.ForEach(assetPath =>
                {
                    var data = new ManifestData();
                    data.AssetPath = assetPath;
                    data.Type = BundleType.AssetBundle;
                    data.ABName = abName;
                    data.Checksum = des.CRC.ToString();
                    data.Dependencies = abmf.GetAllDependencies(abName).ToList();
                    mf.ManifestList.Add(data);
                });
            });
        }
    }
}