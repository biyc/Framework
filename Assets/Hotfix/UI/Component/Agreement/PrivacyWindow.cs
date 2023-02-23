using Blaze.Manage.Csv.Enum;
using ETModel;
using Hotfix.Game.Common.Logic;
using UnityEngine.UI;

namespace ETHotfix.Agreement
{
    [ETModel.ObjectSystem]
    public class PrivacyWindowAwakeSystem : AwakeSystem<PrivacyWindow>
    {
        public override void Awake(PrivacyWindow self) {
            self.Awake();
        }
    }

    public class PrivacyWindow : WindowComponent
    {
        public static async void Create() {
            var com = await BUI.Switch<PrivacyWindow>(Args);
        }

        public static UIArgs Args = new UIArgs() {
            Name = "IntroduceWindow",
            PrefebPath = "Assets/Projects/UI/Agreement/PrivacyWindow.prefab",
            ComponentType = typeof(PrivacyWindow),
            Layer = UILayerEnum.Top,
        };

        public override void Awake() {
            base.Awake();

            _curStage.transform.Find("bg/closeBtn").GetButton().onClick.AddListener(Close);

            _curStage.transform.Find("bg/Scroll View/Viewport/Content").GetComponent<Text>().text =
                GameConfig.GetStr(DefaultConfigType.Privacy);
        }
    }
}