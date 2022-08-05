using System.Collections;
using System.Collections.Generic;
using com.frame;
using UnityEngine;

public class ExampleModuleFacade : BaseModuleFacade
{
    private bool rotateFlag = false;
    public override void InitModuleFacade ()
    {
        base.InitModuleFacade ();
        rotateFlag = true;
        setRotateState ();
    }

    private void setRotateState ()
    {
        AutoRotate [] rotates = GetComponentsInChildren<AutoRotate> ();
        for (int i = 0; i < rotates.Length; i++)
        {
            rotates [i].enabled = rotateFlag;
        }
    }


    protected override void onUIToModuleAtion (CustomEventArgs eventArgs)
    {
        string action = eventArgs.args [0].ToString ();
        Debug.Log ("onUIToModuleAtion action:" + action);
        if (action == "Rotate")
        {
            rotateFlag = !rotateFlag;
            setRotateState ();
        }
    }

    public override void OnQuit ()
    {
        base.OnQuit ();
    }
}
