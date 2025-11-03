using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text resultText;
    public TMP_Text weaponListText;
    public TMP_Text passiveListText;

    private void Start()
    {
        UpdateResultUI();
    }

    private void UpdateResultUI()
    {
        int lastWave = PlayerPrefsData.lastWave;

        if (resultText != null)
            resultText.text = "Survived Wave " + lastWave;

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

    public void OnClickRetry()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
