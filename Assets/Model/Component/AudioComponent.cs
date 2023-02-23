using System;
using System.Collections.Generic;
using UnityEngine.UI;


namespace ETModel
{
    [ETModel.ObjectSystem]
    public class AudioComponentAwakeSystem : AwakeSystem<AudioComponent>
    {
        public override void Awake(AudioComponent self)
        {
            self.Awake();
        }
    }

    public class AudioComponent : Component
    {
        public static AudioComponent _;

        public void Awake()
        {
            _ = this;
        }

        // 按钮点击事件（按钮名称）
        // public Action<string> Click;
        public Action<Button> Click;

        // 按钮点击事件（按钮上挂的 ButtonOverAudio 脚本中的参数）
        public Action<string> ButtonOverAudio;

        // 预留发出备用事件
        public Action<string> Other;
    }
}