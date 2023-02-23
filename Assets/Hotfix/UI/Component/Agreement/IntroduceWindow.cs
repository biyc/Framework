using Blaze.Manage.Csv.Enum;
using ETModel;
using Hotfix.Game.Common.Logic;
using UnityEngine.UI;

namespace ETHotfix.Agreement
{
    [ETModel.ObjectSystem]
    public class IntroduceWindowAwakeSystem : AwakeSystem<IntroduceWindow>
    {
        public override void Awake(IntroduceWindow self) {
            self.Awake();
        }
    }

    public class IntroduceWindow : WindowComponent
    {
        public static async void Create() {
            var com = await BUI.Switch<IntroduceWindow>(Args);
        }

        public static UIArgs Args = new UIArgs() {
            Name = "IntroduceWindow",
            PrefebPath = "Assets/Projects/UI/Agreement/IntroduceWindow.prefab",
            ComponentType = typeof(IntroduceWindow),
            Layer = UILayerEnum.Top,
        };

        public override void Awake() {
            base.Awake();

            _curStage.transform.Find("bg/closeBtn").GetButton().onClick.AddListener(Close);

          //  _curStage.transform.Find("bg/txt_a2_4").GetComponent<Text>().text =
             //   GameConfig.GetStr(DefaultConfigType.Introduce);
        }
    }
}