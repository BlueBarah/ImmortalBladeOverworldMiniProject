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
        public BattleState state;
        public GameObject currentUnit { get; set; }
        void Awake() {
            instance = this;
            currentUnit = GameObject.Find("Jason");
        }
        void Start() {
            UpdateState(BattleState.PlayerChoosingAction);
        }
        public void UpdateState(BattleState in_state) {
            state = in_state;
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
                    break;
                case BattleState.Win:
                    break;
                case BattleState.Lose:
                    break;
                default:
                    Debug.Log($"Unknown State: {nameof(in_state)}");
                    break;
            }

            Event_OnStateChange?.Invoke(in_state);
        }
        private void Handle_PlayerChoosingAction() {
            Debug.Log("Handle Player Turn");
        }
        private async void Handle_PlayerPerformingAction() {
            await Task.Delay(2000);
            UpdateState(BattleState.PlayerChoosingAction);
        }
        private async void Handle_EnemyTurn() {
            await Task.Delay(2000);
            UpdateState(BattleState.PlayerChoosingAction);
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

