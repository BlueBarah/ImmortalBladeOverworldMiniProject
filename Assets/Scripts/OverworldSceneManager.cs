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

    private void SetLightColor() {
        currentColor = directionalLight.GetComponent<Light>().color;
        Color highlightColor = new Color(currentColor.r, currentColor.g, currentColor.b, highlightIntensity);

        directionalLight.GetComponent<Light>().color = currentColor;
        spriteMaterial.SetColor("_LightColor", highlightColor);
    }
    private void SetLightRotation() {
        rotationVector = directionalLight.transform.rotation * Vector3.forward;

        Vector2 newLightDirection = new Vector2(-rotationVector.x/2, -rotationVector.y/2);
        spriteMaterial.SetVector("_LightDirection", newLightDirection);
    }


    // Start is called before the first frame update
    void Start()
    {
        directionalLight = this.gameObject.transform.Find("Directional Light").gameObject;

        currentColor = directionalLight.GetComponent<Light>().color;
        currentIntensity = highlightIntensity;
        currentRotation = directionalLight.transform.rotation;
        rotationVector = directionalLight.transform.rotation * Vector3.forward;
        Debug.Log(currentColor);
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
