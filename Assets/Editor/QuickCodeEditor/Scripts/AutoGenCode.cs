using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Text;
using System;
using System.IO;
using System.Linq;
using Blaze.Utility;

namespace Quick.Code
{
    public class AutoGenCode
    {
        #region 等待处理的代码

        [MenuItem("QuickTool/生成页面引用绑定")]
        public static void OpenWindow()
        {
            var source = "Assets/Projects/UI";
            var target = "Assets/Hotfix/UI/UIBind";

            var genFilter = new GenFilter();
            genFilter.Init();
            Dictionary<string, GameObject> allPrefeb = new Dictionary<string, GameObject>();

            AssetDatabase.GetAllAssetPaths().ToList().FindAll(delegate(string s)
                {
                    return s.Contains(source) && s.Contains(".prefab");
                })
                .ForEach(delegate(string s)
                {
                    if (genFilter.IsGen(s))
                        allPrefeb.Add(s, AssetDatabase.LoadAssetAtPath<GameObject>(s));
                });
            foreach (var pair in allPrefeb)
            {
                try
                {
                    var gen = new AutoGenCode();
                    gen.root = pair.Value;

                    // 清除控件
                    gen.uiWidgets.Clear();

                    // 清除其他
                    gen.uiObjects.Clear();

                    gen.RecursiveUI(pair.Value.transform, (tran) =>
                    {
                        if (tran.name.StartsWith("m_"))
                        {
                            gen.uiObjects.Add(tran.gameObject);
                        }

                        UIBehaviour[] widgets = tran.GetComponents<UIBehaviour>();
                        for (int i = 0; i < widgets.Length; i++)
                        {
                            var widget = widgets[i];
                            if (widget != null && !gen.uiWidgets.Contains(widget))
                            {
                                gen.uiWidgets.Add(widget);
                                string uiNamespace = widget.GetType().Namespace;
                                if (!string.IsNullOrEmpty(uiNamespace) && !gen.uiNamespace.Contains(uiNamespace)) {
                                    gen.uiNamespace.Add(uiNamespace);
                                }
                            }
                        }
                    });

                    // 变量声明
                    gen.BuildStatementCode();

                    // 注册事件
                    // BuildEventCode();

                    // 查找赋值
                    gen.BuildAssignmentCode();

                    var temp = pair.Key.Replace(source, "").Replace(".prefab", "");
                    // 生成脚本
                    gen.CreateCsUIScript($"Assets/Hotfix/UI/UIBind/{temp}Bind.cs");
                }
                catch (Exception e)
                {
                    Tuner.Error(pair.Key);
                }
            }
        }

        //选择的根游戏体
        private GameObject root;

        private SerializedObject serializedObj;

        // ui控件列表
        private List<UIBehaviour> uiWidgets = new List<UIBehaviour>();
        
        // ui控件需要用到的命名空间
        private List<string> uiNamespace = new List<string>();

        // ui游戏对象列表
        private List<GameObject> uiObjects = new List<GameObject>();

        //视图宽度一半
        private float halfViewWidth;

        //视图高度一半
        private float halfViewHeight;

        private Vector2 scrollWidgetPos;
        private Vector2 scrollObjectPos;
        private Vector2 scrollTextPos;

        private int selectedBar = 0;
        private bool isMono = false;

        /// <summary>
        /// 生成C# UI脚本
        /// </summary>
        private void CreateCsUIScript(string path)
        {
            //如果不存在就创建file文件夹
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            className = this.root.name + "Bind";

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append(CodeConfig.codeAnnotation);
            // scriptBuilder.Append(CodeConfig.usingNamespace);
            scriptBuilder.Append("\nusing UnityEngine;\n");
            if (uiNamespace.Count > 0) {
                for (int i = 0; i < uiNamespace.Count; i++) {
                    scriptBuilder.Append($"using {uiNamespace[i]};\n");
                }
            }
            if (isMono)
            {
                scriptBuilder.AppendFormat(CodeConfig.classMonoStart, className);
            }
            else
            {
                scriptBuilder.AppendFormat(CodeConfig.classStart, className);
            }

            scriptBuilder.Append(codeStateText);
            scriptBuilder.Append(codeAssignText);
            scriptBuilder.Append(codeEventText);
            scriptBuilder.Append(CodeConfig.classEnd);

            File.WriteAllText(path, scriptBuilder.ToString(), new UTF8Encoding(false));

            Debug.Log("脚本生成成功,生成路径为:" + path);
        }

