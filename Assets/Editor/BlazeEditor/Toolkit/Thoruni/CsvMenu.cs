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
        

        [MenuItem(MENU_PATH + "Xlsx to csv")]
        public static void XlsxToCsv()
        {
            CsvSupport.XlsxToCsv();
        }
        
        [MenuItem(MENU_PATH + "Generate csv code")]
        public static void GenCsvCode()
        {
            CsvCodeGen.Gen();
        }
        
    }
}