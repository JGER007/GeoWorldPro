using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleModuleManager : BaseModuleManager
{
    public ExampleModuleFacade exampleModuleFacade;



    /// <summary>
    /// 模块准备完成
    /// </summary>
    /// <param name="flag">设备支持AR功能标识</param>
    /// <param name="target"></param>
    public override void OnModuleReady (GameObject target)
    {
        base.OnModuleReady (target);

        //初始化放置目标对象
        if (sceneTarget == null)
        {
            sceneTarget = initSceneTarget (target);
            Debug.Log ("OnModuleReady sceneTarget:" + sceneTarget);
            if (sceneTarget)
            {
                exampleModuleFacade = sceneTarget.GetComponent<ExampleModuleFacade> ();
                exampleModuleFacade.InitModuleFacade ();
                initModuleUI ();
            }

        }
    }

    /// <summary>
    /// 初始化模块UI
    /// </summary>
    private void initModuleUI ()
    {
        if (moduleUIManager == null)
        {
            moduleUIManager = new ExampleMainUIManager ();
            moduleUIManager.InitManager ();
        }
    }

    //退出模块场景
    protected override void quitModuleManager ()
    {
        if (moduleUIManager == null)
        {
            moduleUIManager.OnQuit ();
        }
    }
}
