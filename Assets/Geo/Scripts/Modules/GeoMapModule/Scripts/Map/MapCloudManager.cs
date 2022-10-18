using com.frame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class MapCloudManager : MonoBehaviour, IManager
{
    private Dictionary<string, ClouldVO> earthClouldDic = null;

    private WorldMapGlobe _worldMapGlobe = null;
    private ClouldVO currClouldVO;
    public WorldMapGlobe WorldMapGlobe { get => _worldMapGlobe; set => _worldMapGlobe = value; }

    private int opIndex = 0;
    private List<string> cloudNameList = null;
    public void InitManager(Transform container = null)
    {
        
    }

    public void ShowEarthCloud()
    {
        if (cloudNameList == null)
        {
            cloudNameList = AppConfigManager.Instance.CloudNameList;
        }
        opIndex = 0;
        string cloudName = cloudNameList[opIndex];
        show(cloudName);
    }

    private void show(string cloudName)
    {
        ClouldVO clouldVO = AppConfigManager.Instance.GetClouldVO(cloudName);
        if (clouldVO != null)
        {
            currClouldVO = clouldVO;
            _worldMapGlobe.FlyToLocation(clouldVO.earthLocation, 1);
            StartCoroutine(WaitAction(onFlyToLocationOver, 1));
        }
    }

    private void onFlyToLocationOver()
    {
        WorldMapGlobeControl.Instance.MapZoomTo(0.2f, 1, onZoomToOver);
        _worldMapGlobe.ZoomTo(currClouldVO.earthZoom, 1);
        //StartCoroutine(WaitAction(onZoomToOver, 1.2f));
    }

    private void onZoomToOver() 
    {
        EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "EarthCloud", true, currClouldVO.name);
    }
    
    IEnumerator WaitAction(Action action ,float duration)
    {
        yield return new WaitForSeconds(duration);
        action();
    }

    public void ShowCloudByOp(string op)
    {
        string cloudName = "";
        if(op == "Next")
        {
            opIndex++;
            if (opIndex >= cloudNameList.Count)
            {
                opIndex = 0;
            }
            cloudName = cloudNameList[opIndex];
        }
        else if(op == "Pre")
        {
            opIndex--;
            if(opIndex < 0)
            {
                opIndex = cloudNameList.Count - 1;
            }
            cloudName = cloudNameList[opIndex];
        }

        if(!string.IsNullOrEmpty(cloudName))
        {
            show(cloudName);
        }
    }

    public void HideEarthCloud()
    {
        EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "EarthCloud", false, "");
    }

    public void OnQuit()
    {
        
    }
}



