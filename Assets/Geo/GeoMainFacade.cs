using com.frame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoMainFacade : Facade
{
    protected override void initManagers()
    {
        base.initManagers();
        EventUtil.DispatchEvent(GlobalEvent.Open_Module, "geomap");
    }
}
