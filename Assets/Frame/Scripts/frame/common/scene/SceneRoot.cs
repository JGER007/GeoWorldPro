//***************************************************
// Des：BoboAR
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2020/12/23 10:59:55
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using com.frame;
using UnityEngine;

public class SceneRoot : MonoSingleton<SceneRoot>
{
    /// <summary>场景根容器</summary>
    [SerializeField]
    private Transform sceneContainer = null;

    /// <summary>获取场景容器</summary>
    public Transform GetSceneRoot ()
    {
        return sceneContainer;
    }
}
