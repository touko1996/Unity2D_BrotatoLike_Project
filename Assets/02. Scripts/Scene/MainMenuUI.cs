using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // 버튼 클릭 이벤트
    public void OnStartGame()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void OnQuitGame()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }
}
