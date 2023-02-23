using System;
using Main;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CaptureByRectV2 : MonoBehaviour
{

    private RectTransform RectTrans {
        get { return GetComponent<RectTransform>(); }
    }

    /// <summary>
    /// 通过rectTransform得到一个Rect
    /// </summary>
    /*private Rect TargetRect(RectTransform rectTransform) {
        //factor = ((int)(factor * 1000)) / 1000f;
        float adjustedWidth = rectTransform.GetWidth();
        float adjustedHeight = rectTransform.GetHeight();
        Debug.Log("当前屏幕有效宽度：" + adjustedWidth);
        Debug.Log("当前屏幕有效高度：" + adjustedHeight);
        Vector2 pos = new Vector2(0,0);//这个是用来修改切图的左边距离的，目前基本上没用
        Vector2 size = new Vector2(adjustedWidth, adjustedHeight);
        
        return new Rect(pos, size);
    }*/

    public void CaptureScreen(Action<Texture2D> action) {
        StartCoroutine(CaptureScreenReal(action));
    }

    private Camera mainCamera;
    private System.Collections.IEnumerator CaptureScreenReal(Action<Texture2D> action) {
        
        ScreenWH_Another = new Vector2(GameObject.Find("Bottom").GetComponent<RectTransform>().GetWidth(),
            GameObject.Find("Bottom").GetComponent<RectTransform>().GetHeight());
        //这里与章节里的判定是反过来的，因为章节里移动是手在屏幕上的移动，移动了2560（屏幕分辨率）就是移动了2298（Canvas大小），因此是 * ScreenWH_Another.y / Screen.height
        //但是这里是把Canvas的物体大小映射到屏幕大小
        ScreenScale = Screen.height / ScreenWH_Another.y;
        yield return new WaitForEndOfFrame();

        mainCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        Vector3 pos = mainCamera.WorldToScreenPoint(RectTrans.position);
        
        RectMsg rectMsg = new RectMsg(new Vector2(RectTrans.GetWidth() * ScreenScale,RectTrans.GetHeight() * ScreenScale),new Vector2(Screen.width,Screen.height),pos);
        
        Debug.Log("素材宽高："+rectMsg.W_H+"偏移："+rectMsg.offset+"长宽比"+rectMsg.W_H.y / rectMsg.W_H.x+
                  "截屏宽高："+new Vector2(RectTrans.GetWidth(),RectTrans.GetHeight())+"截屏长宽比"+RectTrans.GetHeight() / RectTrans.GetWidth());
        
        //先创建一个的空纹理，图片有多大rect.width+"="+rect.height这俩数据就有多大，这是用来存储的
        Texture2D screenShot = new Texture2D((int)rectMsg.W_H.x, (int)rectMsg.W_H.y, TextureFormat.RGB24, false);
        //读取屏幕像素信息并存储为纹理数据
        screenShot.ReadPixels(new Rect(rectMsg.offset.x, rectMsg.offset.y, rectMsg.W_H.x, rectMsg.W_H.y), 0, 0);//0,0的原点指的是左下角
        screenShot.Apply();
        action?.Invoke(screenShot);
    }

    //游戏画板高宽
    private Vector2 ScreenWH_Another;
    //屏幕缩放比，RectTrans的GetWidth()GetHeight()是组件内的比例，如果要转化成真实比例
    private float ScreenScale;
    public class RectMsg
    {
        public Vector2 offset = new Vector2(0,0);
        //图片在屏幕中的大小（缩放后）
        public Vector2 W_H = new Vector2(0,0);

        public RectMsg(Vector2 picMsg,Vector2 screen,Vector2 picPos)
        {
            /*if (picMsg.x > screen.x || picMsg.y > screen.y) {
                //截取的宽度或者高度大于了屏幕
                //将大得更大的缩小，小的同比例缩小
                float widthScale = picMsg.x / screen.x;//屏幕不可能为0，所以放心调用
                float heightScale = picMsg.y / screen.y;
                if (heightScale > widthScale) {
                    //高度的缩放比例更大，因此要以高度为标准
                    W_H.y = screen.y;
                    W_H.x = picMsg.x / heightScale;
                } else {
                    //宽度的缩放比例更大，因此要以宽度为标准
                    W_H.y = picMsg.y / widthScale;
                    W_H.x = screen.x;
                }
            }else {
                //截取的宽度且高度小于或者等于屏幕
                W_H.x = picMsg.x;
                W_H.y = picMsg.y;
            }*/
            W_H.x = picMsg.x;
            W_H.y = picMsg.y;
            
            //计算好大小后，在计算偏移
            //左边如果超越了屏幕右边，或者右边去到了左边（上下同理），那么就是无意义的
            if (picPos.x - W_H.x / 2 > screen.x || picPos.x + W_H.x / 2 < 0 || picPos.y - W_H.y / 2 > screen.y || picPos.y + W_H.y / 2 < 0) {
                Debug.LogError("你这个怎么截屏？");
                return;
            }
            if (picPos.x < W_H.x / 2)//左边如果低于了屏幕左边
            {
                if (picPos.x + W_H.x / 2 > screen.x)//右边如果高于了屏幕右边
                {
                    W_H.x = screen.x;
                } else {
                    W_H.x = picPos.x + W_H.x / 2;
                }

            } else {
                offset.x = picPos.x - W_H.x / 2;
                if (picPos.x + W_H.x / 2 > screen.x)//右边如果高于了屏幕右边
                {
                    W_H.x = screen.x - offset.x;
                }
            }

            if (picPos.y < W_H.y / 2 )//下边如果低于了屏幕底部
            {
                if (picPos.y + W_H.y / 2 > screen.y)//右边如果高于了屏幕右边
                {
                    W_H.y = screen.y;
                } else {
                    W_H.y = picPos.y + W_H.y / 2;
                }
            } else {
                Debug.Log(picPos.y +"="+W_H.y / 2);
                offset.y = picPos.y - W_H.y / 2;
                if (picPos.y + W_H.y / 2 > screen.y)//右边如果高于了屏幕右边
                {
                    W_H.y = screen.y - offset.y;
                }
            }

            if (W_H.x == 0 || W_H.y == 0)
            {
                Debug.LogError("你是怎么做到的？");
                return;
            }
        }
    }
}