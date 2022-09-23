using com.frame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class GeoMapModuleFacade : BaseModuleFacade
{
    private bool continentFlag = false; 
    private bool countryFlag = false;
    private bool provinceFlag = false;
    private bool cityFlag = false;
    private bool rlueFlag = false;
    [SerializeField]
    private WorldMapGlobe worldMapGlobe;

    private MapStyleManager mapStyleManager;

    private WorldMapGlobeControl worldMapGlobeControl;

    private int selectProvinceindex = -1;

    private Transform provinceNamesContainer = null;

    private RuleManager ruleManager;
    [SerializeField]
    private GameObject ruleContainer;
    //private Country zhCountry ;
    public override void InitModuleFacade()
    {
        base.InitModuleFacade();
        
        mapStyleManager = new MapStyleManager();
        worldMapGlobeControl = FindObjectOfType<WorldMapGlobeControl>();
        worldMapGlobeControl.WorldMapGlobe = worldMapGlobe;
        mapStyleManager.WorldMapGlobeControl = worldMapGlobeControl;
        mapStyleManager.InitManager();
        worldMapGlobe.earthStyle = EARTH_STYLE.Natural;

        initRlueManager();

        worldMapGlobe.OnProvinceClick += OnProvinceClick;
        worldMapGlobe.OnCityClick += OnCityClick;

        worldMapGlobe.OnCountryClick += OnCountryClick;
        worldMapGlobe.OnCountryPointerUp += OnCountryPointerUp;
        //zhCountry = worldMapGlobe.GetCountry("中国");
        //zhCountry.labelVisible = true;

        worldMapGlobe.ZoomTo(1.333f);
        FlyToCountry("中国");
    }

    /// <summary>
    /// 初始化测距管理类
    /// </summary>
    private void initRlueManager()
    {
        ruleManager = new RuleManager();
        ruleManager.WorldMapGlobe = worldMapGlobe; 
        ruleManager.InitManager(ruleContainer.transform);
    }


    private void OnCountryPointerUp(int countryIndex, int regionIndex)
    {
        if(worldMapGlobe.GetCountry(countryIndex).name != "中国")
        {
            worldMapGlobe.HideProvinceRegionHighlights(true);
        }
        
    }

    private void OnCountryClick(int countryIndex, int regionIndex)
    {
        if (worldMapGlobe.GetCountry(countryIndex).name != "中国")
        {
            worldMapGlobe.HideProvinceRegionHighlights(true);
        }

        /*
        if (worldMapGlobe.showCountryNames)
        {
            Country country = worldMapGlobe.countries[countryIndex];
            InfoVO infoVO = new InfoVO();
            infoVO.SetData(country);
            EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "info", infoVO);
        }*/
    }

    /**
    private void onEnterCountry(int countryIndex, int regionIndex)
    {
        Debug.Log("Entered country (" + countryIndex + ") " + worldMapGlobe.countries[countryIndex].name + ",regionIndex:" + regionIndex);
    }*/

    
    private void showProvinceNames() 
    {
        if(provinceNamesContainer == null)
        {
            provinceNamesContainer = new GameObject("ProvinceNamesContainer").transform;
            provinceNamesContainer.SetParent(worldMapGlobe.transform);
            provinceNamesContainer.transform.localPosition = Vector3.zero;
            provinceNamesContainer.transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            provinceNamesContainer.gameObject.SetActive(true);
            return;
        }

        foreach(Country country in worldMapGlobe.countries)
        {
            if(country.provinces != null)
            {
                for (int p = 0; p < country.provinces.Length; p++)
                {
                    Province state = country.provinces[p];
                    Color color = Color.white; //new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                    TextMesh textMesh = worldMapGlobe.AddText(state.name, state.localPosition, color);
                    textMesh.transform.SetParent(provinceNamesContainer);
                }
            }
        }

        
    }


    public void FlyToCountry(string countryName)
    {
        int countryIndex = worldMapGlobe.GetCountryIndex(countryName);
        worldMapGlobe.FlyToCountry(countryIndex, 2f, 1f, 0.5f);
    }

    public void FlyToCity(string cityName)
    {
        int cityIndex = worldMapGlobe.GetCityIndex(cityName);
        worldMapGlobe.FlyToCity(cityIndex, 2f, 0.2f, 0.5f);
    }

    

    private void OnProvinceClick(int provinceIndex, int regionIndex)
    {
        if(worldMapGlobe.showProvinces)
        {
            if(provinceIndex != -1)
            {
                Province province = worldMapGlobe.provinces[provinceIndex];
                Country country = worldMapGlobe.countries[province.countryIndex];
                ProvinceVO provinceVO = GeoMapConfigManager.GetProvinceVO(province.name);
                InfoVO infoVO = new InfoVO();
                infoVO.SetData(country, province, provinceVO);
                EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "info", infoVO);
            }
            else
            {
                EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "info", null);
            }
        }
    }

    private void OnCityClick(int cityIndex)
    {
        if (worldMapGlobe.showCities)
        {
            if(cityIndex !=-1)
            {
                City city = worldMapGlobe.cities[cityIndex];
                string provinceName = city.province;
                Country country = worldMapGlobe.countries[city.countryIndex];
                InfoVO infoVO = new InfoVO();

                CityVO cityVO = GeoMapConfigManager.GetCityVO(city.name);
                ProvinceVO provinceVO = GeoMapConfigManager.GetProvinceVO(provinceName);
                if (cityVO == null)
                {
                    infoVO.SetData(country, city, provinceVO);
                }
                else
                {

                    infoVO.SetData(country, cityVO, provinceVO);
                }
                EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "info", infoVO);
            }
            else
            {
                EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "info", null);
            }
        }  
    }

    protected override void onUIToModuleAtion(CustomEventArgs eventArgs)
    {
        string action = eventArgs.args[0].ToString();
        if (action == "Continent")
        {
            continentFlag = (bool)eventArgs.args[1];
            worldMapGlobeControl.ShowContinent(continentFlag);
            if(continentFlag)
            {
                togglePolitical();
                string style = StyleEnum.自然模式.ToString();
                mapStyleManager.ChangeMapByStyle(style);
            }
        }
        else if (action == "Country")
        {
            
            countryFlag = (bool)eventArgs.args[1];
            if (countryFlag)
            {
                toggleContinent();
            }
            worldMapGlobe.showFrontiers = countryFlag;
            worldMapGlobe.showCountryNames = countryFlag;
            worldMapGlobe.showCoastalFrontiers = countryFlag;
            worldMapGlobe.showInlandFrontiers = countryFlag;
            worldMapGlobe.enableCountryHighlight = countryFlag;
            worldMapGlobe.highlightAllCountryRegions = countryFlag;
        }
        else if(action == "Province")
        {
            provinceFlag = (bool)eventArgs.args[1];
            if (provinceFlag)
            {
                toggleContinent();
            }
            worldMapGlobe.showProvinces = provinceFlag;
            worldMapGlobe.showProvinceCountryOutline = provinceFlag;

            if(!provinceFlag)
            {
                //worldMapGlobe.HideProvinces();
                worldMapGlobe.HideProvinceRegionHighlights(true);
                //provinceNamesContainer?.gameObject.SetActive(false);
            }
            
            /*else
            {
                showProvinceNames();
            }*/
        }
        else if (action == "City")
        {
            cityFlag = (bool)eventArgs.args[1];
            if (cityFlag)
            {
                toggleContinent();
            }
            
            worldMapGlobe.showCities = cityFlag;
        }
        else if (action == "Style")
        {
            string style = eventArgs.args[1].ToString();

            if(mapStyleManager.IshowPoliticalBorder(style))
            {
                togglePolitical();
                toggleContinent();
            }

            mapStyleManager.ChangeMapByStyle(style);
        }
        else if(action == "Compass")
        {
            worldMapGlobe.ZoomTo(1.333f);
            FlyToCountry("中国");
        }
        else if(action == "LatLonLine")
        {
            bool isOn = (bool)eventArgs.args[1];
            worldMapGlobeControl.SetLatLonLineFlag(isOn);
        }
        else if (action == "LatLon")
        {
            bool isOn = (bool)eventArgs.args[1];
            worldMapGlobeControl.SetShowCursorFlag(isOn);
        }
        else if (action == "Rule")
        {
            rlueFlag = (bool)eventArgs.args[1];
            if (!rlueFlag)
            {
                ruleManager.RemoveRule();
            }
        }
    }

    void Update()
    {
        if (rlueFlag)
        {
            if (Input.GetMouseButtonDown(0) && !checkPointerOverUI())
            {
                Vector3 hitpose;
                if(worldMapGlobe.GetGlobeIntersection(out hitpose))
                {
                    Vector3 pos = worldMapGlobe.transform.InverseTransformPoint(hitpose);
                    ruleManager.AddRulePoint(pos);
                }
            }
        }
        
    }


    /// <summary>
    /// 检查是否点击再UI上
    /// </summary>
    /// <returns></returns>
    private bool checkPointerOverUI() 
    {
        // Check whether the points is on an UI element, then cancels
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
                if (Input.touchSupported && Input.touchCount > 0)
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        Touch currTouch = Input.GetTouch(i);
                        if (currTouch.phase == TouchPhase.Began && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(currTouch.fingerId))
                        {
                            return true;
                        }
                    }
                }
                else if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
                {
                    return true;
                }
        }
        return false;
    }



    public void togglePolitical()  
    {
        EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "toggle", "Country", false);
        EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "toggle", "Province", false);
        EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "toggle", "City", false);
    }

    private void toggleContinent()
    {
        EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "toggle", "Continent", false);
    }

    public override void OnQuit()
    {
        base.OnQuit();
    }
}

