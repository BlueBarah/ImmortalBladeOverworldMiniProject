using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Battle {
    public class InfoBoxManager : MonoBehaviour
    {
        private TMP_Text heading;
        private TMP_Text healthValue;
        private TMP_Text tensionValue;
        private Unit currentUnit;
        void Awake() {
            heading = transform.Find("Heading").Find("Text").GetComponent<TextMeshProUGUI>();
            healthValue = transform.Find("Health").Find("Value").GetComponent<TextMeshProUGUI>();
            tensionValue = transform.Find("Tension").Find("Value").GetComponent<TextMeshProUGUI>();
            BattleSceneManager.Event_OnCurrentUnitChange += EventSub_OnCurrentUnitChange;
        }
        void OnDestroy() {
            BattleSceneManager.Event_OnCurrentUnitChange -= EventSub_OnCurrentUnitChange;
        }
        private void EventSub_OnCurrentUnitChange(Unit in_unit) {
            currentUnit = in_unit;
            SetResourceText();
        }
        private void SetResourceText() {
            heading.text = currentUnit.name;
            healthValue.text = $"{currentUnit.HP_current}/{currentUnit.resources.HP_max.val}";
            tensionValue.text = Mathf.Round((currentUnit.TN_current * 100) - 50).ToString() + "%";
        }
    }
}

