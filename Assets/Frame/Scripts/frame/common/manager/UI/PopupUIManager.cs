using System.Collections.Generic;
using com.frame;
using com.frame.ui;
using UnityEngine;
using System;

public class PopupUIManager : Singleton<PopupUIManager>, IManager
{
    private Dictionary<string, GameObject> popupUIs = new Dictionary<string, GameObject> ();

    public void InitManager (Transform container = null)
    {
        EventUtil.AddListener (GlobalEvent.On_Popup_UI, onPopUpUI);
        EventUtil.AddListener (GlobalEvent.Close_Popup_UI, onClosePopUpUI);
    }

    /// <summary>关闭弹出UI界面 </summary>
    /// <param name="eventArgs"></param>
    private void onClosePopUpUI (CustomEventArgs eventArgs)
    {
        string popUpUIName = eventArgs.args [0].ToString ();
        if (popupUIs.ContainsKey (popUpUIName))
        {
            popupUIs [popUpUIName].SetActiveObj (false);
        }
    }

    /// <summary> 获取弹出UI</summary>
    /// <param name="uiName">UI 配置名称</param>
    public GameObject GetPopUI (string uiName)
    {
        if (popupUIs != null && popupUIs.ContainsKey (uiName))
        {
            return popupUIs [uiName];
        }
        return null;
    }


    /// <summary> 弹出UI提示界面</summary>
    private void onPopUpUI (CustomEventArgs eventArgs)
    {
        string popUpUIName = eventArgs.args [0].ToString ();

        if(popUpUIName == UIManager.LoadingModuleUI)
        {
            return;
        }

        bool isFull = false;
        if (eventArgs.args.Length >= 2)
        {
            if (int.Parse (eventArgs.args [1].ToString ()) == 1)
            {
                //全屏处理
                isFull = true;
            }
        }
        Action<GameObject> backCall = (obj) =>
        {
            obj.SetActiveObj (true);
            PopUpUI popUpUI = obj.GetComponent<PopUpUI> ();
            if (popUpUI != null)
            {
                popUpUI.InitUI ();
            }
        };
        LoadPopupUI (popUpUIName, backCall, isFull);
    }

    /// <summary> 加载弹出UI界面</summary>
    private void LoadPopupUI (string popUpUIName, Action<GameObject> m_backCall, bool isFull = false)
    {
        if (popupUIs.ContainsKey (popUpUIName))
        {
            m_backCall (popupUIs [popUpUIName]);
        }
        else
        {
            Action<GameObject> backCall = (obj) =>
            {
                if (!popupUIs.ContainsKey (popUpUIName))
                {
                    popupUIs.Add (popUpUIName, obj);
                }
                m_backCall (obj);
            };
            UIManager.Instance.LoadConfigUI (popUpUIName, backCall, isFull);
        }
    }

    public void OnQuit ()
    {
        EventUtil.RemoveListener (GlobalEvent.Close_Popup_UI, onClosePopUpUI);
        EventUtil.RemoveListener (GlobalEvent.On_Popup_UI, onPopUpUI);

        if (popupUIs != null)
        {
            foreach (GameObject popUpUI in popupUIs.Values)
            {
                if (popUpUI.GetComponent<PopUpUI> ())
                {
                    popUpUI.GetComponent<PopUpUI> ().OnQuitGame ();
                }
            }
            popupUIs.Clear ();
        }

    }
}