public class InfoVO
{
    public string Country;
    public string Continent;
    public string Province;
    public string City;
    public CityVO cityVO = null; 
    public string Tilte;
    public ProvinceVO provinceDataVO = null;
    public void SetData(Country country)
    {
        Country = country.name;
        Continent = country.continent;
        Tilte = Country;
    }

    public void SetData(Country country , Province province, ProvinceVO provinceVO) 
    {
        Province = province.name;
        Country = country.name;
        Continent = country.continent;
        provinceDataVO = provinceVO;
        Tilte = Province;
    }

    public void SetData(Country country,City city, ProvinceVO provinceVO)
    {
        City = city.name;
        Province = provinceVO.name;
        Country = country.name;
        Continent = country.continent;
        provinceDataVO = provinceVO;
        Tilte = City;
    }

    public void SetData(Country country, CityVO city,ProvinceVO provinceVO)
    {
        City = city.Name;
        if(provinceVO != null)
        {
            Province = provinceVO.name;
        }
        provinceDataVO = provinceVO;
        Country = country.name;
        Continent = country.continent;
        Tilte = City;
        cityVO = city;

    }


    public string getPositionInfo()
    {
        string info = Continent + ">" + Country;
        if (!string.IsNullOrEmpty(Province))
        {
            info = info + ">" + Province;
            if (provinceDataVO != null)
            {
                info = info + "(" + provinceDataVO.Abbreviation + ")";
            }
            //info = "位置:" + info;
        }

        if (!string.IsNullOrEmpty(City))
        {
            info = info + ">" + City;
        }

        return info;
    }

