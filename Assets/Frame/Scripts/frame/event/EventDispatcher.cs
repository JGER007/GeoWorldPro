using System.Collections.Generic;
/// <summary>
/// 事件派发器
/// </summary>
namespace com.frame
{
    public class EventDispatcher
    {
        /// <summary> 事件Map </summary>
        private Dictionary<string, EventListener> dic = new Dictionary<string, EventListener> ();

        /// <summary> 添加事件监听器 </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventHandler">事件处理器</param>
        public void AddListener (string eventType, EventListener.EventHandler eventHandler)
        {
            EventListener invoker;
            if (!dic.TryGetValue (eventType, out invoker))
            {
                invoker = new EventListener ();
                dic.Add (eventType, invoker);
            }
            invoker.eventHandler += eventHandler;
        }

        /// <summary> 移除事件监听器 </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventHandler">事件处理器</param>
        public void RemoveListener (string eventType, EventListener.EventHandler eventHandler)
        {
            EventListener invoker;
            if (dic.TryGetValue (eventType, out invoker)) invoker.eventHandler -= eventHandler;
        }

        /// <summary> 是否已经拥有该类型的事件 </summary>
        /// <param name="eventType">事件类型</param>
        public bool HasListener (string eventType)
        {
            return dic.ContainsKey (eventType);
        }

        /// <summary> 派发事件 </summary>
        /// <param name="eventType">事件类型</param>
        public void DispatchEvent (string eventType, params object [] args)
        {
            EventListener invoker;
            if (dic.TryGetValue (eventType, out invoker))
            {
                CustomEventArgs evt;
                if (args == null || args.Length == 0)
                {
                    evt = new CustomEventArgs (eventType);
                }
                else
                {
                    evt = new CustomEventArgs (eventType, args);
                }
                invoker.Invoke (evt);
            }
        }

        /// <summary> 清理所有事件监听器 </summary>
        public void Clear ()
        {
            foreach (EventListener value in dic.Values)
            {
                value.Clear ();
            }
            dic.Clear ();
        }

    }
}
