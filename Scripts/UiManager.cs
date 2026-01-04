using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager uiManager;
    [SerializeField] AudioSource audioSource;

    [SerializeField] GameObject menu;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject continueButton;
    
    [SerializeField] Slider backGroundMusicSlider;
    [SerializeField] Slider masterSlider;

    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text towerHealthText;
    [SerializeField] TMP_Text currencyText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text levelText;

    void Awake()
    {
        uiManager = this;
    }

    //Audios
    public void OnMusicSliderValueChanged(float volume)
    {
        AudioManager.audioManager.SetMusicVolume(volume);
    }

    public void OnSfxSliderValueChanged(float volume)
    {
        AudioManager.audioManager.SetSfxVolume(volume);
    }

    // menu buttons
    public void OnPlayAgain()
    {
        SetButtons(false);
        GameManager.gameManager.PlayAgain();
    }

    public void OnPause()
    {
        GameManager.gameManager.PauseState(true,0f);
        SetButtons(true);
    }

    public void OnContinue()
    {
        GameManager.gameManager.PauseState(false,1f);
        SetButtons(false);
    }

    public void OnExit()
    {
        ClickAudioSound();
        Application.Quit();
    }

    public void SetButtons(bool paused)
    {
        ClickAudioSound();
        menu.SetActive(paused);
        continueButton.SetActive(paused);
        pauseButton.SetActive(!paused);
    }

    public void ClickAudioSound()
    {
        audioSource.Play();
    }

    public void GameOverMenu(bool state)
    {
        menu.SetActive(state); 
        pauseButton.SetActive(!state);
    }

    //game ui Texts
    public void UpdateTowerHealth(int towerHealth)
    {
        towerHealthText.text = "TowerHealth : " + towerHealth;
    }

    public void UpdateCurrency(int currency)
    { 
        currencyText.text = "Currency : " + currency;
    }
    
    public void UpdateScore(int score)
    {
        scoreText.text = "SCORE : " + score;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = "LEVEL : " + level;
    }

    public void UpdateAll(int score,int currency,int level)
    {
        UpdateCurrency(currency);
        UpdateScore(score);
        UpdateLevel(level);
    }

    public void Timer(string count)
    {
        timerText.text = count;
    }
}
