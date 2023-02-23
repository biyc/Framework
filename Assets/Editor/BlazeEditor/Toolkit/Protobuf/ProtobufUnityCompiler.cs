using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System.Linq;
using Blaze.Utility;

// 生成到proto文件所在的目录中
namespace Plugin.Protobuf
{
    internal class ProtobufUnityCompiler
    {
        /// <summary>
        /// Path to the file of all protobuf files in your Unity folder.
        /// </summary>
        static string[] AllProtoFiles
        {
            get
            {
                string[] protoFiles = Directory.GetFiles(Application.dataPath, "*.proto", SearchOption.AllDirectories);
                return protoFiles;
            }
        }

        internal static void Gen()
        {
            foreach (string s in AllProtoFiles)
            {
                CompileProtobufSystemPath(s);
            }

            AssetDatabase.Refresh();
        }


        internal static void Copy()
        {
            foreach (string s in AllProtoFiles)
            {
            }
        }

        public static void Clean()
        {
            Tuner.Log(Co._("     -= Clean Protobuf Code Done =-:green:b;"));
        }


        private static bool CompileProtobufSystemPath(string protoFileSystemPath)
        {
            if (Path.GetExtension(protoFileSystemPath) != ".proto")
            {
                return false;
            }


            string protoPath = Path.GetDirectoryName(protoFileSystemPath);

            string options = "";
            options += string.Format("\"{0}\"", protoFileSystemPath);
            options += string.Format(" --csharp_out \"{0}\" ", protoPath);
            options += string.Format(" --proto_path \"{0}\" ", protoPath);


            ProcessStartInfo startInfo = new ProcessStartInfo()
                {FileName = "bin/protoc", Arguments = options};

            Process proc = new Process() {StartInfo = startInfo};
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();

            string output = proc.StandardOutput.ReadToEnd();
            string error = proc.StandardError.ReadToEnd();
            proc.WaitForExit();

            if (output != "")
            {
                Tuner.Log("Protobuf Unity : " + output);
            }

            if (error != "")
            {
                Tuner.Error("Protobuf Unity : " + error);
            }

            return true;

        }
    }
}