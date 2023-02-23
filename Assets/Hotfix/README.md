Hotfix 下的代码会生成 Hotfix.DLL 运行时加载到 ILRuntime 中运行

. 游戏逻辑开发根
├── Base  基础开发框架（一般不动）
│   ├── Entity
│   ├── Event
│   ├── Helper
│   ├── Module        通用框架管理器
│   │   ├── Blaze
│   │   │   ├── Common
│   │   │   ├── Core
│   │   │   ├── Csv     CSV 基础依赖
│   │   │   │   ├── Center
│   │   │   │   └── Poco
│   │   │   ├── Data    DataWatch 模块
│   │   │   ├── Locale   多语言模块
│   │   │   │   ├── Data
│   │   │   │   ├── Enum
│   │   │   │   └── Poco
│   │   │   ├── Spring   自动扫描加载对象模块
│   │   │   └── Stage    UI 管理器（只作为代码参考，此项目中不使用）
│   │   │       ├── Core
│   │   │       └── Ugui
│   │   ├── Config
│   │   └── UI          UI 管理器
│   │       └── Stale
│   └── Object
├── CacheGameData    茜木项目中的公共数据与逻辑模块（不推荐继续在此文件夹下放各种逻辑）
├── Game
│   ├── Chat         手机聊天逻辑
│   │   ├── Data   本模块需要使用的数据结构
│   │   ├── Enum   生成器生成的 本模块需要使用的枚举代码
│   │   ├── Logic  本模块逻辑代码放到此文件夹下
│   │   │   ├── Direct
│   │   │   ├── Photo
│   │   │   ├── Program
│   │   │   └── Roles
│   │   ├── Poco     生成器生成的 本模块使用的CSV结构与读取调用（只能生成，不能手动修改！！！）
│   │   └── Slots    存放本模块的存档结构与调用引用句柄
│   └── Common         游戏公共逻辑模块（设置，基础数值等）
│       ├── Action
│       ├── Data
│       ├── Define
│       ├── Enum
│       ├── Logic
│       ├── Poco
│       ├── Proto
│       └── Slots
├── Properties
├── Tools               原项目中存放静态工具的地方，不推荐使用
└── UI
    ├── Component       UI 界面的引用实例
    │   ├── Chapter     存放落樱2的页面章节实例
    │   │   ├── Chapter0
    │   │   └── Chapter1
    │   │       └── SelfIntroduce
    │   ├── UIChat
    │   ├── UICommon
    │   └── UIVipAndRecharge
    ├── UIBind          通过工具全量生成的UI绑定，只靠生成工具维护，不要手动创建！！！
        ├── Chapter
        │   ├── Chapter0
        │   ├── Chapter1
        │   └── Core
        ├── UIChat
        ├── UICommon
        └── VipAndCharge