using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BossHP : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;
    [SerializeField] private TMP_Text bossNameText;

    private BossMonster currentBoss;
    private bool isActive = false;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isActive) return;
        if (currentBoss == null) return;

        float maxHp = currentBoss.GetMaxHp();
        float curHp = currentBoss.GetCurrentHp();
        float fill = maxHp > 0f ? curHp / maxHp : 0f;

        if (hpFillImage != null)
            hpFillImage.fillAmount = fill;
    }

    public void InitBoss(BossMonster boss)
    {
        if (boss == null) return;

        currentBoss = boss;
        isActive = true;

        if (bossNameText != null)
            bossNameText.text = boss.gameObject.name.ToUpper();

        gameObject.SetActive(true);

        // 즉시 HP 한 번 반영 (초기 딸피 문제 해결)
        UpdateBossHPInstant();
    }

    private void UpdateBossHPInstant()
    {
        if (currentBoss == null || hpFillImage == null) return;

        float maxHp = currentBoss.GetMaxHp();
        float curHp = currentBoss.GetCurrentHp();
        float fill = maxHp > 0f ? curHp / maxHp : 0f;
        hpFillImage.fillAmount = fill;
    }

    public void Hide()
    {
        isActive = false;
        currentBoss = null;
        gameObject.SetActive(false);
    }
}
