using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blaze.Bundle;
using Blaze.Common;
using Blaze.Utility;
using Blaze.Utility.Helper;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Networking;

namespace Blaze.Ci
{
    public class ProjectsBuild
    {
        /// <summary>
        /// 构建开发包
        /// </summary>
        [MenuItem("Tools/Build/AndroidDevApk")]
        public static void BuildAndroid()
        {
            Build(EnumPackageType.AndroidDev);
        }

        /// <summary>
        /// 构建开发包
        /// </summary>
        [MenuItem("Tools/Build/AndroidDevBundle")]
        public static void BuildAndroidDevBundle()
        {
            BuildAssetBundle(EnumPackageType.AndroidDev);
            // PushToDingding($"AndroidDevAssetBundle 打包成功");
        }

        [MenuItem("Tools/Build/构建指定版本安装包")]
        public static void BuildAndroidRelase()
        {
            Build(ThorSettings._.PackageType);
        }

        [MenuItem("Tools/Build/构建AssetBundle")]
        public static void BuildAssetBundleMenm()
        {
            BuildAssetBundle(ThorSettings._.PackageType);
            PushToDingding($"{ThorSettings._.PackageType}AssetBundle 打包成功");
        }


        // [MenuItem("Tools/Build/创建所有配置文件")]
        // public static void CreateGameSettings()
        // {
        //     foreach (EnumPackageType item in Enum.GetValues(typeof(EnumPackageType)))
        //     {
        //         GetGameSettings(item);
        //     }
        // }
        //
        //
        // [MenuItem("Tools/Build/创建所有ab")]
        // public static async void CreateAllAb()
        // {
        //     foreach (EnumPackageType item in Enum.GetValues(typeof(EnumPackageType)))
        //     {
        //         Tuner.Log("开始构建" + item);
        //         await BuildAssetBundle(item);
        //         Tuner.Log("结束构建" + item);
        //     }
        // }

