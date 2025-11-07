using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// [GameOverUI]
/// ------------------------------------------------------------
/// 게임 오버 화면의 전체 UI를 관리하는 스크립트.
/// - 마지막 생존한 웨이브 표시
/// - 플레이어가 사용한 무기 / 패시브 아이템 목록 출력
/// - 다시 시작 / 메인 메뉴 / 종료 버튼 기능 제공
/// ------------------------------------------------------------
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private TMP_Text resultText;       // 생존 웨이브 표시 텍스트
    [SerializeField] private TMP_Text weaponListText;   // 무기 목록 텍스트
    [SerializeField] private TMP_Text passiveListText;  // 패시브 목록 텍스트

    [Header("씬 이름")]
    [SerializeField] private string mainMenuScene = "MainMenuScene"; // 메인 메뉴 씬 이름
    [SerializeField] private string battleScene = "BattleScene";     // 전투 씬 이름

    private void Start()
    {
        UpdateResultUI();
    }

    /// <summary>
    /// 게임 오버 시 표시되는 결과 UI를 갱신한다.
    /// </summary>
    private void UpdateResultUI()
    {
        // 마지막 생존 웨이브 표시
        int lastWave = PlayerPrefsData.lastWave;
        resultText?.SetText($"생존한 웨이브: {lastWave}");

        // 무기 목록 출력
        if (weaponListText != null)
            weaponListText.text = BuildItemList(PlayerPrefsData.ownedWeaponNames, "(보유 무기 없음)");

        // 패시브 목록 출력
        if (passiveListText != null)
            passiveListText.text = BuildItemList(PlayerPrefsData.ownedPassiveNames, "(보유 패시브 없음)");
    }

    /// <summary>
    /// 아이템 이름 리스트를 문자열 형태로 구성한다.
    /// </summary>
    private string BuildItemList(List<string> itemNames, string emptyMessage)
    {
        if (itemNames == null || itemNames.Count == 0)
            return emptyMessage;

        StringBuilder sb = new StringBuilder();
        foreach (string itemName in itemNames)
            sb.AppendLine(itemName);

        return sb.Length > 0 ? sb.ToString() : emptyMessage;
    }

    // 버튼 이벤트 처리
    /// <summary>
    /// 다시 시작 버튼 - 전투 씬 재시작
    /// </summary>
    public void OnClickRetry()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayBGM(0.7f);

        SceneManager.LoadScene(battleScene);
    }

    /// <summary>
    /// 메인 메뉴 버튼 - 메인 메뉴 씬으로 이동
    /// </summary>
    public void OnClickMainMenu()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayBGM(0.5f);

        SceneManager.LoadScene(mainMenuScene);
    }

    /// <summary>
    /// 게임 종료 버튼 - 애플리케이션 종료
    /// </summary>
    public void OnClickExit()
    {
        Application.Quit();
    }
}
