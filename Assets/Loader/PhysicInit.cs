public class PhysicInit : ETModel.Init
{
    protected override void LoadHotfix()
    {
        ETModel.Game.Hotfix?.Dispose();
        ETModel.Game.Hotfix = new ETModel.Hotfix();
        // 通过全量代码启动 APP
        ETHotfix.Init.Start();
    }
}