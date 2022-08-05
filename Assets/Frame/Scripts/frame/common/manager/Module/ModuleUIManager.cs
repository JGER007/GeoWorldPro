using com.frame;
using com.frame.ui;
using System;
using UnityEngine;

public class ModuleUIManager : IManager
{
    public GameObject ModuleUI = null;
    public virtual void InitManager (Transform container = null)
    {
    }

    /// <summary>初始化模块UI界面</summary>
    /// <param name="moduleName"></param>
    protected virtual void InitModuleUI (string moduleName)
    {
        Action<GameObject> backCall = (ui) =>
        {
            ModuleUI = ui;
            InitInfo ();
        };
        if (ModuleUI == null)
        {
            UIManager.Instance.LoadConfigUI (moduleName, backCall, true);
        }
        else
        {
            InitInfo ();
        }

        EventUtil.AddListener (GlobalEvent.Module_TO_UI_Action, onModuleToUI);
    }

    protected virtual void onModuleToUI (CustomEventArgs eventArgs)
    {

    }

    protected virtual void InitInfo ()
    {
    }

    public virtual void OnQuit ()
    {
        if (ModuleUI != null)
        {
            if (ModuleUI.activeSelf)
            {
                ModuleUI.SetActive (false);
            }
            //销毁模块ui
            GlobalMonoTool.DestroyImmediate (ModuleUI);
        }
        EventUtil.RemoveListener (GlobalEvent.Module_TO_UI_Action, onModuleToUI);
    }
}
