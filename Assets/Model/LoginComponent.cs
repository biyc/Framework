using System.Collections.Generic;
using Blaze.Core;
using Blaze.Manage.Data;
using Blaze.Resource.AssetBundles;

namespace Model
{
    /// <summary>
    /// 核心装配文件
    /// </summary>
    [Named("AccountInfo")]
    [Persist(CamelFileName = true)]
    public class AccountInfo : Persistable<AccountInfo>
    {
        public Dictionary<int, string> AccountDic = new Dictionary<int, string>();
    }

    public class LoginComponent : Singeton<LoginComponent>
    {
        public ICompleted<int> WatchLogin = new DataWatch<int>();

        public void Login(int account, string pass)
        {
            if (account == -1)
                WatchLogin.Complet(account);

            var accountInfo = AccountInfo.Load(BundleHotfix._.ResBasePath);
            if (accountInfo == null)
            {
                accountInfo = new AccountInfo();
                accountInfo.Config(BundleHotfix._.ResBasePath);
                accountInfo.Save();
            }

            if (!accountInfo.AccountDic.ContainsKey(account) || accountInfo.AccountDic[account] != pass) return;
            WatchLogin.Complet(account);
        }
    }
}