using com.frame.ui;
using UnityEngine;
using UnityEngine.UI;

public class LoadFromLocalUI : PopUpUI
{
    private Text text;
    private string baseContent = "资源准备中";
    private int count = 0;
    private float dt;

    void Start ()
    {
        Init ();
    }

    private void OnDisable ()
    {
        Init ();
    }

    private void Init ()
    {
        if (text == null)
        {
            text = transform.Find ("Text").GetComponent<Text> ();
        }
        text.text = baseContent;
        dt = 0f;
        count = 0;
    }

    void Update ()
    {
        dt = dt + Time.deltaTime;
        if (dt > 0.1f)
        {
            dt = 0f;
            count = count + 1;
            if (count > 3)
            {
                count = 0;
                text.text = baseContent;
            }
            else
            {
                text.text = text.text + ".";
            }
        }

    }

}
