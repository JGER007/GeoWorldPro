using com.frame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class WorldMapGlobeControl : MonoSingleton<WorldMapGlobeControl>
{
    [SerializeField]
    private GameObject earthHD;

    [SerializeField]
    private GameObject worldMapGlobeEarthContinent;  

    [SerializeField]
    private GameObject normalEarth;

    [SerializeField]
    private GameObject globalLight;

    [SerializeField]
    private GameObject sunLight;

    private bool latLonFlag = false;

    private bool showCursorFlag = false;

    private float tileDis = 5400; 

    [SerializeField]
    private Color[] countryColors;

    [SerializeField]
    private Color[] continentColors;

    [SerializeField]
    private string[] continentNames;
    private Material worldMapGlobeBackFacesMeshMat = null ; 
    public Action<string> onLatLonUpdate = null;
    private WorldMapGlobe worldMapGlobe;

    private StyleEnum earthStyle;
    private Camera mainCamera;

    private bool initFlag = false;
    private float mainCameraDis;

    public void Init() 
    {
        worldMapGlobe.cursorColor = Color.white;
        //initFlag = true;
        
        SetLatLonLineFlag(false);

        mainCamera = Camera.main;
        mainCameraDis = 132273000f;
    }

    public Vector3 GetPolePosition()
    {
        return worldMapGlobe.transform.TransformPoint(Vector3.up*-0.5f);
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
       
        if (earthStyle == StyleEnum.���еƹ�)
        {
            night();
        }
        else if(earthStyle == StyleEnum.Ĭ��ģʽ)
        {
            nature();
        }
        else if (earthStyle == StyleEnum.��Ȼģʽ)
        {
            global();
        }
        else if (earthStyle == StyleEnum.�Ʋ�ģʽ || earthStyle == StyleEnum.��Ȼ���)
        {
            clould(earthStyle == StyleEnum.�Ʋ�ģʽ ? 1 : 0);
        }
        else if (earthStyle == StyleEnum.����ģ��)
        {
            ShowSurface(true);
            SetWorldMapGlobeBackFacesColor(new Color(0, 0.21f, 0.42f));
            
            earthHD.SetActive(false);
            normalEarth.SetActive(false);
        }
        else
        {
            earthHD.SetActive(false);
            normalEarth.SetActive(false);
        }

        if(!initFlag)
        {
            initFlag = true;
            return;
        }
        
        if(earthStyle != StyleEnum.�Ʋ�ģʽ)
        {
            float lastZoomLevel = worldMapGlobe.GetZoomLevel();
            MapZoomTo(GeoMapModuleFacade.DefaultEarthZoom, 2, delegate
            {
                MapZoomTo(lastZoomLevel, 2);
            });
        }   
    }

    public bool IsStaticMap()
    {
        if (earthStyle == StyleEnum.���еƹ� || earthStyle == StyleEnum.Ĭ��ģʽ || earthStyle == StyleEnum.��Ȼģʽ || earthStyle == StyleEnum.��Ȼ���)
        {
            return true;
        }
        return false;
    }

    public void MapZoomTo(float zoom ,float duration , Action zoomComplete = null)
    {
        if(IsStaticMap())
        {
            if(zoom < 1)
            {
                zoom = 1;
            }
        }
        StartCoroutine(OnZoomTo(zoom, duration, zoomComplete));
    }


    IEnumerator OnZoomTo(float zoom ,float duration ,Action zoomComplete=null)
    {
        worldMapGlobe.ZoomTo(zoom, duration);
        yield return new WaitForSeconds(duration);
        zoomComplete?.Invoke();
        yield return null;
    }

    public void SetWorldMapGlobeBackFacesColor(Color color)
    {
        if (worldMapGlobeBackFacesMeshMat == null)
        {
            worldMapGlobeBackFacesMeshMat = transform.Find("WorldMapGlobeBackFaces").GetComponent<MeshRenderer>().material;
        }
        worldMapGlobeBackFacesMeshMat.SetColor("_Color", color);
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
            
            
            if (Mathf.Abs(lastCameraDis - cameraDis) > 5)
            {
                lastCameraDis = cameraDis;
                updateFlag = true;
            }
            else
            {
                if(updateFlag)
                {
                    //Debug.Log("mainCamera.transform.position.sqrMagnitude:" + mainCamera.transform.position.sqrMagnitude);
                    float tempDis = mainCamera.transform.position.sqrMagnitude;
                    float changeScale = 0;
                    if (tempDis > 28000000)
                    {
                        changeScale = mainCamera.transform.position.sqrMagnitude / mainCameraDis;
                    }
                    updateFlag = false;
                    worldMapGlobe.countryLabelsSize = 0.35f* changeScale;
                    worldMapGlobe.cityIconSize = changeScale;
                }
            }

            if (cameraDis <= tileDis)
            {
                /**
                if(!smallFlag)
                {
                    smallFlag = true;
                    if (earthStyle == StyleEnum.��Ȼģʽ || earthStyle == StyleEnum.�Ʋ�ģʽ ||
                    earthStyle == StyleEnum.����ģ�� || earthStyle == StyleEnum.���еƹ� ||
                    earthStyle == StyleEnum.Ĭ��ģʽ)
                    {
                        worldMapGlobe.ZoomTo(0.7f);
                    }
                }*/

                    /**
                    if (earthStyle == StyleEnum.��Ȼģʽ)
                    {
                        EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.���ǵ�ͼ);
                    }

                    if (earthStyle == StyleEnum.��Ȼ��� || earthStyle == StyleEnum.�Ʋ�ģʽ)
                    {
                        showClouldByValue(0);
                        EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.���ε���);
                    }*/

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

               // smallFlag = false;
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
