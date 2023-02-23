using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerController : MonoBehaviour
{
    private Canvas Canvas_main;

    // Use this for initialization
    void Awake()
    {
        Canvas_main = GetComponent<Canvas>();
        if (Canvas_main == null)
        {
            Debug.Log("canvas scaler error");
            return;
        }

        GetCur_scaleFactor();
    }

    /// <summary>
    /// 根据标准分辨率比率获取当前的需要使用的比例因子
    /// </summary>
    private void GetCur_scaleFactor()
    {
        var nowScreenRate = (float) Screen.height / (float) Screen.width;
        var scale = GetComponent<CanvasScaler>();

        if (nowScreenRate < 1.775)
        {
            // PAD 模式
            scale.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scale.matchWidthOrHeight = 0.5f;
        }
        else
            // 手机模式
            scale.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;

        Debug.Log("Canvas_main.scaleFactor = " + Canvas_main.scaleFactor);
    }
}