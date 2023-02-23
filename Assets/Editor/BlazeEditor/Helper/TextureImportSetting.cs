using System.IO;
using UnityEditor;

public class TextureImportSetting : AssetPostprocessor
{

    // 图片导入资源时自动压缩
    public void OnPreprocessTexture()
    {
        ProcessTexture(assetImporter);
    }


    [MenuItem("Tools/图片压缩")]
    static void AtlasToAndroid()
    {
        var dirPath = "Assets/Projects";
        string[] files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];
            filePath = filePath.Replace("\\", "/");

            if (filePath.EndsWith(".png") || filePath.EndsWith(".jpg"))
            {
                //筛选出png和jpg图片
                EditorUtility.DisplayProgressBar("处理中>>>", filePath, (float) i / (float) files.Length);

                TextureImporter textureImporter = AssetImporter.GetAtPath(filePath) as TextureImporter;
                if (textureImporter == null)
                    continue;

                ProcessTexture(textureImporter);
            }
        }

        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("成功", "处理完成！", "好的");
    }

    private static void ProcessTexture(AssetImporter assetImporter)
    {
        var path = assetImporter.assetPath;
        // Debug.Log(path);
        if (!path.Contains("Assets/Projects"))
            return;

        TextureImporter import = assetImporter as TextureImporter;
        var noNeed = true;
        var android = import.GetPlatformTextureSettings("Android");
        if (!android.overridden || android.format != TextureImporterFormat.ASTC_5x5)
        {
            import.SetPlatformTextureSettings(AndroidSetting(path));
            noNeed = false;
        }

        var ios = import.GetPlatformTextureSettings("iPhone");
        if (!ios.overridden || ios.format != TextureImporterFormat.ASTC_5x5)
        {
            import.SetPlatformTextureSettings(IosSetting(path));
            noNeed = false;
        }

        if (noNeed)
        {
            return;
        }

        import.textureType = TextureImporterType.Sprite;
        import.mipmapEnabled = false;
        import.spriteImportMode = import.spriteImportMode;
        import.spritePackingTag = "";
        import.spriteBorder = import.spriteBorder;
        import.isReadable = import.isReadable;
        import.SaveAndReimport();
        AssetDatabase.SaveAssets();
    }

    private static TextureImporterPlatformSettings AndroidSetting(string path)
    {
        TextureImporterPlatformSettings importerSettings_Andorid = new TextureImporterPlatformSettings();
        importerSettings_Andorid.overridden = true;
        importerSettings_Andorid.name = "Android";
        // importerSettings_Andorid.textureCompression = TextureImporterCompression.Compressed;
        importerSettings_Andorid.compressionQuality = 100;
        if (importerSettings_Andorid.maxTextureSize > 2048)
        {
            UnityEngine.Debug.LogError(path + "      " + importerSettings_Andorid.maxTextureSize);
            importerSettings_Andorid.maxTextureSize = 2048;
        }

        importerSettings_Andorid.format = TextureImporterFormat.ASTC_5x5;
        return importerSettings_Andorid;
    }

    private static TextureImporterPlatformSettings IosSetting(string path)
    {
        TextureImporterPlatformSettings importerSettings_IOS = new TextureImporterPlatformSettings();
        importerSettings_IOS.overridden = true;
        importerSettings_IOS.name = "iPhone";
        // importerSettings_IOS.textureCompression = TextureImporterCompression.Compressed;
        importerSettings_IOS.compressionQuality = 100;
        if (importerSettings_IOS.maxTextureSize > 2048)
        {
            UnityEngine.Debug.LogError(path + "      " + importerSettings_IOS.maxTextureSize);
            importerSettings_IOS.maxTextureSize = 2048;
        }

        importerSettings_IOS.format = TextureImporterFormat.ASTC_5x5;
        return importerSettings_IOS;
    }
}