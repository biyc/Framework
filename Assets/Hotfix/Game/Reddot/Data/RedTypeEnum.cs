namespace Blaze.Manage.Csv.Enum
{
    /// <summary>
    /// 红点模式
    /// </summary>
    public enum RedMode
    {
        /// <summary>
        /// 独立，不受子节点与父节点影响
        /// </summary>
        Single = 1,

        /// <summary>
        /// 子节点全部消除时，当前节点才消除
        /// </summary>
        Childs = 2,

        /// <summary>
        /// 当前节点消除时，子节点都消除
        /// </summary>
        Base = 3,
    }

    public enum RedType
    {
        // 红点的根
        Root = 0,

        //章节
        Chapter = 1,

        //签到
        Sign = 2,

        //邮箱
        Mail = 3,

        // 手机
        Phone = 4,

        // 任务
        Mission = 5,


        //章节
        ChapterSub = 100,

        //签到
        SignSub = 200,

        //邮箱
        MailSub = 300,

        // 手机
        // PhoneSub = 400,

        // 手机-信息
        PhoneMessages = 410,

        // 手机-发现
        PhoneDiscover = 420,

        // 手机-发现-朋友圈
        PhoneDiscoverMoment = 421,

        // 手机-通讯录
        PhoneDressBook = 430,

        // 任务
        MissionSub = 500,
    }
}