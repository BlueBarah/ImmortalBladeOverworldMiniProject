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
        void Awake() {
            instance = this;
            textObj = transform.Find("Scroll View").Find("Viewport").Find("Content");
            logText = textObj.GetComponent<TextMeshProUGUI>();
            
            // Subscribe to events
            MenuEvents.Event_Log += EventSub_Log;
            MenuEvents.Event_ClearLog += EventSub_ClearLog;
            BattleSceneManager.Event_OnStateChange += EventSub_OnStateChange;
        } 

        private void EventSub_OnStateChange(BattleState in_state) {
            // Don't display the log box while the user is selecting actions
            if (in_state == BattleState.PlayerChoosingAction) {
                gameObject.SetActive(false);
            }
            else {
                gameObject.SetActive(true);
            }
        }
        private void EventSub_Log(LogArgs in_args) {
            SetText(in_args.logStr);
        }
        private void EventSub_ClearLog() {
            ClearLog();
        }
        private void SetText(string in_logStr) {
            logText.text = "";
            gameLog.Insert(0, in_logStr);
            foreach (string log in gameLog) {
                logText.text += log + "\n";
            }
        }
        private void ClearLog() {
            logText.text = "";
            gameLog.Clear();
        }

        void OnDestroy() {
            MenuEvents.Event_Log -= EventSub_Log;
            MenuEvents.Event_ClearLog -= EventSub_ClearLog;
            BattleSceneManager.Event_OnStateChange -= EventSub_OnStateChange;
        }
    }
}

