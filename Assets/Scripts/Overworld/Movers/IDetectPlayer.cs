using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDetectPlayer
{
    float proximityRange { get; set; }
    float detectionRange { get; set; }
    bool flag_playerInRange { get; set; }
    bool flag_playerInProximity { get; set; }
    bool flag_playerDetected { get; set; }
    bool flag_battleStart { get; set; }
}
