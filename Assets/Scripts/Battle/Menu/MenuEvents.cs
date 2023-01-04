using System;

namespace Battle {
    public class LogArgs {
        public string logStr;
    }
    public static class MenuEvents
    {
        public static event Action<LogArgs> Event_Log;
        public static event System.Action Event_ClearLog;
        public static void Log(string in_logStr) {
            Event_Log?.Invoke(new LogArgs {
                logStr = in_logStr,
            });
        }

        public static void ClearLog() {
            Event_ClearLog?.Invoke();
        }
    }
}
