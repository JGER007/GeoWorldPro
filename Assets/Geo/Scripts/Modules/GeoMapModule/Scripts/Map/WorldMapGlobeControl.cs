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

    private GameObject surfaces;

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



    //洲单独贴图
    private Texture2D continentTexture;

    //洲叠加国家贴图
    private Texture2D continentCountryTexture; 
    //洲材质球
    private Material continentMaterial ;

    public void Init() 
    {
        worldMapGlobe.cursorColor = Color.white;
        initFlag = true;
        
        SetLatLonLineFlag(false);

        mainCamera = Camera.main;
        mainCameraDis = 132273000f;

        initContinent();
    }

    /// <summary>
    /// 初始化洲对应的资源
    /// </summary>
    private void initContinent()
    {
        continentMaterial = worldMapGlobeEarthContinent.GetComponent<MeshRenderer>().material;
        continentTexture = Resources.Load<Texture2D>("Textures/Continent");
        continentCountryTexture = Resources.Load<Texture2D>("Textures/Continent_Country");
        continentMaterial.mainTexture = continentTexture;
    }

    public Vector3 GetPolePosition()
    {
        return worldMapGlobe.transform.TransformPoint(Vector3.up*-0.5f);
    }

    public void ShowContinent(bool flag)
    {
        worldMapGlobeEarthContinent.SetActive(flag);
        //worldMapGlobe.allowUserZoom = !flag;
    }

    public void DealContinentAndCountry(bool countryFlag)
    {
        if(worldMapGlobeEarthContinent.activeSelf)
        {
            if(countryFlag)
            {
                continentMaterial.mainTexture = continentCountryTexture;
            }
            else
            {
                continentMaterial.mainTexture = continentTexture;
            }
            

        }
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
        worldMapGlobe.ZoomTo(1.3333f,2);
        earthStyle = style;
       
        if (earthStyle == StyleEnum.城市灯光)
        {
            night();
        }
        else if(earthStyle == StyleEnum.默认模式)
        {
            nature();
        }
        else if (earthStyle == StyleEnum.自然模式)
        {
            global();
        }
        else if (earthStyle == StyleEnum.云层模式 || earthStyle == StyleEnum.自然风光)
        {
            clould(earthStyle == StyleEnum.云层模式 ? 1 : 0);
        }
        else if (earthStyle == StyleEnum.国家模块)
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
    }

    public void SetWorldMapGlobeBackFacesColor(Color color)
    {
        if (worldMapGlobeBackFacesMeshMat == null)
        {
            worldMapGlobeBackFacesMeshMat = transform.Find("WorldMapGlobeBackFaces").GetComponent<MeshRenderer>().material;
        }
        worldMapGlobeBackFacesMeshMat.SetColor("_Color", color);
    }    

    /// <summary>
    /// 展示洲区块信息
    /// </summary>
    public void ShowColorfulContinents()  
    {
        for(int i=0;i< continentNames.Length;i++)
        {
            string continentName = continentNames[i];
            Color color = continentColors[i];
            worldMapGlobe.ToggleContinentSurface(continentName, true, color);
        }
    }


    #region 更新经纬度信息
    private float nextLangLongUpdate = 0;
    private float langLongUpdateRate = 0.1f;
    private float lastCameraDis = 0;
    private float dt;
    private bool updateFlag = false;

    public WorldMapGlobe WorldMapGlobe { get => worldMapGlobe; set => worldMapGlobe = value; }

    private bool smallFlag = false;
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
                    if (earthStyle == StyleEnum.自然模式 || earthStyle == StyleEnum.云层模式 ||
                    earthStyle == StyleEnum.国家模块 || earthStyle == StyleEnum.城市灯光 ||
                    earthStyle == StyleEnum.默认模式)
                    {
                        worldMapGlobe.ZoomTo(0.7f);
                    }
                }*/

                    /**
                    if (earthStyle == StyleEnum.自然模式)
                    {
                        EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.卫星地图);
                    }

                    if (earthStyle == StyleEnum.自然风光 || earthStyle == StyleEnum.云层模式)
                    {
                        showClouldByValue(0);
                        EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "style", StyleEnum.地形地势);
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
        
        if(worldMapGlobeEarthContinent.activeSelf&& !worldMapGlobe.showCountryNames && !worldMapGlobe.showProvinces && !worldMapGlobe.showCities)
        {
            checkSelectContinent();
        }
    }

    #region Continent


    

    private void  checkSelectContinent() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 hitPoint = getHitPoint();
            if(hitPoint != Vector3.zero)
            {
                Vector3 localEarthPoint = transform.InverseTransformPoint(hitPoint);
                Vector2 uv = Conversion.GetUVFromSpherePoint(localEarthPoint);
                //Debug.Log("uv:" + uv);
                int hitW = (int)(continentTexture.width * uv.x);
                int hitH = (int)(continentTexture.height * uv.y);
                Color hitColor = continentTexture.GetPixel(hitW, hitH) * 255;
                Vector3 hitColorValue = new Vector3(hitColor.r, hitColor.g, hitColor.b);
                ContinentVO continentVO = AppConfigManager.Instance.GetMatchContinentVO(hitColorValue);
                if(continentVO != null)
                {
                    Debug.Log(continentVO.name);
                    EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "continentinfo", continentVO);
                }
                else
                {
                    Debug.Log("大洋");
                }
                
            }
        }
    }



    private Vector3 getHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, int.MaxValue))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    #endregion

    private void showClouldByValue(float value)
    {
        worldMapGlobe.cloudsAlpha = value;
        worldMapGlobe.cloudsShadowStrength = value;
    }

    #endregion
}
