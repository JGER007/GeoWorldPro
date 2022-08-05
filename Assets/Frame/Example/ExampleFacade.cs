using System.Collections;
using System.Collections.Generic;
using com.frame;
using UnityEngine;

public class ExampleFacade : Facade
{
    protected override void initManagers ()
    {
        base.initManagers ();
        MsgManager.ShowMsgContent ("测试测试测试测试");
        EventUtil.DispatchEvent (GlobalEvent.Open_Module, "example");
    }
}
