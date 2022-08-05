//***************************************************
// Des：资源配置管理
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2021/01/19 15:28:38
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using System;
using UnityEngine;

namespace com.frame
{
    public class AssetConfigManager
    {
        //加载资源根路径
        private static string loadAssetBasePath = "";

        /// <summary>
        /// 获取加载资源根路径
        /// </summary>
        /// <returns></returns>
        public static string GetLoadAssetRootPath ()
        {
            if (string.IsNullOrEmpty (loadAssetBasePath))
            {
                if (Facade.isLocalResources)
                {
                    loadAssetBasePath = GetRelativePath () + "assets.res.";
                }
                else
                {
                    loadAssetBasePath = ConfigManager.BaseNetPath + "AssetBundles/" + PlatformUtil.GetPlatform () + "/";
                }
            }
            return loadAssetBasePath;
        }

        public static string GetRelativePath ()
        {
            //loadAssetBasePath = ConfigManager.BaseNetPath + "AssetBundles/" + PlatformUtil.GetPlatform() + "/";
#if UNITY_ANDROID
            if (Application.isMobilePlatform || Application.isConsolePlatform)
            {
                return Application.streamingAssetsPath + "/AssetBundles/Android/";
            }
            else
            {
                return Application.dataPath + "/StreamingAssets/AssetBundles/Android/";
            }
#elif UNITY_IPHONE
            if (Application.isMobilePlatform || Application.isConsolePlatform)
            {
                return "file:///" + Application.streamingAssetsPath + "/AssetBundles/iOS/";
            }
            else
            {
                return "file:///" + Application.streamingAssetsPath + "/AssetBundles/iOS/";
            }

#elif UNITY_STANDALONE_WIN
            //return "file://" + Application.streamingAssetsPath + "/AssetBundles/Editor/";
            return Application.streamingAssetsPath + "/AssetBundles/StandaloneWindows64/";
#else
            return "file://" + Application.streamingAssetsPath + "/AssetBundles/Editor/";
            //return "file://" + Application.streamingAssetsPath + "/";
#endif
        }
    }

}
