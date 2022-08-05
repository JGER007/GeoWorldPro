//***************************************************
// Des：BoboAR
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2020/12/22 18:35:19
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************

using UnityEngine;
public interface IManager
{
    /// <summary> 初始化</summary>
    void InitManager(Transform container = null);
    /// <summary> 提出 </summary>
    void OnQuit();
}
