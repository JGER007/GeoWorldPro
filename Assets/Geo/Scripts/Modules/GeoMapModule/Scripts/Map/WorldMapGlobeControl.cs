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

    public Action<string> onLatLonUpdate = null;


    private WorldMapGlobe worldMapGlobe;
    // Start is called before the first frame update

    private bool initFlag = false;
    public void Init() 
    {
        global();
        initFlag = true;
    }


    private void global()  
    {
        normalEarth.SetActive(false);
        sunLight.SetActive(false);
        globalLight.SetActive(true);
        earthHD.SetActive(true);
    }

    private void night() 
    {
        normalEarth.SetActive(false);
        sunLight.SetActive(false);
        globalLight.SetActive(false);
        earthHD.SetActive(true);
    }

    private void nature() 
    {
        normalEarth.SetActive(false);
        sunLight.SetActive(true);
        globalLight.SetActive(false);
        earthHD.SetActive(true);
    }


    public void ChangeMapByStyle(StyleEnum style) 
    {
        if(style == StyleEnum.自然)
        {
            nature();
        }
        else if(style == StyleEnum.灯光模式)
        {
            night();
        }
        else if (style == StyleEnum.默认)
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
    }
    #endregion
}
