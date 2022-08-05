using System;
using System.Collections;
using System.Collections.Generic;
using com.frame;
using UnityEngine;

public class BaseModuleFacade : MonoBehaviour
{
    public Action<string, object []> ModuleToUIAction;

    public virtual void InitModuleFacade ()
    {
        //Debug.Log ("BaseModuleFacade InitModuleFacade");
        EventUtil.AddListener (GlobalEvent.UI_TO_Module_Action, onUIToModuleAtion);
    }

    protected virtual void onUIToModuleAtion (CustomEventArgs eventArgs)
    {

    }

    public virtual void OnQuit ()
    {
        EventUtil.RemoveListener (GlobalEvent.UI_TO_Module_Action, onUIToModuleAtion);
    }
}
