using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        retryButton.onClick.AddListener(OnClickRetry);
        exitButton.onClick.AddListener(OnClickExit);
    }

    private void OnClickRetry()
    {
        // 배틀씬 다시 로드
        SceneManager.LoadScene("BattleScene");
    }

    private void OnClickExit()
    {
        //나가기
        Application.Quit();
        
    }
}
