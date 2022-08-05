using System;
using System.Collections;
using System.Collections.Generic;
using com.frame;
using UnityEngine;

public class ExampleMainUIManager : ModuleUIManager
{
    private ExampleMainUI exampleMainUI = null;
    public override void InitManager (Transform container)
    {
        if (exampleMainUI == null)
        {
            InitModuleUI ("ExampleMainUI");
        }
    }

    protected override void InitInfo ()
    {
        exampleMainUI = ModuleUI.GetComponent<ExampleMainUI> ();
        exampleMainUI.InitUI ();


    }

    protected override void onModuleToUI (CustomEventArgs eventArgs)
    {

    }

    public override void OnQuit ()
    {
        base.OnQuit ();
        if (exampleMainUI != null)
        {
            exampleMainUI = null;
        }
    }
}
