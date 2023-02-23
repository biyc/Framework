namespace Blaze.Ci
{
    /// <summary>
    /// 分包类型
    /// </summary>
    public enum EnumPackageType
    {
        // 正式包安卓
        AndroidRelease,

        // 正式包IOS
        IOSRelease,

        // 线上测试包安卓
        AndroidTestOnline,

        // 线上测试包IOS
        IOSTestOnline,

        // 内部测试包安卓
        AndroidTestInner,

        // 内部测试包IOS
        IOSTestInner,

        // 版号包安卓
        AndroidVerify,

        // 版号包IOS
        IOSVerify,

        // 开发包安卓
        AndroidDev,

        // 开发包IOS
        IOSDev,

        // 开发包OSX_EDITOR
        EditorOSXDev,

        // 开发包WIN64_EDITOR
        EditorWin64Dev,
        
    }
}