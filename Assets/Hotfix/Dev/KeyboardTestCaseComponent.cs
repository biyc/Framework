

using Blaze.Manage.Csv.Enum;
using Blaze.Resource;
using Blaze.Resource.AssetBundles;
using Hotfix.Game.Reddot.Data;
using IngameDebugConsole;
using UnityEngine;

namespace ETHotfix
{
    [ETModel.ObjectSystem]
    public class KeyboardTestCaseComponentAwakeSystem : AwakeSystem<KeyboardTestCaseComponent>
    {
        public override void Awake(KeyboardTestCaseComponent self)
        {
            self.Awake();
        }
    }

    [ETModel.ObjectSystem]
    public class KeyboardTestCaseComponentUpdateSystem : UpdateSystem<KeyboardTestCaseComponent>
    {
        public override void Update(KeyboardTestCaseComponent self)
        {
#if UNITY_EDITOR
            self.KeyControl();
#endif
        }
    }

    public class KeyboardTestCaseComponent : Component
    {
        public async void Awake()
        {
            Init();

            SetCommand();
        }

        private void Init()
        {
            // new GameObject().AddComponent<Ram_Display>();
        }

        private void SetCommand()
        {
            DebugLogConsole.AddCommand("ram", "内存使用情况", delegate() { new GameObject().AddComponent<Ram_Display>(); });

            DebugLogConsole.AddCommand("gc", "回收内存", delegate()
            {
                if (ResManager._.GetAssetProvider() is AssetProviderBundle)
                {
                    (ResManager._.GetAssetProvider() as AssetProviderBundle)?.DevCleanAsset();
                }
            });
            DebugLogConsole.AddCommand("res", "asset 缓存状况", delegate()
            {
                if (ResManager._.GetAssetProvider() is AssetProviderBundle)
                {
                    (ResManager._.GetAssetProvider() as AssetProviderBundle)?.DevAssetShow();
                }
            });
        }



#if UNITY_EDITOR
        // ChatChannelData data =new ChatChannelData();

        bool test = false;

        private bool status = true;
        private RedData red = new RedData(RedType.ChapterSub, RedMode.Single);
        public async void KeyControl()
        {




        }
#endif
    }
}