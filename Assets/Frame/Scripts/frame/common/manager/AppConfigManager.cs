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
    private Dictionary<string , ClouldVO> clouldDic = null;
    private List<string> cloudNameList = null;

    public void InitManager(Transform container = null)
    {
        string basePath = AppResPath4Web + "Geo/Config/AppConfig.json";
        AssetManager.Instance.LoadText(basePath, onLoadAppConfigCallBack);
        clouldDic = new Dictionary<string, ClouldVO>();
        cloudNameList = new List<string>();
    }

    private void onLoadAppConfigCallBack(string appConfigData)
    {
        JsonData appConfigJD = JsonMapper.ToObject(appConfigData);
        JsonData cloudJD = appConfigJD["Clouds"];
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
            clouldDic.Add(clouldVO.name ,clouldVO);
            cloudNameList.Add(clouldVO.name);
        }
    }

    public ClouldVO GetClouldVO(string cloudName)
    {
        ClouldVO clouldVO = null;
        clouldDic.TryGetValue(cloudName, out clouldVO);
        return clouldVO;
    }

    public void OnQuit()
    {
        
    }

    /// <summary>
    /// 应用程序内部资源路径存放路径(www/webrequest专用)
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
