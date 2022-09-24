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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) /*|| Input.GetKeyDown(KeyCode.Home)*/)
        {
            Quit();
        }
    }

    public void Quit()
    {
        //测试时不能执行，打包后可以执行
        Application.Quit();
    }
}
