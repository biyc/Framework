using System.Text;
using System.Threading;
using Blaze.Helper;
using Blaze.Utility;
using Blaze.Utility.Helper;
using UnityEditor;
using UnityEngine;

namespace Toolkit.Game.Thoruni
{
    public class CsvMenu
    {
        private const string MENU_PATH = "Tools/Thor/";

        [MenuItem(MENU_PATH + "---------------:DataCsv")]
        public static void Void3()
        {
        }

        [MenuItem(MENU_PATH + "Xlsx to csv")]
        public static void XlsxToCsv()
        {
            CsvSupport.XlsxToCsv();
        }

        [MenuItem(MENU_PATH + "Enable Auto Xlsx to csv")]
        public static void EnableAutoXlsxToCsv()
        {
            CsvSupport.EnableAutoXlsxToCsv();
        }

        [MenuItem(MENU_PATH + "Disable Auto Xlsx to csv")]
        public static void DisableAutoXlsxToCsv()
        {
            CsvSupport.DisableAutoXlsxToCsv();
        }


        [MenuItem(MENU_PATH + "Generate csv code")]
        public static void GenCsvCode()
        {
            CsvCodeGen.Gen();
        }


        [MenuItem(MENU_PATH + "Clean csv generate code")]
        public static void CleanCsvCode()
        {
            CsvCodeGen.Clean();
        }
    }
}