
using com.frame;
using com.frame.ui;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModuleUI : MonoBehaviour
{
    [HideInInspector]
    //UI类型
    public UITypeEnum UiType;
    [HideInInspector]
    //UI对应预制体名称
    public string UIPerfabName;
    //退出按钮
    protected GameObject backButton;

    //用来UI界面操作与Modulec场景模块交互
    public Action<string> UIToModuleAction;

    public virtual void InitUI ()
    {
        Transform backButtonTran = transform.Find ("BackButton");
        if (backButtonTran)
        {
            backButton = backButtonTran.gameObject;
            backButton.SetActiveObj (true);
            SetBtnEvent (backButton, delegate
            {
                EventUtil.DispatchEvent (GlobalEvent.Quit_Module);  //回到主场景
            });
        }

    }


    protected void SetBtnEvent (GameObject obj, UnityAction action)
    {
        Button btn = obj.GetComponent<Button> ();
        btn.onClick.RemoveAllListeners ();
        btn.onClick.AddListener (action);
    }

    //UI退出
    public virtual void OnQuit ()
    {
        Destroy (gameObject);
    }
}
