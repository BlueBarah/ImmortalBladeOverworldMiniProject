using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInRangeArgs {
    public bool inRange;
    public string enemyName;
}
public class BattleStartArgs {
    public string playerName;
}

public static class HelperFunctions
{
    public static event EventHandler<PlayerInRangeArgs> PlayerInRange;
    public static event EventHandler<BattleStartArgs> BattleStart;
    public static bool CheckProximity(Vector3 in_myPosition, Vector3 in_checkPosition, float in_tolerance) {
        return (System.Math.Abs(in_myPosition.x - in_checkPosition.x) <= in_tolerance &&
                System.Math.Abs(in_myPosition.y - in_checkPosition.y) <= in_tolerance &&
                System.Math.Abs(in_myPosition.z - in_checkPosition.z) <= in_tolerance  );
    }

    public static Vector3 GetRandomPositionInRange(Vector3 in_origin, float in_range) {
            //Get direction
            Vector3 randDirection = new Vector3(UnityEngine.Random.Range(-1.00f, 1.00f), 0, UnityEngine.Random.Range(-1.00f, 1.00f));

            //Get distance
            float distance = UnityEngine.Random.Range(0.00f, in_range);

            //Maybe cast a ray in this direction, make sure it legal. If not, pick a new destination
            return in_origin + (distance * randDirection);
    }

    public static void FirePlayerInRangeEvent(object in_Sender, bool in_inRange, string in_enemyName) {
        PlayerInRange?.Invoke(in_Sender, new PlayerInRangeArgs {
            inRange = in_inRange, 
            enemyName = in_enemyName
        });
    }
    public static void FireBattleStartEvent(object in_sender, string in_playerName) {
        BattleStart?.Invoke(in_sender, new BattleStartArgs {
            playerName = in_playerName,
        });
    }
}
