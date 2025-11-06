using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip bgmMain;      // 배경음악
    public AudioClip sfxGun;       // 기본 총소리
    public AudioClip sfxCoin;      // 코인획득
    public AudioClip sfxGameOver;  // 게임오버
    public AudioClip sfxLevelUp;   // 레벨업

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float bgmVolume = 0.3f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 저장된 볼륨 불러오기
        bgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 0.3f);
        sfxVolume = PlayerPrefs.GetFloat("SFX_VOLUME", 1f);
    }

    private void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        if (bgmSource == null || bgmMain == null) return;

        bgmSource.clip = bgmMain;
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }
    public void PlayBGM(float volume)
    {
        SetBGMVolume(volume);
        PlayBGM();
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;

        PlayerPrefs.SetFloat("BGM_VOLUME", bgmVolume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;

        PlayerPrefs.SetFloat("SFX_VOLUME", sfxVolume);
        PlayerPrefs.Save();
    }

    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip == null || sfxSource == null) return;
        float finalVolume = sfxVolume * volumeMultiplier;
        sfxSource.PlayOneShot(clip, finalVolume);
    }

    public void PlayGunSFX() => PlaySFX(sfxGun, 0.4f);
    public void PlayCoinSFX() => PlaySFX(sfxCoin, 0.3f);
    public void PlayLevelUpSFX() => PlaySFX(sfxLevelUp, 0.6f);

    public void PlayGameOver()
    {
        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.Stop();

        PlaySFX(sfxGameOver, 1f);
    }
}
