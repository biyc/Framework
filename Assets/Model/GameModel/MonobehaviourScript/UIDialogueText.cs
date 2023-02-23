using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueText : MonoBehaviour
{
    private Action action;
    private string strInfo;
    private Color strColor;
    public Image image_m_Content_Bg;
    public Text text_Text_Content;

    public UIDialogueText ShowDialogueText(string strText, Color color, Action callBack = null)
    {
        this.action = callBack;
        this.strInfo = strText;
        this.strColor = color;

        SetTextColor();
        SetTextBgLength();
        PlayTextPrintEffect();
        return this;
    }
    public void CloseDialogue()
    {
        DestroyImmediate(gameObject);        
    }

    public void SetTextColor() {
        text_Text_Content.color = strColor;
    }
    public async void PlayTextPrintEffect()
    {
        text_Text_Content.text = "";
        try
        {
            //对将要打印的文本文档转换为数组，遍历数组
            foreach (char letter in strInfo.ToCharArray())
            {
                if (gameObject == null) return;
                text_Text_Content.text += letter;
                await UniRx.Async.UniTask.Delay(100);
            }
            action?.Invoke();
            action = null;
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    /// <summary>
    /// 获取文本的绘制长度，不同于text的rectTransform.sizeDelta
    /// </summary>
    /// <param name="str">文本</param>
    /// <returns></returns>
    public int GetFontlen(string str)
    {
        int len = 100;
        Font font;
        font = Font.CreateDynamicFontFromOSFont("FZSERZM", 40);
        font.RequestCharactersInTexture(str);
        for (int i = 0; i < str.Length; i++)
        {
            CharacterInfo ch;
            font.GetCharacterInfo(str[i], out ch);
            len += ch.advance;
        }
        return len;
    }
    private void SetTextBgLength()
    {
        image_m_Content_Bg.rectTransform.sizeDelta = new Vector2(GetFontlen(strInfo), 64);
    }
}
