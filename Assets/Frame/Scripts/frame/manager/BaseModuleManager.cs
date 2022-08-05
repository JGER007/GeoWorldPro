//***************************************************
// Des：模块场景基础管理类
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2020/12/28 17:00:14
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using com.frame;
using com.frame.ui;
using UnityEngine;

public class BaseModuleManager : IManager
{
    protected GameObject arOrigin;
    protected GameObject unAROrigin;
    protected Transform sceneContainer;
    protected GameObject sceneTarget;

    protected bool supportARFlag = false;
    protected bool initFlag = false;

    //模块UI管理
    protected ModuleUIManager moduleUIManager = null;

    /// <summary> 开始模块</summary>
    /// <param name="curPrefab"></param>
    public virtual void StartModule (GameObject curPrefab)
    {
        if (curPrefab != null)
        {
            prefab = curPrefab;
            EventUtil.AddListener (GlobalEvent.Start_To_Ready_Module_Resource, OpenModule);
            EventUtil.DispatchEvent (GlobalEvent.Close_Popup_UI, UIManager.LoadFromLocalUI);
            EventUtil.DispatchEvent (GlobalEvent.On_Popup_UI, new object [] { UIManager.ModuleTranstionUI, 1 });
        }
    }
    private GameObject prefab;
    /// <summary> 打开模块</summary>
    private void OpenModule (CustomEventArgs eventArgs)
    {
        EventUtil.RemoveListener (GlobalEvent.Start_To_Ready_Module_Resource, OpenModule);
        InitManager (SceneRoot.Instance.GetSceneRoot ());
        OnModuleReady (prefab);
    }

    public virtual void InitManager (Transform container)
    {
        //设置放置三维目标容器
        sceneContainer = container;
        initModuleManager ();
        EventUtil.AddListener (GlobalEvent.Module_Resource_Is_Ready, OnModuleReourceReadyOK);
    }

    /// <summary>初始化模块场景 </summary>
    protected virtual void initModuleManager ()
    {
    }

    /// <summary> 退出 </summary>
    public virtual void OnQuit ()
    {
        if (moduleUIManager != null)
        {
            moduleUIManager.OnQuit ();
            moduleUIManager = null;
        }

        quitModuleManager ();
        if (sceneTarget)
        {
            GlobalMonoTool.DestroyImmediate (sceneTarget);
            sceneTarget = null;
        }
        EventUtil.RemoveListener (GlobalEvent.Module_Resource_Is_Ready, OnModuleReourceReadyOK);
    }

    /// <summary>退出模块场景 </summary>
    protected virtual void quitModuleManager ()
    {
    }

    /// <summary> 模块资源准备完毕(黑幕过渡UI初始化时调用) </summary>
    public virtual void OnModuleReady (GameObject perfab)
    {

    }

    /// <summary> 初始化场景目标对象 </summary>
    protected virtual GameObject initSceneTarget (GameObject perfab)
    {
        GameObject obj = GameObjectUtil.InstantiateObj (perfab, sceneContainer);
        obj.name = perfab.name;
        if (sceneTarget)
        {
            sceneTarget.SetActive (true);
            sceneTarget.transform.SetParent (sceneContainer);
            sceneTarget.transform.localScale = Vector3.one;
            sceneTarget.transform.position = Vector3.zero;
            sceneTarget.transform.localRotation = Quaternion.identity;
        }

        return obj;
    }

    /// <summary> 模块资源准备完毕(黑幕过渡UI关闭时调用) </summary>
    public virtual void OnModuleReourceReadyOK (CustomEventArgs eventArgs)
    {
        moduleReourceReadyOK ();
    }

    protected virtual void moduleReourceReadyOK ()
    {

    }

}
