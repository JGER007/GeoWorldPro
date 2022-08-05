//***************************************************
// Des：BoboAR
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2020/12/16 17:12:04
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *Copyright(C) 2015 by JJ 
 *All rights reserved. 
 *FileName:     GameObjectUtil.cs 
 *Author:       蒋坚 
 *EMAIL:       270611115@qq.com 
 *PHONE:       17321367005 
 *Version:      2.0 
 *UnityVersion：2018.1.9f2 
 *Date:         2020-05-31 
 *Description:    
 *History: 
*/
namespace com.frame
{
    public class GameObjectUtil
    {
        //缓存加载对象
        private static Dictionary<string, GameObject> GameObjectPerfabDic = new Dictionary<string, GameObject> ();

        /// <summary> 创建Resoure对象</summary>
        /// <param name="path"></param>
        public static GameObject CreateResoureObj (string path, Transform parent = null)
        {
            GameObject loadobj;
            GameObjectPerfabDic.TryGetValue (path, out loadobj);
            if (loadobj == null)
            {
                loadobj = Resources.Load (path) as GameObject;
                GameObjectPerfabDic.Add (path, loadobj);
            }
            return InstantiateObj (loadobj, parent);
        }

        /// <summary> 创建对象</summary>
        /// <param name="name"></param>
        public static GameObject InstantiateObj (GameObject target, Transform parent = null)
        {
            GameObject obj = Object.Instantiate (target);
            if (parent != null)
            {
                obj.transform.SetParent (parent);
            }
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            return obj;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        public static Transform GetChild (GameObject root, string path)
        {
            Transform tra = root.transform.Find (path);
            if (tra == null) Debug.Log (path + "not find");
            return tra;
        }

        /// <summary>
        /// 获取子节点组件
        /// </summary>
        public static T GetChildComponent<T> (GameObject root, string path) where T : Component
        {
            Transform tra = root.transform.Find (path);
            if (tra == null) Debug.Log (path + "not find");
            T t = tra.GetComponent<T> ();
            return t;
        }
    }
}
