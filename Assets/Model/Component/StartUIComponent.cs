using System;
using Blaze.Manage.Data;

namespace ETModel
{
    [ETModel.ObjectSystem]
    public class StartUIComponentAwakeSystem : AwakeSystem<StartUIComponent>
    {
        public override void Awake(StartUIComponent self)
        {
            self.Awake();
        }
    }

    public class StartUIComponent : Component
    {


        public static StartUIComponent _;

        public void Awake()
        {
            _ = this;
        }
       // public Action OnStartGame;

        public void Start()
        {
           // OnStartGame?.Invoke();
        }
    }
}