using System;
using System.Collections.Generic;
using System.Reflection;
using Blaze.Manage.Csv.Enum;
using Blaze.Manage.Locale;
using Blaze.Utility;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Blaze.Resource;
using UniRx;

namespace ETHotfix
{
    /// <summary>
    /// 所有页面基类
    /// 添加dispose自动打断await功能,用法如下
    ///  await RelyAwait(Task.Delay(100));
    ///  var num = await RelyAwait(GetInt());
    /// </summary>
    public class UIBaseComponent : UI
    {
        /// <summary>
        /// 文本对象存储器
        /// </summary>
        public UguiTextHolder Holder;

        // 当前舞台界面对象
        protected GameObject _curStage => GameObject;

        /// <summary>
        /// 循环列表循环组件
        /// </summary>
        private Dictionary<GameObject, UIBaseComponent>
            _loopViewCompDic = new Dictionary<GameObject, UIBaseComponent>();

        /// <summary>
        /// 页面初始化
        /// </summary>
        public virtual void Awake()
        {
            Holder = new UguiTextHolder();
            // 注入联动
            AutoBundle();
            // 注入多语言
            MultiLanguage();
        }

        /// <summary>
        /// 添加底图，平板的功能模块需要底图，不然会穿帮
        /// </summary>
        protected void AddBaseMap()
        {
            _curStage.AddComponent<Image>();
        }
        

        #region 页面注入引用与多语言

        // 注入多语言
        private void MultiLanguage()
        {
            // 多语言UI文字注入
            Lang.ChangeLocaleNotify.OnMessage += delegate(LocaleType type)
            {
                // 去多语言系统 加载 语言表
                var lang = Lang.Table(LanguageName());

                // 多语言加载成功 或 语言改变
                foreach (KeyValuePair<string, GameObject> pair in Holder.GetFieldInfos())
                {
                    var data = lang.GetContent(pair.Key);
                    if (data != null && !data.Equals(""))
                    {
                        if (data.Equals("~")) data = "";
                        pair.Value.GetComponent<Text>().text = data;
                    }
                }
            };
        }

        protected virtual string LanguageName()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// 注入页面引用
        /// </summary>
        /// <param name="panel"></param>
        private void AutoBundle()
        {
            Dictionary<string, GameObject> objs = new Dictionary<string, GameObject>();
            // 递归加载多语言类
            Recursive(_curStage, objs, "txt_", "itxt_");
            foreach (var pair in objs)
            {
                Holder.AddTextComponent(pair.Key, pair.Value);
            }

            // 根据 C# 中的方法绑定
            Dictionary<string, MethodInfo> onClick = new Dictionary<string, MethodInfo>();
            foreach (var methodInfo in GetType().GetMethods())
            {
                try
                {
                    if (methodInfo.Name.StartsWith("OnBtn"))
                    {
                        var key = methodInfo.Name.Replace("OnBtn", "");
                        if (!onClick.ContainsKey(key))
                            onClick.Add(key, methodInfo);
                    }
                }
                catch (Exception e)
                {
                    Tuner.Error("绑定按钮出错：" + methodInfo);
                }
            }

            var stage = this;

            // 根据 C# 中的属性绑定
            foreach (var fieldInfo in stage.GetType().GetFields())
            {
                var name = fieldInfo.Name;

                try
                {
                    if (name.StartsWith("btn"))
                    {
                        try
                        {
                            name = name.Replace("btn", "");
                            var btn = Find<Button>("btn_" + name);

                            // 绑定按钮到挂载对象
                            fieldInfo.SetValue(stage, btn);

                            // 有点击触发方法时，绑定触发方法
                            if (onClick.ContainsKey(name))
                                btn.onClick.AddListener(delegate { onClick[name]?.Invoke(this, null); });
                        }
                        catch (Exception e)
                        {
                            Tuner.Warn("Havn't {0} OnBtn Function! {1}", name, e);
                        }
                    }
                    // else if (name.StartsWith("img"))
                    // {
                    //     name = name.Replace("img", "");
                    //     fieldInfo.SetValue(stage,
                    //         panel.GetChild("img_" + name).asImage);
                    // }
                    // else if (name.StartsWith("txt"))
                    // {
                    //     name = name.Replace("txt", "");
                    //     fieldInfo.SetValue(stage,
                    //         panel.GetChild("txt_" + name).asTextField);
                    // }
                }
                catch (Exception e)
                {
                    Tuner.Log($"[>>] UGui [[{name}]] Injection Error: {e.Message}");
                    Tuner.Warn($"[>>] UGui [[{name}]] Injection Error: {e.Message}");
                }
            }
        }

        public T Find<T>(string name) where T : UnityEngine.Object
        {
            return UGui.FindChild<T>(_curStage.transform, name);
        }

