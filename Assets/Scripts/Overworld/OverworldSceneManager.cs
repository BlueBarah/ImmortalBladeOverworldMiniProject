using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        static List<Enemy> enemiesInRange = new List<Enemy>();

        public static void Event_PlayerFightRange(Enemy in_sender, bool in_flag)
        {
            if (in_flag && !enemiesInRange.Contains(in_sender))
            {
                enemiesInRange.Add(in_sender);
            }
            else if (!in_flag && enemiesInRange.Contains(in_sender))
            {
                enemiesInRange.Remove(in_sender);
            }
            string logStr = "";
            foreach (Enemy enemy in enemiesInRange)
            {
                logStr += enemy.name + ", ";
            }
            Debug.Log(logStr);
        }
        public static void Event_BattleStart(Player in_player)
        {
            string logStr = in_player.name + " started a battle with ";
            foreach (Enemy enemy in enemiesInRange)
            {
                logStr += enemy.name + ", ";
                enemy.Flag_BattleStart = true;
            }
            // Debug.Log(logStr);

            WorldSceneTransitioner sceneTransitioner = GameObject.FindObjectOfType<WorldSceneTransitioner>();
            sceneTransitioner.TransitionToBattleScene();
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


        // Start is called before the first frame update
        void Start()
        {
            // Initialize Shader Values
            directionalLight = gameObject.transform.Find("Directional Light").gameObject;

            currentColor = directionalLight.GetComponent<Light>().color;
            currentIntensity = highlightIntensity;
            currentRotation = directionalLight.transform.rotation;
            rotationVector = directionalLight.transform.rotation * Vector3.forward;


            // Initialize PlayerInRange Event
            Enemy.Event_EnemyInRange += Event_PlayerFightRange;
            //Initialize BattleStartEvent
            Player.Event_BattleStart += Event_BattleStart;
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
            Enemy.Event_EnemyInRange -= Event_PlayerFightRange;
            //Initialize BattleStartEvent
            Player.Event_BattleStart -= Event_BattleStart;

        }
    }
}