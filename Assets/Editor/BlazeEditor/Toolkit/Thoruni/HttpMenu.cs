using Blaze.Helper;
using UnityEditor;

namespace Toolkit.Game.Thoruni
{
    public class HttpMenu
    {
        private const string MENU_PATH = "Tools/Thor/";
        
        [MenuItem(MENU_PATH + "Http Restart")]
        public static void MenuHttpRestart()
        {
            HttpBackgroundSupport.Restart();
        }
    }
}