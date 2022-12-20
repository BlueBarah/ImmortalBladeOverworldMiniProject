using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldSceneManager : MonoBehaviour
{
    [SerializeField] Material spriteMaterial;
    [SerializeField] float highlightIntensity;
    GameObject directionalLight;
    Quaternion currentRotation;
    float currentIntensity;
    Color currentColor;
    Vector3 rotationVector;
    static List<string> enemiesInRange = new List<string>();

    public static void Event_PlayerFightRange(object in_sender, PlayerInRangeArgs in_args) {
        if(in_args.inRange && !enemiesInRange.Contains(in_args.enemyName)) {
            enemiesInRange.Add(in_args.enemyName);
        }
        else if (!in_args.inRange && enemiesInRange.Contains(in_args.enemyName)) {
            enemiesInRange.Remove(in_args.enemyName);
        }
        string logStr = "";
        foreach (string enemy in enemiesInRange) {
            logStr += enemy + ", ";
        }
        //Debug.Log(logStr);
    }
    public static void Event_BattleStart(object in_sender, BattleStartArgs in_args) {
        string logStr = in_args.playerName + " started a battle with ";
        foreach (string enemy in enemiesInRange) {
            logStr += enemy + ", ";
        }
        //Debug.Log(logStr);
    }

    private void SetLightColor() {
        currentColor = directionalLight.GetComponent<Light>().color;
        Color highlightColor = new Color(currentColor.r, currentColor.g, currentColor.b, highlightIntensity);

        directionalLight.GetComponent<Light>().color = currentColor;
        spriteMaterial.SetColor("_LightColor", highlightColor);
    }
    private void SetLightRotation() {
        rotationVector = directionalLight.transform.rotation * Vector3.forward;

        Vector2 newLightDirection = new Vector2(-rotationVector.x/2, -rotationVector.y/2);
        spriteMaterial.SetVector("_MyLightDirection", newLightDirection);
    }


    // Start is called before the first frame update
    void Start()
    {
        // Initialize Shader Values
        directionalLight = this.gameObject.transform.Find("Directional Light").gameObject;

        currentColor = directionalLight.GetComponent<Light>().color;
        currentIntensity = highlightIntensity;
        currentRotation = directionalLight.transform.rotation;
        rotationVector = directionalLight.transform.rotation * Vector3.forward;
        
        
        // Initialize PlayerInRange Event
        HelperFunctions.PlayerInRange += Event_PlayerFightRange;
        //Initialize BattleStartEvent
        HelperFunctions.BattleStart += Event_BattleStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationVector != directionalLight.transform.rotation * Vector3.forward) {
            SetLightRotation();
        }
        if (currentColor != directionalLight.GetComponent<Light>().color || currentIntensity != highlightIntensity) {
            SetLightColor();
        }
        
    }
}
