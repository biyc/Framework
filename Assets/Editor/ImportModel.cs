using System.IO;
using System.Linq;
using Codice.Client.Common;
using DG.Tweening.Plugins.Options;
using Editor.BuildEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using PathHelper = Blaze.Utility.Helper.PathHelper;

namespace Company
{
    public class ImportModel : OdinEditorWindow
    {
        [ShowInInspector] public static string ImportPath;

        private const string AssetModelPath = "Assets/Projects/3d/Models/";

        private static ImportModel _window;

        [MenuItem("Tools/ImportModel")]
        private static void Import()
        {
            _window = GetWindow<ImportModel>();
            _window.Show();
        }

        [Button]
        private void StartImport()
        {
            _window.Close();
            if (string.IsNullOrEmpty(ImportPath) || !Directory.Exists(ImportPath)) return;
            Directory.CreateDirectory(ImportPath).GetDirectories().ToList().ForEach(m =>
            {
                var localPath = PathHelper.Combine(AssetModelPath, m.Name);
                if (Directory.Exists(localPath))
                    Directory.Delete(localPath, true);
                CopyFolder(m.FullName, localPath);
            });
        }

        /// <summary>
        /// 复制文件夹(递归)
        /// </summary>
        /// <param name="varFromDirectory">源文件夹路径</param>
        /// <param name="varToDirectory">目标文件夹路径</param>
        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyFolder(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }

            string[] files = Directory.GetFiles(varFromDirectory);
            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                }
            }
        }
    }
}