        /// <summary>
        /// 
        /// 设置不同平台的配置文件
        /// </summary>
        private static GameSettings GetGameSettings(EnumPackageType packageType)
        {
            // 创建配置文件
            var gameSettings =
                AssetDatabase.LoadAssetAtPath<GameSettings>(GameSettings.GetFullPath(packageType.ToString()));
            if (gameSettings == null)
            {
                gameSettings = GameSettings.CreateInstance<GameSettings>();
                gameSettings.MotherVersion = 1;
                AssetDatabase.CreateAsset(gameSettings, GameSettings.GetFullPath(packageType.ToString()));
            }

            gameSettings.Channel = packageType.ToString();
            gameSettings.UseAssetBundle = true;


            // 初始安装包中是否包含 AB 资源
            switch (packageType)
            {
                case EnumPackageType.AndroidRelease:
                case EnumPackageType.IOSRelease:
                //case EnumPackageType.AndroidTestOnline:
                // case EnumPackageType.IOSTestOnline:
                case EnumPackageType.AndroidTestInner:
                case EnumPackageType.IOSTestInner:
                    // case EnumPackageType.AndroidVerify:
                    // case EnumPackageType.IOSVerify:
                    // case EnumPackageType.AndroidM3839FirstTest:
                    gameSettings.IsContentAssetBundle = true;
                    break;
                default:
                    gameSettings.IsContentAssetBundle = false;
                    break;
            }

            // 强制版本检测 IsForceCheckVersion
            gameSettings.IsForceCheckVersion = false;
            // switch (packageType)
            // {
            //     case EnumPackageType.AndroidRelease:
            //     case EnumPackageType.IOSRelease:
            //         // case EnumPackageType.AndroidTestOnline:
            //         // case EnumPackageType.IOSTestOnline:
            //         // case EnumPackageType.AndroidTestInner:
            //         // case EnumPackageType.IOSTestInner:
            //         // 开启强制版本检测
            //         gameSettings.IsForceCheckVersion = true;
            //         break;
            //     default:
            //         // 关闭强制版本检测
            //         gameSettings.IsForceCheckVersion = false;
            //         break;
            // }

            // 是否开启调试模式 UseDev
            switch (packageType)
            {
                case EnumPackageType.AndroidDev:
                case EnumPackageType.IOSDev:
                case EnumPackageType.EditorOSXDev:
                case EnumPackageType.EditorWin64Dev:
                case EnumPackageType.AndroidTestInner:
                case EnumPackageType.IOSTestInner:
                    gameSettings.UseDev = true;
                    break;
                default:
                    // 默认不开启DEV模式
                    gameSettings.UseDev = false;
                    break;
            }

            // 是否使用 ilruntime IsILRuntime
            gameSettings.UseILRuntime = true;
            // switch (packageType)
            // {
            //     case EnumPackageType.AndroidVerify:
            //     case EnumPackageType.IOSVerify:
            //         // case EnumPackageType.AndroidM3839Dev:
            //         gameSettings.UseILRuntime = false;
            //         break;
            //     default:
            //         gameSettings.UseILRuntime = true;
            //         break;
            // }


            // 资源服务器地址列表
            gameSettings.ResServerList = new List<string>();
            switch (packageType)
            {
                case EnumPackageType.AndroidRelease:
                case EnumPackageType.IOSRelease:
                case EnumPackageType.AndroidTestInner:
                case EnumPackageType.IOSTestInner:
                    //https://zhiyuanzhongyi.obs.cn-east-3.myhuaweicloud.com/3d/AndroidRelease/Android/VersionCheck.json
                    // gameSettings.ResServerList.Add($"https://lv2m3839first.oss-cn-chengdu.aliyuncs.com/zhongyao/{packageType.ToString()}");
                    // gameSettings.ResServerList.Add(
                    //     $"https://zhiyuanzhongyi.obs.cn-east-3.myhuaweicloud.com/3d/{packageType.ToString()}");
                    gameSettings.ResServerList.Add(null);
                    break;
                case EnumPackageType.AndroidDev:
                case EnumPackageType.IOSDev:
                case EnumPackageType.EditorOSXDev:
                case EnumPackageType.EditorWin64Dev:
                    gameSettings.ResServerList.Add($"http://192.168.8.6:8088/{packageType.ToString()}"); //公司服务器
                    gameSettings.ResServerList.Add($"http://192.168.1.4:8088/{packageType.ToString()}"); //家里
                    break;
            }

            // 应用名称
            switch (packageType)
            {
                case EnumPackageType.AndroidRelease:
                case EnumPackageType.IOSRelease:
                    gameSettings.ProductName = "知源中医";
                    break;
                default:
                    gameSettings.ProductName = packageType.ToString();
                    break;
            }


            // 保存配置文件
            EditorUtility.SetDirty(gameSettings);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return gameSettings;
        }

        private static void SaveAsset(UnityEngine.Object target)
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static async void Build(EnumPackageType packageType)
        {
            // 打包类型
            BuildTarget buildTarget = BuildTarget.Android;
            if (packageType.ToString().ToLower().Contains("android"))
                buildTarget = BuildTarget.Android;
            if (packageType.ToString().ToLower().Contains("ios"))
                buildTarget = BuildTarget.iOS;

            // 切换到 对应 平台
            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);


            // 设置配置文件
            var gameSettings = GetGameSettings(packageType);
            // build 号自增
            gameSettings.BuildNum++;
            SaveAsset(gameSettings);
            var commonSetting = AssetDatabase.LoadAssetAtPath<CommonSetting>(CommonSetting.GetFullPath());
            if (commonSetting == null)
            {
                commonSetting = CommonSetting.CreateInstance<CommonSetting>();
                AssetDatabase.CreateAsset(commonSetting, CommonSetting.GetFullPath());
            }

            commonSetting.GameSetting = packageType.ToString();
            EditorUtility.SetDirty(commonSetting);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            PlayerSettings.companyName = "nineton"; //公司名称
            // 应用名称
            PlayerSettings.productName = gameSettings.ProductName; //


            if (packageType == EnumPackageType.AndroidRelease)
                PlayerSettings.applicationIdentifier = "com.nineton.tcm";
            else
                PlayerSettings.applicationIdentifier = "com.nineton.tcm" + packageType;
            if (packageType.ToString().ToLower().Contains("android"))
            {
                PlayerSettings.bundleVersion = gameSettings.GetVersion();
            }
            else if (packageType.ToString().ToLower().Contains("ios"))
            {
                PlayerSettings.bundleVersion = gameSettings.GetVersionIos();
            }

