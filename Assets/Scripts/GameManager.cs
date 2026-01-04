using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip timerClip;
    public static event Action ResetNodes;
    public static event Action ClearTowers;

    public bool gamePaused  { get; private set; } = false;
    public bool gameOver { get; private set; } = false;

    public int towerHealth = 20;
    int Score = 0;
    int Level = 0;
    public int level { get { return Level; } }

    // currencyIncByLevel * (level+offset) for next level
    int offset = 1;
    int currencyIncByLevel = 150;
    public int currency;
    public int LevelReset = 0;
    int waitTime;
    bool starting;
    

    void Awake()
    {
        starting = true;
        gameManager = this;
    }

    void Start()
    {
        UiManager.uiManager.UpdateTowerHealth(towerHealth);
        UiManager.uiManager.UpdateLevel(Level);
        CurrencyManager(currencyIncByLevel);
        ScoreManager(Score);
        Reset();
    }

    public void NextLevel()
    {
        Reset();
    }

    private void Reset()
    {
        StartCoroutine(ResetLevelDetails());
    }

    IEnumerator ResetLevelDetails()
    {
        yield return new WaitForSeconds(1f);

        if(!starting)
        {
            if(gameOver){ ResetGameDetails(); gameOver = false; }
            else  LevelManager();

            towerHealth = 20;
            UiManager.uiManager.UpdateTowerHealth(towerHealth);
            EnemyManager.enemyManager.ResetEnemies();
            GridManager.gridManager.ResetNodes();
            ResetNodes?.Invoke();
            ClearTowers?.Invoke();
        }

        if(starting) starting = false;
        waitTime = 3;
        while(0 < waitTime && !gameOver)
        {
            UiManager.uiManager.Timer(waitTime.ToString());
            audioSource.PlayOneShot(timerClip);
            waitTime--;
            yield return new WaitForSeconds(1f);
        }

        UiManager.uiManager.Timer("");
        EnemyManager.enemyManager.Invoker();
    }

    void ResetGameDetails()
    {
        Score = 0;
        Level = 0;
        LevelReset = 0;
        currency = currencyIncByLevel;

        UiManager.uiManager.UpdateAll(Score,currency,Level); 
    }

    //gameState
    public void PlayAgain()
    {
        gameOver = true;
        gamePaused = false;
        UiManager.uiManager.Timer("");
        Time.timeScale = 1f;
        StopAllCoroutines();
        Reset();
    }

    public void PauseState(bool state,float value)
    {
        gamePaused = state;
        Time.timeScale = value;
    }

    //Managers
    public void TowerHealthManager(int healthDec)
    {
        if(!gameOver)
        {
            towerHealth -= healthDec;
            if(towerHealth <= 0)
            { 
                gameOver = true; 
                gamePaused = true; 
                UiManager.uiManager.GameOverMenu(gameOver); 
                towerHealth = 0; 
            }
            UiManager.uiManager.UpdateTowerHealth(towerHealth);
        }
    }
    
    public void ScoreManager(int score)
    {
        Score += score;
        UiManager.uiManager.UpdateScore(Score);
    }

    public void LevelManager()
    {
        Level++;
        UiManager.uiManager.UpdateLevel(Level);

        currency = 0;
        CurrencyManager(currencyIncByLevel * (offset + Level));

        if(LevelReset == 9) LevelReset = 0;
        else LevelReset++;
    }

    public void CurrencyManager(int currencyInc)
    {
        currency += currencyInc;
        UiManager.uiManager.UpdateCurrency(currency);
    }
}
