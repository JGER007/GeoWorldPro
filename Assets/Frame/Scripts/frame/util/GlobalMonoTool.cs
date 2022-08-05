//***************************************************
// Des：全局Mono工具
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2020/12/28 10:54:03
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************

using UnityEngine;
using com.frame;

public class GlobalMonoTool : MonoSingleton<GlobalMonoTool>
{

    public void DestroyTarget (Object obj)
    {
        Destroy (obj);
    }
}