            PlayerSettings.Android.bundleVersionCode = gameSettings.BuildNum;
            PlayerSettings.Android.keystoreName = "build/biyc.keystore";
            PlayerSettings.Android.keystorePass = "biyc123";
            PlayerSettings.Android.keyaliasName = "zhongyao";
            PlayerSettings.Android.keyaliasPass = "zhongyao123";

            // 关闭启动页unity logo显示
            PlayerSettings.SplashScreen.show = false;


            if (buildTarget == BuildTarget.Android)
            {
                // if (packageType == EnumPackageType.AndroidM3839Dev)
                // {
                //     PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
                // }
                // else
                // {
                //     
                // }
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android,
                    Il2CppCompilerConfiguration.Release);
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
            }

            // 执行AB 打包
            switch (packageType)
            {
                case EnumPackageType.IOSTestInner:
                    break;
                default:
                    // 删除 StreamingAsset
                    if (Directory.Exists(PathHelper.GetStreamingPath()))
                        Directory.Delete(PathHelper.GetStreamingPath(), true);
                    await BuildAssetBundle(packageType);
                    break;
            }


            //设置BuildSetting中的打包场景配置
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            //要把Init场景放在第一个，是初始加载场景，如果把Empty场景放前边，会什么都不显示，报dlopen异常
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene($"Assets/Scenes/Publish.unity", true));
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene("Assets/Scenes/Empty.unity", true));
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

            List<string> levels = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                // 获取有效的 Scene
                levels.Add(scene.path);
            }

            string apkName = $"{gameSettings.GetVersion()}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.apk";

            string outPath = "";
            if (packageType.ToString().ToLower().Contains("android"))
                outPath = $"Publish/Install/{apkName}";
            if (packageType.ToString().ToLower().Contains("ios"))
                outPath = $"Publish/Install/IOS_OUT";


            // 执行打包
            BuildReport buildReport =
                BuildPipeline.BuildPlayer(levels.ToArray(), outPath, buildTarget, BuildOptions.None);


            Debug.Log("开发版 APK 打包完成");

            // 推送打包成功信息
            PushToDingding($"打包成功 http://192.168.8.16:8088/Install/{apkName}");

            // 应用名称
            //打包完成后将项目的名字恢复
            PlayerSettings.productName = EnumPackageType.EditorWin64Dev.ToString(); //
        }

        private static Task<bool> BuildAssetBundle(EnumPackageType target)
        {
            var game = GetGameSettings(target);
            var conf = new BundleBuilderConf();
            conf.TargetMode = ThorSettings._.BuildTargetMode;
            if (target.ToString().Contains("Android"))
                conf.TargetMode = EnumRuntimeTarget.Android;
            if (target.ToString().Contains("IOS"))
                conf.TargetMode = EnumRuntimeTarget.iOS;
            if (target == EnumPackageType.EditorWin64Dev)
                conf.TargetMode = EnumRuntimeTarget.EditorWin64;
            if (target == EnumPackageType.EditorOSXDev)
                conf.TargetMode = EnumRuntimeTarget.EditorOSX;


            conf.IsPublishStreaming = game.IsContentAssetBundle;
            conf.BuildOperator = game.Channel;
            // 构建渠道
            conf.Channel = game.Channel;
            conf.Major = game.MotherVersion;
            // 执行构建
            return ThorBundleBuilder._.Execution(conf);
        }

        private static void PushToDingding(string msg)
        {
            string data =
                "{\"msgtype\": \"text\",\"text\": {\"content\":\"" + msg + "\"}}";
            var url =
                "https://oapi.dingtalk.com/robot/send?access_token=cbe1a5c2b3616945dc36eb52e618caeefd679824b6b47ee3c9053d938dc3ad20";
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(data); // 把字符串转换为bype数组
            var www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            www.chunkedTransfer = false;
            www.uploadHandler = new UploadHandlerRaw(postData);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application/json");
            www.SendWebRequest();
        }
    }
}