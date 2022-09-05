using com.frame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class GeoMapModuleFacade : BaseModuleFacade
{
    private bool countryFlag = false;
    private bool provinceFlag = false;
    private bool cityFlag = false;
    [SerializeField]
    private WorldMapGlobe worldMapGlobe;

    private MapStyleManager mapStyleManager;

    private WorldMapGlobeControl worldMapGlobeControl;

    private int selectProvinceindex = -1;

    private Country zhCountry ;

    public override void InitModuleFacade()
    {
        base.InitModuleFacade();
        
        mapStyleManager = new MapStyleManager();
        worldMapGlobeControl = FindObjectOfType<WorldMapGlobeControl>();
        worldMapGlobeControl.WorldMapGlobe = worldMapGlobe;
        mapStyleManager.WorldMapGlobeControl = worldMapGlobeControl;
        mapStyleManager.InitManager();
        worldMapGlobe.earthStyle = EARTH_STYLE.Natural;

        //worldMapGlobe.OnCountryEnter += onEnterCountry;

        /**
        worldMapGlobe.OnCountryEnter += (int countryIndex, int regionIndex) => Debug.Log("Entered country (" + countryIndex + ") " + worldMapGlobe.countries[countryIndex].name + ",regionIndex:" + regionIndex);
        worldMapGlobe.OnCountryExit += (int countryIndex, int r1024egionIndex) => Debug.Log("Exited country " + worldMapGlobe.countries[countryIndex].name);
        worldMapGlobe.OnCountryPointerDown += (int countryIndex, int regionIndex) => Debug.Log("Pointer down on country " + worldMapGlobe.countries[countryIndex].name);
        worldMapGlobe.OnCountryClick += (int countryIndex, int regionIndex) => Debug.Log("Clicked country " + worldMapGlobe.countries[countryIndex].name);
        worldMapGlobe.OnCountryPointerUp += (int countryIndex, int regionIndex) => Debug.Log("Pointer up on country " + worldMapGlobe.countries[countryIndex].name);
        */
        //worldMapGlobe.OnCountryClick += OnCountryClick ;

        worldMapGlobe.OnProvinceClick += OnProvinceClick;
        worldMapGlobe.OnCityClick += OnCityClick;
        zhCountry = worldMapGlobe.GetCountry("中国");
        FlyToCountry("中国");
    }

    /**
    private void onEnterCountry(int countryIndex, int regionIndex)
    {
        Debug.Log("Entered country (" + countryIndex + ") " + worldMapGlobe.countries[countryIndex].name + ",regionIndex:" + regionIndex);
    }*/

    /**
    public void ShowProvinceNames() 
    {
        // First we ensure only states for USA are shown
        int countryChinaIndex = worldMapGlobe.GetCountryIndex("中国");
        for (int k = 0; k < worldMapGlobe.countries.Length; k++)
        {
            if (k != countryChinaIndex)
            {
                worldMapGlobe.countries[k].allowShowProvinces = false;
            }
        }
        worldMapGlobe.showProvinces = true;
        worldMapGlobe.drawAllProvinces = true;

        // Now, hide all country names and show states for USA
        //worldMapGlobe.showCountryNames = false;
        Country chinaCountry = worldMapGlobe.countries[countryChinaIndex];
        for (int p = 0; p < chinaCountry.provinces.Length; p++)
        {
            Province province = chinaCountry.provinces[p];
            Color color = Color.white; //new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            worldMapGlobe.AddText(province.name, province.localPosition, color);
        }
    }*/


    public void FlyToCountry(string countryName)
    {
        int countryIndex = worldMapGlobe.GetCountryIndex(countryName);
        
        ///Country zhcountry = worldMapGlobe.GetCountry(countryIndex);
        //Country ydcountry = worldMapGlobe.GetCountry("印度");
        //worldMapGlobe.AddLine(new Vector2[] { zhcountry.latlonCenter, ydcountry.latlonCenter }, Color.blue,0.05f);
        //worldMapGlobe.AddText("Distances", zhcountry.latlonCenter, Color.red,0.1f);


        worldMapGlobe.FlyToCountry(countryIndex, 2f, 1f, 0.5f);
    }

    public void FlyToCity(string cityName)
    {
        int cityIndex = worldMapGlobe.GetCityIndex(cityName);
        worldMapGlobe.FlyToCity(cityIndex, 2f, 0.2f, 0.5f);
    }

    private void OnCountryClick(int countryIndex, int regionIndex)
    {
        if(worldMapGlobe.showCountryNames)
        {
            Country country = worldMapGlobe.countries[countryIndex];
            InfoVO infoVO = new InfoVO();
            infoVO.SetData(country);

            EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "info", infoVO);
        }
       
    }

    private void OnProvinceClick(int provinceIndex, int regionIndex)
    {
        if(worldMapGlobe.showProvinces)
        {
            Province province = worldMapGlobe.provinces[provinceIndex];
            Country country = worldMapGlobe.countries[province.countryIndex];
            ProvinceVO provinceVO = GeoMapConfigManager.GetProvinceVO(province.name);
            InfoVO infoVO = new InfoVO();
            infoVO.SetData(country, province, provinceVO);
            EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "info", infoVO);
        }
        

        
    }

    private void OnCityClick(int cityIndex)
    {
        if (worldMapGlobe.showCities)
        {
            //Debug.Log("Clicked city " + worldMapGlobe.cities[cityIndex].name);
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
    }

    protected override void onUIToModuleAtion(CustomEventArgs eventArgs)
    {
        string action = eventArgs.args[0].ToString();
        
        if (action == "Country")
        {
            countryFlag = (bool)eventArgs.args[1];
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
            worldMapGlobe.showProvinces = provinceFlag;
            worldMapGlobe.showProvinceCountryOutline = provinceFlag;

            if(!provinceFlag)
            {
                //worldMapGlobe.HideProvinces();
                worldMapGlobe.HideProvinceRegionHighlights(true);
            }
        }
        else if (action == "City")
        {
            zhCountry.labelVisible = false;
            cityFlag = (bool)eventArgs.args[1];
            worldMapGlobe.showCities = cityFlag;
        }
        else if (action == "Style")
        {
            string style = eventArgs.args[1].ToString();
            mapStyleManager.ChangeMapByStyle(style);
        }
        else if(action == "Compass")
        {
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
            bool isOn = (bool)eventArgs.args[1];
            
        }

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
