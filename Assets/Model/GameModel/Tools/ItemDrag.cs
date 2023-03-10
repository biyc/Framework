using System;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragParameterObject : System.Object
{
    public GameObject gameObject;
    public PointerEventData eventData;

    public DragParameterObject(GameObject gameObject, PointerEventData eventData)
    {
        this.gameObject = gameObject;
        this.eventData = eventData;
    }
}

public class ItemDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform canvasTransform;
    RectTransform selfTranform;

    /**是否可以点击*/
    public bool isClick = true;

    /**是否允许拖拽*/
    public bool allowDrag = true;

    /**物品类型*/
    private bool isDrag = false;

    // 调整大小
    public Resize resize;

    // 删除
    public Button deleteButton;

    /// <summary>
    /// 线框
    /// </summary>
    public Image wireframe;

    // 最小缩放
    public float minSize = 0.5f;

    // 最大缩放
    public float maxSize = 2.0f;

    private int ID;


    private Vector2 offset = Vector2.zero; //用来得到鼠标和图片的差值
    public bool useOffset = true; //使用offset
    public Action<DragParameterObject> actionDragBegin;
    public Action<DragParameterObject> actionDragEnd;
    public Action<DragParameterObject> actionDraging;
    public Action<DragParameterObject> actionPointClick;
    public Action<DragParameterObject> actionPointDown;
    public Action<DragParameterObject> actionPointUp;
    public Func<GameObject, Vector3, Vector3> funcOutOfEdge; //超出边界
    [Tooltip("视差")] public Vector2 parallax = Vector2.zero;

    void Start()
    {
        var canss = transform.GetComponentsInParent<Canvas>().ToList();
        canvasTransform = canss[canss.Count - 1].GetComponent<RectTransform>();
        selfTranform = transform.GetComponent<RectTransform>();
    }

    void Update()
    {
    }

    //开始拖拽
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!allowDrag) return;
        isDrag = true;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, eventData.position,
            eventData.pressEventCamera, out Vector2 localPoint))
        {
            offset = selfTranform.anchoredPosition - localPoint;
        }

        SetDraggedPosition(eventData);
        actionDragBegin?.Invoke(new DragParameterObject(gameObject, eventData));
    }

    //拖拽
    public void OnDrag(PointerEventData eventData)
    {
        if (!allowDrag)
            return;

        SetDraggedPosition(eventData);
        actionDraging?.Invoke(new DragParameterObject(gameObject, eventData));
    }

    //拖拽结束
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!allowDrag)
            return;
        isDrag = false;
        SetDraggedPosition(eventData);
        actionDragEnd?.Invoke(new DragParameterObject(gameObject, eventData));
    }

    private void SetDraggedPosition(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, eventData.position,
            eventData.pressEventCamera, out Vector2 localPoint))
        {
            var targetPosition = useOffset ? localPoint + offset : localPoint;
            targetPosition += parallax;
            if (funcOutOfEdge != null)
            {
                targetPosition = funcOutOfEdge.Invoke(gameObject, targetPosition);
            }

            selfTranform.anchoredPosition = targetPosition;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDrag || !isClick)
        {
            return;
        }

        actionPointClick?.Invoke(new DragParameterObject(gameObject, eventData));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isDrag)
        {
            return;
        }

        actionPointDown?.Invoke(new DragParameterObject(gameObject, eventData));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDrag)
        {
            return;
        }

        actionPointUp?.Invoke(new DragParameterObject(gameObject, eventData));
    }
}