using System;

namespace net.shutosg.UniEaseCopy
{
    public static class LogUtil
    {
        public static void LogSuccess(string message, Action<UniEaseCopyLog> onLogged)
        {
            onLogged?.Invoke(new UniEaseCopyLog() { Message = message, Type = LogType.Success });
        }

        public static void Log(string message, Action<UniEaseCopyLog> onLogged)
        {
            onLogged?.Invoke(new UniEaseCopyLog() { Message = message, Type = LogType.Log });
        }

        public static void LogWarning(string message, Action<UniEaseCopyLog> onLogged)
        {
            onLogged?.Invoke(new UniEaseCopyLog() { Message = message, Type = LogType.Warning });
        }

        public static void LogError(string message, Action<UniEaseCopyLog> onLogged)
        {
            onLogged?.Invoke(new UniEaseCopyLog() { Message = message, Type = LogType.Error });
        }
    }
}
