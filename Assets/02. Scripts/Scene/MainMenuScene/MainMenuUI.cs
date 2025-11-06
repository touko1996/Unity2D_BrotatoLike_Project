using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// [MainMenuUI]
/// --------------------------------------------------------------------
/// 메인 메뉴 UI 버튼 제어 스크립트
/// - 게임 시작, 종료, 옵션(볼륨), 조작법 창 열기/닫기 담당
/// --------------------------------------------------------------------
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    private const string BattleSceneName = "BattleScene"; //배틀씬이름 상수화

    [Header("패널 참조")]
    [SerializeField] private GameObject audioSettingsPanel;
    [SerializeField] private GameObject controlsPanel;

    private void Start()
    {
        // 메인 메뉴 진입 시 BGM 재생 보장 (선택 사항)
        AudioManager.Instance?.PlayBGM();
    }

    /// <summary>
    /// [게임 시작 버튼] → 배틀씬 로드
    /// </summary>
    public void OnStartGame()
    {
        SceneManager.LoadScene(BattleSceneName);
    }

    /// <summary>
    /// [게임 종료 버튼] → 애플리케이션 종료
    /// </summary>
    public void OnQuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// [옵션 버튼] → 오디오 설정창 열기
    /// </summary>
    public void OnClickOptions()
    {
        audioSettingsPanel?.SetActive(true); //null safe 연산자 : 객체가 null이 아닐 때만 SetActive 실행
    }

    /// <summary>
    /// [설정창 닫기 버튼]
    /// </summary>
    public void OnClickClose()
    {
        audioSettingsPanel?.SetActive(false);
    }

    /// <summary>
    /// [조작법 버튼] → 조작 설명창 열기
    /// </summary>
    public void OnClickControls()
    {
        controlsPanel?.SetActive(true);
    }

    /// <summary>
    /// [조작창 닫기 버튼]
    /// </summary>
    public void OnClickCloseControls()
    {
        controlsPanel?.SetActive(false);
    }
}
