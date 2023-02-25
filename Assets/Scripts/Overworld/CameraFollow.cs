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
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
            }
            else if (shouldHaveFollowTarget){
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }
}