//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/06 | Initialize core skeleton |
*/

using System;
using System.Threading;
using Blaze.Bundle;
using Blaze.Ci;
using Blaze.Common;
using Blaze.Utility.Helper;
// using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class ThorSettings : ScriptableObject
{
    // 默认的设置存放位置
    const string TplSettingsAssetPath = "Assets/Configs/ThorSettings.asset";

#pragma warning disable

    [Tooltip("XLSX自动转CSV"), SerializeField]
    public bool IsAutoXlsxToCsv = false;

    [Tooltip("启动DATA HTTP服务端口8087"), SerializeField]
    public bool IsUseDataHttp = false;

    [Tooltip("启动AB HTTP服务端口8088"), SerializeField]
    public bool IsUseAbHttp = true;


    [FormerlySerializedAs("CurrentTargetMode")] [Tooltip("目标资源类型"), SerializeField]
    public EnumRuntimeTarget BuildTargetMode = EnumRuntimeTarget.EditorOSX;

    [Tooltip("构建发布人"), SerializeField] public string BuildOperator = "";

    [Tooltip("发布到StreamingAsset目录"), SerializeField]
    public bool IsPublishStreaming = false;


    [Tooltip("指定版本安装包"), SerializeField]
    public EnumPackageType PackageType = EnumPackageType.AndroidDev;



#pragma warning restore

    /// <summary>
    /// 获取模板参数实例
    /// </summary>
    public static ThorSettings _ => GetSettings();

    private static ThorSettings GetSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<ThorSettings>(TplSettingsAssetPath);
        if (settings == null)
        {
            settings = CreateInstance<ThorSettings>();
            PathHelper.CheckOrCreate("Assets/Configs");
            AssetDatabase.CreateAsset(settings, TplSettingsAssetPath);
        }

        return settings;
    }

    /// <summary>
    /// 执行配置文件的初始化
    /// </summary>
    public void Initialize()
    {
        GetSettings();
    }
}