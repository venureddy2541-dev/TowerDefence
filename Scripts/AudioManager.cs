using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;
    [SerializeField] AudioMixer audioMixer;

    void Awake()
    {
        audioManager = this;
    }
    
    public void SetMusicVolume(float volume)
    {
        //To Convert The Volume Into Deccibles 0f - -80f
        volume = Mathf.Clamp(volume,0.001f,1f);
        volume = Mathf.Log10(volume)*20f;
        audioMixer.SetFloat("MusicVolume",volume);
    }

    public void SetSfxVolume(float volume)
    {
        volume = Mathf.Clamp(volume,0.001f,1f);
        volume = Mathf.Log10(volume)*20f;
        audioMixer.SetFloat("SfxVolume",volume);
    }
}
