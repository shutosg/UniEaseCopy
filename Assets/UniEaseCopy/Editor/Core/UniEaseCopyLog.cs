namespace net.shutosg.UniEaseCopy
{
    public struct UniEaseCopyLog
    {
        public LogType Type;
        public string Message;
    }

    public enum LogType
    {
        Success,
        Log,
        Warning,
        Error,
    }
}
