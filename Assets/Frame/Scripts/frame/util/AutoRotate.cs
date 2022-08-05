//***************************************************
// Des：BoboAR
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2020/12/16 16:06:14
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float RoateSpeed = 1;
    void Update ()
    {
        //自转
        transform.Rotate (Vector3.up, Space.Self);
    }

}
