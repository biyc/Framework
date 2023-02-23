using Blaze.Common;

namespace Blaze.Bundle
{
    /// <summary>
    /// 构建配置信息
    /// </summary>
    public class BundleBuilderConf
    {
        // 主版本号，与母包版本进行匹配
        public int Major = 1;

        // 次版本号，升级时相对上一个资源版本相当于全量更新（_Cache 目录中的AB包开始全量编译）
        public int Minor = 0;

        // 本次构建人员
        public string BuildOperator = "YOULOFT";

        // 渠道
        public string Channel = "Default";

        // 资源发布路径 (Publish/渠道)
        public string PUBLISH_PATH = "Publish";

        // 构建目标平台
        public EnumRuntimeTarget TargetMode = EnumRuntimeTarget.EditorOSX;

        public bool IsPublishStreaming = false;
    }
}