    public string GetInfo()
    {
        string info = "";
        /**
        //Continent + ">" + Country;
        if(!string.IsNullOrEmpty(Province))
        {
            info = info + ">" + Province ;
            if(provinceDataVO != null)
            {
                info = info + "(" + provinceDataVO.Abbreviation + ")";
            }
            info = "位置:" + info;
        }

        if (!string.IsNullOrEmpty(City))
        {
            info = info + ">" + City;
        }*/

        if (cityVO != null)
        {
            if(!string.IsNullOrEmpty(cityVO.Population))
            {
                info = info + "\n户籍人口：" + cityVO.Population + "(万)";
            }

            if (!string.IsNullOrEmpty(cityVO.Area))
            {
                info = info + "\n面积：" + cityVO.Area + "(平方公里)";
            }

            if(!string.IsNullOrEmpty(cityVO.WaterResources))
            {
                info = info + "\n水资源：" + cityVO.WaterResources + "(万立方米)";
            }

            if (!string.IsNullOrEmpty(cityVO.GreenLandArea))
            {
                info = info + "\n绿地面积：" + cityVO.GreenLandArea + "(公顷)";
            }
        }
        else if(provinceDataVO != null)
        {
            info = info + "\n人口：" + provinceDataVO.Population + "(人)";//+ "(万)";
            /**
            info = info + "\n面积：" + provinceDataVO.Area + "(平方公里)";

            if (!string.IsNullOrEmpty(provinceDataVO.WaterResources))
            {
                info = info + "\n水资源：" + provinceDataVO.WaterResources + "(万立方米)";
            }

            if (!string.IsNullOrEmpty(provinceDataVO.GreenLandArea))
            {
                info = info + "\n绿地面积：" + provinceDataVO.GreenLandArea + "(公顷)";
            }*/
        }

        return info;
    }
}