        #endregion

        #region 代码生成

        private StringBuilder codeStateText;
        private StringBuilder codeEventText;
        private StringBuilder codeAssignText;
        private StringBuilder codeAllText;

        //缓存所有变量名和对应控件对象，对重名作处理
        private Dictionary<string, object> variableNameDic = new Dictionary<string, object>();

        //变量编号
        private int variableNum;

        //需要注册事件的控件,可通过toggle选择
        private Dictionary<string, bool> selectedEventWidgets = new Dictionary<string, bool>();

        //UI 类名
        private string className;

        //生成脚本的类型
        private Type scriptType;

        #endregion

        #region 代码格式分类

        private string regionStartFmt
        {
            get { return selectedBar == 0 ? CodeConfig.regionStartFmt : CodeConfig.regionStartFmtLua; }
        }

        private string regionEnd
        {
            get { return selectedBar == 0 ? CodeConfig.regionEnd : CodeConfig.regionEndLua; }
        }

        private string statementRegion
        {
            get { return CodeConfig.statementRegion; }
        }

        private string eventRegion
        {
            get { return selectedBar == 0 ? CodeConfig.eventRegion : CodeConfig.eventRegionLua; }
        }

        private string assignRegion
        {
            get { return selectedBar == 0 ? CodeConfig.assignRegion : CodeConfig.assignRegionLua; }
        }

        private string methodStartFmt
        {
            get { return selectedBar == 0 ? CodeConfig.methodStartFmt : CodeConfig.methodStartFmtLua; }
        }

        private string methodEnd
        {
            get { return selectedBar == 0 ? CodeConfig.methodEnd : CodeConfig.methodEndLua; }
        }

        private string assignCodeFmt
        {
            get { return selectedBar == 0 ? CodeConfig.assignCodeFmt : CodeConfig.assignCodeFmtLua; }
        }

        private string assignGameObjectCodeFmt
        {
            get
            {
                return selectedBar == 0 ? CodeConfig.assignGameObjectCodeFmt : CodeConfig.assignGameObjectCodeFmtLua;
            }
        }

        private string assignRootCodeFmt
        {
            get { return selectedBar == 0 ? CodeConfig.assignRootCodeFmt : CodeConfig.assignRootCodeFmtLua; }
        }

        private string onClickSerilCode
        {
            get { return selectedBar == 0 ? CodeConfig.onClickSerilCode : CodeConfig.onClickSerilCodeLua; }
        }

        private string onValueChangeSerilCode
        {
            get { return selectedBar == 0 ? CodeConfig.onValueChangeSerilCode : CodeConfig.onValueChangeSerilCodeLua; }
        }

        private string btnCallbackSerilCode
        {
            get { return selectedBar == 0 ? CodeConfig.btnCallbackSerilCode : CodeConfig.btnCallbackSerilCodeLua; }
        }

        private string eventCallbackSerilCode
        {
            get { return selectedBar == 0 ? CodeConfig.eventCallbackSerilCode : CodeConfig.eventCallbackSerilCodeLua; }
        }

        #endregion

        #region 有用

