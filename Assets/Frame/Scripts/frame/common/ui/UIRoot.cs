//***************************************************
// Des：UIRoot UI根容器管理
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2020/12/22 17:45:51
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using UnityEngine;
using System.Collections.Generic;

namespace com.frame.ui
{
    public class UIRoot : MonoSingleton<UIRoot>
    {
        [SerializeField]
        public Canvas canvas;
        //固定UI界面--如主界面 位于底层
        private Transform fixedUIContainer;
        //普通UI界面容器 位于中层
        private Transform normalUIContainer;
        //弹出界面UI容器 位于顶层
        private Transform popupUpUIContainer;
        //提示消息UI容器
        private Transform msgUIContainer;
        public Transform msgUI { get { return msgUIContainer; } }

        private Dictionary<UITypeEnum, Transform> uiDic = null;
        //初始化UI根节点
        public void InitUIRoot ()
        {
            Vector2 size = transform.parent.GetComponent<RectTransform> ().sizeDelta;
            GetComponent<RectTransform> ().sizeDelta = size;

            fixedUIContainer = transform.Find ("FixedUIContainer");
            fixedUIContainer.GetComponent<RectTransform> ().sizeDelta = size;

            normalUIContainer = transform.Find ("NormalUIContainer");
            normalUIContainer.GetComponent<RectTransform> ().sizeDelta = size;

            popupUpUIContainer = transform.Find ("PopUpUIContainer");
            popupUpUIContainer.GetComponent<RectTransform> ().sizeDelta = size;

            msgUIContainer = transform.Find ("MsgUIContainer");
            msgUIContainer.GetComponent<RectTransform> ().sizeDelta = size;



            uiDic = new Dictionary<UITypeEnum, Transform> ();
            uiDic.Add (UITypeEnum.Fixed, fixedUIContainer);
            uiDic.Add (UITypeEnum.Normal, normalUIContainer);
            uiDic.Add (UITypeEnum.PopupUp, popupUpUIContainer);
            uiDic.Add (UITypeEnum.Msg, msgUIContainer);
        }

        //获取UI根容器
        public Transform GetUIRootContainer (UITypeEnum uIType)
        {
            if (uiDic != null)
            {
                if (uiDic.ContainsKey (uIType))
                {
                    return uiDic [uIType];
                }
                else
                {
                    return normalUIContainer;
                }
            }
            return normalUIContainer;
        }

        public Canvas GetCanvas ()
        {
            return canvas;
        }
    }



    public enum UITypeEnum
    {
        Fixed,
        Normal,
        PopupUp,
        Msg
    }

}
