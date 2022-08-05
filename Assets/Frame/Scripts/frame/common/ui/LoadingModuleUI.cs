using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using com.frame.ui;
using com.frame;

public class LoadingModuleUI : PopUpUI
{
    private Slider loadingSlider;
    private Text progressTxt;
    private Text nameTxt;
    private UnityWebRequest unityWebRequest = null;
    //UI初始化
    public override void InitUI ()
    {
        RectTransform rt = GetComponent<RectTransform> ();
        rt.sizeDelta = rt.parent.GetComponent<RectTransform> ().sizeDelta;
        rt.localPosition = Vector3.zero;
        rt.localEulerAngles = Vector3.zero;
        rt.offsetMax = Vector2.zero;
        rt.offsetMin = Vector2.zero;
        //rt.localScale = Vector3.one;

        loadingSlider = transform.Find ("Slider").GetComponent<Slider> ();
        progressTxt = transform.Find ("ProgressTxt").GetComponent<Text> ();
        nameTxt = transform.Find ("NameTxt").GetComponent<Text> ();
        EventUtil.AddListener (GlobalEvent.Show_Loading_Progress, onShowLoadingContent);
    }

    /// <summary>展示加载内容</summary>
    /// <param name="eventArgs 参数两个时开始显示过渡条，参数一个时关闭过渡条"></param>
    private void onShowLoadingContent (CustomEventArgs eventArgs)
    {
        //Debug.Log ("onShowLoadingContent eventArgs.args.Length:" + eventArgs.args.Length);
        if (eventArgs.args.Length > 0)
        {
            string content = eventArgs.args [0].ToString ();
            if (eventArgs.args.Length == 1)
            {
                //内容为空--加载完成
                stopLoading ();
                unityWebRequest = null;
            }
            else if (eventArgs.args.Length == 2)
            {
                //开始加载
                startLoading (content);
                unityWebRequest = eventArgs.args [1] as UnityWebRequest;
            }
        }
    }

    void Update ()
    {
        if (unityWebRequest != null)
        {
            if (unityWebRequest.downloadProgress < 1)
            {
                showProcess (unityWebRequest.downloadProgress);
            }
            else
            {
                unityWebRequest = null;
                nameTxt.text = "正在解压资源";
            }

        }
    }

    //开始加载
    private void startLoading (string namestr)
    {
        //Debug.Log("startLoading namestr:" + namestr);
        nameTxt.text = "正在加载资源:" + namestr;
        showProcess (0);
    }

    //展示加载进度
    public void showProcess (float progress)
    {
        if (progress < 0.01f)
        {
            progress = 0.01f;
        }
        loadingSlider.value = progress;
        //progressTxt.text = string.Format ("加载进度: {0} %", ((int)(progress * 100)));
        progressTxt.text = ((int)(progress * 100)) + " %";//"加载进度:" +
    }

    //结束加载
    public void stopLoading ()
    {
        progressTxt.text = "100%";//加载完成
        Invoke ("delayHide", 0.1f);
    }

    private void delayHide ()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive (false);
        }
    }

    public override void OnQuitGame ()
    {
        base.OnQuitGame ();
        EventUtil.RemoveListener (GlobalEvent.Show_Loading_Progress, onShowLoadingContent);
    }
}
