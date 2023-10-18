
namespace Hotfix.Game.Common.Data
{
    /// <summary>
    /// 背包资源
    /// </summary>
    public class ResPack
    {
        /// 资源类型ID
        public int ResTypeId;

        /// 资源数量
         public long ResNum;
        
        /// 资源是否执行过初始化
        public bool IsInit;

        /// 定期恢复型数值，最后恢复时间
        public long ResetTime;

    }
}