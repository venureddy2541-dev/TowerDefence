using UnityEngine;

public class WeaponSelection : MonoBehaviour
{
    public CurrentWeapon currentWeapon;

    [SerializeField] WeaponType machineGun;
    [SerializeField] WeaponType laserGun;
    [SerializeField] WeaponType flameThrower;

    void Awake()
    {   
        int requiredCrrency = 0;
        GameObject weapon = null;
        int damage = 0;
        currentWeapon.CurrentWeaponObject(weapon,requiredCrrency,damage);
    }

    public void MachineGun()
    {
        PlayAudio();
        currentWeapon.CurrentWeaponObject(machineGun.Weapon,machineGun.requiredCurrencyToPlace,machineGun.weaponDamage);       
    }

    public void FlameThrower()
    {
        PlayAudio();
        currentWeapon.CurrentWeaponObject(flameThrower.Weapon,flameThrower.requiredCurrencyToPlace,flameThrower.weaponDamage); 
    }

    public void LaserGun()
    {
        PlayAudio();
        currentWeapon.CurrentWeaponObject(laserGun.Weapon,laserGun.requiredCurrencyToPlace,laserGun.weaponDamage); 
    }

    void PlayAudio()
    {
        UiManager.uiManager.ClickAudioSound();
    }
}
