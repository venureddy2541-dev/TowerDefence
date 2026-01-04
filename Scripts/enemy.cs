using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class enemy : MonoBehaviour
{
    public WeaponRank weaponRank;
    [SerializeField] EnemyData enemyData;
    [SerializeField] GameObject blastParticle;
    List<Node> Path = new List<Node>();
    [SerializeField] WeaponType machineGun;
    [SerializeField] WeaponType flameThrower;
    [SerializeField] WeaponType laserGun;
    [SerializeField] int enemyHealth;
    int enemyHealthInc = 0;
    int weaponBulletDamage;
    bool dead;
    public static event Action<enemy,EnemyStates> OnSpawned;
    Coroutine movement;

    void OnEnable()
    {
        enemyHealth = enemyData.health + enemyHealthInc;
        OnSpawned?.Invoke(this,EnemyStates.Active);
        dead = false;
        NewPath(true);
    }

    void OnDisable()
    {
        OnSpawned?.Invoke(this,EnemyStates.InActive);
    }

    void NewPath(bool newPath)
    { 
        Vector2Int coordinates;
        if(newPath)
        {
            coordinates = PathFinder.pathFinder.StartCoords;
        }
        else
        {
            coordinates = GridManager.gridManager.PositionToCoordinates(transform.position);
        }

        if(movement != null)
            StopCoroutine(movement);
        
        Path = PathFinder.pathFinder.RecalculatePath(coordinates);
        movement = StartCoroutine(EnemyPath());
    }

    IEnumerator EnemyPath()
    {
        for(int i = 1;i<Path.Count;i++)
        {
            Vector3 endPos = GridManager.gridManager.CoordinatesToPosition(Path[i].coordinates);

            transform.LookAt(endPos);
            while(Vector3.Distance(transform.position,endPos) > 0.05f)
            {
                while(GameManager.gameManager.gameOver) yield return null;

                transform.position = Vector3.MoveTowards(transform.position,endPos,Time.deltaTime*enemyData.speed);
                yield return null;
            }
            transform.position = endPos;
        }

        GameManager.gameManager.TowerHealthManager(enemyData.damageToTower);
        EnemyDeadState();
    }

    void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("MachineGun"))
        {
            weaponBulletDamage =  machineGun.weaponDamage;
        }

        if(other.CompareTag("FlameGun"))
        {
            weaponBulletDamage = flameThrower.weaponDamage;
        }

        if(other.CompareTag("LaserGun"))
        {
            weaponBulletDamage = laserGun.weaponDamage;
        }
        

        enemyHealth -= weaponBulletDamage;
        if(enemyHealth <= 0 && !dead)
        {
            dead = true;
            GameManager.gameManager.ScoreManager(enemyData.scoreForDeath);
            GameManager.gameManager.CurrencyManager(enemyData.currencyForEnemyDestroy);
            EnemyDeadState();
        }
    }

    void EnemyDeadState()
    {
        Instantiate(blastParticle,transform.position + Vector3.up*5f,Quaternion.identity);
        gameObject.SetActive(false);
        enemyHealthInc += enemyData.healthInc + 1*GameManager.gameManager.level;
        transform.position = GridManager.gridManager.CoordinatesToPosition(PathFinder.pathFinder.StartCoords);
        EnemyManager.enemyManager.Dead();
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        transform.position = GridManager.gridManager.CoordinatesToPosition(PathFinder.pathFinder.StartCoords);
        enemyHealthInc = 0;
    }
}
