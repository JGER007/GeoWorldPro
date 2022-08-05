using com.frame;
using com.frame.ui;
using UnityEngine;

namespace com.ls
{
    public class ModuleTranstionUI : PopUpUI
    {
        /// <summary> UI初始化 </summary>
        public override void InitUI ()
        {
            gameObject.GetComponent<RectTransform> ().SetAsLastSibling ();
            EventUtil.DispatchEvent (GlobalEvent.Start_To_Ready_Module_Resource);
        }

        public void OnAnimationOver ()
        {
            //第一次显示 资源处理提示，第一次以后就不在显示
            EventUtil.DispatchEvent (GlobalEvent.Close_Popup_UI, new object [] { UIManager.ModuleTranstionUI });
            EventUtil.DispatchEvent (GlobalEvent.Module_Resource_Is_Ready);
        }
    }

}