        public GameObject Find(string name)
        {
            return UGui.FindChild(_curStage.transform, name).gameObject;
        }

        // 递归找出所有要求的对象
        private static void Recursive(GameObject parenGameObject, Dictionary<string, GameObject> objs,
            params string[] rules)
        {
            foreach (Transform child in parenGameObject.transform)
            {
                for (int i = 0; i < rules.Length; i++)
                {
                    // 符合规则的加入对象池中
                    if (child.name.StartsWith(rules[i]))
                        objs[child.name] = child.gameObject;
                }

                Recursive(child.gameObject, objs, rules);
            }
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
            _loopViewCompDic.Clear();
            Holder = null;
        }

        #region 当前页面通用工具

        /// <summary>
        /// 获取循环列表的上Item组件
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetLoopViewItemComp<T>(GameObject obj) where T : UIBaseComponent, new()
        {
            if (_loopViewCompDic.ContainsKey(obj))
                return _loopViewCompDic[obj] as T;
            var comp = BUI.CreateFromGo<T>(obj);
            Add(comp);
            _loopViewCompDic.Add(obj, comp);
            Debug.Log("create");
            return comp;
        }

        /// <summary>
        /// 从对象上获取组件，组件不存在时添加组件
        /// </summary>
        /// <param name="go"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Comp<T>(GameObject go) where T : class
        {
            if (!go.TryGetComponent(out T compType))
                compType = go.AddComponent(typeof(T)) as T;
            return compType;
        }

        /// <summary>
        /// 向对象身上绑定点击事件
        /// </summary>
        /// <param name="go"></param>
        /// <param name="onClick"></param>
        /// <param name="isClear"></param>
        public static void Btn(GameObject go, Action onClick, bool isClear = true, bool onlyOnce = false)
        {
            // 获取按钮组件
            if (!go.TryGetComponent(out Button button))
                button = go.AddComponent<Button>();

            // 根据设定清空按钮上的时间
            if (isClear)
                button.onClick.RemoveAllListeners();

            // 绑定按钮事件
            button.onClick.AddListener(delegate
            {
                onClick?.Invoke();
                if (onlyOnce)
                    button.onClick.RemoveAllListeners();
            });
        }

        public static void Btn(Image img, Action onClick, bool isClear = true, bool onlyOnce = false)
        {
            Btn(img.gameObject, onClick, isClear, onlyOnce);
        }

        public static void Btn(Button button, Action onClick, bool isClear = true, bool onlyOnce = false)
        {
            // 根据设定清空按钮上的时间
            if (isClear)
                button.onClick.RemoveAllListeners();

            // 绑定按钮事件
            button.onClick.AddListener(delegate
            {
                onClick?.Invoke();
                if (onlyOnce)
                    button.interactable = false;
            });
        }


        /// <summary>
        /// 进入动画
        /// </summary>
        /// <param name="endCallBack"></param>
        public virtual void EnterAnimation(Action endCallBack, float time = 0.8f)
        {
            Tweener tweener = null;
            // 默认进入动画   
            var cg = Comp<CanvasGroup>(_curStage);
            cg.alpha = 0;
            tweener = cg.DOFade(1, time);

            tweener.onComplete += delegate
            {
                endCallBack?.Invoke();
                AfterEnterAction();
            };
            Observable.NextFrame().Subscribe(_ => Show()); //同帧渲染可能会出错
        }


        /// <summary>
        /// 进入动画完成之后调用
        /// </summary>
        public virtual void AfterEnterAction()
        {
        }

        /// <summary>
        /// 退出动画
        /// </summary>
        /// <param name="closeThisPage">关闭当前页面</param>
        /// <param name="showNextPage">显示下一页面</param>
        /// <param name="time">关闭当前页面渐变动画时间</param>
        /// <param name="isFuse">是否与下一页面融合渐变，即当前页面与下一页动画同时进行</param>
        public virtual void ExitAnimation(Action closeThisPage, Action showNextPage = null, float time = 0.8f,
            bool isFuse = false)
        {
            Tweener tweener = null;
            // 默认退出动画
            var cg = Comp<CanvasGroup>(_curStage);
            tweener = cg.DOFade(0, time);
            tweener.onComplete = () =>
            {
                closeThisPage?.Invoke();
                if (!isFuse)
                    showNextPage?.Invoke();
                AfterExitAction();
            };
            if (isFuse)
                showNextPage?.Invoke();
        }

        /// <summary>
        /// 退出动画完成之后调用
        /// </summary>
        public virtual void AfterExitAction()
        {
        }


        protected virtual void Finish()
        {
            FinishNext(null);
        }


        protected void FinishNext(UIArgs next)
        {
   
            OnFinish?.Invoke(next);
        }
        #endregion
    }
}