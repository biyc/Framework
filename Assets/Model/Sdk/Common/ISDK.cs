using System;
using System.Collections.Generic;

namespace Game.Sdk
{
    public interface ISDK
    {

        /// <summary>
        /// 自定义埋点
        /// </summary>
        /// <param name="key">事件名字</param>
        /// <param name="args">事件参数</param>
        void CustomEvent(string key, Dictionary<string, string> args);

        /// <summary>
        /// 开始关卡
        /// </summary>
        /// <param name="levelName"></param>
        void LevelStart(string levelName);

        /// <summary>
        /// 完成关卡
        /// </summary>
        /// <param name="levelName"></param>
        void LevelPass(string levelName);

        /// <summary>
        /// 关卡失败
        /// </summary>
        /// <param name="levelName"></param>
        void LevelFail(string levelName);


        /// <summary>
        /// 基本事件
        /// </summary>
        /// <param name="eventId">友盟后台设定的事件Id</param>
        /// <param name="label">分类标签</param>
         void Event(string eventId, string label = null);




        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="url">连接</param>
        /// <param name="imgPath">图片</param>
        void Share(string title, string content, string url, string imgPath);


        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="levelName"></param>
        void LoginWX(Action successAction, Action<string> failAction);

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <returns></returns>
        string GetHeadUrl();

        /// <summary>
        ///获取昵称
        /// </summary>
        /// <returns></returns>
        string GetNickName();

        /// <summary>
        /// 获取性别
        /// </summary>
        /// <returns>1：代表男 2：代表女 0：代表未知</returns>
        int GetSex();

        /// <summary>
        /// 是否微信登录
        /// </summary>
        /// <returns></returns>
        bool IsWxLogined();

        void Login(Action<string> successAction = null, Action<string> failAction = null);

        /// <summary>
        /// 存档
        /// </summary>
        void WriteStorage(string key, string value, Action successAction, Action<string> failAction);

        /// <summary>
        /// 读档
        /// </summary>
        void ReadStorage(string key, Action<string> successAction, Action<string> failAction);

        /// <summary>
        /// 播放激励广告
        /// </summary>
        /// <param name="endCb">是否播放成功 , 成功后返回的唯一token（通过TOKEN 去服务器领取奖励</param>
        void AdRewardPlay(Action<bool, string> endCb);

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="successCb"></param>
        /// <param name="failCb"></param>
        void Pay(string productId, double price, Action<string> successCb, Action<string> failCb);


        /// <summary>
        /// 调用商店评价
        /// </summary>
        void Evaluate();

        /// <summary>
        /// 清除通知
        /// </summary>
        void NotifyClean();

        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="id">通知编号</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="second">在多少秒后显示推送</param>
        void NotifyAdd(int id,string title,string content,long second);


        /// <summary>
        /// 跳转到游戏渠道
        /// </summary>
        /// <param name="cb">跳转后回调</param>
        void ToChannel(Action<string> cb);


        /// <summary>
        /// 跳转到指定网页或APP
        /// </summary>
        /// <param name="webUrl">网页连接</param>
        /// <param name="appUri">app拉起连接</param>
        /// <param name="cb">跳转后回调</param>
         void ToPage(string webUrl, string appUri, Action<string> cb);


        /// <summary>
        /// 礼包兑换码
        /// </summary>
        void GiftCode(String code, Action<string> successCb, Action<string> failCb);


    }
}