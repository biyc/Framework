using Blaze.Common;
using Blaze.Utility;
using ETModel;
using UnityEngine;

public class Starter : MonoBehaviour
{
    public ETModel.Init ILRuntime;
    public ETModel.Init Native;

    void Start()
    {
        var commonSetting = CommonSetting.GetSetting();
        Tuner.Log(commonSetting.GameSetting);

        var game = GameSettings.GetSetting(commonSetting.GameSetting);
        Define.GameSettings = game;
        Define.IsExportProject = game.IsExportProject;

        // 选择使用原生代码或者ILruntime 启动游戏逻辑
        ETModel.Init env = Native;
        if (game.UseILRuntime)
            env = ILRuntime;

        env.UseAB = game.UseAssetBundle;
        env.isDev = game.UseDev;
        // 激活脚本
        env.enabled = true;
        env.transform.gameObject.SetActive(true);
    }
}