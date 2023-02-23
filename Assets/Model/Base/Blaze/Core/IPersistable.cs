namespace Blaze.Core
{
    /// <summary>
    /// 可持久化的接口,用来过滤泛型的Extension
    /// </summary>
    public interface IPersistable
    {
        string Save();
    }
}