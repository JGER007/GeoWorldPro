using System.Collections;
using System.Collections.Generic;
using com.frame;
using UnityEngine;
using UnityEngine.UI;

public class ExampleMainUI : ModuleUI
{
    public override void InitUI ()
    {
        base.InitUI ();
        Button rotateBtn = transform.Find ("RotateBtn").GetComponent<Button> ();
        rotateBtn.onClick.AddListener (delegate
        {
            Debug.Log ("ExampleMainUI rotateBtn.onClick");
            EventUtil.DispatchEvent (GlobalEvent.UI_TO_Module_Action, "Rotate");
        });
    }
}
