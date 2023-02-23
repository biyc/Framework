using System;
using DG.Tweening;
using ETHotfix;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


public static class StaticTool
{
    /// <summary>
    /// 显示隐藏
    /// </summary>
    public static void ActionGameObject(UnityEngine.MonoBehaviour monoBehaviour, bool active) =>
        ActionGameObject(monoBehaviour.gameObject, active);

    /// <summary>
    /// 显示隐藏
    /// </summary>
    public static void ActionGameObject(UnityEngine.GameObject gameObject, bool active)
    {
        if (gameObject == null)
            return;
        if (gameObject.activeSelf == active)
            return;
        gameObject.SetActive(active);
    }

    #region 逐渐显示或者隐藏

    public static void CanvasGroupHide(Image img, float time = 1, Action callback = null)
    {
        CanvasGroupHide(img.transform, time, callback);
    }

    public static void CanvasGroupHide(Transform tr, float time = 1, Action callback = null)
    {
        var cg = tr.GetCanvasGroup();
        DOTween.To(() => cg.alpha, x => cg.alpha = x, 0, time).onComplete = () => callback?.Invoke();
    }

    public static void CanvasGroupShow(Image img, float time = 1, Action callback = null)
    {
        CanvasGroupShow(img.transform, time, callback);
    }

    public static void CanvasGroupShow(Transform tr, float time = 1, Action callback = null)
    {
        var cg = tr.GetCanvasGroup();
        cg.alpha = 0;
        DOTween.To(() => cg.alpha, x => cg.alpha = x, 1, time).onComplete = () => callback?.Invoke();
    }

    public static void ImageHide(Image image, float time = 1, Action callback = null)
    {
        image.DOColor(GetZeroColor(image), time).onComplete = () => callback?.Invoke();
    }

    public static void ImageShow(Image image, float time = 1, Action callback = null)
    {
        image.DOColor(GetOneColor(image), time).onComplete = () => callback?.Invoke();
    }

    #endregion

    /// <summary>
    /// 获得图片透明度为0的颜色值
    /// </summary>
    /// <param name="img"></param>
    /// <returns></returns>
    public static Color GetZeroColor(Image img)
    {
        return new Color(img.color.r, img.color.g, img.color.b, 0);
    }

    /// <summary>
    ///  获得图片透明度为1的颜色值
    /// </summary>
    /// <param name="img"></param>
    /// <returns></returns>
    public static Color GetOneColor(Image img)
    {
        return new Color(img.color.r, img.color.g, img.color.b, 1);
    }

    /// <summary>
    /// sprite 转换成 texture2d
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    public static Texture2D SpriteToTexture2D(Sprite sprite)
    {
        var tex = new Texture2D((int) sprite.rect.width, (int) sprite.rect.height);
        var pixels = sprite.texture.GetPixels((int) sprite.textureRect.x, (int) sprite.textureRect.y,
            (int) sprite.textureRect.width, (int) sprite.textureRect.height);
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    #region 未审核的代码（可能工作异常）

    /// <summary>
    /// 截图
    /// </summary>
    public static void CaptureScreen(CaptureByRect captureByRect, string textureName, Action<Texture2D> callback,
        bool write2Local = true)
    {
        captureByRect?.CaptureScreen((texture2d) =>
        {
            // 然后将这些纹理数据，成一个png图片文件
            if (write2Local)
            {
                byte[] bytes = texture2d.EncodeToPNG();
                string filename = Application.persistentDataPath + "/" + textureName;
                System.IO.File.WriteAllBytes(filename, bytes);
            }

            callback?.Invoke(texture2d);
        });
    }

    /// <summary>
    /// 截图版本2
    /// </summary>
    /// <param name="captureByRect"></param>
    /// <param name="textureName"></param>
    /// <param name="callback"></param>
    /// <param name="write2Local"></param>
    public static void CaptureScreen(CaptureByRectV2 captureByRect, string textureName, Action<Texture2D> callback,
        bool write2Local = true)
    {
        captureByRect?.CaptureScreen((texture2d) =>
        {
            // 然后将这些纹理数据，成一个png图片文件
            if (write2Local)
            {
                byte[] bytes = texture2d.EncodeToPNG();
                string filename = Application.persistentDataPath + "/" + textureName;
                System.IO.File.WriteAllBytes(filename, bytes);
            }

            callback?.Invoke(texture2d);
        });
    }

    public static Sprite TextureToSprite(Texture2D texture2d)
    {
        Sprite createSprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
        return createSprite;
    }


    public static string FormatSecond2Set(int timeStamp, string colorCode)
    {
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0, System.DateTimeKind.Utc);
        long time = Convert.ToInt64(ts.TotalSeconds);
        Debug.Log(time);
        var syTimeStamp = timeStamp - time;
        long lTime = long.Parse(syTimeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        var day = toNow.Days.ToString().Trim();
        var hour = toNow.Hours.ToString().Trim();
        var minute = toNow.Minutes.ToString().Trim();
        return string.Format($" <color='{colorCode}'>倒计时：{day}天{hour}小时{minute}分钟</color>");
    }


    // 参数：
    // POINT p 指定的某个点
    // LPPOINT ptPolygon 多边形的各个顶点坐标（首末点可以不一致）
    // int nCount 多边形定点的个数

    public static bool PointInPolygon(Vector2 p, Vector2[] ptPolygon, int nCount)
    {
        int nCross = 0;

        for (int i = 0; i < nCount; i++)
        {
            Vector2 p1 = ptPolygon[i]; //当前节点
            Vector2 p2 = ptPolygon[(i + 1) % nCount]; //下一个节点

            // 求解 y=p.y 与 p1p2 的交点

            if (p1.y == p2.y) // p1p2 与 y=p0.y平行
                continue;

            if (p.y < Mathf.Min(p1.y, p2.y)) // 交点在p1p2延长线上
                continue;
            if (p.y >= Mathf.Max(p1.y, p2.y)) // 交点在p1p2延长线上
                continue;

            // 从P发射一条水平射线 求交点的 X 坐标 ------原理: ((p2.y-p1.y)/(p2.x-p1.x))=((y-p1.y)/(x-p1.x))
            //直线k值相等 交点y=p.y
            double x = (double) (p.y - p1.y) * (double) (p2.x - p1.x) / (double) (p2.y - p1.y) + p1.x;

            if (x > p.x)
                nCross++; // 只统计单边交点
        }

        // 单边交点为偶数，点在多边形之外 ---
        return (nCross % 2 == 1);
    }


    //截图分享时，应该分辨率导致不一样，这里统一成一样的
    public static void CalcCpatureRect(GameObject uiGameObject, GameObject captureRectGameObject)
    {
        var parentCanvas = uiGameObject.GetComponentInParent<UnityEngine.UI.CanvasScaler>();
        var rectTransform = parentCanvas.GetComponent<RectTransform>();
        var goTransform = captureRectGameObject.GetComponent<RectTransform>();
        float x = 0;
        float y = 0;
        if (parentCanvas.matchWidthOrHeight == 1)
        {
            x = (rectTransform.sizeDelta.x - 1242) / 2;
        }
        else
        {
            y = (2688 - rectTransform.sizeDelta.y) / 2;
        }

        goTransform.offsetMax = new Vector2(goTransform.offsetMax.x - x, Math.Min(0, goTransform.offsetMax.y + y));
        goTransform.offsetMin = new Vector2(goTransform.offsetMin.x + x, Math.Max(0, goTransform.offsetMin.y - y));
    }

    #endregion
}