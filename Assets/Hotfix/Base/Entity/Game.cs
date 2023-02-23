namespace ETHotfix
{
    /// <summary>
    /// 游戏运行时环境
    /// </summary>
    public static class Game
    {
        private static Scene _scene;

        /// <summary>
        /// Component 上下文存放处
        /// </summary>
        public static Scene Scene => _scene ?? (_scene = new Scene());

        private static EventSystem _eventSystem;

        public static EventSystem EventSystem => _eventSystem ?? (_eventSystem = new EventSystem());

        private static ObjectPool _objectPool;

        public static ObjectPool ObjectPool => _objectPool ?? (_objectPool = new ObjectPool());

        public static void Close()
        {
            _scene.Dispose();
            _scene = null;
            // _eventSystem = null;
            // _objectPool = null;
        }
    }
}