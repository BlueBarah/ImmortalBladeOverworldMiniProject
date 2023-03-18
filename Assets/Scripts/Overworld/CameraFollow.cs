using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float smoothTime = 0.3f;
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool spriteSortCameraDirection = true;
        [SerializeField] private bool shouldHaveFollowTarget;
        private Vector3 velocity = Vector3.zero;
        private Vector3 truePosition;
        private float wallContactZ;
        private bool flag_wallContact = false;
        private Camera cam;
        void Awake()
        {
            cam = transform.Find("Main Camera").GetComponent<Camera>();
            cam.transparencySortMode = TransparencySortMode.CustomAxis;
            if (spriteSortCameraDirection)
            {
                cam.transparencySortAxis = cam.transform.rotation * Vector3.forward;
            }
            else
            {
                cam.transparencySortAxis = new Vector3(0, 0, 1f);
            }
        }
        private void FixedUpdate()
        {
            if (target != null)
            {
                Vector3 targetPos = target.position + offset;
                truePosition = targetPos;
                if (flag_wallContact && truePosition.z <= wallContactZ) targetPos.z = wallContactZ;
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
            }
            else if (shouldHaveFollowTarget){
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        void OnTriggerEnter(Collider in_collider) {
            Debug.Log("Camera Collision");
            if (in_collider.gameObject.name == "4th Wall") {
                flag_wallContact = true;
                wallContactZ = in_collider.ClosestPoint(transform.position).z;
            }
        }
        void OnTriggerExit(Collider in_collider) {
            
            if (in_collider.gameObject.name == "4th Wall") {
                flag_wallContact = false;
            }
        }
    }
}