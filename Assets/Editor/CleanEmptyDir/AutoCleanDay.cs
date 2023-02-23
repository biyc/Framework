using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AltProg.CleanEmptyDir
{
    [InitializeOnLoad]
    public class AutoCleanDay
    {
        static AutoCleanDay()
        {
            var key = "CleanDieTime";
            var lastTime = PlayerPrefs.GetString(key);
            // 第一次
            if (string.IsNullOrEmpty(lastTime))
            {
                CleanEmptyDir();
                PlayerPrefs.SetString(key, DateTime.Today.ToString());
                return;
            }

            // 超过一天，清理一次
            if (DateTime.Today.Subtract(DateTime.Parse(lastTime)).Days > 0)
            {
                CleanEmptyDir();
                PlayerPrefs.SetString(key, DateTime.Today.ToString());
                return;
            }
        }

        // 清理目录
        private static void CleanEmptyDir()
        {
            List<DirectoryInfo> emptyDirs = new List<DirectoryInfo>();
            Core.FillEmptyDirList(out emptyDirs);
            Core.DeleteAllEmptyDirAndMeta(ref emptyDirs);
        }
    }
}