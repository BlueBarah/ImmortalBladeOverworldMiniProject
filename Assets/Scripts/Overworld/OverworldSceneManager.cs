using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overworld
{
    public class OverworldSceneManager : MonoBehaviour
    {
        [SerializeField] Material spriteMaterial;
        [SerializeField] float highlightIntensity;
        GameObject directionalLight;
        Quaternion currentRotation;
        float currentIntensity;
        Color currentColor;
        Vector3 rotationVector;
        public static List<Mover> unitsInRange = new List<Mover>();
        public static List<Mover> unitsNotInRange = new List<Mover>();
        public static bool flag_battleStart = false;

        public void Event_PlayerFightRange(Enemy in_sender, bool in_flag)
        {
            if (in_flag)
            {
                if (!unitsInRange.Contains(in_sender)) unitsInRange.Add(in_sender);
                if (unitsNotInRange.Contains(in_sender)) unitsNotInRange.Remove(in_sender);
            }
            else
            {
                if (unitsInRange.Contains(in_sender)) unitsInRange.Remove(in_sender);
                if (!unitsNotInRange.Contains(in_sender)) unitsNotInRange.Add(in_sender);
            }
            string logStr = "";
            foreach (Mover unit in unitsInRange)
            {
                logStr += unit.name + ", ";
            }
            Debug.Log("In Battle: " + logStr);
            logStr = "";
            foreach (Mover unit in unitsNotInRange)
            {
                logStr += unit.name + ", ";
            }
            Debug.Log("Not In Battle: " + logStr);
        }
        public void Event_AllyInRange(FieldAlly in_sender, bool in_flag) {
            if (in_flag)
            {
                if (!unitsInRange.Contains(in_sender)) unitsInRange.Add(in_sender);
                if (unitsNotInRange.Contains(in_sender)) unitsNotInRange.Remove(in_sender);
            }
            else
            {
                if (unitsInRange.Contains(in_sender)) unitsInRange.Remove(in_sender);
                if (!unitsNotInRange.Contains(in_sender)) unitsNotInRange.Add(in_sender);
            }
            string logStr = "";
            foreach (Mover unit in unitsInRange)
            {
                logStr += unit.name + ", ";
            }
            Debug.Log("In Battle: " + logStr);
            logStr = "";
            foreach (Mover unit in unitsNotInRange)
            {
                logStr += unit.name + ", ";
            }
            Debug.Log("Not In Battle: " + logStr);

        }
        public void Event_BattleStart(Player in_player)
        {
            if (!flag_battleStart) {
                flag_battleStart = true;
                string logStr = in_player.name + " started a battle with ";
                // Empty the current list
                GameDataManager.instance.unitsInBattle = new List<OverworldUnitData>();
                GameDataManager.instance.unitsNotInBattle = new List<OverworldUnitData>();
                // Add the player
                in_player.unitData.ownerPosition = in_player.transform.localPosition;
                in_player.unitData.prefab = LoadPrefab(in_player.name);
                GameDataManager.instance.unitsInBattle.Add(in_player.unitData);

                // Add all enemies
                foreach (Mover unit in unitsInRange)
                {
                    unit.unitData.ownerPosition = unit.transform.localPosition;
                    unit.unitData.prefab = LoadPrefab(unit.name);
                    GameDataManager.instance.unitsInBattle.Add(unit.unitData);
                    logStr += unit.name + ", ";
                    if (unit.GetType() == typeof(Enemy) || unit.GetType() == typeof(FieldAlly)) {
                        IDetectPlayer enemy = (IDetectPlayer)unit;
                        enemy.flag_battleStart = true;
                    }
                }
                foreach (Mover unit in unitsNotInRange) {
                    unit.unitData.ownerPosition = unit.transform.localPosition;
                    unit.unitData.prefab = LoadPrefab(unit.name);
                    GameDataManager.instance.unitsNotInBattle.Add(unit.unitData);
                }
                // Debug.Log(logStr);
                StartCoroutine(GameDataManager.instance.SwitchScene("Battle"));
            }
        }

        private void SetLightColor()
        {
            currentColor = directionalLight.GetComponent<Light>().color;
            Color highlightColor = new Color(currentColor.r, currentColor.g, currentColor.b, highlightIntensity);

            directionalLight.GetComponent<Light>().color = currentColor;
            spriteMaterial.SetColor("_LightColor", highlightColor);
        }
        private void SetLightRotation()
        {
            rotationVector = directionalLight.transform.rotation * Vector3.forward;

            Vector2 newLightDirection = new Vector2(-rotationVector.x / 2, -rotationVector.y / 2);
            spriteMaterial.SetVector("_MyLightDirection", newLightDirection);
        }

        void Awake() {
            // Initialize PlayerInRange Event
            Enemy.event_enemyInRange += Event_PlayerFightRange;
            FieldAlly.event_allyInRange += Event_AllyInRange;
            //Initialize BattleStartEvent
            Player.Event_BattleStart += Event_BattleStart;
            unitsInRange.Clear();
            unitsNotInRange.Clear();
            flag_battleStart = false;

        }
        void Start()
        {
            // Initialize Shader Values
            directionalLight = gameObject.transform.Find("Directional Light").gameObject;

            currentColor = directionalLight.GetComponent<Light>().color;
            currentIntensity = highlightIntensity;
            currentRotation = directionalLight.transform.rotation;
            rotationVector = directionalLight.transform.rotation * Vector3.forward;

            Transform unitContainer = transform.Find("Units");

            if (!GameDataManager.instance.Flag_InitialSceneLoad) {

                // Clear out the existing units
                foreach (Transform overworldUnit in unitContainer) {
                    GameObject.Destroy(overworldUnit.gameObject);
                }

                foreach (OverworldUnitData overworldUnit in GameDataManager.instance.unitsInBattle) if (overworldUnit.prefab.GetComponent<Mover>().GetType() != typeof(Overworld.Enemy)){
                    GameObject unitInstance = GameObject.Instantiate(overworldUnit.prefab);
                    unitInstance.name = unitInstance.name.Replace("(Clone)", "");
                    unitInstance.transform.SetParent(unitContainer);
                    unitInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    unitInstance.transform.localPosition = overworldUnit.ownerPosition;
                    
                    unitInstance.GetComponent<Mover>().Init();
                    unitInstance.GetComponent<Mover>().unitData = overworldUnit;
                }
                // Initialize the units from the Game Data Manager
                foreach (OverworldUnitData overworldUnit in GameDataManager.instance.unitsNotInBattle) {
                    GameObject unitInstance = GameObject.Instantiate(overworldUnit.prefab);
                    unitInstance.name = unitInstance.name.Replace("(Clone)", "");
                    unitInstance.transform.SetParent(unitContainer);
                    unitInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    unitInstance.transform.localPosition = overworldUnit.ownerPosition;

                    unitInstance.GetComponent<Mover>().Init();
                    unitInstance.GetComponent<Mover>().unitData = overworldUnit;
                }

            }
            else {
                Debug.Log("Initial Load");

                foreach (Transform unit in unitContainer) {
                    Mover unitMover = unit.GetComponent<Mover>();
                    foreach (BattleUnitData entry in unitMover.unitData.encounter) {
                        Battle.Unit battleUnit = entry.prefab.GetComponent<Battle.Unit>();
                        entry.currentHealth = battleUnit._maxHP;
                    }
                }

                GameDataManager.instance.Flag_InitialSceneLoad = false;
            }
            GameDataManager.instance.unitsInBattle.Clear();
            GameDataManager.instance.unitsNotInBattle.Clear();


        }

        // Update is called once per frame
        void Update()
        {
            if (rotationVector != directionalLight.transform.rotation * Vector3.forward)
            {
                SetLightRotation();
            }
            if (currentColor != directionalLight.GetComponent<Light>().color || currentIntensity != highlightIntensity)
            {
                SetLightColor();
            }

        }

        void OnDestroy() {
            // Initialize PlayerInRange Event
            Enemy.event_enemyInRange -= Event_PlayerFightRange;
            FieldAlly.event_allyInRange -= Event_AllyInRange;
            //Initialize BattleStartEvent
            Player.Event_BattleStart -= Event_BattleStart;

        }

        private static GameObject LoadPrefab(string in_name) {
            string path = @"Prefabs\Movers\" + in_name;
            return (GameObject)Resources.Load(path);
        }
    }
}