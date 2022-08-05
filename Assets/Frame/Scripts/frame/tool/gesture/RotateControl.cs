//***************************************************
// Des：单点触发旋转（真实模型旋转）
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2021/01/28 18:34:23
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.frame.tool
{
    public class RotateControl : MonoBehaviour
    {
        private float roateSpeed = 500;
        void Update ()
        {
            if (Input.touchCount == 1)
            {
                //判断是否点击在UI上 点击目标是UI退出
#if UNITY_ANDROID || UNITY_IPHONE
                if (EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId))
#else
                        if (EventSystem.current.IsPointerOverGameObject ())
#endif
                {
                    return;
                }


                Touch touch = Input.GetTouch (0);
                //触摸为移动类型
                if (touch.phase == TouchPhase.Moved)
                {
                    try
                    {
                        float XX = Input.GetAxis ("Mouse X");
                        //判断左右滑动的距离与上下滑动距离大小
                        //单指向左滑动情况
                        if (XX < 0)
                        {
                            transform.Rotate (Vector3.up, roateSpeed * Time.deltaTime, Space.World);
                        }
                        //单指向右滑动情况
                        if (XX > 0)
                        {
                            transform.Rotate (-Vector3.up, roateSpeed * Time.deltaTime, Space.World);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log (e.ToString ());
                    }
                }
            }
        }
    }
}