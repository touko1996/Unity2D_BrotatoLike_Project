using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // 코루틴용 네임스페이스 추가

public class PlayerStats : MonoBehaviour
{
    [Header("Current Stats")]
    public float currentDamage = 10f;
    public float currentRange = 5f;
    public float currentAttackSpeed = 1f;
    public float currentMoveSpeed = 5f;

    [Header("HP")]
    public float maxHp = 10f;
    public float currentHp = 10f;

    private bool isDead = false;
    private UI_PlayerStatus uiStatus;

    private void Start()
    {
        currentHp = maxHp;
        uiStatus = FindObjectOfType<UI_PlayerStatus>();
        UpdateHpUI();
    }

    // ---------------------------------------------
    // 스탯 변화 (패시브 아이템용)
    // ---------------------------------------------
    public void AddStatModifier(float dmg, float range, float atkSpeed, float move)
    {
        currentDamage += dmg;
        currentRange += range;
        currentAttackSpeed += atkSpeed;
        currentMoveSpeed += move;
    }

    public void RemoveStatModifier(float dmg, float range, float atkSpeed, float move)
    {
        currentDamage -= dmg;
        currentRange -= range;
        currentAttackSpeed -= atkSpeed;
        currentMoveSpeed -= move;
    }

    // ---------------------------------------------
    // 체력 변화
    // ---------------------------------------------
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHp -= amount;
        if (currentHp < 0f) currentHp = 0f;

        UpdateHpUI();

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHp += amount;
        if (currentHp > maxHp) currentHp = maxHp;
        UpdateHpUI();
    }

    // ---------------------------------------------
    // 사망 처리
    // ---------------------------------------------
    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log("Die() called: Player HP reached 0");

        // 게임오버 사운드 재생 (배경음악 정지 포함)
        AudioManager.Instance.PlayGameOver();

        // 웨이브 저장
        UI_GameWave waveUI = FindObjectOfType<UI_GameWave>();
        if (waveUI != null)
        {
            PlayerPrefsData.lastWave = waveUI.GetCurrentWave();
            Debug.Log("Wave data saved: " + PlayerPrefsData.lastWave);
        }

        // 인벤토리 데이터 복제 저장
        PlayerInventory inventory = GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            PlayerPrefsData.SaveFromInventory(inventory);
            Debug.Log("Inventory data copied to PlayerPrefsData");
        }

        // 약간의 딜레이 후 게임오버 씬 로드
        StartCoroutine(LoadGameOverSceneAfterDelay(1.5f));
    }

    private IEnumerator LoadGameOverSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameOverScene");
    }

    // ---------------------------------------------
    // HP UI 갱신
    // ---------------------------------------------
    private void UpdateHpUI()
    {
        if (uiStatus != null)
        {
            uiStatus.UpdateHPUI(currentHp, maxHp);
        }
    }
}
