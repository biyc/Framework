using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class EraseMask : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    /// <summary>
    /// 是否擦除了
    /// </summary>
    public bool isStartEraser;

    /// <summary>
    /// 是否擦除结束了
    /// </summary>
    public bool isEndEraser;

    /// <summary>
    /// 开始事件
    /// </summary>
    public Action eraserStartEvent;

    /// <summary>
    /// 结束事件
    /// </summary>
    public Action eraserEndEvent;

    public RawImage uiTex;
    Texture2D tex;
    /// <summary>
    /// 监听事件
    /// </summary>
    /// <param name="eraseComplete">完成事件</param>
    /// <param name="eraseStart">开始事件</param>
    public void RegisterNotify(Action eraseComplete, Action eraseStart = null)
    {
        if (this.gameObject.TryGetComponent<EraseMaskThread>(out var newVersionCSharp))
        {
            eraseMaskThread = newVersionCSharp;
            this.gameObject.GetComponent<EraseMask>().enabled = false;
            newVersionCSharp.enabled = true;
        } else {
            eraseMaskThread = this.gameObject.AddComponent<EraseMaskThread>();
            this.gameObject.GetComponent<EraseMask>().enabled = false;
        }
        
        this.eraserEndEvent = eraseComplete;
        this.eraserStartEvent = eraseStart;
        if (eraseMaskThread != null)
        {
            eraseMaskThread.eraserEndEvent = eraseComplete;
            eraseMaskThread.eraserStartEvent = eraseStart;
        }
    }

    /// <summary>
    /// 是否在自动滑动
    /// </summary>
    public bool IsAuto
    {
        set
        {
            _ISAuto = value;
            if (eraseMaskThread != null)
            {
                eraseMaskThread.IsAuto = value;
            }
        }
        get
        {
            return _ISAuto;
        }
    }
    public bool _ISAuto = false;

    bool FirstAuto = true;
    /// <summary>
    /// 自动擦除
    /// </summary>
    /// <param name="v">擦除坐标</param>
    public void AutoErase(Vector2 pos)
    {
        if(!IsAuto)
        {
            return;
        }
        if (FirstAuto)
        {
            penultPos = pos;
            CheckPoint(pos);
            FirstAuto = false;
            return;
        }
        if (twoPoints && Vector2.Distance(pos, lastPos) > distance)//如果两次记录的鼠标坐标距离大于一定的距离，开始记录鼠标的点
        {
            float dis = Vector2.Distance(lastPos, pos);
            CheckPoint(pos);
            int segments = (int)(dis / radius);//计算出平滑的段数                                              
            segments = segments < 1 ? 1 : segments;
            if (segments >= 10) { segments = 10; }
            pointsOnDrag = Beizier(penultPos, lastPos, pos, segments);//进行贝塞尔平滑
            for (int i = 0; i < pointsOnDrag.Length; i++)
            {
                CheckPoint(pointsOnDrag[i]);
            }
            lastPos = pos;
            if (pointsOnDrag.Length > 2)
                penultPos = pointsOnDrag[pointsOnDrag.Length - 2];
        }
        else
        {
            twoPoints = true;
            lastPos = pos;
        }
    }


    Texture2D MyTex;
    int mWidth;
    int mHeight;
    /// <summary>
    /// 刷子尺寸
    /// </summary>
    [Header("Brush Size")]
    public int brushSize = 50;
    /// <summary>
    /// 完成的擦除比列
    /// </summary>
    [Header("Rate")]
    public int rate = 90;

    float maxColorA;
    float colorA;
    EraseMaskThread eraseMaskThread = null;
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }
    private void Awake()
    {
        //Initial();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initial()
    {
        tex = (Texture2D)uiTex.mainTexture;
        MyTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        mWidth = MyTex.width;
        mHeight = MyTex.height;

        MyTex.SetPixels(tex.GetPixels());
        MyTex.Apply();
        uiTex.texture = MyTex;
        maxColorA = MyTex.GetPixels().Length;
        colorA = 0;
        isEndEraser = false;
        isStartEraser = false;
    }

    //private void OnDisable()
    //{
    //    MyTex = null;
    //    tex = null;
    //}

    /// <summary>
    /// 贝塞尔平滑
    /// </summary>
    /// <param name="start">起点</param>
    /// <param name="mid">中点</param>
    /// <param name="end">终点</param>
    /// <param name="segments">段数</param>
    /// <returns></returns>
    public Vector2[] Beizier(Vector2 start, Vector2 mid, Vector2 end, int segments)
    {
        float d = 1f / segments;
        Vector2[] points = new Vector2[segments - 1];
        for (int i = 0; i < points.Length; i++)
        {
            float t = d * (i + 1);
            points[i] = (1 - t) * (1 - t) * mid + 2 * t * (1 - t) * start + t * t * end;
        }
        List<Vector2> rps = new List<Vector2>();
        rps.Add(mid);
        rps.AddRange(points);
        rps.Add(end);
        return rps.ToArray();
    }



    bool startDraw = false;
    bool twoPoints = false;
    Vector2 lastPos;//最后一个点
    Vector2 penultPos;//倒数第二个点
    float radius = 12f;
    float distance = 1f;



    #region 事件
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isEndEraser) { return; }
        IsAuto = false;
        startDraw = true;
        twoPoints = false;
        penultPos = eventData.position;
        CheckPoint(penultPos);
    }

    Vector2[] pointsOnDrag;
    public void OnDrag(PointerEventData eventData)
    {
        if (isEndEraser) { return; }
        IsAuto = false;
        if (twoPoints && Vector2.Distance(eventData.position, lastPos) > distance)//如果两次记录的鼠标坐标距离大于一定的距离，开始记录鼠标的点
        {
            Vector2 pos = eventData.position;
            float dis = Vector2.Distance(lastPos, pos);
            CheckPoint(eventData.position);
            int segments = (int)(dis / radius);//计算出平滑的段数                                              
            segments = segments < 1 ? 1 : segments;
            if (segments >= 10) { segments = 10; }
            pointsOnDrag = Beizier(penultPos, lastPos, pos, segments);//进行贝塞尔平滑
            for (int i = 0; i < pointsOnDrag.Length; i++)
            {
                CheckPoint(pointsOnDrag[i]);
            }
            lastPos = pos;
            if (pointsOnDrag.Length > 2)
                penultPos = pointsOnDrag[pointsOnDrag.Length - 2];
        }
        else
        {
            twoPoints = true;
            lastPos = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isEndEraser) { return; }
        //CheckPoint(eventData.position);
        startDraw = false;
        twoPoints = false;
    }


    #endregion

    Canvas canvas;

    Color checkPointColor;
    Vector3 checkPointWorldPos;
    Vector3 checkPointLocalPos;

    void CheckPoint(Vector3 pScreenPos)
    {
        checkPointWorldPos = canvas.worldCamera.ScreenToWorldPoint(pScreenPos);
        checkPointLocalPos = uiTex.gameObject.transform.InverseTransformPoint(checkPointWorldPos);

        if (checkPointLocalPos.x > -mWidth / 2 && checkPointLocalPos.x < mWidth / 2 && checkPointLocalPos.y > -mHeight / 2 && checkPointLocalPos.y < mHeight / 2)
        {
            for (int i = (int)checkPointLocalPos.x - brushSize; i < (int)checkPointLocalPos.x + brushSize; i++)
            {
                for (int j = (int)checkPointLocalPos.y - brushSize; j < (int)checkPointLocalPos.y + brushSize; j++)
                {
                    if (Mathf.Pow(i - checkPointLocalPos.x, 2) + Mathf.Pow(j - checkPointLocalPos.y, 2) > Mathf.Pow(brushSize, 2))
                        continue;
                    if (i < 0) { if (i < -mWidth / 2) { continue; } }
                    if (i > 0) { if (i > mWidth / 2) { continue; } }
                    if (j < 0) { if (j < -mHeight / 2) { continue; } }
                    if (j > 0) { if (j > mHeight / 2) { continue; } }

                    checkPointColor = MyTex.GetPixel(i + (int)mWidth / 2, j + (int)mHeight / 2);
                    if (checkPointColor.a != 0f)
                    {
                        checkPointColor.a = 0.0f;
                        colorA++;
                        MyTex.SetPixel(i + (int)mWidth / 2, j + (int)mHeight / 2, checkPointColor);
                    }
                }
            }


            //开始刮的时候 去判断进度
            if (!isStartEraser && !IsAuto)
            {
                isStartEraser = true;
                InvokeRepeating("getTransparentPercent", 0f, 0.2f);
                if (eraserStartEvent != null)
                {
                    eraserStartEvent.Invoke();
                }
            }

            MyTex.Apply();
        }
    }



    double fate;


    /// <summary> 
    /// 检测当前刮刮卡 进度
    /// </summary>
    /// <returns></returns>
    public void getTransparentPercent()
    {
        if (isEndEraser) { return; }


        fate = colorA / maxColorA * 100;

        fate = (float)Math.Round(fate, 2);

        //Debug.LogError("当前百分比: " + fate);

        if (fate >= rate)
        {
            isEndEraser = true;
            CancelInvoke("getTransparentPercent");
           // gameObject.SetActive(false);
            Debug.Log("擦除完毕");
            //触发结束事件
            if (eraserEndEvent != null)
                eraserEndEvent.Invoke();

        }
    }

}