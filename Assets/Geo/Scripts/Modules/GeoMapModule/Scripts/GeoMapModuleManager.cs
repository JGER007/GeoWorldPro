using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoMapModuleManager : BaseModuleManager
{
    public GeoMapModuleFacade geoMapModuleFacade;

    /// <summary>
    /// ģ��׼�����
    /// </summary>
    /// <param name="flag">�豸֧��AR���ܱ�ʶ</param>
    /// <param name="target"></param>
    public override void OnModuleReady(GameObject target)
    {
        base.OnModuleReady(target);

        //��ʼ������Ŀ�����
        if (sceneTarget == null)
        {
            sceneTarget = initSceneTarget(target);
            if (sceneTarget)
            {
                geoMapModuleFacade = sceneTarget.GetComponent<GeoMapModuleFacade>();
                geoMapModuleFacade.InitModuleFacade();
                initModuleUI();
            }

        }
    }

    /// <summary>
    /// ��ʼ��ģ��UI
    /// </summary>
    private void initModuleUI()
    {
        if (moduleUIManager == null)
        {
            moduleUIManager = new GeoMapMainUIManager();
            moduleUIManager.InitManager();
        }
    }

    //�˳�ģ�鳡��
    protected override void quitModuleManager()
    {
        if (moduleUIManager == null)
        {
            moduleUIManager.OnQuit();
        }
    }
}
