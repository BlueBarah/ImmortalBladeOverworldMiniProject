using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Battle {
    public class ActionMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        private Transform menuContent;
        
        void Awake() {
            menuContent = transform.Find("Scroll View").Find("Viewport").Find("Content");
            
            UnityAction myAction = () => {
                BattleSceneManager.instance.UpdateState(BattleState.PlayerPerformingAction);
            };

            PopulateTestButtons();

            // Subscribe to events
            BattleSceneManager.Event_OnStateChange += EventSub_OnStateChange;
        }
        void OnDestroy() {
            BattleSceneManager.Event_OnStateChange -= EventSub_OnStateChange;
        }

        private void EventSub_OnStateChange(BattleState in_state) {
            // Don't display the log box while the user is selecting actions
            if (in_state == BattleState.PlayerChoosingAction) {
                gameObject.SetActive(true);
                PopulateTestButtons();
            }
            else {
                gameObject.SetActive(false);
            }
        }

        private void ClearMenuButtons() {
            foreach (Transform button in menuContent) {
                Destroy(button.gameObject);
            }
        }
        private void PopulateTestButtons() {
            ClearMenuButtons();

            InstantiateActionButton("Action", () => {
                PopulateAttackButtons();
            });
            InstantiateActionButton("End Turn", () => {
                BattleSceneManager.instance.UpdateState(BattleState.EnemyTurn);
                MenuEvents.ClearLog();
                MenuEvents.Log("Enemy Performing Action");
            });
        }
        private void PopulateAttackButtons() {
            ClearMenuButtons();

            UnitActions currentUnitActions = BattleSceneManager.instance.currentUnit.GetComponent<UnitActions>();
            List<IAction> attackList = currentUnitActions.GetActions(ActionTypes.Attack);
            foreach(Attack attack in attackList) {
                InstantiateActionButton(attack.name, () => {
                    BattleSceneManager.instance.UpdateState(BattleState.PlayerPerformingAction);
                    MenuEvents.ClearLog();
                    MenuEvents.Log($"Player Performing Action: {attack.name}");
                });
            }
        }

        private void InstantiateActionButton(string in_text, UnityAction in_action) {
            // Make the button
            GameObject button = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            
            // Give the button a unique name
            button.name = in_text;

            // Set the text of the button
            TMP_Text buttonText = button.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            buttonText.text = in_text;

            // Add an onclick event
            button.GetComponent<Button>().onClick.AddListener(in_action);

            // Add the button to the menu
            button.transform.SetParent(menuContent);
            button.transform.localScale = new Vector3(1,1,1);
            
        }

    }
}

