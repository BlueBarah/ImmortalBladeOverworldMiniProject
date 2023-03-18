using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
    public class FieldAlly : Mover, IDetectPlayer
    {
        [SerializeField] private float detectionRadius;
        public bool flag_playerInRange { get; set; } = false;
        public bool flag_playerInProximity { get; set; } = false;
        public bool flag_playerDetected { get; set; } = false;
        public bool flag_battleStart { get; set; } = false;
        public float proximityRange { get; set; }
        public float detectionRange { get; set; }
        public static event Action<FieldAlly, bool> event_allyInRange; 

        private GameObject target;

        new void Awake() {
            proximityRange = detectionRadius;
        }

        new void Update() {
            if (target == null) {
                target = GameObject.FindGameObjectWithTag("Player");
                event_allyInRange?.Invoke(this, flag_playerInProximity);
            }
            else {
                if (flag_playerInProximity != CheckProximity()) {
                    flag_playerInProximity = CheckProximity();
                    event_allyInRange?.Invoke(this, flag_playerInProximity);
                }
            }
        }

        private bool CheckProximity() {
            return Vector3.Distance(transform.position, target.transform.position) <= detectionRadius;
        }
    }

}
