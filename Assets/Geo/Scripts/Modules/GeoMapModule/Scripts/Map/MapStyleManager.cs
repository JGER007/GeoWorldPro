using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class MapStyleManager : IManager
{
    WorldMapGlobeControl _worldMapGlobeControl = null;
    private Dictionary<string, Color> countryColorDic = null;

    public WorldMapGlobeControl WorldMapGlobeControl { get => _worldMapGlobeControl; set => _worldMapGlobeControl = value; }

    public void InitManager(Transform container = null)
    {
        countryColorDic = new Dictionary<string, Color>();
        //WorldMapGlobe.instance.earthStyle = EARTH_STYLE.Nature_No_Cloud;
        _worldMapGlobeControl.Init();
    }

    public void ChangeMapByStyle(string style)
    {
       StyleEnum styleEnum = (StyleEnum)Enum.Parse(typeof(StyleEnum), style);
        WorldMapGlobe.instance.HideCountrySurfaces();
        TILE_SERVER tILE_SERVER = getTitleServer(styleEnum);
        _worldMapGlobeControl.ShowCloulds(false);
        if (tILE_SERVER != TILE_SERVER.None)
        {
            WorldMapGlobe.instance.showTiles = true;
            WorldMapGlobe.instance.tileServer = tILE_SERVER;
        }
        else
        {
            WorldMapGlobe.instance.showTiles = false;
            if (styleEnum == StyleEnum.国家模块)
            {
                Country[] countries = WorldMapGlobe.instance.countries;
                for (int colorizeIndex = 0; colorizeIndex < countries.Length; colorizeIndex++)
                {
                    string countryName = countries[colorizeIndex].name;
                    Color color = getCountryColor(countryName);
                    WorldMapGlobe.instance.ToggleCountrySurface(countryName, true, color);
                }
            }
            else if(styleEnum == StyleEnum.云层模式)
            {
                _worldMapGlobeControl.ShowCloulds(true);
            }
        }
        _worldMapGlobeControl.ChangeMapByStyle(styleEnum);

    }

    /// <summary>
    /// 存储获取国家区块颜色
    /// </summary>
    /// <param name="countryName"></param>
    /// <returns></returns>
    private Color getCountryColor(string countryName)
    {
        if(countryColorDic.ContainsKey(countryName))
        {
            return countryColorDic[countryName];
        }

        Color color = _worldMapGlobeControl.GetColor();//new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
        countryColorDic.Add(countryName, color);
        return color;
    }

    private TILE_SERVER getTitleServer(StyleEnum style)  
    {
        TILE_SERVER tILE_SERVER = TILE_SERVER.None;
        switch(style)
        {
            case StyleEnum.城市模式:
                tILE_SERVER = TILE_SERVER.City;
                break;

            case StyleEnum.卫星地图:
                tILE_SERVER = TILE_SERVER.Satellite;
                break;

            case StyleEnum.地形地势:
                tILE_SERVER = TILE_SERVER.Terrain;
                break;

            case StyleEnum.自然风光:
                tILE_SERVER = TILE_SERVER.MapsForFree;
                break;

        }

        return tILE_SERVER;
    }

    public void OnQuit()
    {
        
    }
}

public enum StyleEnum
{
    自然模式,
    国家模块,
    云层模式,
    城市灯光,
    城市模式,
    自然风光,
    卫星地图,
    地形地势
}

