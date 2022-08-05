namespace com.frame
{
    public class GlobalEvent
    {
        /// <summary>完成配置信息初始化</summary>
        public static string Init_Config_Complete = "Init_Config_Complete";
        /// <summary>返回主场景</summary>
        public static string Back_To_Main_Scene = "Back_To_Main_Scene";
        /// <summary>弹出UI界面</summary>
        public static string On_Popup_UI = "On_Popup_UI";
        /// <summary>关闭弹出UI界面</summary>
        public static string Close_Popup_UI = "Close_Popup_UI";
        /// <summary>引藏UI界面</summary>
        public static string Hide_Guide_UI = "Hide_Guide_UI";
        /// <summary>打开UI界面</summary>
        public static string Open_UI = "Open_UI";
        /// <summary>关闭UI界面</summary>
        public static string Close_UI = "Close_UI";
        /// <summary>打开AR模块</summary>
        public static string Open_Module = "Open_Module";
        /// <summary>关闭AR模块</summary>
        public static string Quit_Module = "Quit_Module";
        ///<summary>展示加载进度(参数两个时开始显示过渡条，参数一个时关闭过渡条)</summary>
        public static string Show_Loading_Progress = "Show_Loading_Progress";
        /// <summary>开支准备素材资源</summary>
        public static string Start_To_Ready_Module_Resource = "Start_To_Ready_Module_Resource";
        /// <summary>模块资源准备完毕</summary>
        public static string Module_Resource_Is_Ready = "Module_Resource_Is_Ready";
        /// <summary>关闭UI界面</summary>
        public static string Show_Msg = "Show_Msg";

        //UI界面操作调用场景模块
        public static string UI_TO_Module_Action = "UI_TO_Module_Action";
        //场景模块调用UI界面
        public static string Module_TO_UI_Action = "Module_TO_UI_Action";

    }
}