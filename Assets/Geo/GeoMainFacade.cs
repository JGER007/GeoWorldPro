using com.frame;
using com.frame.ui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoMainFacade : Facade
{
    protected override void initManagers()
    {
        base.initManagers();
        EventUtil.AddListener(GlobalEvent.Tiles_Unzip_Finish, onUnzipTilesFinish);
        EventUtil.DispatchEvent(GlobalEvent.On_Popup_UI, UIManager.TileLoadAndUnzipUI );
        //EventUtil.DispatchEvent(GlobalEvent.Open_Module, "geomap");
    }

    private void onUnzipTilesFinish(CustomEventArgs eventArgs)
    {
        EventUtil.DispatchEvent(GlobalEvent.Close_Popup_UI, UIManager.TileLoadAndUnzipUI);
        EventUtil.DispatchEvent(GlobalEvent.Open_Module, "geomap");
        EventUtil.RemoveListener(GlobalEvent.Tiles_Unzip_Finish, onUnzipTilesFinish);
    }
}
