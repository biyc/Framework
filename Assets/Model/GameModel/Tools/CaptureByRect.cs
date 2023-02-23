using System;
using Main;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CaptureByRect : MonoBehaviour
{
    private float ScaleFactor {
        get {
            CanvasScaler canvasScaler = transform.GetComponentInParent<CanvasScaler>();
            float curr = 1;
            if (canvasScaler.matchWidthOrHeight == 0) //宽度适配
            {
                curr = Screen.width / 1242f;
            } else {
                curr = Screen.height / 2688f;
            }

            return curr;
        }
    }

    private RectTransform RectTrans {
        get { return GetComponent<RectTransform>(); }
    }

    private Rect TargetRect() {
        return TargetRect(RectTrans);
    }

    /// <summary>
    /// 通过rectTransform得到一个Rect
    /// </summary>
    private Rect TargetRect(RectTransform rectTransform) {
        float factor = ScaleFactor;
        //factor = ((int)(factor * 1000)) / 1000f;
        float adjustedWidth = Screen.width / factor;
        float clipWidth = adjustedWidth - 1242f;
        float adjustedHeight = adjustedWidth * Screen.height / Screen.width;
        float clipHeight = 2688.0f - adjustedHeight;
        Debug.Log("当前屏幕有效宽度：" + adjustedWidth);
        Debug.Log("当前屏幕有效高度：" + adjustedHeight);
        Vector2 pos = factor * (rectTransform.rect.min + new Vector2(adjustedWidth / 2, (adjustedHeight / 2)) +
                                rectTransform.anchoredPosition);
        Vector2 size = factor * (new Vector2(adjustedWidth, adjustedHeight) -
                                 new Vector2(Mathf.Abs(rectTransform.sizeDelta.x) /*- clipWidth*/,
                                     Mathf.Abs(rectTransform.sizeDelta.y) /*- clipHeight*/));
        return new Rect(pos, size);
    }

    public void CaptureScreen(Action<Texture2D> action) {
        StartCoroutine(CaptureScreenReal(action));
    }

    private System.Collections.IEnumerator CaptureScreenReal(Action<Texture2D> action) {
        yield return new WaitForEndOfFrame();
        var rect = TargetRect();
        if (CheckOutOfScreen(rect)) {
            rect.width = Screen.width - rect.position.x;
            rect.height = Screen.height - rect.position.y;
        }

        //先创建一个的空纹理，大小可根据实现需要来设置
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        //读取屏幕像素信息并存储为纹理数据
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        action?.Invoke(screenShot);
    }

    private bool CheckOutOfScreen(Rect rect) {
        bool outofScreen = false;
        var xMin = ((int)(rect.xMin * 1000)) / 1000f;
        var yMin = ((int)(rect.yMin * 1000)) / 1000f;
        var xMax = ((int)(rect.xMax * 1000)) / 1000f;
        var yMax = ((int)(rect.yMax * 1000)) / 1000f;
        if (xMin < 0 || yMin < 0 || xMax > Screen.width || yMax > Screen.height) {
            outofScreen = true;
            Debug.Log($"截屏对象:{gameObject.name}的矩形超出屏幕");
        }

        return outofScreen;
    }
}