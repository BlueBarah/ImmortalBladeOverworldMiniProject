using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Battle {
    public class LogBoxManager : MonoBehaviour
    {
        private static LogBoxManager instance;
        private Transform textObj;
        private TMP_Text logText;
        private List<string> gameLog = new List<string>();
        public static void EventSub_Log(object in_sender, LogArgs in_args) {
            instance.SetText(in_args.logStr);
        }
        private void SetText(string in_logStr) {
            logText.text = "";
            gameLog.Insert(0, in_logStr);
            foreach (string log in gameLog) {
                logText.text += log + "\n";
            }
        }
        void Awake() {
            instance = this;
            textObj = transform.Find("Scroll View").Find("Viewport").Find("Content");
            logText = textObj.GetComponent<TextMeshProUGUI>();
            Debug.Log(logText.text);
            MenuEvents.logEvent += EventSub_Log;
        } 
        void OnDestroy() {
            MenuEvents.logEvent -= EventSub_Log;
        }
    }
}

