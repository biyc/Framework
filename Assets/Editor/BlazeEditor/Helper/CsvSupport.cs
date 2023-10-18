//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/21 | Initialize core skeleton |
*/

using Blaze.Utility;
using Blaze.Utility.Helper;
using System.Diagnostics;
using System.Threading;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Blaze.Helper
{
    /// <summary>
    /// Csv自动解析器
    /// </summary>
    public class CsvSupport
    {
        private static bool debug = false;

        // 文件观察进程
        private static Process _process;

        // 转换进程
        private static Process _csvProcess;
        private static bool _isXlsxAuto;
        private static string rootPath = "";
        private static string dataPath = "";
        private static string xlsxDataPath = "";
        private static string csvDataPath = "";


        // private static bool running = false;

        private static void InitPath()
        {
            rootPath = Application.dataPath.Replace("Assets", "");
            dataPath = rootPath + "Data" + "/";
            xlsxDataPath = dataPath + "XlsxData";
            csvDataPath = dataPath + "CsvData";
        }
        
        /// <summary>
        /// 启动 fswatch 观察CSV文件变化
        /// </summary>
        private static void Init()
        {
            var winpath = PathHelper.GetCurrentPath();
#if UNITY_EDITOR_OSX
            var filename = "/usr/local/bin/fswatch";
#else
			var filename = winpath + @"\bin\fswatch.exe";
#endif

            var path = xlsxDataPath + "/";
#if UNITY_EDITOR_OSX
            var arguments = string.Format(@"-x ""{0}""", path);
#else
			var arguments = string.Format( @"-p ""{0}"" -w 0", path );
#endif

            var windowStyle = ProcessWindowStyle.Hidden;
            if (debug) Debug.Log(arguments);


            var info = new ProcessStartInfo
            {
                FileName = filename,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WindowStyle = windowStyle,
                Arguments = arguments,
            };


            Dispose();
            _process = Process.Start(info);
            _process.OutputDataReceived += OnReceived;
            _process.BeginOutputReadLine();

            if (debug) Debug.Log("[>>] Start Xlsx2csv");
        }


        private static void Dispose()
        {
            if (_process != null)
            {
                if (!_process.HasExited)
                {
                    _process.Kill();
                }

                _process.Dispose();
                _process = null;
            }

            if (_csvProcess != null)
            {
                if (!_csvProcess.HasExited)
                {
                    _csvProcess.Kill();
                }

                _csvProcess.Dispose();
                _csvProcess = null;
            }

            if (debug) Debug.Log("[>>] Stop Xlsx2csv");
        }


        private static void OnReceived(object sender, DataReceivedEventArgs e)
        {
            var message = e.Data;
            if (null == message) return;
            // 直接调用状态值时候会为空，
            if (_isXlsxAuto)
            {
                if (message.Contains("Created") || message.Contains("Renamed") || message.Contains("Updated") ||
                    message.Contains("Removed") || message.Contains("OnChanged") || message.Contains("OnRenamed"))
                {
                    XlsxToCsvExe();
                }
            }
        }

        public static void XlsxToCsv()
        {
            // 在全新的线程中执行，不要在主线程中执行
            InitPath();
            new Thread(XlsxToCsvExe).Start();
        }
        private static void XlsxToCsvExe()
        {
            var winpath = PathHelper.GetCurrentPath();
            if (debug) Debug.Log("[>>] " + winpath);
#if UNITY_EDITOR_OSX
            var filename = rootPath + "bin/xlsx2csv";
#else
			var filename = winpath + @"\bin\xlsx2csv.exe";
#endif
            Debug.Log(string.Format(@"bin/xlsx2csv -s {0} -t {1}", xlsxDataPath, csvDataPath));

            var xlsx2cvsInfo = new ProcessStartInfo
            {
                FileName = filename,
                UseShellExecute = false,
                RedirectStandardOutput = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = string.Format(@" -s {0} -t {1}", xlsxDataPath, csvDataPath)
            };

            _csvProcess = Process.Start(xlsx2cvsInfo);
            _csvProcess.OutputDataReceived += OnReceivedCsvPress;
//            _csvProcess.BeginOutputReadLine();

            Debug.Log("XlsxToCsv is a success");
        }

        private static void OnReceivedCsvPress(object sender, DataReceivedEventArgs e)
        {
            var message = e.Data;
            if (debug) Debug.Log(message);
        }
    }
}