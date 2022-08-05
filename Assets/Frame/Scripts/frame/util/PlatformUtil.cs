//***************************************************
// Des：平台工具类
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2021/01/18 11:15:00
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformUtil
{
    SceneManager dd;
    private static string platformStr = "";
    public static string GetPlatform()
    {
        if (!string.IsNullOrEmpty(platformStr))
        {
            return platformStr;
        }
        Debug.Log("Application.platform:---------" + Application.platform);

#if UNITY_ANDROID
        platformStr = "Android";
#elif UNITY_IPHONE
       platformStr = "iOS";
#elif UNITY_STANDALONE_WIN
        platformStr = "StandaloneWindows64";
#else
        platformStr = "Editor";
#endif

        //if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        //{
        //    platformStr = "Editor";
        //    Debug.Log ("这是Windows编辑器模式。。。");
        //}
        //if (Application.platform == RuntimePlatform.IPhonePlayer) // 使用Unity切换Platform无法模拟
        //{
        //    platformStr = "IOS";
        //    Debug.Log ("这是iPhone平台。。。");
        //}
        //if (Application.platform == RuntimePlatform.Android)// 使用Unity切换Platform无法模拟
        //{
        //    platformStr = "Android";
        //    Debug.Log ("这是安卓平台。。。");
        //}

        return platformStr;
    }
}