        private void BuildAssignmentCode()
        {
            codeAssignText = new StringBuilder();
            codeAssignText.Append(assignRegion);
            codeAssignText.AppendFormat(methodStartFmt, "InitUI");
            if (!isMono && selectedBar == 0)
            {
                codeAssignText.Append(CodeConfig.assignTransform);
            }

            var allPath = GetChildrenPaths(root);

            if (variableNameDic == null)
            {
                return;
            }

            //格式：变量名 = transform.Find("").Getcomponent<>();
            foreach (var name in variableNameDic.Keys)
            {
                var obj = variableNameDic[name];
                if (obj == null) continue;

                string path = "";
                bool isRootComponent = false;
                foreach (var tran in allPath.Keys)
                {
                    if (tran == null) continue;

                    UIBehaviour behav = obj as UIBehaviour;
                    if (behav != null)
                    {
                        //判断是否挂在根上，根上不需要路径
                        isRootComponent = behav.gameObject == root;
                        if (isRootComponent) break;

                        if (behav.gameObject == tran.gameObject)
                        {
                            path = allPath[tran];
                            break;
                        }
                    }
                    else
                    {
                        if (tran.gameObject == obj)
                        {
                            path = allPath[tran];
                            break;
                        }
                    }
                }

                if (obj is GameObject)
                {
                    codeAssignText.AppendFormat(assignGameObjectCodeFmt, name, path);
                }
                else
                {
                    if (isRootComponent)
                    {
                        codeAssignText.AppendFormat(assignRootCodeFmt, name, obj.GetType().Name);
                    }
                    else
                    {
                        codeAssignText.AppendFormat(assignCodeFmt, name, path, obj.GetType().Name);
                    }
                }
            }

            codeAssignText.Append(methodEnd);
            codeAssignText.Append(regionEnd);
            //Debug.Log(codeAssignText.ToString());
        }

        private Dictionary<Transform, string> GetChildrenPaths(GameObject rootGo)
        {
            Dictionary<Transform, string> pathDic = new Dictionary<Transform, string>();
            string path = string.Empty;
            try
            {
                Transform[] tfArray = rootGo.GetComponentsInChildren<Transform>(true);
                for (int i = 0; i < tfArray.Length; i++)
                {
                    Transform node = tfArray[i];

                    string str = node.name;
                    while (node.parent != null && node.gameObject != rootGo && node.parent.gameObject != rootGo)
                    {
                        str = string.Format("{0}/{1}", node.parent.name, str);
                        node = node.parent;
                    }

                    path += string.Format("{0}\n", str);

                    if (!pathDic.ContainsKey(tfArray[i]))
                    {
                        pathDic.Add(tfArray[i], str);
                    }
                }
            }
            catch (Exception e)
            {
            }

            return pathDic;
        }

