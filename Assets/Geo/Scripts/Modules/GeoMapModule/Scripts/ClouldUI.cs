using com.frame;
using com.frame.ui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WPM;

public class ClouldUI : BaseUI
{
    [SerializeField]
    private InputField testInputField;

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private Image clouldImg;

    private Dictionary<string, Sprite> cloudSp = null;

    private WorldMapGlobe worldMapGlobe;

    private ClouldVO currClouldVO;
    public override void InitUI()
    {
        cloudSp = new Dictionary<string, Sprite>();
        content.transform.Find("BackBtn").GetComponent<Button>().onClick.AddListener(delegate { onBack(); });
        content.transform.Find("NextBtn").GetComponent<Button>().onClick.AddListener(delegate { onNext(); });
        content.transform.Find("PreBtn").GetComponent<Button>().onClick.AddListener(delegate { onPre(); }); 

        content.SetActive(false);
        worldMapGlobe = FindObjectOfType<WorldMapGlobe>();
        testInputField.onEndEdit.AddListener(delegate { onEndEdit(); });
    }

    private void onEndEdit()
    {
        float value = float.Parse(testInputField.text);
        Debug.Log("value:" + value);
        worldMapGlobe.ZoomTo(value,0.1F);
    }

    private void onPre()
    {
        content.SetActive(false);
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, "EarthCloud", "Pre");
    }

    private void onNext()
    {
        content.SetActive(false);
        EventUtil.DispatchEvent(GlobalEvent.UI_TO_Module_Action, "EarthCloud", "Next");
    }

    private void onBack()
    {
        content.SetActive(false);
        EventUtil.DispatchEvent(GlobalEvent.Module_TO_UI_Action, "Style", StyleEnum.Œ¿–«µÿÕº) ;
    }

    public void ShowClould(ClouldVO clouldVO)
    {
        currClouldVO = clouldVO;
        
        Sprite sprite = null; 
        cloudSp.TryGetValue(clouldVO.path,out sprite);
        if(sprite == null)
        {
            clouldImg.sprite = null;
            AssetManager.Instance.LoadTexture(clouldVO.path, onLoadTextureComplete);
            
        }
        else
        {
            showClouldImg(sprite);
        }
    }

    private void showClouldImg(Sprite sp)
    {
        clouldImg.sprite = sp;
        content.SetActive(true);
    }

    private void onLoadTextureComplete(Texture2D t)
    {
        Sprite sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height),Vector2.zero, 1f);
        cloudSp.Add(currClouldVO.path, sprite);
        showClouldImg(sprite);
    }

    public void HideClould()
    {
        content.SetActive(false);
    }
}

