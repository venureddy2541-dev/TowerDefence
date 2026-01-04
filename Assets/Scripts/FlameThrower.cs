using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class Weapon : MonoBehaviour
{
    public ParticleSystem mazilFlash;
    public ParticleSystem bullet;
    public WeaponType weaponData;
    float weaponRange;
    public AudioSource audioSource;
    public bool enemyDitected = false;
    float distanceBtwenemyAndWeaponRange;
    float bulletsPerSecond;
    public float bulletFireRate;
    public enemy nearestEnemy;
    TowersPool towersPool;

    protected virtual void Start()
    {
        weaponRange = weaponData.weaponRange;
        bulletsPerSecond = weaponData.FireRate;
        bulletFireRate = 1f/bulletsPerSecond;

        towersPool = GetComponentInParent<TowersPool>();

        var mazilFlashEmi = mazilFlash.emission;
        var bulletEmi = bullet.emission;
        
        mazilFlashEmi.enabled = enemyDitected;
        bulletEmi.enabled = enemyDitected;
    }

    protected virtual void Update()
    {
        WeaponFacing();
        Firing();
    }

    void WeaponFacing()
    {
        if(towersPool.enemies.Count <= 0) return;
        
        float maxDist = Mathf.Infinity;
        foreach(enemy enemy in towersPool.enemies)
        {
            distanceBtwenemyAndWeaponRange = Vector3.Distance(transform.position,enemy.transform.position);
            if(distanceBtwenemyAndWeaponRange<maxDist)
            {    
                nearestEnemy = enemy;
                maxDist = distanceBtwenemyAndWeaponRange;
            }
        }
    }

    protected virtual void Firing()
    {
        if(nearestEnemy)
        {
            float targetPos = Vector3.Distance(transform.position,nearestEnemy.transform.position);
            if(targetPos<weaponRange && !GameManager.gameManager.gamePaused && nearestEnemy.gameObject.activeInHierarchy)
            {
                enemyDitected = true;
                PlayAudio();
                Rotate();
            }
            else
            {
                enemyDitected = false; 
                StopAudio();
            }
            
            Fire();   
        }
    }

    protected virtual void PlayAudio()
    {
        if(!audioSource.isPlaying && enemyDitected)
        {
            audioSource.Play();
        }
    }

    protected virtual void StopAudio()
    {
        audioSource.Stop();
    }

    void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                Quaternion.LookRotation((nearestEnemy.transform.position - transform.position)),
                                                Time.deltaTime*weaponData.rotationSpeed);
    }

    //Inherits from laser
    protected virtual void Fire()
    {
        var mazilFlashEmi = mazilFlash.emission;
        var bulletEmi = bullet.emission;
        
        mazilFlashEmi.enabled = enemyDitected;
        bulletEmi.enabled = enemyDitected;
    }

    public virtual void BulletDamager(float fireRate)
    {
        weaponRange++;
        bulletsPerSecond += fireRate;
        this.bulletFireRate = 1f/bulletsPerSecond;

        var bulletEmissionsRateOfFire = bullet.emission;
        bulletEmissionsRateOfFire.rateOverTime = bulletsPerSecond;

        var main = bullet.main;
        float currentlife = main.startLifetime.constant;
        currentlife += weaponData.bulletRangeInc;
        main.startLifetime = currentlife;

        var mazilFlashEmissionsRateOfFire = mazilFlash.emission;
        mazilFlashEmissionsRateOfFire.rateOverTime = bulletsPerSecond;
    }
}
