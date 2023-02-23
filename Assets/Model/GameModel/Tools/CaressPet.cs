using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 抚摸宠物(一次完整的按下抬起，且按下时在宠物图标上)
/// </summary>
public class CaressPet : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerExitHandler
{
    private GameObject okObj;
    private Action successCallback;
    private bool completed = true;

    public void RegisterCallback(GameObject okObj, Action successCallback)
    {
        this.okObj = okObj;
        this.successCallback = successCallback;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.selectedObject == okObj)
        {
            completed = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        TryComplete();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.LogError($"离开了:{eventData.selectedObject.name}");
        if (eventData.selectedObject == okObj)
        {
            TryComplete();
        }
    }

    private void TryComplete()
    {
        if (completed)
        {
            return;
        }
        //Debug.LogError("成功");
        this.successCallback?.Invoke();
        completed = true;
    }
}
