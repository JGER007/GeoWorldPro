using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoMapConfigManager
{
    private static Dictionary<string, ProvinceVO> provinces = null; 
    private static Dictionary<string, CityVO> cities = null;

    public static CityVO GetCityVO(string cityName)
    {
        initMapConfig();

        if(cities.ContainsKey(cityName))
        {
            return cities[cityName];
        }

        return null;
    }


    public static ProvinceVO GetProvinceVO(string name)
    {
        initMapConfig();

        if (provinces.ContainsKey(name))
        {
            return provinces[name];
        }

        return null;
    }

    private static void initMapConfig()
    {
        if(provinces == null)
        {
            MapProvinceConfig mapProvinceConfig = JsonUtility.FromJson<MapProvinceConfig>(Resources.Load<TextAsset>("Config/ProvinceWorldConfig").text);
            List<ProvinceVO> Provinces = mapProvinceConfig.Provinces;
            provinces = new Dictionary<string, ProvinceVO>();
            foreach (ProvinceVO provinceVO in Provinces)
            {
                provinces.Add(provinceVO.name, provinceVO);
                if (provinceVO.name != provinceVO.Abbreviation)
                {
                    provinces.Add(provinceVO.Abbreviation, provinceVO);
                }
            }
        }

        if(cities == null)
        {
            MapCityConfig mapCityConfig = JsonUtility.FromJson<MapCityConfig>(Resources.Load<TextAsset>("Config/CityWorldConfig").text);
            List<CityVO> Cities = mapCityConfig.Cities;
            cities = new Dictionary<string, CityVO>();
            foreach(CityVO cityVO in Cities)
            {
                cities.Add(cityVO.Name, cityVO);
                if(cityVO.Name != cityVO.Abbreviation)
                {
                    cities.Add(cityVO.Abbreviation, cityVO);
                }
            }
        }
    }
}

[Serializable]
public class MapCityConfig
{
    public List<CityVO> Cities;
}

[Serializable]
public class CityVO
{
    public string Name = "Name";
    public string Abbreviation = "Abbreviation";
    public string Country = "Country";
    public string Province = "Province";
    public string CityType = "CityType";
    public string Population = "Population";
    public string Area = "Total area";
    public string WaterResources = "Total Water Resources(10 000 cu.m)";
    public string GreenLandArea = "Area of Green Land (hectare)";
}


[Serializable]
public class MapProvinceConfig 
{
    public List<ProvinceVO> Provinces; 
}

[Serializable]
public class ProvinceVO 
{
    public string name = "Name";
    public string countryName = "CountryName";
    public string packedRegions = "PackedRegions";
    public string Abbreviation = "Abbreviation";
    public string Population = "Population";
    public string Area = "Total area";
    public string WaterResources = "Total Water Resources(10 000 cu.m)";
    public string GreenLandArea = "Area of Green Land (hectare)";
}
