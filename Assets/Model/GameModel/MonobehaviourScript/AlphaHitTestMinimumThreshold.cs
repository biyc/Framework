using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 移除掉图片的区域alpha 小于0.1的事件，
/// </summary>
public class AlphaHitTestMinimumThreshold : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
    
}
