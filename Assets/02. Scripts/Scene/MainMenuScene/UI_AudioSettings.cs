using UnityEngine;
using UnityEngine.UI;

public class UI_AudioSettings : MonoBehaviour
{
    [Header("Sliders")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            bgmSlider.value = AudioManager.Instance.bgmVolume;
            sfxSlider.value = AudioManager.Instance.sfxVolume;
        }

        // 이벤트 등록
        bgmSlider.onValueChanged.AddListener(OnBGMChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    private void OnBGMChanged(float value)
    {
        AudioManager.Instance?.SetBGMVolume(value);
    }

    private void OnSFXChanged(float value)
    {
        AudioManager.Instance?.SetSFXVolume(value);
    }
}
