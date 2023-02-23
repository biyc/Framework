// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Sirenix.Utilities;
// using Sirenix.Utilities.Editor;
// using UnityEngine;
// using UnityEditor;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
//
// [InitializeOnLoad]
// public static class UISelet
// {
//     static UISelet()
//     {
//         SceneView.duringSceneGui += OnSceneGui;
//         // Debug.LogError("dd");
//         // EditorApplication.update += () =>
//         // {
//         //     Debug.LogError("dd");
//         // };
//     }
//
//     private static void OnSceneGui(SceneView view)
//     {
//         var e = Event.current;
//         if (e != null && e.button == 1 && e.type == EventType.MouseUp)
//         {
//             if (Selection.activeTransform == null) return;
//
//             // 当前屏幕坐标，左上角是（0，0）右下角（camera.pixelWidth，camera.pixelHeight）
//             Vector2 mousePosition = Event.current.mousePosition;
//             // Retina 屏幕需要拉伸值
//             float mult = EditorGUIUtility.pixelsPerPoint;
//             // 转换成摄像机可接受的屏幕坐标，左下角是（0，0，0）右上角是（camera.pixelWidth，camera.pixelHeight，0）
//             mousePosition.y = view.camera.pixelHeight - mousePosition.y * mult;
//             mousePosition.x *= mult;
//
//             var parentRts = Selection.activeTransform.GetComponentsInParent<RectTransform>();
//             var canvas = parentRts[parentRts.Length - 1];
//             var rts = canvas.GetComponentsInChildren<RectTransform>().Where(x => x.gameObject.activeInHierarchy).Where(
//                 x =>
//                     RectTransformUtility.RectangleContainsScreenPoint(x, mousePosition, view.camera));
//             var gc = new GenericMenu();
//             rts = rts.Reverse();
//             rts.ForEach(x =>
//             {
//                 if (x.name.Contains("Canvas") || x.name.Contains("Chapter")) return;
//                 var isTxt = x.TryGetComponent<Text>(out Text txtComp);
//                 var content = x.name + " " + (isTxt ? txtComp.text : "");
//                 var imageComp = x.GetComponent<Image>();
//
//                 gc.AddItem(new GUIContent(content), false, () =>
//                 {
//                     Selection.activeTransform = x;
//                     EditorGUIUtility.PingObject(x);
//                 });
//             });
//             e.Use();
//             gc.ShowAsContext();
//         }
//     }
// }