//***************************************************
// Des：提示信息管理类
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2021/01/28 14:43:01
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace com.frame
{
    public class MsgManager : Singleton<MsgManager>
    {
        private static Dictionary<string, string> msgDic = null;

        public static void InitMsgConfig (JsonData msgJsonData)
        {
            msgDic = new Dictionary<string, string> ();
            /*
            IDictionary dict = msgJsonData as IDictionary;

            foreach (string key in dict.Keys)
            {
                Debug.Log("KEY:" + key);
                msgDic.Add (key, dict [key].ToString ());
            }*/
        }

        /// <summary>
        /// 展示提示信息
        /// </summary>
        /// <param name="msgKey">提示信息注册key</param>
        public static void ShowMsg (string msgKey)
        {
            if (msgDic.ContainsKey (msgKey))
            {
                EventUtil.DispatchEvent (GlobalEvent.Show_Msg, msgDic [msgKey]);
            }
        }

        /// <summary>
        /// 展示提示信息
        /// </summary>
        /// <param name="msgKey">提示信息注册key</param>
        public static void ShowMsgContent (string content)
        {
            if (!string.IsNullOrEmpty (content))
            {
                EventUtil.DispatchEvent (GlobalEvent.Show_Msg, content);
            }
        }
    }
}

