using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public static class RectTransformExtension
{
    public static void SetDefaultScale(this RectTransform trans)
    {
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
    {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    public static Vector2 GetSize(this RectTransform trans)
    {
        return trans.rect.size;
    }

    public static float GetWidth(this RectTransform trans)
    {
        return trans.rect.width;
    }

    public static float GetHeight(this RectTransform trans)
    {
        return trans.rect.height;
    }

    public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
    }

    public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width),
            newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width),
            newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width),
            newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width),
            newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static Vector2 GetLocalCenterPos(this RectTransform trans)
    {
        float x = trans.localPosition.x - (trans.pivot.x - .5f) * trans.rect.width;
        float y = trans.localPosition.y - (trans.pivot.y - .5f) * trans.rect.height;
        return new Vector3(x, y, trans.localPosition.z);
    }

    public static void SetSize(this RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax +
                          new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }

    public static void SetWidth(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }

    public static void SetHeight(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }


    public static void CopyRectTransform(this RectTransform rect, RectTransform target)
    {
        rect.anchorMin = new Vector2(target.anchorMin.x, target.anchorMin.y);
        rect.anchorMax = new Vector2(target.anchorMax.x, target.anchorMax.y);
        rect.pivot = new Vector2(target.pivot.x, target.pivot.y);
        rect.anchoredPosition = new Vector2(target.anchoredPosition.x, target.anchoredPosition.y);
        rect.offsetMin = new Vector2(target.offsetMin.x, target.offsetMin.y);
        rect.offsetMax = new Vector2(target.offsetMax.x, target.offsetMax.y);
        rect.sizeDelta = new Vector2(target.sizeDelta.x, target.sizeDelta.y);
        rect.rotation = new Quaternion(target.localRotation.x, target.localRotation.y, target.localRotation.z,
            target.localRotation.w);
        rect.localScale = new Vector3(target.localScale.x, target.localScale.y, target.localScale.z);
        rect.localEulerAngles =
            new Vector3(target.localEulerAngles.x, target.localEulerAngles.y, target.localEulerAngles.z);
    }

    public static void SetNormalPivot(this RectTransform rect)
    {
        return;
        Vector2 vector = new Vector2(0.5f, 0.5f);
        rect.pivot = vector;
        rect.offsetMin = vector;
        rect.offsetMax = vector;
    }

    public static void Hide(this Image img) => img.gameObject.SetActive(false);


    public static void Show(this Image img) => img.gameObject.SetActive(true);


    public static void Hide(this GameObject obj) => obj.SetActive(false);

    public static void Show(this GameObject obj) => obj.SetActive(true);


    public static void Hide(this Transform tr) => tr.gameObject.SetActive(false);


    public static void Show(this Transform tr) => tr.gameObject.SetActive(true);


    public static T GetRandomValue<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
            return default(T);
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static Animator GetAnimator(this Transform tr) => tr.gameObject.GetAnimator();

    public static Animator GetAnimator(this GameObject obj) => obj.GetComponent<Animator>();

    public static void SetLocalPosY(this Transform tr, float y)
    {
        var pos = tr.transform.localPosition;
        tr.transform.localPosition = new Vector3(pos.x, y, pos.z);
    }

    public static void SetLocalPosX(this Transform tr, float x)
    {
        var pos = tr.transform.localPosition;
        tr.transform.localPosition = new Vector3(x, pos.y, pos.z);
    }

    public static void DoLocalMoveDisX(this Transform tr, float dis,float time)
    {
        var pos = tr.transform.localPosition;
        tr.transform.DOLocalMoveX(pos.x + dis, time);
    }

    public static void DoLocalMoveDisY(this Transform tr, float dis,float time)
    {
        var pos = tr.transform.localPosition;
        tr.transform.DOLocalMoveY(pos.y + dis, time);
    }
    public static T Comp<T>(this Transform tr) where T : class
    {
        if (!tr.TryGetComponent(out T compType))
            compType = tr.gameObject.AddComponent(typeof(T)) as T;
        return compType;
    }

    public static CanvasGroup GetCanvasGroup(this Transform tr) => tr.Comp<CanvasGroup>();

    public static CanvasGroup GetCanvasGroup(this Image image) => image.transform.GetCanvasGroup();

    public static List<T> GetComponentsInChildrenNoRoot<T>(this Transform tr)
    {
        var comps = tr.GetComponentsInChildren<T>().ToList();
        var m = tr.GetComponent<T>();
        if (m != null && comps.Contains(m))
            comps.Remove(m);
        //comps.RemoveAt(0);
        return comps;
    }

    public static List<T> GetComponentsInChildrenNoRoot<T>(this GameObject obj) =>
        obj.transform.GetComponentsInChildrenNoRoot<T>();

    public static RectTransform GetRectTransform(this Transform tr) => tr.GetComponent<RectTransform>();


    public static Button GetButton(this Transform tr) => tr.Comp<Button>();


    public static void BindClick(this Button button, Action onClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { onClick?.Invoke(); });
    }

    public static void SetAlpha(this Graphic graphic, float alpha)
    {
        alpha = Mathf.Clamp(alpha, 0, 1);
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }

    public static float GetClipLengthByName(this Animator animator, string name)
    {
        if (animator == null) return 0;
        var infos = animator.runtimeAnimatorController.animationClips;
        if (infos == null || infos.Length <= 0) return 0;
        foreach (var v in infos)
        {
            if (v.name == name)
                return v.length;
        }

        return 0;
    }
}