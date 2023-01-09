using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle {
    public class BattleSceneManager : MonoBehaviour
    {
        public static BattleSceneManager instance;
        public static event Action<BattleState> Event_OnStateChange;
        public static event Action<Unit> Event_OnCurrentUnitChange;
        [SerializeField] private Material UI_Material;
        public BattleState state;
        public Unit currentUnit { get; set; }
        public List<Unit> turnOrder = new List<Unit>();
        void Awake() {
            instance = this;

            // Initialize List
            foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Battle Unit")) {
                turnOrder.Add(unit.GetComponent<Unit>());
            }
        }
        void Start() {
            turnOrder.Sort();
            turnOrder.Reverse();
            currentUnit = turnOrder[0];
            Event_OnCurrentUnitChange?.Invoke(currentUnit);

            // Log
            string logStr = "Turn Order: ";
            foreach (Unit unit in turnOrder) {
                logStr += $"{unit.name}, ";
            }
            Debug.Log(logStr);
            logStr = "Enemy Units: ";
            foreach (EnemyUnit unit in GetEnemyUnits()) {
                logStr += $"{unit.name}, ";
            }
            Debug.Log(logStr);
            UpdateState(BattleState.PlayerChoosingAction);
        }
        public void UpdateState(BattleState in_state) {
            state = in_state;
            Event_OnStateChange?.Invoke(in_state);
            switch (in_state) {
                case BattleState.PlayerChoosingAction:
                    Handle_PlayerChoosingAction();
                    break;
                case BattleState.PlayerPerformingAction:
                    Handle_PlayerPerformingAction();
                    break;
                case BattleState.EnemyTurn:
                    Handle_EnemyTurn();
                    break;
                case BattleState.Decide:
                    Handle_Decide();
                    break;
                case BattleState.Win:
                    MenuEvents.ClearLog();
                    MenuEvents.Log("Win");
                    break;
                case BattleState.Lose:
                    MenuEvents.ClearLog();
                    MenuEvents.Log("Lose");
                    break;
                default:
                    Debug.Log($"Unknown State: {nameof(in_state)}");
                    break;
            }

        }

        public void EndTurn() {
            int nextIndex = turnOrder.IndexOf(currentUnit) + 1;
            currentUnit = (nextIndex < turnOrder.Count) ? turnOrder[nextIndex] : turnOrder[0];

            Event_OnCurrentUnitChange?.Invoke(currentUnit);
            UpdateState(BattleState.PlayerChoosingAction);
        }
        public List<Unit> GetEnemyUnits() {
            return turnOrder.FindAll(unit => unit.GetType() == typeof(EnemyUnit));
        }
        private void Handle_PlayerChoosingAction() {
            Debug.Log("Player Turn");
        }
        private async void Handle_PlayerPerformingAction() {
            await Task.Delay(5000);
            UpdateState(BattleState.Decide);
        }
        private async void Handle_EnemyTurn() {
            await Task.Delay(2000);
            UpdateState(BattleState.Decide);
        }
        private void Handle_Decide() {
            bool Flag_GameOver = true;
            bool Flag_Victory = true;
            for (int i = turnOrder.Count - 1; i >= 0; i--) {
                Unit unit = turnOrder[i];
                if (unit.GetType() == typeof(EnemyUnit) && unit.HP_state == HP.Incapacitated) {
                    turnOrder.Remove(unit);
                    Destroy(unit.gameObject);
                }
                if (unit.GetType() == typeof(EnemyUnit) && unit.HP_state != HP.Incapacitated) {
                    Debug.Log("No Victory");
                    Flag_Victory = false;
                }
                else if (unit.GetType() == typeof(PlayerUnit) && unit.HP_state != HP.Incapacitated) {
                    Debug.Log("No Game Over");
                    Flag_GameOver = false;
                }
            }
            if (Flag_GameOver) {
                UpdateState(BattleState.Lose);
            }
            else if (Flag_Victory) {
                UpdateState(BattleState.Win);
            }
            else {
                UpdateState(BattleState.PlayerChoosingAction);
            }
        }
    }

    public enum BattleState {
        PlayerChoosingAction,
        PlayerPerformingAction,
        EnemyTurn,
        Decide,
        Win,
        Lose
    }
}

