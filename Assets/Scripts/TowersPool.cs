using UnityEngine;
using System.Collections.Generic;

public class TowersPool : MonoBehaviour
{
    public List<enemy> enemies = new List<enemy>();

    void OnEnable()
    {
        enemy.OnSpawned += EnemyStatus;
    }

    void OnDisable()
    {
        enemy.OnSpawned -= EnemyStatus;
    }

    void EnemyStatus(enemy newEnemy,EnemyStates enemyStates)
    {
        if(EnemyStates.Active == enemyStates){ enemies.Add(newEnemy); }
        else { enemies.Remove(newEnemy); }
    }
}
