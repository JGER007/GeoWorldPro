using System.Collections;
using System.Collections.Generic;
using com.frame;
using com.frame.ui;
using UnityEngine;

public class Facade : MonoBehaviour
{
    //管理类入口
    private List<IManager> managers;
    [Header ("是否用本地Assetbundle资源测试")]
    public bool isLocalResourcesTest = false;
    //是否用本地Assetbundle资源测试
    public static bool isLocalResources = false;

    // Start is called before the first frame update
    void Start ()
    {
        isLocalResources = isLocalResourcesTest;

        //配置文件初始化
        if (!Model.IsConfigInitComplete ())
        {
            AssetManager.Instance.InitManager ();
            EventUtil.AddListener (GlobalEvent.Init_Config_Complete, onConfigInitComplete);
            ConfigManager.Instance.InitManager ();
        }
        else
        {
            initApp ();
        }
    }

    /// <summary> 配置文件初始化完成 </summary>
    private void onConfigInitComplete (CustomEventArgs eventArgs)
    {
        EventUtil.RemoveListener (GlobalEvent.Init_Config_Complete, onConfigInitComplete);
        initApp ();
    }

    /// <summary> App初始化 </summary>
    protected virtual void initApp ()
    {
        EventUtil.AddListener (GlobalEvent.Back_To_Main_Scene, onBackMainScene);//返回主场景事件
        initManagers ();//初始化管理类
    }

    /// <summary>初始化管理类</summary>
    protected virtual void initManagers ()
    {
        managers = new List<IManager> ();
        managers.Add (UIManager.Instance);//UI管理类 UIRoot.Instance.InitUIRoot ();初始化UI容器      
        managers.Add (PopupUIManager.Instance);//弹出界面管理类            
        managers.Add (new ModuleControlManager ());//AR管理类

        foreach (IManager manager in managers)
        {
            manager.InitManager ();
        }
    }

    /// <summary>退出管理类</summary>
    protected virtual void quitManagers ()
    {
        foreach (IManager manager in managers)
        {
            manager.OnQuit ();
        }
        managers.Clear ();
        AssetManager.Instance.OnQuit ();
    }


    /// <summary>返回主场景(暂时没用到) </summary>
    protected virtual void onBackMainScene (CustomEventArgs eventArgs)
    {
        EventUtil.RemoveListener (GlobalEvent.Back_To_Main_Scene, onBackMainScene);
        quitManagers ();
    }


    void OnDisable ()
    {
        EventUtil.RemoveListener (GlobalEvent.Init_Config_Complete, onConfigInitComplete);
        EventUtil.RemoveListener (GlobalEvent.Back_To_Main_Scene, onBackMainScene);
    }
}
