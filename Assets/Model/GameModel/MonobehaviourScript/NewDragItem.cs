/***
 *		Title：          可以被拖拽的物品基类
 *		Description：    
 *
 *		Data：           2019/3/19
 *		Version：        1.0
 *		Modify Recoder：
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //virtual 虚方法
    public virtual void Item_OnBeginDrag(PointerEventData eventData)
    {

    }

    public virtual void Item_OnDrag(PointerEventData eventData)
    {

    }

    public virtual void Item_OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Item_OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Item_OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Item_OnEndDrag(eventData);
    }

}
