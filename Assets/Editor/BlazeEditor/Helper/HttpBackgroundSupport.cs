//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/21 | Initialize core skeleton |
*/

using System.Diagnostics;
using Blaze.Utility;
using Blaze.Utility.Helper;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Blaze.Helper
{
    public class HttpBackgroundSupport
    {
        private static bool debug = true;


        private static Process _processData;
        private static Process _processAB;
        private static bool _isRefresh;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            var winpath = PathHelper.GetCurrentPath();
            // if (debug) Debug.Log("[>>] " + winpath);
#if UNITY_EDITOR_OSX
            var filename = winpath + "/bin/abhttpd";
            var datafilename = winpath + "/bin/datahttpd";
#else
			var filename = winpath + @"\bin\abhttpd.exe";
			var datafilename = winpath + @"\bin\datahttpd.exe";
#endif

            var windowStyle = ProcessWindowStyle.Hidden;

            var dataInfo = new ProcessStartInfo
            {
                FileName = datafilename,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WindowStyle = windowStyle,
#if UNITY_EDITOR_OSX
                Arguments = string.Format(@" -p 8087 -w {0}/Data -n datahttpd", winpath),
#else
                Arguments = string.Format(@" -p 8087 -w {0}\Data -n datahttpd", winpath),
#endif
            }; 

            var abInfo = new ProcessStartInfo
            {
                FileName = filename,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WindowStyle = windowStyle,
#if UNITY_EDITOR_OSX
                Arguments = string.Format(@" -p 8088 -w {0}/Publish -n abhttpd", winpath),
#else
                Arguments = string.Format(@" -p 8088 -w {0}\Publish -n abhttpd", winpath),
#endif
            };


            if (ThorSettings._.IsUseAbHttp)
            {
                _processAB = Process.Start(abInfo);
                _processAB.BeginOutputReadLine();
            }

            if (ThorSettings._.IsUseDataHttp)
            {
                _processData = Process.Start(dataInfo);
                _processData.BeginOutputReadLine();
            }

            EditorApplication.quitting += OnQuit;
        }

        private static void OnQuit()
        {
            DisposeAB();
            DisposeData();
        }

        public static void Restart()
        {
            OnQuit();
            Init();
        }

        private static void DisposeAB()
        {
            if (_processAB == null) return;

            if (!_processAB.HasExited)
            {
                _processAB.Kill();
            }

            _processAB.Dispose();
            _processAB = null;
        }

        private static void DisposeData()
        {
            if (_processData == null) return;

            if (!_processData.HasExited)
            {
                _processData.Kill();
            }

            _processData.Dispose();
            _processData = null;
        }
    }
}