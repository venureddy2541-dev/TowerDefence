using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    [SerializeField] GameObject gunToolsObject;
    const string bullet = "Bullet";
    const string gunTools = "GunTools";

    void Awake()
    {
        Building();
    }

    void OnEnable()
    {
        GameManager.ClearTowers += ClearMe;
    }

    void OnDisable()
    {
        GameManager.ClearTowers -= ClearMe;
    }

    void OnDestroy()
    {
        GameManager.ClearTowers -= ClearMe;
    }

    void ClearMe()
    {
        Destroy(gameObject);
    }

    public void Building()
    {
        StartCoroutine(TowerBuild());
    }

    IEnumerator TowerBuild()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
            foreach(Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(false);
            }
        }

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            foreach(Transform grandChild in child)
            {
                if(!grandChild.CompareTag(bullet) && !grandChild.CompareTag(gunTools))
                {
                    grandChild.gameObject.SetActive(true);
                }
            }
        }
    }

    void OnMouseEnter()
    {
        gunToolsObject.SetActive(true);
    }

    void OnMouseExit()
    {
        gunToolsObject.SetActive(false);
    }
}
