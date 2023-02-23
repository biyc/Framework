using Blaze.Utility.Helper;

namespace Blaze.Manage.Safety
{
    /// <summary>
    /// 安全认证数据
    /// </summary>
    public class SafetyStartData
    {
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 当前是否开放游戏
        /// </summary>
        public bool IsOpen;

        /// <summary>
        /// 最后能登录游戏的时间
        /// </summary>
        public long DeadlineTime;

        public string OutConfig()
        {
            var data = LitJson.JsonMapper.ToJson(this);
            return CryptoHelper.XxteaEncryptToString(data);
        }

        public static SafetyStartData LoadFromData(string data)
        {
            var str = CryptoHelper.XxteaDecryptByString(data);
            return LitJson.JsonMapper.ToObject<SafetyStartData>(str);
        }
    }
}