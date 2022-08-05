//***************************************************
// Des：资源信息
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2021/01/19 15:18:47
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using System;
using System.IO;
using UnityEngine;

namespace com.frame
{
    public class AssetVO
    {
        /// <summary> 描述</summary>
        public string Desc;
        /// <summary>预制名称</summary>
        public string PerfabName;
        /// <summary>资源名称</summary>
        public string AssetName;
        /// <summary>资源路径</summary>
        public string AssetPath;
        //资源类型</summary>
        public string Type = "Perfab";
        /// <summary>资源加载路径</summary>
        public string Path;
        /// <summary>资源加载完成毁掉</summary>
        public Action<GameObject> CallBack;
        /// <summary>资源版本信息</summary>
        public float Version = 0;
        /// <summary>请求返回</summary>
        public AssetBundle Asset = null;
        /// <summary>加载错误信息 为空标识加载成功</summary>
        public string LoadErrorInfo = "";
        /// <summary>是否显示loading</summary>
        public bool ShowLoading = false;

        /// <summary>showLoading 是否显示loading</summary>
        public AssetVO (UIConfigVO config, bool showLoading = false)
        {
            Desc = config.Desc;
            PerfabName = config.PerfabName;
            AssetPath = config.assetbundle;
            AssetName = config.assetbundle.ToLower();
            Version = config.version;
            ShowLoading = showLoading;
        }

        public AssetVO(ModuleConfigVO config, bool showLoading = true)
        {
            Desc = config.desc;
            PerfabName = config.name;
            AssetPath = config.assetbundle;
            AssetName = config.assetbundle.ToLower();
            Version = config.version;
            ShowLoading = showLoading;
        }

        /// <summary> perfabCallBack 回调</summary>
        public void RegisterInfo (Action<GameObject> m_CallBack)
        {
            if (IsExitLocal ())
            {
                Path = GetAssetLocalPath ();
            }
            else
            {
                Path = AssetConfigManager.GetLoadAssetRootPath () + AssetName;
            }
            CallBack = m_CallBack;
        }

        public AssetVO (string desc, string perfabName, string assetName, float version, string path, bool showLoading, Action<GameObject> m_CallBack)
        {
            Desc = desc;
            PerfabName = perfabName;
            AssetName = assetName;
            Path = path;
            Version = version;
            CallBack = m_CallBack;
            ShowLoading = showLoading;
        }


        /// <summary> 保存资产到本地 </summary>
        public void SaveLoadedAsset (byte [] data)
        {
            ///用于测试
            string dir = localRootPath () + Type;
            if (!Directory.Exists (dir))
            {
                Directory.CreateDirectory (dir);
            }
            File.WriteAllBytes (GetAssetLocalPath (), data);
        }

        /// <summary>是否存在本地 </summary>
        public bool IsExitLocal ()
        {
            return File.Exists (GetAssetLocalPath ());
        }

        /// <summary>获取Asset本地地址 </summary>
        public string GetAssetLocalPath ()
        {
            string path = localRootPath () + Type + "/" + AssetName + "_" + Version;
            return path;
        }

        /// <summary>局部根路径 </summary>
        private static string localRootPath ()
        {
            return Application.persistentDataPath + "/AssetBundles/";
        }
    }
}
