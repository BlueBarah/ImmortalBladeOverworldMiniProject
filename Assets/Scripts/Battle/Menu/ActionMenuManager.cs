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
        private Unit currentUnit;
        private IAction currentAction;
        private Transform menuContent;
        
        void Awake() {
            menuContent = transform.Find("Scroll View").Find("Viewport").Find("Content");

            PopulateTestButtons();

            // Subscribe to events
            BattleSceneManager.Event_OnStateChange += EventSub_OnStateChange;
            BattleSceneManager.Event_OnCurrentUnitChange += EventSub_OnCurrentUnitChange;
        }
        void OnDestroy() {
            BattleSceneManager.Event_OnStateChange -= EventSub_OnStateChange;
            BattleSceneManager.Event_OnCurrentUnitChange -= EventSub_OnCurrentUnitChange;
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
        private void EventSub_OnCurrentUnitChange(Unit in_unit) {
            currentUnit = in_unit;
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
                BattleSceneManager.instance.EndTurn();
            });
        }
        private void PopulateAttackButtons() {
            ClearMenuButtons();

            UnitActions currentUnitActions = currentUnit.GetComponent<UnitActions>();
            List<IAction> attackList = currentUnitActions.GetActions(ActionTypes.Attack);
            foreach(Attack attack in attackList) {
                InstantiateActionButton(attack.name, () => {
                    PopulateEnemyTargets(attack);
                });
            }
            InstantiateActionButton("Back", () => {
                PopulateTestButtons();
            });
        }
        private void PopulateEnemyTargets(Attack in_attack) {
            ClearMenuButtons();

            UnitActions currentUnitActions = currentUnit.GetComponent<UnitActions>();
            List<Unit> enemyList = BattleSceneManager.instance.GetEnemyUnits();
            foreach(Unit enemy in enemyList) {
                InstantiateActionButton(enemy.name, () => {
                    BattleSceneManager.instance.UpdateState(BattleState.PlayerPerformingAction);
                    currentUnitActions.PerformAttack(enemy, in_attack);
                });
            }
            InstantiateActionButton("Back", () => {
                PopulateAttackButtons();
            });

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

