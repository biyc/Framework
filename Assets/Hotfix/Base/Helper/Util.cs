using Main;
using UnityEngine;
using UnityEngine.UI;

public class Util
{
    /// <summary>
    /// 按比例设置图片大小
    /// </summary>
    /// <param name="img"></param>
    /// <param name="sprite"></param>
    public static void SetImg(Image img, Sprite sprite) {
        RectTransform rectTran = img.GetComponent<RectTransform>();
        float originRatio = rectTran.GetWidth() / rectTran.GetHeight();
        float spriteRatio = sprite.rect.width / sprite.rect.height;
        if (originRatio > spriteRatio) {
            rectTran.SetWidth(spriteRatio * rectTran.GetHeight());
        } else {
            rectTran.SetHeight(rectTran.GetWidth() / spriteRatio);
        }

        img.sprite = sprite;
    }
}