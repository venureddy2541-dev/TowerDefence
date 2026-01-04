using UnityEngine;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour
{
    [SerializeField] Transform towerParent;
    Vector2Int coordinates;
    [SerializeField] bool isPlacable = true;
    bool originalisPlacable;
    WeaponSelection weaponSelection;
    public CurrentWeapon currentWeapon;

    [SerializeField] int requiredCurrencyForTowerPlacement;

    void Awake()
    {
        originalisPlacable = isPlacable;
        weaponSelection = FindFirstObjectByType<WeaponSelection>();
    }

    void Start()
    {
        if(GridManager.gridManager != null)
        {
            coordinates = GridManager.gridManager.PositionToCoordinates(transform.position);
            if(!isPlacable)
            {
                GridManager.gridManager.BlockNode(coordinates);
            }
        }
    }

    void OnEnable()
    {
        GameManager.ResetNodes += ResetBlockedNodes;
    }

    void OnDisable()
    {
        GameManager.ResetNodes -= ResetBlockedNodes;
    }

    void OnDestroy()
    {
        GameManager.ResetNodes -= ResetBlockedNodes;
    }

    void ResetBlockedNodes()
    {
        if(originalisPlacable) 
        {
            isPlacable = true;
            GridManager.gridManager.UnBlockNode(coordinates);
        }
    }


    public void PlaceTower()
    {
        if(currentWeapon.weapon == null){ return; }
        
        if(GridManager.gridManager.Grid.ContainsKey(coordinates))
        {
            WeaponPlacing();
        }
    }

    void WeaponPlacing()
    {
        if(GridManager.gridManager.Grid[coordinates].isWakable && GameManager.gameManager.currency >= currentWeapon.requiredCrrency)
        {
            if(!PathFinder.pathFinder.WillBlockPath(coordinates))
            {
                bool isSuccessful = Instantiate(currentWeapon.weapon,transform.position,Quaternion.identity,towerParent);
                GameManager.gameManager.CurrencyManager(-currentWeapon.requiredCrrency);
                if(isSuccessful)
                {
                    GridManager.gridManager.BlockNode(coordinates);
                    isPlacable = false;
                }
                PathFinder.pathFinder.NotifyReceivers();
            }
        }
    }
}
