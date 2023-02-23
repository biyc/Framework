using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AxisZRotateWithMouse : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("实际旋转的物体可以不必是拖拽的物体")]
    public Transform rotateTransform;//区分 Drag的物体和实际转动的物体
    private Vector3 cache;
    private Vector3 temp;
    public float value;
    public float target;
    [Header("旋转灵敏度参数")]
    public float sensitivity = 200.0f;
    private Action callbackComplete;
    private Action<float> callbackUpdate;
    private Action callbackDragEnd;
    private Action callbackDragBegin;
    private bool flag = false;
    private float z;

    private void Start()
    {
        z = rotateTransform.eulerAngles.z;
        sensitivity = 17.0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (flag)
        {
            return;
        }
        temp = new Vector3(eventData.position.x, eventData.position.y, 0) - rotateTransform.position;
        value += Vector3.SignedAngle(cache, temp, -Vector3.Cross(cache, temp)) * sensitivity;
        if (Mathf.Abs(value) >= Mathf.Abs(target))
        {
            value = target;
            callbackComplete?.Invoke();
            callbackComplete = null;
            callbackUpdate = null;
            flag = true;
        }
        rotateTransform.eulerAngles = new Vector3(0, 0, z + value);
        cache = temp;
        callbackUpdate?.Invoke(value / target);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        cache = new Vector3(eventData.position.x, eventData.position.y, 0) - rotateTransform.position;
        callbackDragBegin?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        callbackDragEnd?.Invoke();
    }

    public void RegisterData(float target, Action callbackComplete, Action<float> callbackUpdate, Action callbackDragEnd, Action callbackDragBegin)
    {
        this.target = target;
        this.callbackComplete = callbackComplete;
        this.callbackUpdate = callbackUpdate;
        this.callbackDragEnd = callbackDragEnd;
        this.callbackDragBegin = callbackDragBegin;
    }


}