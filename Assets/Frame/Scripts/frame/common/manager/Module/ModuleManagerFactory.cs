//***************************************************
// Des：放置目标对象工厂
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2020/12/28 17:25:12
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using System;
using System.Collections.Generic;
using com.frame;
using UnityEngine;

public class ModuleManagerFactory : Singleton<ModuleManagerFactory>
{
    /// <summary> 创建模块 </summary>
    public static BaseModuleManager CreateModule (ModuleConfigVO configVO)
    {
        ModuleName curState = (ModuleName)Enum.Parse (typeof (ModuleName), configVO.name, true);
        BaseModuleManager baseModuleManager = null;

        switch (curState)
        {
        case ModuleName.example:
            baseModuleManager = new ExampleModuleManager ();
            break;
            case ModuleName.geomap:
                baseModuleManager = new GeoMapModuleManager();
                break;
        }

        if (baseModuleManager != null)
        {
            AssetManager.Instance.LoadAssetPerfab (new AssetVO (configVO), baseModuleManager.StartModule);
        }
        return baseModuleManager;
    }

    /// <summary>App状态类型</summary> 
    public enum ModuleName
    {
        none,
        example,
        geomap
    }
}
