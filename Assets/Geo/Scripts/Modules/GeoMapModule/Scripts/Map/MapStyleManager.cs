using com.frame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPM;

public class MapStyleManager : IManager
{
    private Dictionary<string, Color> countryColorDic = null;

    public void InitManager(Transform container = null)
    {
        countryColorDic = new Dictionary<string, Color>();
        WorldMapGlobeControl.Instance.Init();
    }

    public void ChangeMapByStyle(string style)
    {
       StyleEnum styleEnum = (StyleEnum)Enum.Parse(typeof(StyleEnum), style);
        WorldMapGlobe.instance.HideCountrySurfaces();
        TILE_SERVER tILE_SERVER = getTitleServer(styleEnum);
        if (tILE_SERVER != TILE_SERVER.None)
        {
            WorldMapGlobe.instance.showTiles = true;
            WorldMapGlobe.instance.tileServer = tILE_SERVER;

            EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "Toggle", "Country", false);
            EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "Toggle", "Province", false);
            EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "Toggle", "City", false);
            Color color = Color.white;
            if (styleEnum == StyleEnum.地形地势)
            {
                color = new Color(0.843f, 0.925f, 0.694f);
            }
            else if (styleEnum == StyleEnum.卫星地图)
            {
                color = new Color(0.184f, 0.29f, 0.122f);
            }
            else if (styleEnum == StyleEnum.城市模式)
            {
                color = new Color(1, 0.976f, 0.949f);
            }
            WorldMapGlobeControl.Instance.SetWorldMapGlobeBackFacesColor(color);
        }
        else
        {
            WorldMapGlobe.instance.showTiles = false;
            WorldMapGlobe.instance.ResetTiles();
            if (styleEnum == StyleEnum.国家模块)
            {
                Country[] countries = WorldMapGlobe.instance.countries;
                for (int colorizeIndex = 0; colorizeIndex < countries.Length; colorizeIndex++)
                {
                    string countryName = countries[colorizeIndex].name;
                    Color color = getCountryColor(countryName);
                    WorldMapGlobe.instance.ToggleCountrySurface(countryName, true, color);
                }
                EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "Toggle", "Country", true);
                EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "Toggle", "Province", false);
                EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "Toggle", "City", false);
            }
        }
        WorldMapGlobeControl.Instance.ChangeMapByStyle(styleEnum);

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

        Color color = WorldMapGlobeControl.Instance.GetColorIndex();
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
            case StyleEnum.云层模式:
                tILE_SERVER = TILE_SERVER.Satellite;
                break;

            case StyleEnum.地形地势:
                tILE_SERVER = TILE_SERVER.Terrain;
                break;

            case StyleEnum.地形地貌:
                tILE_SERVER = TILE_SERVER.MapsForFree;
                break;

        }

        return tILE_SERVER;
    }

    public void OnQuit()
    {
        
    }

    public bool IshowPoliticalBorder(string style) 
    {
        if(style == StyleEnum.默认模式.ToString() ||
            style == StyleEnum.自然模式.ToString() ||
            style == StyleEnum.国家模块.ToString())
        {
            return false;
        }

        return true;
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
    地形地势,
    地形地貌,
    默认模式,
    None
}

