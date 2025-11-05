using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text resultText;
    public TMP_Text weaponListText;
    public TMP_Text passiveListText;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuScene = "MainMenuScene";
    [SerializeField] private string battleScene = "BattleScene";

    private void Start()
    {
        UpdateResultUI();
    }

    private void UpdateResultUI()
    {
        int lastWave = PlayerPrefsData.lastWave;

        if (resultText != null)
            resultText.text = "생존한 웨이브: " + lastWave;

        if (weaponListText != null)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string name in PlayerPrefsData.weaponNames)
                sb.AppendLine(name);
            weaponListText.text = sb.Length > 0 ? sb.ToString() : "(No Weapons)";
        }

        if (passiveListText != null)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string name in PlayerPrefsData.passiveNames)
                sb.AppendLine(name);
            passiveListText.text = sb.Length > 0 ? sb.ToString() : "(No Passives)";
        }
    }

    // ---------------------------------------------
    // 다시 시작 버튼
    // ---------------------------------------------
    public void OnClickRetry()
    {
        // 배경음악 재생 (전투씬용)
        AudioManager.Instance.PlayBGM(0.7f);

        // 전투씬 재시작
        SceneManager.LoadScene(battleScene);
    }

    // ---------------------------------------------
    // 메인 메뉴로 버튼
    // ---------------------------------------------
    public void OnClickMainMenu()
    {
        // 배경음악 재생 (메인 메뉴용)
        AudioManager.Instance.PlayBGM(0.5f);

        // 메인 메뉴 씬 로드
        SceneManager.LoadScene(mainMenuScene);
    }

    // ---------------------------------------------
    // 게임 종료 버튼
    // ---------------------------------------------
    public void OnClickExit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
