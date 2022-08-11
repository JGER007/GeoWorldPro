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

    [SerializeField]
    private GameObject surfaces; 

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
        worldMapGlobe.cursorColor = Color.white;
        //global();
        initFlag = true;
    }


    private void global()  
    {
        //ShowCloulds(true);
        normalEarth.SetActive(false);
        sunLight.SetActive(false);
        globalLight.SetActive(true);
        earthHD.SetActive(true);
    }

    private void night() 
    {
        //ShowCloulds(false);
        normalEarth.SetActive(false);
        sunLight.SetActive(false);
        globalLight.SetActive(false);
        earthHD.SetActive(true);
    }

    private void nature() 
    {
        //ShowCloulds(false);
        normalEarth.SetActive(false);
        sunLight.SetActive(true);
        globalLight.SetActive(false);
        earthHD.SetActive(true);
    }

    public void ShowSurface(bool isShow)
    {
        transform.Find("Surfaces").gameObject.SetActive(isShow);
    }

    public void ChangeMapByStyle(StyleEnum style) 
    {
        earthStyle = style;
        if(style == StyleEnum.城市灯光)
        {
            night();
        }
        else if (style == StyleEnum.自然模式 || style == StyleEnum.云层模式)
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

    public void ShowCloulds(bool flag)
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
    private float lastCameraDis = 0;
    private float dt;
    private bool updateFlag = false;

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

        dt = dt + Time.deltaTime;
        
        if(dt >0.05f)
        {
            dt = 0;
            float cameraDis = mainCamera.transform.position.magnitude;
            
            if (Mathf.Abs(lastCameraDis - cameraDis) > 0.05f)
            {
                lastCameraDis = cameraDis;
                updateFlag = true;
            }
            else
            {
                if(updateFlag)
                {
                    updateFlag = false;
                    worldMapGlobe.countryLabelsSize = 0.25f - (60.1f - cameraDis) * 0.025f;
                    worldMapGlobe.cityIconSize = 1 - (60.1f - cameraDis) * 0.1f;
                }
            }

            if (earthStyle == StyleEnum.自然模式)
            {
                if (cameraDis <= 51.5f)
                {
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.卫星地图);
                    //ChangeMapByStyle(StyleEnum.卫星影像);
                }

            }
            else if (earthStyle == StyleEnum.卫星地图)
            {
                if (cameraDis > 51.5f)
                {
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.自然模式);
                    //ChangeMapByStyle(StyleEnum.卫星影像);
                }
            }
        }    
    }
    #endregion
}
