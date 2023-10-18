namespace Blaze.Ci
{
    /// <summary>
    /// 分包类型
    /// </summary>
    public enum EnumPackageType
    {
        // 正式包安卓
        AndroidRelease,
        
        AndroidDev,

        // 正式包IOS
        IOSRelease,

        // 开发包OSX_EDITOR
        EditorOSXDev,

        // 开发包WIN64_EDITOR
        EditorWin64Dev,
    }
}