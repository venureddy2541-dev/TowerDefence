using UnityEngine;

[CreateAssetMenu(fileName = "CurrentWeapon", menuName = "Scriptable Objects/CurrentWeapon")]
public class CurrentWeapon : ScriptableObject
{
    public GameObject weapon;
    public int requiredCrrency;
    public int damage;

    void Start()
    {
        weapon = null;
        requiredCrrency = 0;
        damage = 0;
    }

    public void CurrentWeaponObject(GameObject weapon,int requiredCrrency,int damage)
    {
        this.weapon = weapon;
        this.requiredCrrency = requiredCrrency;
        this.damage = damage;
    }
}
