using System;

namespace Blaze.Ci
{
    /// <summary>
    /// 分包类型
    /// </summary>
    public enum EnumPackageType
    {
        // 开发包安卓
        AndroidDev,

        // 内部测试包安卓
        AndroidTestInner,

        //内部测试安卓导出包
        AndroidExportTestInner,

        // 正式包安卓
        AndroidExportRelease,

        // 开发包IOS
        IOSDev,

        // 内部测试包IOS
        IOSTestInner,

        //内部测试安卓导出包
        IOSExportTestInner,

        // 正式包IOS
        IOSExportRelease,

        // 开发包OSX_EDITOR
        EditorOSXDev,

        // 开发包WIN64_EDITOR
        EditorWin64Dev,
    }
}