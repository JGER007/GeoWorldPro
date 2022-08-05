//***************************************************
// Des：BoboAR
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2021/05/27 10:45:55
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
using System.Collections.Generic;
using System.IO;
using com.frame;
using LitJson;
using UnityEngine;

public class ConfigManager : Singleton<ConfigManager>, IManager
{
    //ui配置信息字典
    private Dictionary<string, UIConfigVO> uiConfig = null;
    //模块配置信息字典
    private Dictionary<string, ModuleConfigVO> moduleConfig = null;
    public Dictionary<string, ModuleConfigVO> ModuleConfig { get { return moduleConfig; } }

    //加载网络资源根路径
    public static string BaseNetPath = "http://boboother.oss-cn-beijing.aliyuncs.com/AR/";

    /// <summary>初始化配置文件</summary>
    /// <param name="container"></param>
    public void InitManager (Transform container = null)
    {
        /**
        if (Facade.isLocalResources)
        {
            AssetManager.Instance.LoadText (BaseNetPath + "Config/ARConfigTest.json", loadTextCallBack);
        }
        else
        {
            //初始化UI配置文件
            ////https://boboother.oss-cn-beijing.aliyuncs.com/AR/Config/ARConfig.json
            AssetManager.Instance.LoadText (BaseNetPath + "Config/ARConfig.json", loadTextCallBack);
        }*/
        string config = Resources.Load<TextAsset> ("Config/Config").text;
        loadTextCallBack (config);
    }

    /// <summary>加载配置文件回调</summary>
    private void loadTextCallBack (string rawconfig)
    {
        ConfigInfo config = JsonUtility.FromJson<ConfigInfo> (rawconfig);
        initUIConfig (config.ui);
        initModuleConfig (config.module);
        AssetManager.Instance.CleanOldVersionAsset (moduleConfig);
        //设置配置文件初始化完成
        Model.InitConfigOK ();
    }

    /// <summary> 初始化ui配置信息</summary>
    private void initUIConfig (List<UIConfigVO> uiList)
    {
        uiConfig = new Dictionary<string, UIConfigVO> ();
        foreach (UIConfigVO uIConfigVO in uiList)
        {
            uiConfig.Add (uIConfigVO.PerfabName, uIConfigVO);
            IsAssetExists (uIConfigVO);
        }
    }

    public ModuleConfigVO GetModuleConfigVO (string moduleName)
    {
        if (moduleConfig != null && moduleConfig.ContainsKey (moduleName))
        {
            return moduleConfig [moduleName];
        }
        return null;
    }

    /// <summary> 初始化模块配置信息</summary>
    private void initModuleConfig (List<ModuleConfigVO> modules)
    {
        moduleConfig = new Dictionary<string, ModuleConfigVO> ();
        foreach (ModuleConfigVO moduleConfigVO in modules)
        {
            //moduleConfigVO.assetbundle = moduleConfigVO.assetbundle.ToLower();
            moduleConfigVO.uiConfigVO = GetUIConfigVO (moduleConfigVO.ui);
            moduleConfig.Add (moduleConfigVO.name, moduleConfigVO);
        }
    }

    public void OnQuit ()
    {
        if (uiConfig != null)
        {
            uiConfig.Clear ();
        }
        if (moduleConfig != null)
        {
            moduleConfig.Clear ();
        }
    }

    public UIConfigVO GetUIConfigVO (string perfabName)
    {
        if (uiConfig != null && uiConfig.ContainsKey (perfabName))
        {
            return uiConfig [perfabName];
        }
        return null;
    }

    /// <summary>判断UI资产是否存在</summary>
    private void IsAssetExists (UIConfigVO uIConfigVO)
    {
#if !UNITY_STANDALONE && UNITY_EDITOR
        if (!string.IsNullOrEmpty (uIConfigVO.Path))
        {
            string curPath = Application.dataPath + "/Resources/" + uIConfigVO.Path + ".prefab";
            if (!File.Exists (curPath))
                Debug.LogError (curPath + "资产不存在--------请检查" + uIConfigVO.PerfabName);
        }
        else if (!string.IsNullOrEmpty (uIConfigVO.assetbundle))
        {
            string curPath = AssetConfigManager.GetLoadAssetRootPath () +  uIConfigVO.assetbundle.ToLower();
#if UNITY_IPHONE
            curPath = Application.streamingAssetsPath + "/AssetBundles/iOS/" + uIConfigVO.assetbundle;
#endif

            if (Facade.isLocalResources && !File.Exists (curPath))
            {
                Debug.LogError (curPath + "资产不存在--------请检查" + uIConfigVO.PerfabName);
            }
        }
        else
        {
            Debug.LogError ("地址未分配--------请检查" + uIConfigVO.PerfabName);
        }
#endif
    }
}

#region 数据类
[System.Serializable]
public class ConfigInfo
{
    public List<ModuleConfigVO> module = new List<ModuleConfigVO> ();
    public List<UIConfigVO> ui = new List<UIConfigVO> ();
}


[System.Serializable]
public class UIConfigVO
{
    /// <summary>UI预制体名称 </summary>
    public string PerfabName;
    /// <summary>描述 </summary>
    public string Desc;
    /// <summary>UI预制文件保存路径（为空的情况下不存在Resoures资源）</summary>
    public string Path;
    /// <summary>模块assetbundle资源（为空的情况下不存在AssetBundle资源）</summary>
    public string assetbundle;
    /// <summary>线上资源版本</summary>
    public float version;
    /// <summary>UI所属界面类型  0 固定界面（Fixed）、1 普通界面（Normal）、2 弹出界面（Popup）</summary>
    public int Type;
}


[System.Serializable]
public class ModuleConfigVO
{
    /// <summary>模块名称</summary>
    public string name;
    /// <summary>模块展示名称</summary>
    public string showname;
    /// <summary>模块描述信息</summary>
    public string desc;
    /// <summary>模块UI界面 （为空的情况下不存在ui界面）</summary>
    public string ui;
    /// <summary>模块assetbundle资源（为空的情况下不存在AssetBundle资源）</summary>
    public string assetbundle;
    /// <summary>线上资源版本</summary>
    public float version;
    /// <summary>模块状态 0 模块不可用  1 模块可用</summary>
    public int status;

    /// <summary>模块UI配置信息</summary>
    public UIConfigVO uiConfigVO;
    /// <summary>获取资源保存在本地的文件名称</summary>
    public string GetAssetLocalName ()
    {
        return assetbundle + "_" + version;
    }
}

#endregion