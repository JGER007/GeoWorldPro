using System.Collections.Generic;
using UnityEngine;

namespace com.frame
{
    public class ModuleControlManager : IManager
    {
        protected BaseModuleManager moduleManager = null;

        public void InitManager (Transform container = null)
        {
            EventUtil.AddListener (GlobalEvent.Open_Module, onOpenModule);
            EventUtil.AddListener (GlobalEvent.Quit_Module, onQuitModule);
        }

        /// <summary>退出AR模块</summary>
        public virtual void onQuitModule (CustomEventArgs eventArgs)
        {
            quitModuleManager ();
        }


        /// <summary>打开AR模块</summary>
        public virtual void onOpenModule (CustomEventArgs eventArgs)
        {
            string moduleName = eventArgs.args [0].ToString ();
            ModuleConfigVO moduleConfigVO = ConfigManager.Instance.GetModuleConfigVO (moduleName);

            if (moduleConfigVO != null)
            {
                moduleManager = ModuleManagerFactory.CreateModule (moduleConfigVO);
            }

        }

        /// <summary>退出模块管理类 </summary>
        protected virtual void quitModuleManager ()
        {
            if (moduleManager != null)
            {
                moduleManager.OnQuit ();
                moduleManager = null;
            }
        }

        public virtual void OnQuit ()
        {
            EventUtil.RemoveListener (GlobalEvent.Open_Module, onOpenModule);
            EventUtil.RemoveListener (GlobalEvent.Quit_Module, onQuitModule);
            quitModuleManager ();
        }
    }

}
