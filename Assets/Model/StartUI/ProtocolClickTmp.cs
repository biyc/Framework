using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Model.StartUI
{
    // 隐私协议中的跳转连接
    public class ProtocolClickTmp : MonoBehaviour, IPointerClickHandler
    {
        public TextMeshProUGUI text;

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 pos = new Vector3(eventData.position.x, eventData.position.y, 0);
            int linkIndex =
                TMP_TextUtilities.FindIntersectingLink(text, pos,
                    GameObject.Find("UICamera").GetComponent<Camera>()); //--UI相机
            if (linkIndex > -1)
            {
                TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
                Debug.Log(linkInfo.GetLinkText());
                Debug.Log(linkInfo.GetLinkID());


                string page = null;
                switch (linkInfo.GetLinkID())
                {
                    // case "page1":
                    //     // 隐私协议
                    //     page = "https://sy.goodbaike.com/a29.html";
                    //     break;
                    // case "page2":
                    //     // 良心游戏用户服务协议：
                    //     page = "https://sy.goodbaike.com/a30.html";
                    //     break;
                    case "page1":
                        // 隐私协议
                        page = "https://sy.goodbaike.com/a1.html";
                        break;
                    case "page2":
                        // 良心游戏用户服务协议：
                        page = "https://sy.goodbaike.com/a2.html";
                        break;
                }

                if (page != null)
                    Application.OpenURL(page);
            }
        }
    }
}