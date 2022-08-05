//***************************************************
// Des：UI管理类
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2020/12/22 17:40:26
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.frame.ui
{
    public class UIManager : Singleton<UIManager>, IManager
    {
        public static string ModuleTranstionUI = "ModuleTranstionUI";
        public static string LoadFromLocalUI = "LoadFromLocalUI";
        public static string LoadingModuleUI = "LoadingModuleUI";
        public static string MsgItem = "MsgItem";
        //产生的UI实例
        private Dictionary<string, GameObject> uIDic = null;


        public void InitManager (Transform container = null)
        {
            //初始化UI容器
            UIRoot.Instance.InitUIRoot ();
            uIDic = new Dictionary<string, GameObject> ();
            EventUtil.AddListener (GlobalEvent.Open_UI, onOpenUI);
            EventUtil.AddListener (GlobalEvent.Close_UI, onCloseUI);
            EventUtil.AddListener (GlobalEvent.Show_Msg, onShowMsg);
        }

        /// <summary>
        /// 展示提示信息
        /// </summary>
        /// <param name="eventArgs"></param>
        private void onShowMsg (CustomEventArgs eventArgs)
        {
            string msg = eventArgs.args [0].ToString ();
            showMsg (msg);
        }

        private void onCloseUI (CustomEventArgs eventArgs)
        {
            string uiName = eventArgs.args [0].ToString ();
            if (uIDic.ContainsKey (uiName))
            {
                uIDic [uiName].SetActive (false);
            }
        }

        private void onOpenUI (CustomEventArgs eventArgs)
        {
            string uiName = eventArgs.args [0].ToString ();
            GameObject ui = null;
            uIDic.TryGetValue (uiName, out ui);

            if (ui == null)
            {
                Action<GameObject> backCall = (obj) =>
                {
                    BaseUI baseUI = obj.GetComponent<BaseUI> ();
                    if (baseUI != null)
                    {
                        baseUI.InitUI ();
                    }
                };
                LoadConfigUI (uiName, backCall, true);
            }
            else
            {
                ui.SetActiveObj (true);
            }
        }

        public void OnQuit ()
        {
            EventUtil.RemoveListener (GlobalEvent.Open_UI, onOpenUI);
            EventUtil.RemoveListener (GlobalEvent.Close_UI, onCloseUI);
            EventUtil.RemoveListener (GlobalEvent.Show_Msg, onShowMsg);

            if (uIDic != null)
            {
                foreach (GameObject ui in uIDic.Values)
                {
                    if (ui && ui.GetComponent<BaseUI> ())
                    {
                        ui.GetComponent<BaseUI> ().OnQuitGame ();
                    }
                }
            }
        }

        /// <summary>获取UI</summary>
        /// <param name="uiConfigName">UI 配置名称 不支持重名</param>
        /// <returns></returns>
        public void GetUI (UIConfigVO config, Action<GameObject> m_backCall)
        {
            string uiConfigName = config.PerfabName;
            GameObject ui = null;
            if (uIDic.ContainsKey (uiConfigName))
            {
                ui = uIDic [uiConfigName];
                if (ui != null)
                {
                    ui.SetActiveObj (true);
                    m_backCall (ui);
                    return;
                }
                else
                {
                    uIDic.Remove (uiConfigName);
                }
            }

            Transform parent = config.Type >= 0 ? UIRoot.Instance.GetUIRootContainer ((UITypeEnum)config.Type) : UIRoot.Instance.msgUI;
            Action<GameObject> backCall = (obj) =>
            {
                GameObject curUi = GameObjectUtil.InstantiateObj (obj, parent);
                curUi.name = uiConfigName;
                uIDic.Add (uiConfigName, curUi);
                m_backCall (curUi);
            };
            AssetManager.Instance.LoadAssetPerfab (new AssetVO (config), backCall);
        }


        /// <summary> 获取UI</summary>
        /// <param name="uiConfigName">UI 配置名称 不支持重名</param>
        /// <returns></returns>
        public GameObject GetUI (UIConfigVO config)
        {
            string uiConfigName = config.PerfabName;
            GameObject ui;
            if (uIDic.ContainsKey (uiConfigName))
            {
                ui = uIDic [uiConfigName];
                if (ui != null)
                {
                    ui.SetActiveObj (true);
                    return ui;
                }
                else
                {
                    uIDic.Remove (uiConfigName);
                }
            }
            Transform parent = config.Type >= 0 ? UIRoot.Instance.GetUIRootContainer ((UITypeEnum)config.Type) : UIRoot.Instance.msgUI;
            //Debug.Log ("CreateResoureObj config.Path:" + config.Path);
            ui = GameObjectUtil.CreateResoureObj (config.Path, parent);
            if (ui != null)
            {
                ui.name = uiConfigName;
                uIDic.Add (uiConfigName, ui);
            }
            return ui;
        }

        /// <summary> 设置为全屏UI </summary>
        private void SetFullScreenUI (GameObject ui)
        {
            ui.GetComponent<RectTransform> ().offsetMin = Vector2.zero;
            ui.GetComponent<RectTransform> ().offsetMax = Vector2.zero;
        }

        /// <summary>加载ConfigUI </summary>
        public void LoadConfigUI (string popUpUIName, Action<GameObject> m_backCall, bool isFull = false)
        {
            UIConfigVO config = ConfigManager.Instance.GetUIConfigVO (popUpUIName);
            if (!string.IsNullOrEmpty (config.assetbundle))
            {
                Action<GameObject> backCall = (ui) =>
                {
                    if (isFull)
                    {
                        SetFullScreenUI (ui);
                    }
                    m_backCall (ui);
                };
                GetUI (config, backCall);
            }
            else if (!string.IsNullOrEmpty (config.Path))
            {
                GameObject popUpUI = GetUI (config);
                if (isFull)
                {
                    SetFullScreenUI (popUpUI);
                }
                m_backCall (popUpUI);
            }
            else
            {
                Debug.LogError ("错误------prefab和assetbundle都不存在");
            }
        }

        //展示提示信息
        private void showMsg (string msg)
        {
            //Debug.Log ("showMsg:" + msg);
            Action<GameObject> backCall = (item) =>
            {
                item.SetActive (true);
                item.transform.Find ("Text").GetComponent<Text> ().text = msg;
                Timer.CreateTimer ("Toast_Timer").StartTiming (2, () =>
                  {
                      if (item != null) { GlobalMonoTool.DestroyImmediate (item); }
                  });
            };
            LoadConfigUI (MsgItem, backCall, true);
        }
    }
}

