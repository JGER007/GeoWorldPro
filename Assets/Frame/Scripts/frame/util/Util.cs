using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.frame
{
    public static class Util
    {
        public static void SetActiveObj (this GameObject obj, bool isActive)
        {
            if (obj.activeSelf != isActive)
            {
                obj.SetActive (isActive);
            }
        }

        public static void SetParent (this Transform obj, Transform parent, Vector3 localPos)
        {
            if (parent != null)
            {
                obj.SetParent (parent);
                obj.localPosition = localPos;
            }
            else
            {
                obj.position = localPos;
            }
            obj.localScale = Vector3.one;
            obj.localEulerAngles = Vector3.zero;
        }

        public static Vector3 InputPostion
        {
            get
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    return Input.mousePosition;
                }
                else
                {
                    if (Input.touchCount <= 0)
                    {
                        return Vector3.zero;
                    }
                    else
                    {
                        return Input.touches [0].position;
                    }
                }

                //else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                //#if UNITY_IOS || UNITY_ANDROID
                //#else
                //#endif

                //#if UNITY_EDITOR
                //if (Input.touchCount == 1) //单点触碰移动摄像机
                //{
                //if (Input.touches[0].phase == TouchPhase.Ended)
                //{
                //}
                //if (Input.touches[0].phase == TouchPhase.Began)
                //{

                //}
                //if (Input.touches[0].phase == TouchPhase.Moved)
                //{
                //}
                //}

                //Util.InputPostion
            }
        }
    }
}
