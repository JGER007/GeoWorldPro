//***************************************************
// Des：BoboAR
// Author:KaKa 
// Email：jiangjian@prismostudio.io
// Mobile：17321367005
// CreateTime：2021/04/22 15:40:32
// Version：v 0.1
// @CopyRight：上海琉森教育科技有限公司
// ***************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ThreadCallTools : MonoBehaviour
{
    /// <summary>
    /// update列表
    /// </summary>
    static List<Action<float>> m_UpdateActionList = new List<Action<float>>();
    /// <summary>
    /// 携程队列
    /// </summary>
    static Queue<IEnumerator> m_coroutineQueue = new Queue<IEnumerator>();
    static Queue<Action> m_unityThreadQueue = new Queue<Action>();

    /// <summary> 加载图片携程队列 </summary>
    static Queue<IEnumerator> m_LoadImageQueue = new Queue<IEnumerator>();

    /// <summary>图片协程 </summary>
    public static void AddLoadImageQueue(IEnumerator ie)
    {
        m_LoadImageQueue.Enqueue(ie);
    }

    public static void ClearLoadImageQueue()
    {
        m_LoadImageQueue.Clear();
    }
    /// <summary>
    /// 加入到update列表
    /// </summary>
    static public void addUpdateAction(Action<float> action)
    {
        m_UpdateActionList.Add(action);
    }
    /// <summary>
    /// 从update列表移除
    /// </summary>
    static public void removeUpdateAction(Action<float> action)
    {
        m_UpdateActionList.Remove(action);
    }
    /// <summary>
    /// 开始协程
    /// </summary>
    /// <param name="ie"></param>
    static public new void StartCoroutine(IEnumerator ie)
    {
        m_coroutineQueue.Enqueue(ie);
    }
    static public void StartInUnityThread(Action action)
    {
        m_unityThreadQueue.Enqueue(action);
    }
    /// <summary>
    /// 等待时间
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    static public void WaitForAction(float waitTime, Action action)
    {
        StartCoroutine(IE_WaitForAction(waitTime, action));
    }
    static private IEnumerator IE_WaitForAction(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    static public void WaitForEndOfFrame(Action action)
    {
        StartCoroutine(IE_WaitForEndOfFrameAction(action));
    }
    static private IEnumerator IE_WaitForEndOfFrameAction(Action action)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        yield return wait;
        wait = null;
        action();
    }

    public static bool isRunCoroutine = false;

    /// <summary>
    /// 主循环
    /// </summary>
    void Update()
    {
        if (m_coroutineQueue.Count > 0)
        {
            var ie = m_coroutineQueue.Dequeue();
            base.StartCoroutine(ie);
        }
        if (m_unityThreadQueue.Count > 0)
        {
            Action action = m_unityThreadQueue.Dequeue();
            action();
        }

        if (m_LoadImageQueue.Count > 0 && !isRunCoroutine)
        {
            isRunCoroutine = true;
            IEnumerator ie = m_LoadImageQueue.Dequeue();
            base.StartCoroutine(ie);
        }

        for (int i = 0; i < m_UpdateActionList.Count; i++)
        {
            m_UpdateActionList[i](Time.deltaTime);
        }
    }

    private bool LoadStart(List<System.Action> list)
    {
        if (list.Count < 1) return true;
        var acion = list[0];
        int index = 0;
        while (acion != null)
        {
            index++;
            lock (acion)
            {
                acion();
            }
            if (list.Count > index)
            {
                acion = list[index];
            }
            else break;
        }
        return true;
    }
}
