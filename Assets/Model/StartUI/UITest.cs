using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class UITest : MonoBehaviour
    {
        #region UI Variable Statement

        // public Transform transform;
        public Image image_Gift2Button;
        public Button button_Gift2Button;
        public Text text_Text;
        public Image image_Gift2Button1;
        public Button button_Gift2Button1;
        public Text text_Text6;
        public Image image_Gift2Button2;
        public Button button_Gift2Button2;
        public Text text_Text9;
        public Image image_Gift2Button3;
        public Button button_Gift2Button3;
        public Text text_Text12;
        public Image image_Gift2Button4;
        public Button button_Gift2Button4;
        public Text text_Text15;
        public Image image_Gift2Button5;
        public Button button_Gift2Button5;
        public Text text_Text18;
        public Image image_Gift2Button6;
        public Button button_Gift2Button6;
        public Text text_Text21;
        public Image image_Gift2Button7;
        public Button button_Gift2Button7;
        public Text text_Text24;
        public Image image_Gift2Button8;
        public Button button_Gift2Button8;
        public Text text_Text27;
        public Image image_Gift2Button9;
        public Button button_Gift2Button9;
        public Text text_Text30;

        #endregion


        #region UI Variable Assignment

        public void InitUI()
        {
            //assign transform by your ui framework
            // transform = Ga;
            image_Gift2Button = transform.Find("Gift2Button").GetComponent<Image>();
            button_Gift2Button = transform.Find("Gift2Button").GetComponent<Button>();
            text_Text = transform.Find("Gift2Button/Text").GetComponent<Text>();
            image_Gift2Button1 = transform.Find("Gift2Button1").GetComponent<Image>();
            button_Gift2Button1 = transform.Find("Gift2Button1").GetComponent<Button>();
            text_Text6 = transform.Find("Gift2Button1/Text").GetComponent<Text>();
            image_Gift2Button2 = transform.Find("Gift2Button2").GetComponent<Image>();
            button_Gift2Button2 = transform.Find("Gift2Button2").GetComponent<Button>();
            text_Text9 = transform.Find("Gift2Button2/Text").GetComponent<Text>();
            image_Gift2Button3 = transform.Find("Gift2Button3").GetComponent<Image>();
            button_Gift2Button3 = transform.Find("Gift2Button3").GetComponent<Button>();
            text_Text12 = transform.Find("Gift2Button3/Text").GetComponent<Text>();
            image_Gift2Button4 = transform.Find("Gift2Button4").GetComponent<Image>();
            button_Gift2Button4 = transform.Find("Gift2Button4").GetComponent<Button>();
            text_Text15 = transform.Find("Gift2Button4/Text").GetComponent<Text>();
            image_Gift2Button5 = transform.Find("Gift2Button5").GetComponent<Image>();
            button_Gift2Button5 = transform.Find("Gift2Button5").GetComponent<Button>();
            text_Text18 = transform.Find("Gift2Button5/Text").GetComponent<Text>();
            image_Gift2Button6 = transform.Find("Gift2Button6").GetComponent<Image>();
            button_Gift2Button6 = transform.Find("Gift2Button6").GetComponent<Button>();
            text_Text21 = transform.Find("Gift2Button6/Text").GetComponent<Text>();
            image_Gift2Button7 = transform.Find("Gift2Button7").GetComponent<Image>();
            button_Gift2Button7 = transform.Find("Gift2Button7").GetComponent<Button>();
            text_Text24 = transform.Find("Gift2Button7/Text").GetComponent<Text>();
            image_Gift2Button8 = transform.Find("Gift2Button8").GetComponent<Image>();
            button_Gift2Button8 = transform.Find("Gift2Button8").GetComponent<Button>();
            text_Text27 = transform.Find("Gift2Button8/Text").GetComponent<Text>();
            image_Gift2Button9 = transform.Find("Gift2Button9").GetComponent<Image>();
            button_Gift2Button9 = transform.Find("Gift2Button9").GetComponent<Button>();
            text_Text30 = transform.Find("Gift2Button9/Text").GetComponent<Text>();
        }

        public void AddEvent()
        {
            button_Gift2Button.onClick.AddListener(OnGift2ButtonClicked);
            button_Gift2Button1.onClick.AddListener(OnGift2Button1Clicked);
            button_Gift2Button2.onClick.AddListener(OnGift2Button2Clicked);
            button_Gift2Button3.onClick.AddListener(OnGift2Button3Clicked);
            button_Gift2Button4.onClick.AddListener(OnGift2Button4Clicked);
            button_Gift2Button5.onClick.AddListener(OnGift2Button5Clicked);
            button_Gift2Button6.onClick.AddListener(OnGift2Button6Clicked);
            button_Gift2Button7.onClick.AddListener(OnGift2Button7Clicked);
            button_Gift2Button8.onClick.AddListener(OnGift2Button8Clicked);
            button_Gift2Button9.onClick.AddListener(OnGift2Button9Clicked);
        }

        #endregion


        private void Start()
        {
            InitUI();
            AddEvent();
            button_Gift2Button.transform.Find("Text").GetComponent<Text>().text = "";
            button_Gift2Button1.transform.Find("Text").GetComponent<Text>().text = "";
            button_Gift2Button2.transform.Find("Text").GetComponent<Text>().text = "";
            button_Gift2Button3.transform.Find("Text").GetComponent<Text>().text = "";
            button_Gift2Button4.transform.Find("Text").GetComponent<Text>().text = "";
            button_Gift2Button5.transform.Find("Text").GetComponent<Text>().text = "";
            button_Gift2Button6.transform.Find("Text").GetComponent<Text>().text = "";
            button_Gift2Button7.transform.Find("Text").GetComponent<Text>().text = "";
            button_Gift2Button8.transform.Find("Text").GetComponent<Text>().text = "";
            button_Gift2Button9.transform.Find("Text").GetComponent<Text>().text = "";
        }


        private void OnGift2ButtonClicked()
        {
        }

        private void OnGift2Button1Clicked()
        {
        }

        private void OnGift2Button2Clicked()
        {
        }

        private void OnGift2Button3Clicked()
        {
        }

        private void OnGift2Button4Clicked()
        {
        }

        private void OnGift2Button5Clicked()
        {
        }

        private void OnGift2Button6Clicked()
        {
        }

        private void OnGift2Button7Clicked()
        {
        }

        private void OnGift2Button8Clicked()
        {
        }

        private void OnGift2Button9Clicked()
        {
        }
    }
}