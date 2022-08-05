/// <summary> 事件参数
/// <para>ZhangYu 2019-03-05</para>
/// </summary>
namespace com.frame
{
    public class CustomEventArgs
    {

        /// <summary> 事件类型 </summary>
        public readonly string type;
        /// <summary> 事件参数 </summary>
        public readonly object [] args;

        public CustomEventArgs (string type)
        {
            this.type = type;
        }

        public CustomEventArgs (string type, params object [] args)
        {
            this.type = type;
            this.args = args;
        }
    }
}
