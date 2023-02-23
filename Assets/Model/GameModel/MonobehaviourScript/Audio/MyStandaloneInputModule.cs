using System;
using System.Linq;
using Blaze.Utility;
using ETModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = UnityEngine.Component;

public class MyStandaloneInputModule : StandaloneInputModule
{
    public override void Process()
    {
        base.Process();
        if (Input.GetMouseButtonDown(0))
        {
            GameObject obj = m_PointerData.First().Value.pointerPress;
            if (obj != null)
            {
                PlayButtonAudio(obj);
            }
        }
    }

    private bool PlayButtonAudio(GameObject obj)
    {
        // 自定义音频参数组件，等级最高
        var boa = obj.GetComponent<AudioParam>();
        if (boa != null)
        {
            string aName = obj.GetComponent<AudioParam>().audioName;
            if (!string.IsNullOrEmpty(aName))
            {
                AudioComponent._?.ButtonOverAudio?.Invoke(aName);
            }
            return true;
        }

        var btn = obj.GetComponent<Button>();
        if (btn != null)
        {
            AudioComponent._?.Click?.Invoke(btn);
            return true;
        }
        return false;
    }
}