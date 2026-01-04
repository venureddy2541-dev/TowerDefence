using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManager;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip blastClip;

    //All enemies data
    [System.Serializable]
    public class poolData
    {
        public int rank;
        public float gapOffSet;
        public GameObject prefab;
        public int Size = 10;
        public List<enemy> pool;
    }
    public List<poolData> enemyPoolsData = new List<poolData>();
    Dictionary<int,List<enemy>> enemyPools = new Dictionary<int, List<enemy>>();

    //Level ScriptableObjects
    public List<LevelData> levelSbs;
    [SerializeField] TMP_Text waveText;
    [SerializeField] int Waves = 10;
    int Wave = 0;
    [SerializeField] int enemyCount = 10;

    void Awake()
    {
        enemyManager = this;
        EnemyPooling();
    }

    void EnemyPooling()
    {
        foreach(poolData pl in enemyPoolsData)
        {
            pl.pool = new List<enemy>();
            for(int i=0;i<pl.Size;i++)
            {
                GameObject enemyRef = Instantiate(pl.prefab,transform.position,Quaternion.identity,transform);
                pl.pool.Add(enemyRef.GetComponent<enemy>());
                enemyRef.SetActive(false);
            }
            enemyPools[pl.rank] = pl.pool;
        }
    }

    public void Invoker()
    {
        if(Wave<Waves)
        {
            waveText.text = "WAVE : "+Wave +"/"+Waves;
            PoolStarter();
        }
        else
        {
            GameManager.gameManager.NextLevel();
        }
    }

    void PoolStarter()
    {
        StartCoroutine(EnemyStarter());
    }

    IEnumerator EnemyStarter()
    {
        yield return new WaitForSeconds(1f);

        float waitTime = 1f;
        int rank = levelSbs[GameManager.gameManager.LevelReset].waveData[Wave].rank;
        List<enemy> enemies = enemyPools[rank];
        
        foreach (poolData item in enemyPoolsData)
        {
            if(rank == item.rank)
            {
                waitTime = item.gapOffSet;
                break;
            }
        }

        for(int i = 0;i<enemies.Count;i++)
        {
            yield return new WaitForSeconds(waitTime);
            if(GameManager.gameManager.gameOver) yield break;
            enemies[i].gameObject.SetActive(true);
        }
        Wave++;
    }

    public void Dead()
    {
        audioSource.PlayOneShot(blastClip);
        enemyCount--;
        if(GameManager.gameManager.towerHealth > 0 && enemyCount == 0)
        {
            enemyCount = 10;
            Invoker();
        }
    }

    public void ResetEnemies()
    {
        StopCoroutine(EnemyStarter());
        foreach (var items in enemyPools)
        {
            foreach (enemy item in items.Value)
            {
                item.Reset();
            }
        }
        enemyCount = 10;
        Wave = 0;
        waveText.text = "WAVE : "+Wave +"/"+Waves;
    }
}
