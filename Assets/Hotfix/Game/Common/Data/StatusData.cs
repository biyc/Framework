namespace Hotfix.Game.Common.Data
{
    public class StatusData
    {
        // 错误码
        public int Code;

        // 错误信息描述
        public string Desc;

        // 资源类型数量（只在资源不足错误中使用）
        public long ResType;
        public long ResNum;


        public static StatusData DressSuccess = Create(1000, "小屋装扮完成");
        public static StatusData SuccessBuy = Create(2, "购买成功！");
        public static StatusData Success = Create(1, "成功");

        public static StatusData Fail = Create(-1, "失败");
        public static StatusData ResNotEnough = Create(-2, "资源不足");
        public static StatusData ResNotEnoughHb = Create(-20, "花瓣不足");
        public static StatusData ResNotEnoughFurniture = Create(-21, "家具币不足");
        public static StatusData IsHave = Create(-3, "已拥有该物品");
        public static StatusData ItemExist = Create(-4, "道具不存在");

        public override string ToString()
        {
            return Code + " : " + Desc + "  type:" + ResType + "  num:" + ResNum;
        }

        public bool IsSuccess()
        {
            return Code >= 0;
        }

        public bool Equals(StatusData data)
        {
            return data.Code == this.Code;
        }

        public static StatusData Create(int code)
        {
            var data = new StatusData();
            data.Code = code;
            data.Desc = "";
            return data;
        }

        public static StatusData Create(int code, string desc)
        {
            var data = new StatusData();
            data.Code = code;
            data.Desc = desc;
            return data;
        }
    }
}