using System.IO;
using Blaze.Common;
using Blaze.Manage.Safety;
using UnityEditor;

namespace Toolkit.Game.Thoruni
{
    public class NetMenu
    {
        private const string MENU_PATH = "Tools/Thor/";

        [MenuItem(MENU_PATH + "---------------:Net Connection")]
        public static void Void3()
        {
        }

        [MenuItem(MENU_PATH + "Local Net Server")]
        public static void MenuLocalServer()
        {
            // var conf = Config._.App;
            // conf.NetConnection = conf.NetConnection.Replace("ngrok.mt3ex.com", "localhost");
            // conf.Save();
        }

        [MenuItem(MENU_PATH + "Online Net Server")]
        public static void MenuOnline()
        {
            // var conf = Config._.App;
            // conf.NetConnection = conf.NetConnection.Replace("localhost", "ngrok.mt3ex.com");
            // conf.Save();
        }

        [MenuItem(MENU_PATH + "生成保护文件")]
        public static void GenKey()
        {
            var dat =new SafetyStartData();
            dat.DeadlineTime = 1669305600000;
            dat.IsOpen = true;

            var path = "Start.conf";
            if (!File.Exists(path))
                File.Create(path).Dispose();
            File.WriteAllText(path, dat.OutConfig());
        }
    }
}