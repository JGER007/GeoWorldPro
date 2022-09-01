using System;
using System.Collections.Generic;
using System.IO;
using com.frame.ui;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace com.frame
{
    public class AssetManager : Singleton<AssetManager>, IManager
    {
        private AssetLoadManager assetLoadManager;
        //加载完成的资源列表
        private Dictionary<string, GameObject> loadedPerfabs = null;
        //所有的加载列表
        private Dictionary<string, AssetVO> assetDic = null;
        //本地资源基础路径
        private string localAssetBasePath = "";

        public void InitManager (Transform container = null)
        {
            assetDic = new Dictionary<string, AssetVO> ();
            loadedPerfabs = new Dictionary<string, GameObject> ();
            localAssetBasePath = Application.persistentDataPath + "/AssetBundles/Perfab/";

        }

        private AssetLoadManager getAssetLoadManager ()
        {
            if (SceneRoot.Instance.GetComponent<AssetLoadManager> () == null)
            {
                SceneRoot.Instance.gameObject.AddComponent<AssetLoadManager> ();
            }
            assetLoadManager = SceneRoot.Instance.gameObject.GetComponent<AssetLoadManager> ();
            return assetLoadManager;
        }

        /// <summary>清理旧版本资源 </summary>
        /// <param name="moduleConfig">模块资源配置信息</param>
        public void CleanOldVersionAsset (Dictionary<string, ModuleConfigVO> moduleConfig)
        {
            foreach (ModuleConfigVO moduleConfigVO in moduleConfig.Values)
            {
                removeOldVersionAsset (moduleConfigVO);
            }
        }

        /// <summary>删除旧的资源文件 </summary>
        private void removeOldVersionAsset (ModuleConfigVO moduleConfigVO)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo (localAssetBasePath);
            if (directoryInfo.Exists)
            {
                FileInfo [] files = directoryInfo.GetFiles ();
                string preName = moduleConfigVO.name;
                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = files [i].Name;
                    if (fileName.IndexOf (preName) != -1 && fileName != moduleConfigVO.GetAssetLocalName ())
                    {
                        files [i].Delete ();
                    }
                }
            }
        }

        public void LoadAssetPerfab (AssetVO assetVO, Action<GameObject> perfabCallBack)
        {
#if UNITY_EDITOR //UNITY_STANDALONE
            LoadAsset(assetVO, perfabCallBack);
#else
            LoadAssetBundle (assetVO, perfabCallBack);
#endif
        }
        public void LoadAsset (AssetVO assetVO, Action<GameObject> callBack)
        {
#if UNITY_EDITOR//UNITY_STANDALONE
            string assetName = "Assets/Res/" + assetVO.AssetPath.Replace ('.', '/') + ".prefab";
            //Debug.Log (assetName);
            GameObject assetres = AssetDatabase.LoadAssetAtPath<GameObject> (assetName);

            if (assetres == null)
            {
                Debug.LogError ("assets no exist-------" + assetName);
                return;
            }
            if (callBack != null)
            {
                callBack (assetres);
            }
#endif
        }

        /// <summary> Asset数据 perfabCallBack 回调 </summary>
        public void LoadAssetBundle (AssetVO assetVO, Action<GameObject> callBack)
        {
            string assetName = assetVO.AssetName;
            if (loadedPerfabs.ContainsKey (assetName))
            {
                GameObject perfab = loadedPerfabs [assetName];
                callBack (perfab);
            }
            else
            {
                if (assetDic.ContainsKey (assetName))
                {
                    Debug.LogError ("加载错误---------" + assetName);
                    return;
                }
                assetVO.RegisterInfo (callBack);
                assetDic.Add (assetName, assetVO);
                //#if UNITY_EDITOR
                //                loadAsset(assetVO);
                //#else
                //#endif
                if (assetVO.IsExitLocal ())
                {
                    readFromLocal (assetVO);
                }
                else
                {
                    loadAsset (assetVO);
                }
            }
        }

        /// <summary> 服务器读取</summary>
        private void loadAsset (AssetVO assetVO)
        {
            //显示加载界面
            if (assetVO.ShowLoading)
            {
                EventUtil.DispatchEvent (GlobalEvent.On_Popup_UI, new object [] { UIManager.LoadingModuleUI });
            }
            getAssetLoadManager ().LoadAsset (assetVO, loadCallBack);
        }

        /// <summary> 本地读取</summary>
        private void readFromLocal (AssetVO assetVO)
        {
            if (assetVO.ShowLoading)
            {
                EventUtil.DispatchEvent (GlobalEvent.On_Popup_UI, new object [] { UIManager.LoadFromLocalUI, 1 });
            }
            getAssetLoadManager ().ReadAsset (assetVO, readCallBack);
        }

        /// <summary> 读取资源回调 </summary>
        private void readCallBack (AssetVO assetVO)
        {
            GameObject obj = assetVO.Asset.LoadAsset<GameObject> (assetVO.PerfabName);
            loadedPerfabs.Add (assetVO.AssetName, obj);
            assetVO.CallBack (obj);
            assetVO.Asset.Unload (false);
        }

        /// <summary> 加载文本 </summary>
        public void LoadText (string path, Action<string> localTextCallBack)
        {
            getAssetLoadManager ().LoadTextAsset (path, localTextCallBack);
        }

        /// <summary>加载音频</summary>
        public void LoadAudio (string path, Action<AudioClip> localAudioCallBack)
        {
            //Debug.Log ("LoadAudio path:" + path);
            getAssetLoadManager ().LoadAudioAsset (path, localAudioCallBack);
        }

        /// <summary>加载返回</summary>
        private void loadCallBack (AssetVO assetVO)
        {
            if (string.IsNullOrEmpty (assetVO.LoadErrorInfo))
            {
                //加载成功
                readFromLocal (assetVO);
                //Debug.Log("加载成功:" + assetVO.LoadErrorInfo + "   ----    " + assetVO.Path + " ---- " + assetVO.AssetName);
            }
            else
            {
                //加载失败
                Debug.LogError ("加载失败:" + assetVO.LoadErrorInfo + "   ----    " + assetVO.Path + " ---- " + assetVO.AssetName + " -----  " + assetVO.PerfabName);
            }
            //关闭加载进渡条
            if (assetVO.ShowLoading)
            {
                EventUtil.DispatchEvent (GlobalEvent.Show_Loading_Progress, new object [] { assetVO.AssetName });
            }
        }

        /// <summary>退出</summary>
        public void OnQuit ()
        {
            if (assetDic != null && assetDic.Count > 0)
            {
                foreach (AssetVO assetVO in assetDic.Values)
                {
                    if (assetVO.Asset != null)
                    {
                        assetVO.Asset.Unload (true);
                    }
                }
                assetDic.Clear ();
            }
            loadedPerfabs.Clear ();
        }
    }
}
