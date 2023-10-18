//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/13 | Initialize core skeleton |
*/

using Blaze.Bundle;
using Blaze.Helper;
using Blaze.Utility;
using UnityEditor;
using UnityEngine;

namespace Blaze
{
    /// <summary>
    /// 编辑器启动器,初始化编辑器的所有初始化的方法
    /// </summary>
    [InitializeOnLoad]
    public static class EditorStarter
    {
        static EditorStarter()
        {
            Tuner.Inco("Thor Editor Engine Starting");
            Tuner.Log(Co._(
                $"[>>] Application platform is; [{Application.platform.ToString().ToUpper()}] :lightred:b;"));
            // 配置系统初始化
            ThorSettings._.Initialize();
        }


        [MenuItem("Tools/Thor/Locate Settings", priority = 2051)]
        internal static void ShowSettingsInspector()
        {
            var setting = ThorSettings._;
            EditorGUIUtility.PingObject(setting);
            Selection.activeObject = setting;
        }
    }
}