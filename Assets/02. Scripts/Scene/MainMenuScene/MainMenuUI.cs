using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject audioSettingsPanel;
    [SerializeField] private GameObject controlsPanel;

    // ------------------------------
    // 게임 시작 버튼
    // ------------------------------
    public void OnStartGame()
    {
        SceneManager.LoadScene("BattleScene");
    }

    // ------------------------------
    // 게임 종료 버튼
    // ------------------------------
    public void OnQuitGame()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }

    // ------------------------------
    // 옵션 버튼 (볼륨 설정창 열기)
    // ------------------------------
    public void OnClickOptions()
    {
        if (audioSettingsPanel != null)
            audioSettingsPanel.SetActive(true);
    }

    // ------------------------------
    // 닫기 버튼 (설정창 닫기)
    // ------------------------------
    public void OnClickClose()
    {
        if (audioSettingsPanel != null)
            audioSettingsPanel.SetActive(false);
    }
    public void OnClickControls()
    {
        controlsPanel?.SetActive(true);
    }
    public void OnClickCloseControls()
    {
        controlsPanel?.SetActive(false);
    }
}
