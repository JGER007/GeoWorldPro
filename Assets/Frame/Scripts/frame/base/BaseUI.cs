using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.frame.ui
{
    public class BaseUI : MonoBehaviour
    {
        /// <summary> UI类型 </summary>
        [HideInInspector]
        public UITypeEnum UiType;
        /// <summary>UI对应预制体名称</summary>
        [HideInInspector]
        public string UIPerfabName;

        /// <summary>UI初始化</summary>
        public virtual void InitUI ()
        {
        }

        /// <summary>UI退出</summary>
        public virtual void OnQuitGame ()
        {
            Destroy (gameObject);
        }

        /// <summary> 设置UI按钮事件</summary>
        public void SetBtnEvent (GameObject obj, UnityAction action)
        {
            Button btn = obj.GetComponent<Button> ();
            btn.onClick.RemoveAllListeners ();
            btn.onClick.AddListener (action);
        }

        /// <summary> 设置UI按钮事件</summary>
        public void SetBtnEvent (Transform tran, UnityAction action)
        {
            Button btn = tran.GetComponent<Button> ();
            btn.onClick.RemoveAllListeners ();
            btn.onClick.AddListener (action);
        }

    }
}
