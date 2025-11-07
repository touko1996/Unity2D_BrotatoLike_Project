using UnityEngine;

/// <summary>
/// [AudioManager]
/// ----------------------------------------------------------------------
/// - 게임 전반의 BGM 및 SFX를 관리하는 싱글톤 오디오 매니저
/// - 볼륨 설정은 PlayerPrefs에 저장 및 불러오기 지원
/// - 씬이 전환되어도 파괴되지 않으며, 중복 생성을 방지함
/// - 리트라이(씬 재시작) 시 BGM이 갑자기 커지는 버그를 수정함
/// ----------------------------------------------------------------------
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("오디오 소스")]
    [SerializeField] private AudioSource bgmSource; // 배경음 전용
    [SerializeField] private AudioSource sfxSource; // 효과음 전용

    [Header("오디오 클립")]
    [SerializeField] private AudioClip bgmMain;      // 메인 BGM
    [SerializeField] private AudioClip sfxGun;       // 총소리
    [SerializeField] private AudioClip sfxCoin;      // 코인 획득
    [SerializeField] private AudioClip sfxGameOver;  // 게임 오버
    [SerializeField] private AudioClip sfxLevelUp;   // 레벨업

    [Header("볼륨 설정")]
    [Range(0f, 1f)] public float bgmVolume = 0.3f; // BGM 기본 볼륨
    [Range(0f, 1f)] public float sfxVolume = 1f;   // SFX 기본 볼륨

    private void Awake()
    {
        // 싱글톤 중복 방지
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

        // 저장된 볼륨 불러오기 (없으면 기본값 사용)
        bgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 0.3f);
        sfxVolume = PlayerPrefs.GetFloat("SFX_VOLUME", 1f);
    }

    private void OnEnable()
    {
        // 씬 재진입 시 볼륨 즉시 복원 (볼륨 튀는 문제 방지)
        if (bgmSource != null) bgmSource.volume = bgmVolume;
        if (sfxSource != null) sfxSource.volume = sfxVolume;
    }

    private void Start()
    {
        PlayBGM();
    }

    /// <summary>
    /// 메인 BGM 재생 (이미 재생 중이면 중복 재생 방지)
    /// </summary>
    public void PlayBGM()
    {
        if (bgmSource == null || bgmMain == null) return;

        // 볼륨 먼저 확실히 반영
        bgmSource.volume = bgmVolume;

        // 이미 같은 클립이 재생 중이면 재시작하지 않음
        if (bgmSource.isPlaying && bgmSource.clip == bgmMain)
            return;

        bgmSource.clip = bgmMain;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    /// <summary>
    /// 볼륨 지정 후 즉시 BGM 재생
    /// </summary>
    public void PlayBGM(float volume)
    {
        SetBGMVolume(volume);
        PlayBGM();
    }

    /// <summary>
    /// BGM 볼륨 설정 및 저장
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;

        PlayerPrefs.SetFloat("BGM_VOLUME", bgmVolume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// SFX 볼륨 설정 및 저장
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;

        PlayerPrefs.SetFloat("SFX_VOLUME", sfxVolume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 일반 SFX 재생
    /// </summary>
    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip == null || sfxSource == null) return;
        float finalVolume = sfxVolume * volumeMultiplier;
        sfxSource.PlayOneShot(clip, finalVolume);
    }

    /// <summary>총소리 재생</summary>
    public void PlayGunSFX() => PlaySFX(sfxGun, 0.4f);

    /// <summary>코인 획득음 재생</summary>
    public void PlayCoinSFX() => PlaySFX(sfxCoin, 0.3f);

    /// <summary>레벨업 효과음 재생</summary>
    public void PlayLevelUpSFX() => PlaySFX(sfxLevelUp, 0.6f);

    /// <summary>
    /// 게임 오버 시 처리: BGM 정지 후 효과음 재생
    /// </summary>
    public void PlayGameOver()
    {
        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.Stop();

        PlaySFX(sfxGameOver, 1f);
    }
}
