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

    private bool latLonFlag = false;

    private bool showCursorFlag = false;

    [SerializeField]
    private float tileDis = 9.0f; 

    private GameObject surfaces;

    [SerializeField]
    private Color[] countryColors;
    private Material worldMapGlobeBackFacesMeshMat = null ; 


    public Action<string> onLatLonUpdate = null;


    private WorldMapGlobe worldMapGlobe;

    private StyleEnum earthStyle;
    private Camera mainCamera;

    private bool initFlag = false;
    public void Init() 
    {
        mainCamera = Camera.main;
        worldMapGlobe.cursorColor = Color.white;
        initFlag = true;
        nature();
        SetLatLonLineFlag(false);
    }

    public Color GetColorIndex()
    {
        int index = UnityEngine.Random.Range(0, 1000)%25;
        return countryColors[index];
    }

    public void SetLatLonLineFlag(bool flag)
    {
        latLonFlag = flag;
        worldMapGlobe.showLatitudeLines = latLonFlag;
        worldMapGlobe.showLongitudeLines = latLonFlag;
    }

    public void SetShowCursorFlag(bool flag)
    {
        showCursorFlag = flag;
        worldMapGlobe.showCursor = showCursorFlag;
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
        if (style == StyleEnum.���еƹ�)
        {
            night();
        }
        else if (style == StyleEnum.��Ȼģʽ)
        {
            global();
        }
        else if (style == StyleEnum.�Ʋ�ģʽ || style == StyleEnum.��Ȼ���)
        {
            clould(style == StyleEnum.�Ʋ�ģʽ ? 1 : 0);
        }
        else if (style == StyleEnum.����ģ��)
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


    #region ���¾�γ����Ϣ
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
                    worldMapGlobe.countryLabelsSize = 0.25f - (15.25f - cameraDis) * 0.05f;
                    worldMapGlobe.cityIconSize = 1 - (15.25f - cameraDis) * 0.3f;
                }
            }


            if(cameraDis <= tileDis)
            {
                if (earthStyle == StyleEnum.��Ȼģʽ)
                {
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.���ǵ�ͼ);
                }

                if (earthStyle == StyleEnum.��Ȼ��� || earthStyle == StyleEnum.�Ʋ�ģʽ)
                {
                    showClouldByValue(0);
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.���ε���);
                }

                //------
                WorldMapGlobe.showCursor = false;
                if (latLonFlag)
                {
                    worldMapGlobe.showLatitudeLines = false;
                    worldMapGlobe.showLongitudeLines = false;
                }
            }
            else
            {
                /**
                if (earthStyle == StyleEnum.���ǵ�ͼ)
                {
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.��Ȼģʽ);
                }
                
                if (earthStyle == StyleEnum.���ε���)
                {
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.��Ȼ���);
                }*/
                //------
                if(showCursorFlag)
                {
                    WorldMapGlobe.showCursor = true;
                }
                
                if (latLonFlag)
                {
                    worldMapGlobe.showLatitudeLines = true;
                    worldMapGlobe.showLongitudeLines = true;
                }
            }

            /**
            if (earthStyle == StyleEnum.��Ȼģʽ)
            {
                if (cameraDis <= tileDis)
                {
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.���ǵ�ͼ);
                }

            }
            else if (earthStyle == StyleEnum.���ǵ�ͼ)
            {
                if (cameraDis > tileDis)
                {
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.��Ȼģʽ);
                }
            }

            ///���ε�����Ƭ����Ȼ���ģʽ�л�
            if (earthStyle == StyleEnum.��Ȼ��� || earthStyle == StyleEnum.�Ʋ�ģʽ)
            {
                if (cameraDis <= tileDis)
                {
                    showClouldByValue(0);
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.���ε���);
                }

            }
            else if (earthStyle == StyleEnum.���ε���)
            {
                if (cameraDis > tileDis)
                {
                    //clould(0);
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.��Ȼ���);
                }
            }

            if (cameraDis > tileDis)
            {
                WorldMapGlobe.showCursor = true;

                if(latLonFlag)
                {
                    worldMapGlobe.showLatitudeLines = true;
                    worldMapGlobe.showLongitudeLines = true;
                }
            }
            else
            {
                WorldMapGlobe.showCursor = false;
                if (latLonFlag)
                {
                    worldMapGlobe.showLatitudeLines = false;
                    worldMapGlobe.showLongitudeLines = false;
                }
            }*/
        }    
    }

    private void showClouldByValue(float value)
    {
        worldMapGlobe.cloudsAlpha = value;
        worldMapGlobe.cloudsShadowStrength = value;
    }

    #endregion
}