        /// <summary>
        /// 遍历UI
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="callback">回调</param>
        public void RecursiveUI(Transform parent, UnityAction<Transform> callback)
        {
            if (callback != null)
                callback(parent);

            if (parent.childCount >= 0)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    Transform child = parent.GetChild(i);

                    RecursiveUI(child, callback);
                }
            }
        }

        private string BuildStatementCode()
        {
            variableNum = 0;
            variableNameDic.Clear();

            codeStateText = null;
            codeStateText = new StringBuilder();

            codeStateText.Append(CodeConfig.statementRegion);
            //非mono类声明一个transform
            if (!isMono)
            {
                codeStateText.Append(CodeConfig.stateTransform);
            }

            //控件列表
            for (int i = 0; i < uiWidgets.Count; i++)
            {
                if (uiWidgets[i] == null) continue;

                Type type = uiWidgets[i].GetType();
                if (type == null)
                {
                    Debug.LogError("BuildUICode type error !");
                    return "";
                }

                string typeName = type.Name;
                string variableName = string.Format("{0}_{1}", typeName.ToLower(), uiWidgets[i].name);
                variableName = variableName.Replace(' ', '_'); //命名有空格的情况
                variableName = variableName.Replace('(', '_'); //命名有括号
                variableName = variableName.Replace(')', '_'); //命名有括号的情况
                variableName = variableName.Replace('（', '_'); //命名有括号的情况
                variableName = variableName.Replace('）', '_'); //命名有括号的情况
                variableName = variableName.Replace('-', '_'); //命名有括号的情况
                //重名处理
                ++variableNum;
                if (variableNameDic.ContainsKey(variableName))
                {
                    variableName += variableNum;
                }

                if (variableNameDic.ContainsKey(variableName))
                {
                    ++variableNum;
                    variableName += variableNum;
                }

                variableNameDic.Add(variableName, uiWidgets[i]);

                if (isMono)
                {
                    codeStateText.AppendFormat(CodeConfig.serilStateCodeFmt, typeName, variableName);
                }
                else
                {
                    codeStateText.AppendFormat(CodeConfig.stateCodeFmt, typeName, variableName);
                }
            }

            //其他对象列表，目前都是GameObject
            for (int i = 0; i < uiObjects.Count; i++)
            {
                if (uiObjects[i] == null) continue;

                Type type = uiObjects[i].GetType();
                if (type == null)
                {
                    Debug.LogError("BuildUICode type error !");
                    return "";
                }

                string typeName = type.Name;
                string variableName = string.Format("go_{0}", uiObjects[i].name);
                variableName = variableName.Replace(' ', '_'); //命名有空格的情况
                variableName = variableName.Replace('(', '_'); //命名有括号
                variableName = variableName.Replace(')', '_'); //命名有括号
                //重名处理
                ++variableNum;
                if (variableNameDic.ContainsKey(variableName))
                {
                    variableName += variableNum;
                }

                variableNameDic.Add(variableName, uiObjects[i]);

                if (isMono)
                {
                    codeStateText.AppendFormat(CodeConfig.serilStateCodeFmt, typeName, variableName);
                }
                else
                {
                    codeStateText.AppendFormat(CodeConfig.stateCodeFmt, typeName, variableName);
                }
            }

            codeStateText.Append(CodeConfig.regionEnd);
            codeStateText.Append("public void InitUI(Transform trans) {transform = trans; this.InitUI();} ");
            //Debug.Log(codeStateText);
            return codeStateText.ToString();
        }

        /// <summary>
        /// 构建注册控件事件的代码
        /// </summary>
        /// <returns></returns>
        private string BuildEventCode()
        {
            codeEventText = null;
            codeEventText = new StringBuilder();

            codeEventText.Append(eventRegion);
            codeEventText.AppendFormat(methodStartFmt, "AddEvent");
            var codeEventCallbackList = new List<string>();
            bool hasEventWidget = false; //标识是否有控件注册了事件
            for (int i = 0; i < uiWidgets.Count; i++)
            {
                if (uiWidgets[i] == null) continue;

                //剔除不是事件或者是事件但未勾选toggle的控件
                string typeName = uiWidgets[i].GetType().Name;
                if (!selectedEventWidgets.ContainsKey(typeName) || !selectedEventWidgets[typeName])
                {
                    continue;
                }

                foreach (var vName in variableNameDic.Keys)
                {
                    if (uiWidgets[i].Equals(variableNameDic[vName]))
                    {
                        string variableName = vName;
                        if (!string.IsNullOrEmpty(variableName))
                        {
                            string methodName = variableName.Substring(variableName.IndexOf('_') + 1);
                            if (uiWidgets[i] is Button)
                            {
                                codeEventText.AppendFormat(onClickSerilCode, variableName, methodName);
                                //codeEventText.AppendFormat(btnCallbackSerilCode, methodName);
                                codeEventCallbackList.Add(string.Format(btnCallbackSerilCode, methodName));
                                hasEventWidget = true;
                            }
                            else
                            {
                                string addEventStr = string.Format(onValueChangeSerilCode, variableName, methodName);
                                if (hasEventWidget)
                                {
                                    codeEventText.Insert(codeEventText.ToString().LastIndexOf(';') + 1, addEventStr);
                                }
                                else
                                {
                                    codeEventText.Append(addEventStr);
                                }

                                string paramType = "";
                                foreach (string widgetType in CodeConfig.eventCBParamDic.Keys)
                                {
                                    if (typeName == widgetType)
                                    {
                                        paramType = CodeConfig.eventCBParamDic[widgetType];
                                        break;
                                    }
                                }

                                if (!string.IsNullOrEmpty(paramType))
                                {
                                    //codeEventText.AppendFormat(eventCallbackSerilCode, methodName, paramType);
                                    codeEventCallbackList.Add(string.Format(eventCallbackSerilCode, methodName,
                                        paramType));
                                }

                                hasEventWidget = true;
                            }
                        }

                        break;
                    }
                }
            }

            string codeStr = codeEventText.ToString();
            if (hasEventWidget)
            {
                codeEventText.Insert(codeStr.LastIndexOf(';') + 1, methodEnd);
            }
            else
            {
                codeEventText.Append(methodEnd);
            }

            foreach (var item in codeEventCallbackList)
            {
                codeEventText.Append(item);
            }

            codeEventCallbackList.Clear();
            codeEventText.Append(regionEnd);
            return codeEventText.ToString();
        }

        #endregion
    }
}