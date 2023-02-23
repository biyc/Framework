namespace Blaze.Core
{
    // 单例模板
    public class Singeton<T> where T : class, new()
    {
        private static readonly object locker = new object();
        private static T instance;

        public static T _
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                        }
                    }
                }

                return instance;
            }
        }
    }
}