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

    private GameObject surfaces;

    [SerializeField]
    private Color[] countryColors;
    private Material worldMapGlobeBackFacesMeshMat = null ; 


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
        nature();
    }

    public Color GetColor()
    {
        int index = UnityEngine.Random.Range(0, 1000)%25;
        return countryColors[index];
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

    private void clould(float clouldAlpha) 
    {
        normalEarth.SetActive(true);
        sunLight.SetActive(true);
        globalLight.SetActive(false);
        earthHD.SetActive(false);
        worldMapGlobe.earthStyle = EARTH_STYLE.NaturalHighRes16KScenic;
        showClouldByValue(clouldAlpha);
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
        if (style == StyleEnum.城市灯光)
        {
            night();
        }
        else if (style == StyleEnum.自然模式)
        {
            global();
        }
        else if (style == StyleEnum.云层模式 || style == StyleEnum.自然风光)
        {
            clould(style == StyleEnum.云层模式 ? 1 : 0);
        }
        else if (style == StyleEnum.国家模块)
        {
            ShowSurface(true);
            if (worldMapGlobeBackFacesMeshMat == null)
            {
                worldMapGlobeBackFacesMeshMat = transform.Find("WorldMapGlobeBackFaces").GetComponent<MeshRenderer>().material;
                worldMapGlobeBackFacesMeshMat.SetColor("_Color", new Color(0, 0.21f, 0.42f));
            }
            earthHD.SetActive(false);
            normalEarth.SetActive(false);
        }
        else
        {
            earthHD.SetActive(false);
            normalEarth.SetActive(false);
        }
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
                    worldMapGlobe.countryLabelsSize = 0.25f - (60.1f - cameraDis) * 0.05f;
                    worldMapGlobe.cityIconSize = 1 - (60.1f - cameraDis) * 0.3f;
                }
            }


            if (earthStyle == StyleEnum.自然模式)
            {
                if (cameraDis <= 51.5f)
                {
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.卫星地图);
                }

            }
            else if (earthStyle == StyleEnum.卫星地图)
            {
                if (cameraDis > 52f)
                {
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.自然模式);
                }
            }

            ///地形地势瓦片与自然风光模式切换
            if (earthStyle == StyleEnum.自然风光 || earthStyle == StyleEnum.云层模式)
            {
                if (cameraDis <= 51.5f)
                {
                    showClouldByValue(0);
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.地形地势);
                }

            }
            else if (earthStyle == StyleEnum.地形地势)
            {
                if (cameraDis > 52f)
                {
                    //clould(0);
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.自然风光);
                }
            }
        }    
    }

    private void showClouldByValue(float value)
    {
        worldMapGlobe.cloudsAlpha = value;
        worldMapGlobe.cloudsShadowStrength = value;
    }

    #endregion
}
