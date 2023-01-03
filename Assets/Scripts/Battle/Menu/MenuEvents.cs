using System;

namespace Battle {
    public class LogArgs {
        public string logStr;
    }
    public static class MenuEvents
    {
        public static event EventHandler<LogArgs> logEvent;
        public static void Event_Log(object in_sender, string in_logStr) {
            logEvent?.Invoke(in_sender, new LogArgs {
                logStr = in_logStr,
            });
        }
    }
}
