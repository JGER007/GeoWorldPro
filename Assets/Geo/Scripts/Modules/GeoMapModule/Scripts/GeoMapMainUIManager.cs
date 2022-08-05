using com.frame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoMapMainUIManager : ModuleUIManager
{
    private GeoMapMainUI geoMapMainUI = null;
    public override void InitManager(Transform container)
    {
        if (geoMapMainUI == null)
        {
            InitModuleUI("GeoMapMainUI");
        }
    }

    protected override void InitInfo()
    {
        geoMapMainUI = ModuleUI.GetComponent<GeoMapMainUI>();
        geoMapMainUI.InitUI();
    }

    protected override void onModuleToUI(CustomEventArgs eventArgs)
    {

    }

    public override void OnQuit()
    {
        base.OnQuit();
        if (geoMapMainUI != null)
        {
            geoMapMainUI = null;
        }
    }
}
