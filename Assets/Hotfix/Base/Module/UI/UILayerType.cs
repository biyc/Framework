namespace ETModel
{
    public enum UILayerEnum
    {
        // 未初始化的枚举
        Null = 0,

        /// 隐藏层，当调用Close的时候，实际上是把UI物体移到该层中进行隐藏
        Hide = 1,

        /// 底层，一般用来放置最底层的UI
        Bottom = 2,

        /// 中间层，比较常用，大部分界面均是放在此层
        Medium = 3,

        /// 上层，一般是用来放各种弹窗，小窗口之类的
        Top = 4,

        /// 最上层，一般用来做各种遮罩层，屏蔽输入，或者切换动画等
        TopMost = 5,
        
    }
}