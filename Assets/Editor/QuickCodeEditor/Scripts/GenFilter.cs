using System.Collections.Generic;

namespace Quick.Code
{
    public class GenFilter
    {
        // 白名单
        HashSet<string> whiteList =new HashSet<string>();
        // 黑名单
        HashSet<string> blackList =new HashSet<string>();

        // 初始化名单
        public void Init()
        {
            whiteList.Add("Assets/Projects/UI/Dress/UIDress.prefab");
            blackList.Add("Assets/Projects/UI/Dress/");
        }

        public bool IsGen(string path)
        {
            // 文件在白名单中
            if(whiteList.Contains(path)) return true;

            foreach (var s in blackList)
            {
                // 路径中包含黑名单过滤物品
                if (path.Contains(s))
                {
                    return false;
                }
            }

            return true;
        }
    }
}