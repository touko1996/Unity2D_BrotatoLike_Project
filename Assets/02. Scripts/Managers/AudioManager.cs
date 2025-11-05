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
    }

    private void Start()
    {
        PlayBGM();
    }

    public void PlayBGM(float volume = 0.5f)
    {
        if (bgmSource == null || bgmMain == null) return;
        bgmSource.clip = bgmMain;
        bgmSource.loop = true;
        bgmSource.volume = volume;
        bgmSource.Play();
    }

    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
            bgmSource.volume = volume;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayGunSFX() => PlaySFX(sfxGun, 0.9f);
    public void PlayCoinSFX() => PlaySFX(sfxCoin, 0.8f);

    public void PlayGameOver()
    {
        // 배경음악 정지
        if (bgmSource.isPlaying)
            bgmSource.Stop();

        PlaySFX(sfxGameOver, 1f);
    }
}
