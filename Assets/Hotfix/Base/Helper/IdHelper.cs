namespace ETHotfix
{
    public class IdHelper
    {
        private static readonly ulong Base = 1;

        public static readonly ulong Non = 0; //没有类型

        /// <summary>
        /// 根据id获取属于的类型
        /// </summary>
        /// <returns></returns>
        public static ulong GetTypeById(int id) {
            ulong type = Non;

            return type;
        }

        /// <summary>
        /// 判断typeTable中是否包含value
        /// </summary>
        /// <param name="typeTable"></param>
        /// <param name="value"></param>
        /// <param name="entirely">true: 完全包含才返回true; false: 部分包含即可返回true</param>
        /// <returns>true: 包含; false: 不包含</returns>
        public static bool ContainType(ulong typeTable, ulong value, bool entirely = true) {
            ulong v = typeTable & value;
            if (v == 0) {
                return false;
            }

            if (entirely) {
                if (v == value) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }

        /// <summary>
        /// 判断所给id是否属于所给类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTypeById(int id, ulong type) {
            return ContainType(type, GetTypeById(id));
        }

        /// <summary>
        /// 合并id
        /// </summary>
        /// <returns></returns>
        public static ulong CoalesceId(params ulong[] ids) {
            ulong r = 0;
            foreach (ulong id in ids) {
                r |= id;
            }

            return r;
        }

        /// <summary>
        /// 根据id获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CargoInfo GetCargoInfoById(int id) {
            CargoInfo cargoInfo = new CargoInfo();

            return cargoInfo;
        }
    }

    public class CargoInfo
    {
        public int id = 0;
        public string name = null;
        public string thumbnailImgPath = null;

        public void SetData(int id, string name, string thumbnailImgPath) {
            this.id = id;
            this.name = name;
            this.thumbnailImgPath = thumbnailImgPath;
        }

        public CargoInfo() {
        }

        public CargoInfo(int id, float value, string name, string thumbnailImgPath) {
            SetData(id, name, thumbnailImgPath);
        }
    }
}