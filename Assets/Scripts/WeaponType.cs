using UnityEngine;

[CreateAssetMenu(fileName = "WeaponType", menuName = "Scriptable Objects/WeaponType")]
public class WeaponType : ScriptableObject
{
    public int index;
    public int requiredCurrencyToPlace;
    public GameObject Weapon;
    public float weaponRange;
    public int weaponDamage;
    public float rotationSpeed;
    public float FireRate;
    public float bulletRangeInc;
    public float FireRateIncrement;
    public int RequiredCurrencyForWeaponUpgrade;
}
