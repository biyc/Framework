using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blaze.Bundle;
using Blaze.Bundle.PrefabBundle;
using Blaze.Ci;
using Blaze.Common;
using Blaze.Resource.Common;
using Blaze.Utility.Helper;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

namespace Editor.BuildEditor
{
    public class BuildModelAB : OdinEditorWindow
    {
        /// <summary>
        /// 是否打包安卓和ios
        /// </summary>
        [ShowInInspector] public static bool AndroidIos = false;

        /// <summary>
        /// 包的类型
        /// </summary>
        // [ShowInInspector, HideIf("AndroidIos")]
        // public static EnumPackageType PackageType;

        /// <summary>
        /// 构建目标平台
        /// </summary>
        [ShowInInspector, HideIf("AndroidIos")]
        public static EnumRuntimeTarget TargetPlatform = EnumRuntimeTarget.Android;

        /// <summary>
        /// 模型名字
        /// </summary>
        [ShowInInspector] public static string ModelIndexs;


        /// <summary>
        /// 输出路径
        /// </summary>
        [ShowInInspector, ValueDropdown("_outPaths")]
        public static string OutPath;

        private static List<string> _outPaths;


        /// <summary>
        /// 是否打包所有模型
        /// </summary>
        [HideInInspector] public static bool IsBuildAllModel = false;


        private static BuildModelAB _window;

        private static ModelABBuildConfig _abBuildConfig;


        [MenuItem("Assets/BuildModelAB", false, 3)]
        public static void BuildSinglePrefabAB()
        {
            var obj = Selection.objects;
            if (obj.Length == 0) return;

            for (var i = 0; i < obj.Length; i++)
            {
                var path = AssetDatabase.GetAssetPath(obj[i]);
                if (!Directory.Exists(path) || !path.StartsWith("Assets/Projects/3d/Models") ||
                    path.Split('/').Length != 5)
                {
                    EditorUtility.DisplayDialog("", "请选择有效文件夹", "yes", "no");
                    return;
                }
            }

            var indexs = "";
            obj.ToList().ConvertAll(m => m.name.Split('_')[0]).ForEach(m => indexs += (m + "|"));
            ModelIndexs = indexs;
            IsBuildAllModel = false;
            _outPaths = ModelABOutPathConfig.GetModelConfig().OutPaths;
            _window = GetWindow<BuildModelAB>();
            _window.Show();
        }

        /// <summary>
        /// 打ab包
        /// </summary>
        [Button("BuildBundle")]
        public static void Build()
        {
            if (!string.IsNullOrEmpty(OutPath))
            {
                if (!Directory.Exists(OutPath))
                {
                    EditorUtility.DisplayDialog("", "该输出路径不存在，请检查！", "Ok");
                    return;
                }

                PathHelper.CheckOrCreate(PathHelper.Combine(OutPath, "Android"));
                PathHelper.CheckOrCreate(PathHelper.Combine(OutPath, "Ios"));
            }

            _window.Close();
            var abBuildConfig = new ModelABBuildConfig()
            {
                AndroidIos = AndroidIos,
                ExtraOutPath = OutPath,
                //PackageType = PackageType,
            };
            // if (AndroidIos)
            //     abBuildConfig.RuntimeTarget = new List<EnumRuntimeTarget>()
            //         {EnumRuntimeTarget.Android, EnumRuntimeTarget.IOS};
            // else
            //     abBuildConfig.RuntimeTarget = new List<EnumRuntimeTarget>() {TargetPlatform};
            var allModelNames = Directory.GetDirectories("Assets/Projects/3d/Models").ToList()
                .ConvertAll(m => new DirectoryInfo(m).Name);
            var names = new List<string>();
            ModelIndexs.Split('|').ForEach(m =>
            {
                if (string.IsNullOrEmpty(m)) return;
                var n = allModelNames.Find(v => v.Split('_')[0] == m);
                if (!string.IsNullOrEmpty(n))
                    names.Add(n);
                else
                    Debug.LogError($"该索引未找到模型，请确认！[>>] {m} [<<]");
            });

            var list = new List<EnumRuntimeTarget>();

            if (AndroidIos)
                list = new List<EnumRuntimeTarget>() {EnumRuntimeTarget.Android, EnumRuntimeTarget.IOS};
            else
                list = new List<EnumRuntimeTarget>() {TargetPlatform};

            list.ForEach(m =>
            {
                abBuildConfig.RuntimeTarget = new List<EnumRuntimeTarget>() {m};
                BuildModel(names, abBuildConfig, IsBuildAllModel);
            });
            //BuildModel(names, abBuildConfig, IsBuildAllModel);
        }

        public static void BuildModel(List<string> names, ModelABBuildConfig config, bool isBuildAllModel = false)
        {
            _abBuildConfig = config;

            if (isBuildAllModel)
            {
                var dirs = Directory.GetDirectories("Assets/Projects/3d/Models");
                dirs.ForEach(m => BuildModel(new DirectoryInfo(m).Name));
            }
            else
                names.ForEach(async v =>
                {
                    if (!string.IsNullOrEmpty(v))
                        BuildModel(v);
                });

            AssetDatabase.Refresh();
        }

        /// <summary>
        ///打包模型ab包
        /// </summary>
        /// <param name="name"></param>
        private static void BuildModel(string name)
        {
            if (!ModelPreProcess._.Execution(name))
                return;
            _abBuildConfig.Name = name;
            ModelBundleStep1._.Execution(_abBuildConfig);
            ModelBundleStep2._.Execution();
            ModelBundleStep3._.Execution();
        }
    }
}