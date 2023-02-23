using System.Text;
using Blaze.Helper;
using Blaze.Utility;
using Blaze.Utility.Helper;
using Plugin.Protobuf;
using UnityEditor;
using UnityEngine;

namespace Toolkit.Game.Thoruni
{
    public class HttpMenu
    {
        private const string MENU_PATH = "Tools/Thor/";

        [MenuItem(MENU_PATH + "---------------:Http")]
        public static void Void3()
        {
        }

        [MenuItem(MENU_PATH + "Http Restart")]
        public static void MenuHttpRestart()
        {
            HttpBackgroundSupport.Restart();
        }


    }
}