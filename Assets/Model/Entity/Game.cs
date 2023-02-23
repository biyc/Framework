namespace ETModel
{
    public static class Game
    {
        private static Scene scene;
        public static Scene Scene => scene ?? (scene = new Scene());

        private static EventSystem eventSystem;

        public static EventSystem EventSystem => eventSystem ?? (eventSystem = new EventSystem());

        private static ObjectPool _objectPool;

        public static ObjectPool ObjectPool => _objectPool ?? (_objectPool = new ObjectPool());

        // public static Hotfix Hotfix = new Hotfix();
        public static Hotfix Hotfix;

        public static void Close()
        {
            scene?.Dispose();
            eventSystem = null;
            scene = null;
            _objectPool = null;
            Hotfix = null;
        }
    }
}