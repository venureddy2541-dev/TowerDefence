using UnityEngine;
using TMPro;

public class GunTools : MonoBehaviour
{
    Tower tower;
    [SerializeField] Weapon weapon;
    [SerializeField] WeaponType weaponType;
    int currencyForUpgrade;
    [SerializeField] TMP_Text weaponLevelText;
    [SerializeField] GameObject Parent;
    int level = 1;
    int maxLevel = 4;
    int valueToGetHalf = 2;

    void Start()
    {
        tower = GetComponentInParent<Tower>();
        currencyForUpgrade = weaponType.RequiredCurrencyForWeaponUpgrade;
    }

    public void OnUpgradeWeapon()
    {
        if(GameManager.gameManager.currency >= currencyForUpgrade && !GameManager.gameManager.gamePaused)
        {
            if(level<maxLevel)
            {
                level++;
                weaponLevelText.text = level.ToString();
                WeaponUpgrading();
            }  
            else if(level == maxLevel)
            {
                level++;
                weaponLevelText.text = "MAXED";
                WeaponUpgrading();
            }  
        }
    }

    public void OnDestroyWeapon()
    {
        if(!GameManager.gameManager.gamePaused)
        {
           GameManager.gameManager.CurrencyManager(currencyForUpgrade/valueToGetHalf);
            Vector2Int newCoordinates = GridManager.gridManager.PositionToCoordinates(transform.parent.position);
            GridManager.gridManager.Grid[newCoordinates].isWakable = true;
            Destroy(Parent); 
        }
    }

    void WeaponUpgrading()
    {
        weapon.BulletDamager(weaponType.FireRate);
        GameManager.gameManager.CurrencyManager(-currencyForUpgrade);
        currencyForUpgrade += weaponType.RequiredCurrencyForWeaponUpgrade;
        tower.Building();
    }
}
