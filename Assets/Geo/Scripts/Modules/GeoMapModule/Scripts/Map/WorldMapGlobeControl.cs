using com.frame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class WorldMapGlobeControl : MonoBehaviour
{
    [SerializeField]
    private GameObject earthHD;

    [SerializeField]
    private GameObject normalEarth;


    [SerializeField]
    private GameObject globalLight;

    [SerializeField]
    private GameObject sunLight;

    [SerializeField]
    private GameObject cloulds;

    private Material clouldsMat = null;

    public Action<string> onLatLonUpdate = null;


    private WorldMapGlobe worldMapGlobe;

    private StyleEnum earthStyle;
    private Camera mainCamera;
    // Start is called before the first frame update

    private bool initFlag = false;
    public void Init() 
    {
        mainCamera = Camera.main;
        global();
        initFlag = true;
    }


    private void global()  
    {
        showCloulds(true);
        normalEarth.SetActive(false);
        sunLight.SetActive(false);
        globalLight.SetActive(true);
        earthHD.SetActive(true);
    }

    private void night() 
    {
        showCloulds(false);
        normalEarth.SetActive(false);
        sunLight.SetActive(false);
        globalLight.SetActive(false);
        earthHD.SetActive(true);
    }

    private void nature() 
    {
        showCloulds(false);
        normalEarth.SetActive(false);
        sunLight.SetActive(true);
        globalLight.SetActive(false);
        earthHD.SetActive(true);
    }



    public void ChangeMapByStyle(StyleEnum style) 
    {
        earthStyle = style;
        if (style == StyleEnum.自然)
        {
            nature();
            
        }
        else if(style == StyleEnum.灯光模式)
        {
            night();
        }
        else if (style == StyleEnum.卫星影像_离线)
        {
            global();
        }
        else
        {
            nature();
            Country zhCountry = worldMapGlobe.GetCountry("中国");
            if (style == StyleEnum.国家模块)
            {
                zhCountry.labelVisible = true;
                worldMapGlobe.showCountryNames = true;
                earthHD.SetActive(true);
            }
            else
            {
                if(worldMapGlobe.showProvinces || worldMapGlobe.showCities)
                {
                    zhCountry.labelVisible = false;
                }
                earthHD.SetActive(false);
            }
        }
    }

    private void showCloulds(bool flag)
    {
        if(clouldsMat == null)
        {
            clouldsMat = Resources.Load<Material>("Materials/HighCloud");
            cloulds.GetComponent<MeshRenderer>().material = clouldsMat;
        }

        cloulds.gameObject.SetActive(flag);
    }


    #region 更新经纬度信息
    private float nextLangLongUpdate = 0;
    private float langLongUpdateRate = 0.1f;

    public WorldMapGlobe WorldMapGlobe { get => worldMapGlobe; set => worldMapGlobe = value; }

    void Update()
    {
        // Check whether a country or city is selected, then show a label
        if (initFlag && WorldMapGlobe.mouseIsOver)
        {
            if (Time.time > nextLangLongUpdate)
            {
                onLatLonUpdate?.Invoke(WorldMapGlobe.calc.prettyCurrentLatLon);
                nextLangLongUpdate = Time.time + langLongUpdateRate;
            }
        }

        if(earthStyle == StyleEnum.卫星影像_离线)
        {
            if (mainCamera.transform.position.magnitude <= 51.5f)
            {
                EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.卫星影像);
                //ChangeMapByStyle(StyleEnum.卫星影像);
            }
        }
    }
    #endregion
}
