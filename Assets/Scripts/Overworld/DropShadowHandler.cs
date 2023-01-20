using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Overworld
{
    public class DropShadowHandler : MonoBehaviour
    {
        [SerializeField] private Material shadowMaterial;
        [SerializeField] private Material waterMaterial;
        private DecalProjector shadowProjector;
        // private Rigidbody parent;

        void Awake()
        {
            shadowProjector = GetComponent<DecalProjector>();
            // parent = transform.parent.gameObject.GetComponent<Rigidbody>();
        }
        void Update()
        {
            // Debug.Log(parent?.velocity);
        }

        public void SetShadowType(string in_type)
        {
            if (in_type == "water")
            {
                shadowProjector.material = waterMaterial;
                transform.position = transform.position + new Vector3(0f, 0f, 0.75f);
            }
            else
            {
                shadowProjector.material = shadowMaterial;
                transform.position = transform.position - new Vector3(0f, 0f, 0.75f);
            }
        }
    }
}