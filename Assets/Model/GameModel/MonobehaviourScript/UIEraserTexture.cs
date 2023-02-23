using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIEraserTexture : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int brushScale = 30;
    public int imageWidth;
    public int imageHeight;
    Texture2D texRender;
    RectTransform mRectTransform;
    Canvas canvas;
    Image image;
    Vector2 start = Vector2.zero;
    Vector2 end = Vector2.zero;
    bool isMove = false;
    List<Vector2> listAlpha0 = new List<Vector2>();
    Action notify;

    void Start()
    {
        mRectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();
        texRender = new Texture2D(imageWidth, imageWidth, TextureFormat.ARGB32, true);
        Reset();
    }

    void Update()
    {
        if (isMove)
        {
            OnMouseMove(Input.mousePosition);
        }
    }

    void Reset()
    {
        for (int i = 0; i < texRender.width; i++)
        {

            for (int j = 0; j < texRender.height; j++)
            {

                Color color = texRender.GetPixel(i, j);
                color.a = 1;
                texRender.SetPixel(i, j, color);
            }
        }
        texRender.Apply();
        image.material.SetTexture("_RenderTex", texRender);
        listAlpha0?.Clear();
    }

    public void OnPointerDown(PointerEventData data)
    {
        start = ConvertSceneToUI(data.position);
        isMove = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        isMove = false;
        OnMouseMove(data.position);
        start = Vector2.zero;
    }

    Vector2 ConvertSceneToUI(Vector3 posi)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mRectTransform, posi, canvas.worldCamera, out Vector2 postion))
        {
            return postion;
        }
        return Vector2.zero;
    }

    void OnMouseMove(Vector2 position)
    {
        end = ConvertSceneToUI(position);
        Draw(new Rect(end.x + texRender.width / 2, end.y + texRender.height / 2, brushScale, brushScale));

        if (start.Equals(Vector2.zero))
        {
            return;
        }

        //Rect disract = new Rect((start + end).x / 2 + texRender.width / 2, (start + end).y / 2 + texRender.height / 2, Mathf.Abs(end.x - start.x), Mathf.Abs(end.y - start.y));

        //for (int x = (int)disract.xMin; x < (int)disract.xMax; x++)
        //{
        //    for (int y = (int)disract.yMin; y < (int)disract.yMax; y++)
        //    {
        //        Draw(new Rect(x, y, brushScale, brushScale));
        //    }
        //}

        start = end;
    }

    void Draw(Rect rect)
    {
        for (int x = (int)rect.xMin; x < (int)rect.xMax; x++)
        {
            for (int y = (int)rect.yMin; y < (int)rect.yMax; y++)
            {
                if (x < 0 || x > texRender.width || y < 0 || y > texRender.height)
                {
                    return;
                }
                Color color = texRender.GetPixel(x, y);
                color.a = 0;
                texRender.SetPixel(x, y, color);
                if (!listAlpha0.Exists(a => a.x == x && a.y == y))
                    listAlpha0.Add(new Vector2(x, y));
            }
        }

        texRender.Apply();
        image.material.SetTexture("_RenderTex", texRender);
        TryNotify();
    }

    private void TryNotify()
    {
        if (notify == null)
        {
            return;
        }
        float a = imageWidth * imageHeight / (float)brushScale;
        float result = listAlpha0.Count / a;
        if (result < 0.8f)
        {
            return;
        }
        notify.Invoke();
        notify = null;
    }

    public void RegisterNotify(Action notify)
    {
        this.notify = notify;
    }
}