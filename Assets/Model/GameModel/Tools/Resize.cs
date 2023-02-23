using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// unity实现窗口的放大缩小
/// </summary>
public class Resize : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    // 控制的对象
    public RectTransform content;
    // 最小缩放
    public float minSize = 0.5f;
    // 最大缩放
    public float maxSize = 2.0f;

    private Vector2 initSizeDelta;
    private float initSizeScale;
    private Vector2 originalLocalPointerPosition;
    private Vector2 originalSizeDelta;

    private void Awake()
    {
        if (content != null)
        {
            initSizeDelta = content.sizeDelta;
            initSizeScale = content.sizeDelta.y / content.sizeDelta.x;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    public void SetData(RectTransform inContent, float sizeMin, float sizeMax)
    {
        content = inContent;
        minSize = sizeMin;
        maxSize = sizeMax;
        initSizeDelta = content.sizeDelta;
        initSizeScale = content.sizeDelta.y / content.sizeDelta.x;
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (content == null)
            return;

        originalSizeDelta = content.sizeDelta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, data.position, data.pressEventCamera, out originalLocalPointerPosition);
    }

    public void OnDrag(PointerEventData data)
    {
        if (content == null)
            return;

        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, data.position, data.pressEventCamera, out localPointerPosition);
        Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;

        float v = offsetToOriginal.x > offsetToOriginal.y ? offsetToOriginal.x : offsetToOriginal.y;

        offsetToOriginal = new Vector3(v, v * initSizeScale);

        Vector2 sizeDelta = originalSizeDelta + new Vector2(offsetToOriginal.x, offsetToOriginal.y);
        if(maxSize > 0.0f)
        {
            sizeDelta = new Vector2(
                Mathf.Clamp(sizeDelta.x, initSizeDelta.x * minSize, initSizeDelta.x * maxSize),
                Mathf.Clamp(sizeDelta.y, initSizeDelta.y * minSize, initSizeDelta.y * maxSize)
            );
        }

        content.sizeDelta = sizeDelta;
    }
}