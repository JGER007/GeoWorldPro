using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace com.frame
{
    public class AssetLoadManager : MonoBehaviour
    {

        //加载文本资源
        public void LoadTextAsset (string path, Action<string> loadTextCallBack)
        {
            StartCoroutine (LoadText (path, loadTextCallBack));
        }

        //加载音频资源
        public void LoadAudioAsset (string path, Action<AudioClip> loadAudioCallBack)
        {
            StartCoroutine (LoadAudio (path, loadAudioCallBack));
        }

        //加载图片资源
        public void LoadTextureAsset(string path, Action<Texture2D> loadTextureCallBack)
        {
            StartCoroutine(LoadTexture(path, loadTextureCallBack));
        }


        /// <summary>
        /// 加载图片资源
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="loadTextCallBack">加载回调</param>
        /// <returns></returns>
        IEnumerator LoadTexture(string path, Action<Texture2D> loadTextureCallBack) 
        {
            UnityWebRequest wr = new UnityWebRequest(path);
            DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
            wr.downloadHandler = texDl;
            yield return wr.Send();
            if (!wr.isError)
            {
                Texture2D t = texDl.texture;
                loadTextureCallBack?.Invoke(t) ;
            }

        }



        /// <summary>
        /// 加载音频资源
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="loadTextCallBack">加载回调</param>
        /// <returns></returns>
        IEnumerator LoadAudio (string path, Action<AudioClip> loadAudioCallBack)
        {
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip (path, AudioType.WAV))
            {
                yield return uwr.SendWebRequest ();
                if (uwr.isNetworkError)
                {
                    Debug.LogError ("uwrERROR:" + uwr.error);
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent (uwr);
                    if (loadAudioCallBack != null)
                    {
                        loadAudioCallBack (clip);
                    }
                }
            }
        }

        /// <summary>
        /// 加载文本文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="loadTextCallBack">加载回调</param>
        /// <returns></returns>
        IEnumerator LoadText (string path, Action<string> loadTextCallBack)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get (path);
            yield return webRequest.SendWebRequest ();
            //异常处理，很多博文用了error!=null这是错误的，请看下文其他属性部分
            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.Log ("加载配置文件失败" + webRequest.error);
            }
            else
            {
                Debug.Log ("配置文件:" + webRequest.downloadHandler.text);
                Debug.Log(path);
                loadTextCallBack (webRequest.downloadHandler.text);
            }
        }

        //加载资源
        public void LoadAsset (AssetVO assetVO, Action<AssetVO> loadCallBack)
        {
            StartCoroutine (UseUnityWebRequest (assetVO, loadCallBack));
        }

        //读取资源
        public void ReadAsset (AssetVO assetVO, Action<AssetVO> readCallBack)
        {
            StartCoroutine (UseAssetBundleCreateRequest (assetVO, readCallBack));
        }

        //从本地加载读取资源
        IEnumerator UseAssetBundleCreateRequest (AssetVO assetVO, Action<AssetVO> readCallBack)
        {
            //通过解压的方式去加载一下
            //AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync (assetVO.GetAssetLocalPath ());
            AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync (File.ReadAllBytes (assetVO.GetAssetLocalPath ()));
            //这是自己添加的文件方便填写，后边的是需要加载的物体
            yield return assetBundleCreateRequest;
            AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;
            assetVO.Asset = assetBundle;
            if (readCallBack != null)
            {
                readCallBack (assetVO);
            }
        }

        //网络加载资源并保存到本地
        IEnumerator UseUnityWebRequest (AssetVO assetVO, Action<AssetVO> loadCallBack)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get (assetVO.Path))
            {
                if (assetVO.ShowLoading)
                {
                    EventUtil.DispatchEvent (GlobalEvent.Show_Loading_Progress, new object [] { assetVO.PerfabName, webRequest });
                }
                yield return webRequest.SendWebRequest ();
                webRequest.disposeDownloadHandlerOnDispose = true;
                assetVO.LoadErrorInfo = webRequest.error;

                if (string.IsNullOrEmpty (webRequest.error) && webRequest.isDone)
                {
                    assetVO.SaveLoadedAsset (webRequest.downloadHandler.data);
                    //Debug.Log ("加载成功");
                    if (loadCallBack != null)
                    {
                        loadCallBack (assetVO);
                    }
                    ////不添加这一句 第二次加载的时候会阻塞 停止加载
                    //webRequest.downloadHandler.Dispose ();
                    //webRequest.Dispose ();
                }
                else
                {
                    assetVO.Asset = null;
                    loadCallBack (assetVO);
                }
                //不添加这一句 第二次加载的时候会阻塞 停止加载
                webRequest.Dispose ();
            }
        }
    }
}

