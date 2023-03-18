using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Battle {
    public class BattleSceneManager : MonoBehaviour
    {
        public static BattleSceneManager instance;
        public static event Action<BattleState> Event_OnStateChange;
        public static event Action<Unit> Event_OnCurrentUnitChange;
        public event EventHandler Event_BattleEnd;
        [SerializeField] private Material UI_Material;
        public BattleState state;
        public Unit currentUnit { get; set; }
        public List<Unit> turnOrder = new List<Unit>();
        private Transform characterContainer;
        private Transform enemyContainer;
        void Awake() {
            instance = this;
            turnOrder = new List<Unit>();

            characterContainer = transform.Find("Units").Find("Characters");
            enemyContainer = transform.Find("Units").Find("Enemies");

            // Initialize List
            if (GameDataManager.instance.unitsInBattle.Count > 0) {
                turnOrder = PopulateUnits();
            }
            else {
                foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Battle Unit")) {
                    if (!unit.GetComponent<Unit>().flag_init) unit.GetComponent<Unit>().Init();
                    turnOrder.Add(unit.GetComponent<Unit>());
                }
            }

            turnOrder.Sort();
            turnOrder.Reverse();

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
        }
        void Start() {
            currentUnit = turnOrder[0];
            StartTurn();
        }
        private void UpdateState(BattleState in_state) {
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
                    Handle_Win();
                    //Event_BattleEnd(this, new EventArgs());
                    break;
                case BattleState.Lose:
                    MenuEvents.Log("Lose");
                    //Event_BattleEnd(this, new EventArgs());
                    break;
                default:
                    Debug.Log($"Unknown State: {nameof(in_state)}");
                    break;
            }

        }
        public async void AttackSelected(List<Unit> in_units, Attack in_action) {
            UpdateState(BattleState.PlayerPerformingAction);

            UnitActions currentUnitActions = currentUnit.GetComponent<UnitActions>();
            List<Task> performActions = new List<Task>();
            foreach (Unit unit in in_units) {
                performActions.Add(currentUnitActions.PerformAttack(unit, in_action));
            }
            await Task.WhenAll(performActions);

            UpdateState(BattleState.Decide);
        }

        public void EndTurn() {
            currentUnit.EndTurn();

            int nextIndex = turnOrder.IndexOf(currentUnit) + 1;
            currentUnit = (nextIndex < turnOrder.Count) ? turnOrder[nextIndex] : turnOrder[0];

            StartTurn();
        }
        public void StartTurn() {
            Event_OnCurrentUnitChange?.Invoke(currentUnit);
            currentUnit.StartTurn();
            if (currentUnit.GetType() == typeof(PlayerUnit)) {
                UpdateState(BattleState.PlayerChoosingAction);
            }
            else {
                UpdateState(BattleState.EnemyTurn);
            }

        }
        public List<Unit> GetEnemyUnits() {
            return turnOrder.FindAll(unit => unit.GetType() == typeof(EnemyUnit));
        }
        public List<Unit> GetPlayerUnits() {
            return turnOrder.FindAll(unit => (unit.GetType() == typeof(PlayerUnit) && unit.HP_state != HP.Incapacitated));
        }
        private void Handle_Win() {
            MenuEvents.Log("Win");

            
            foreach (OverworldUnitData overworldUnit in GameDataManager.instance.unitsInBattle) {
                foreach (BattleUnitData battleUnit in overworldUnit.encounter){
                    battleUnit.currentHealth = battleUnit.instance.HP_current;
                }
            }

            SceneManager.LoadScene("OverworldScene");

        }
        private void Handle_PlayerChoosingAction() {
            if (currentUnit.HP_state == HP.Incapacitated) {
                EndTurn();
            }
        }
        private void Handle_PlayerPerformingAction() {
            //UpdateState(BattleState.Decide);
        }
        private async void Handle_EnemyTurn() {
            IEnemyUnitAI enemyAI = currentUnit.gameObject.GetComponent<IEnemyUnitAI>();
            await enemyAI.Act();
            enemyAI.ManageAggro();
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
                    Flag_Victory = false;
                }
                else if (unit.GetType() == typeof(PlayerUnit) && unit.HP_state != HP.Incapacitated) {
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
                if (currentUnit.GetType() == typeof(EnemyUnit)) {
                    EndTurn();
                }
                else {
                    UpdateState(BattleState.PlayerChoosingAction);
                }
            }
        }

        private List<Unit> PopulateUnits() {
            List<Unit> units = new List<Unit>();

            foreach (Transform entry in characterContainer) {
                Destroy(entry.gameObject);
            }
            foreach (Transform entry in enemyContainer) {
                Destroy(entry.gameObject);
            }

            float numPlayers = 1;
            float numEnemies = 1;
            foreach (OverworldUnitData overworldUnit in GameDataManager.instance.unitsInBattle) {
                foreach (BattleUnitData battleUnit in overworldUnit.encounter){
                    if (battleUnit.prefab.GetComponent<Unit>().GetType() == typeof(PlayerUnit)) {
                        GameObject unitInstance = GameObject.Instantiate(battleUnit.prefab);
                        
                        unitInstance.transform.SetParent(characterContainer);
                        unitInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        unitInstance.transform.position = new Vector3(-(3.5f * numPlayers), 0.5f, (numPlayers % 2) * 2f);
                        unitInstance.name = unitInstance.name.Replace("(Clone)", "");

                        Unit unit = unitInstance.GetComponent<Unit>();
                        unit.Init();
                        unit.HP_current = battleUnit.currentHealth;
                        battleUnit.instance = unit;

                        units.Add(unit);
                        numPlayers += 1;
                    }
                    else if (battleUnit.prefab.GetComponent<Unit>().GetType() == typeof(EnemyUnit)) {
                        GameObject unitInstance = GameObject.Instantiate(battleUnit.prefab);
                        
                        unitInstance.transform.SetParent(enemyContainer);
                        unitInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        unitInstance.transform.position = new Vector3(3.5f * numEnemies, 0.5f, (numEnemies % 2) * 2f);
                        unitInstance.name = unitInstance.name.Replace("(Clone)", "");

                        Unit unit = unitInstance.GetComponent<Unit>();
                        unit.Init();
                        unit.HP_current = battleUnit.currentHealth;
                        battleUnit.instance = unit;

                        units.Add(unit);
                        numEnemies += 1;
                    }
                }
            }

            return units;
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

