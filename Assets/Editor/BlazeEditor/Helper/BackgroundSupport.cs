// //  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
// //  Copyright (c) DONOPO and contributors
//
// //  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
// //  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.
//
// /*
// | VER | AUTHOR | DATE       | DESCRIPTION              |
// |
// | 1.0 | tim    | 2020/02/21 | Initialize core skeleton |
// */
//
// using System.Diagnostics;
// using System.Threading;
// using Blaze.Utility;
// using Blaze.Utility.Helper;
// using UnityEditor;
// using UnityEngine;
// using Debug = UnityEngine.Debug;
//
// namespace Blaze.Helper
// {
//     public class BackgroundSupport
//     {
//         private static bool _isStart = false;
//         private static string _dataPath = "";
//
//         private static bool debug = false;
//
//         private const string CONSOLE_APP_PATH = @"/bin/fswatch.exe";
//
//         private static Thread _thread;
//         private static Process _process;
//         private static bool _isRefresh;
//
//
//         // [InitializeOnLoadMethod]
//         private static void Start()
//         {
//
//             // 没有启动，在独立线程中启动fswatch
//             // if (!_isStart)
//             // {
//                 // _isStart = true;
//                 // _dataPath = Application.dataPath;
//                 // _thread =new Thread(Init);
//                 // _thread.Start();
//                 //
//                 // EditorApplication.update += OnUpdate;
//                 // EditorApplication.quitting += Dispose;
//             // }
//         }
//
//
//         private static void Init()
//         {
//             var winpath = PathHelper.GetCurrentPath();
// #if UNITY_EDITOR_OSX
//             var filename = "/usr/local/bin/fswatch";
// #else
// 			var filename = winpath + "/" + CONSOLE_APP_PATH;
// #endif
//             // var path = Application.dataPath;
// #if UNITY_EDITOR_OSX
//             var arguments = string.Format(@"-x ""{0}""", _dataPath);
// #else
// 			var arguments = string.Format( @"-w ""{0}""", _dataPath );
// #endif
//             var windowStyle = ProcessWindowStyle.Hidden;
//
//             var info = new ProcessStartInfo
//             {
//                 FileName = filename,
//                 UseShellExecute = false,
//                 RedirectStandardOutput = true,
//                 CreateNoWindow = true,
//                 WindowStyle = windowStyle,
//                 Arguments = arguments,
//             };
//
//             _process = Process.Start(info);
//             _process.OutputDataReceived += OnReceived;
//             _process.BeginOutputReadLine();
//
//             if (debug) Debug.Log(Co._("[>>] File Watching Service is ;ON:green:b;"));
//
//         }
//
//
//         private static void Dispose()
//         {
//             if (_process == null) return;
//
//             if (!_process.HasExited)
//             {
//                 _process.Kill();
//             }
//
//             _process.Dispose();
//             _process = null;
//
//             // if (debug) Debug.Log("[>>] Stop Watching");
//             if (debug) Debug.Log(Co._("[>>] File Watching Service is ;OFF:lightred:b;"));
//         }
//
//         private static void OnUpdate()
//         {
//             if (!_isRefresh) return;
//             if (EditorApplication.isCompiling) return;
//             if (EditorApplication.isUpdating) return;
//
//             if (debug) Debug.Log("[>>] Start Compiling");
//
//             Dispose();
//             _isRefresh = false;
//             Thread.Sleep(500);
//
//             AssetDatabase.Refresh();
//         }
//
//         private static void OnReceived(object sender, DataReceivedEventArgs e)
//         {
//             var message = e.Data;
//             if (null == message) return;
//
// #if UNITY_EDITOR_OSX
//             if (message.Contains("Created") || message.Contains("Renamed") || message.Contains("Updated") ||
//                 message.Contains("Removed"))
//             {
//                 _isRefresh = true;
//             }
// #else
// 			if ( message.Contains( "MODIFY" ) || message.Contains( "CREATE" ) )
// 			{
// 				_isRefresh = true;
// 			}
// #endif
//         }
//     }
// }