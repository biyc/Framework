using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace ETModel
{
    public static class NetHelper
    {
        public static string[] GetAddressIPs()
        {
            //获取本地的IP地址
            List<string> addressIPs = new List<string>();
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (address.AddressFamily.ToString() == "InterNetwork")
                {
                    addressIPs.Add(address.ToString());
                }
            }

            return addressIPs.ToArray();
        }


        //当前网络不可用
        public static bool NetworkNotReachable()
        {
            return Application.internetReachability == NetworkReachability.NotReachable;
        }

        //当前网络是wifi
        public static bool NetworkIsWifi()
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }
}