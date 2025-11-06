using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [UI_AudioSettings]
/// --------------------------------------------------------------------
/// - 메인 메뉴의 오디오 설정 패널 전용 스크립트
/// - 슬라이더를 통해 AudioManager의 BGM/SFX 볼륨을 조절
/// - 설정값은 AudioManager 내부에서 자동으로 PlayerPrefs에 저장됨
/// --------------------------------------------------------------------
/// </summary>
public class UI_AudioSettings : MonoBehaviour
{
    [Header("슬라이더 참조")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // AudioManager가 존재하면 현재 볼륨값을 UI에 반영
        if (AudioManager.Instance != null)
        {
            if (bgmSlider != null)
                bgmSlider.value = AudioManager.Instance.bgmVolume;

            if (sfxSlider != null)
                sfxSlider.value = AudioManager.Instance.sfxVolume;
        }

        // 이벤트 등록 (값 변경 시 AudioManager에 전달)
        if (bgmSlider != null)
            bgmSlider.onValueChanged.AddListener(OnBGMChanged);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    /// <summary>
    /// BGM 슬라이더 값 변경 시 AudioManager에 전달
    /// </summary>
    private void OnBGMChanged(float value)
    {
        AudioManager.Instance?.SetBGMVolume(value);
    }

    /// <summary>
    /// SFX 슬라이더 값 변경 시 AudioManager에 전달
    /// </summary>
    private void OnSFXChanged(float value)
    {
        AudioManager.Instance?.SetSFXVolume(value);
    }

    /// <summary>
    /// 씬 전환 시 이벤트 중복 방지 (리스너 해제)
    /// </summary>
    private void OnDestroy()
    {
        if (bgmSlider != null)
            bgmSlider.onValueChanged.RemoveListener(OnBGMChanged);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSFXChanged);
    }
}
