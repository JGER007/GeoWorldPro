using com.frame;


public class Model : Singleton<Model>
{
    /// <summary> 配置初始化标识 false 未完成初始化  true 完成初始化 </summary>
    private static bool configFlag = false;


    /// <summary>完成初始化配置</summary>
    public static void InitConfigOK ()
    {
        configFlag = true;
        EventUtil.DispatchEvent (GlobalEvent.Init_Config_Complete);
    }

    /// <summary> 配置文件是否初始化完成 </summary>
    public static bool IsConfigInitComplete ()
    {
        return configFlag;
    }
}
