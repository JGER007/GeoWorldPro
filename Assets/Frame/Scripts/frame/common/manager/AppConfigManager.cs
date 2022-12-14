using com.frame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LitJson;
using System.IO;

public class AppConfigManager : Singleton<AppConfigManager>, IManager
{
    public void InitManager(Transform container = null)
    {
        string basePath = AppResPath4Web + "Geo/Config/AppConfig.json";
        AssetManager.Instance.LoadText(basePath, onLoadAppConfigCallBack);
    }

    private void onLoadAppConfigCallBack(string appConfigData)
    {
        JsonData appConfigJD = JsonMapper.ToObject(appConfigData);

        JsonData cloudJD = appConfigJD["Clouds"];
        parseCloudData(cloudJD);

        JsonData continentJD = appConfigJD["Continents"];
        parseContinentData(continentJD);
    }

    #region ??????
    private Dictionary<string, ContinentVO> continentDic = null;
    private void parseContinentData(JsonData continentJD)
    {
        continentDic = new Dictionary<string, ContinentVO>();
        foreach (JsonData continent in continentJD)
        {
            ContinentVO continentVO = new ContinentVO();
            continentVO.name = continent["name"].ToString();
            continentVO.desc = continent["desc"].ToString();
            continentVO.en = continent["En"].ToString();
            continentVO.area = int.Parse(continent["area"].ToString());

            JsonData colorJD = continent["color"];
            continentVO.colorValue = new Vector3();
            continentVO.colorValue.x = float.Parse(colorJD["r"].ToString());
            continentVO.colorValue.y = float.Parse(colorJD["g"].ToString());
            continentVO.colorValue.z = float.Parse(colorJD["b"].ToString());

            JsonData locationJD = continent["location"];
            continentVO.location = new Vector3();
            continentVO.location.x = float.Parse(locationJD["x"].ToString());
            continentVO.location.y = float.Parse(locationJD["y"].ToString());
            continentVO.location.z = float.Parse(locationJD["z"].ToString());

            continentDic.Add(continentVO.name,continentVO);
        }
    }


    private Vector3 defaultColorValue = new Vector3(186.0f, 227.0f, 235.0f);
    public ContinentVO GetMatchContinentVO(Vector3 hitColorValue)
    {
        ContinentVO continentVO = null;
        float minDis = Vector3.Distance(hitColorValue, defaultColorValue);
        foreach (string key in continentDic.Keys)
        {
            ContinentVO vo = continentDic[key];
            float dis = Vector3.Distance(hitColorValue, vo.colorValue);
            if(dis < minDis)
            {
                continentVO = vo;
                minDis = dis;
            }
        }
        if(minDis <10)
        {
            return continentVO;
        }

        return null;
    }


    #endregion

    #region ?Ʋ?????
    private Dictionary<string, ClouldVO> clouldDic = null;
    private List<string> cloudNameList = null;

    /// <summary>
    /// ?????Ʋ?????
    /// </summary>
    /// <param name="cloudJD"></param>
    private void parseCloudData(JsonData cloudJD)
    {
        clouldDic = new Dictionary<string, ClouldVO>();
        cloudNameList = new List<string>();
        foreach (JsonData cloud in cloudJD)
        {
            ClouldVO clouldVO = new ClouldVO();
            clouldVO.name = cloud["name"].ToString();
            clouldVO.desc = cloud["desc"].ToString();
            clouldVO.fileName = cloud["fileName"].ToString();
            clouldVO.earthZoom = float.Parse(cloud["earthZoom"].ToString());
            clouldVO.earthLocation = new Vector3();
            JsonData earthLocationJD = cloud["earthLocation"];
            clouldVO.earthLocation.x = float.Parse(earthLocationJD["x"].ToString());
            clouldVO.earthLocation.y = float.Parse(earthLocationJD["y"].ToString());
            clouldVO.earthLocation.z = float.Parse(earthLocationJD["z"].ToString());
            clouldVO.path = AppResPath4Web + "Geo/Res/Clouds/" + clouldVO.fileName;
            clouldDic.Add(clouldVO.name, clouldVO);
            cloudNameList.Add(clouldVO.name);
        }
        
    }

    public ClouldVO GetClouldVO(string cloudName)
    {
        ClouldVO clouldVO = null;
        clouldDic.TryGetValue(cloudName, out clouldVO);
        return clouldVO;
    }
    /// <summary>
    /// Ӧ?ó????ڲ???Դ·??????·??(www/webrequestר??)
    /// </summary>
    public static string AppResPath4Web
    {
        get
        {
#if UNITY_IOS
                return $"file://{Application.streamingAssetsPath}";//Application.dataPath+"/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
            return "file://" + Application.streamingAssetsPath + "/";
#else
                return "jar:file://" + Application.dataPath + "!/assets" + "/" ;
#endif
        }
    }

    public List<string> CloudNameList { get => cloudNameList; set => cloudNameList = value; }

    #endregion
    public void OnQuit()
    {

    }
}

public class AppConfigInfo
{
    public List<ClouldVO> Clouds = new List<ClouldVO>();
}

public class ClouldVO
{
    public string name;
    public string desc;
    public string fileName;
    public string path;
    public Vector3 earthLocation;
    public float earthZoom;
   
}

public class ContinentVO
{
    public string name;
    public string desc;
    public int area;
    public string en; 
    public Vector3 colorValue;
    public Vector3 location;

    private string info = "";
    public string GetInfo()
    {
        if(string.IsNullOrEmpty(info))
        {
            info = "\n????:" + area + " (??ƽ??????)\n"
                + desc;
        }
        return info;
    }
}
