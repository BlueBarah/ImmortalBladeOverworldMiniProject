using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Battle {
    public class InfoPanelManager : MonoBehaviour
    {
        private Unit owner;
        private List<string> bonusLog = new List<string>();
        [SerializeField] private GameObject entryPrefab;
        
        void Awake()
        {
            owner = transform.parent.parent.GetComponent<Unit>();
            bonusLog = new List<string>() {
                "Bonuses",
                "Test Bonus: Value",
                "Test Bonus 2: Value",
                "Ailments",
                "Test Ailment: Buildup"
            };
            ClearEntries();
            foreach (string entry in bonusLog) {
                InstantiateEntry(entry);
            }

            
        }

        void Update()
        {
            List<string> newLog = new List<string>();
            List<string> temp = new List<string>();

            temp = owner.damageBonuses.GetAppliedBonuses();
            if (temp.Count != 0) {
                newLog.Add("Damage Bonuses");
                newLog.AddRange(temp);
            }
            
            temp = owner.damageResistances.GetAppliedBonuses();
            if (temp.Count != 0) {
                newLog.Add("Damage Resistances");
                newLog.AddRange(temp);
            }
            
            temp = owner.rateBonuses.GetAppliedBonuses();
            if (temp.Count != 0) {
                newLog.Add("Rate Bonuses");
                newLog.AddRange(temp);
            }
            
            temp = owner.ailmentResistances.GetAppliedBonuses();
            if (temp.Count != 0) {
                newLog.Add("Ailment Resistances");
                newLog.AddRange(temp);
            }

            temp = owner.ailmentList.GetAppliedAilments();
            if (temp.Count != 0) {
                newLog.Add("Ailments");
                newLog.AddRange(temp);
            }

            temp = owner.transform.GetComponent<SpecialRuleList>().GetAppliedRules();
            if (temp.Count != 0) {
                newLog.Add("Special Rules");
                newLog.AddRange(temp);
            }

            if (newLog != bonusLog) {
                bonusLog = newLog;
                ClearEntries();
                foreach (string entry in bonusLog) {
                    InstantiateEntry(entry);
                }
            }
        }

        void InstantiateEntry(string in_entryText) {
            GameObject entry = Instantiate(entryPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            
            TMP_Text entryText = entry.transform.GetComponent<TextMeshPro>();
            entryText.text = in_entryText;

            entry.transform.SetParent(transform);
            entry.transform.localScale = new Vector3(1,1,1);
            entry.transform.localPosition = Vector3.zero;
        }

        void ClearEntries() {
            foreach (Transform entry in transform) {
                Destroy(entry.gameObject);
            }
        }
    }
}

