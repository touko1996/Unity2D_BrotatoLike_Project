using UnityEngine;
using TMPro;

/// <summary>
/// [DamageText]
/// ------------------------------------------------------------
/// 데미지 발생 시 화면에 표시되는 텍스트를 제어하는 스크립트.
/// - 위로 부드럽게 떠오르며
/// - 일정 속도로 서서히 사라짐 (페이드아웃)
/// - 텍스트 색상과 크기 랜덤화 지원
/// ------------------------------------------------------------
/// </summary>
public class DamageText : MonoBehaviour
{
    [Header("컴포넌트 참조")]
    [SerializeField] private TMP_Text damageText;      // 표시할 데미지 텍스트
    private CanvasGroup canvasGroup;                   // 투명도 제어용 컴포넌트

    [Header("이동 및 페이드 설정")]
    [SerializeField] private float floatSpeed = 1f;    // 위로 떠오르는 속도
    [SerializeField] private float fadeSpeed = 1.5f;   // 서서히 사라지는 속도

    private void Awake()
    {
        // CanvasGroup 캐싱 (없으면 자동 추가)
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // TMP_Text 연결 (직접 참조 또는 자식에서 탐색)
        if (damageText == null)
            damageText = GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    /// 기본 흰색 텍스트로 데미지 표시 (몬스터용)
    /// </summary>
    public void SetText(float damage)
    {
        SetText(damage, Color.white);
    }

    /// <summary>
    /// 지정된 색상으로 데미지 표시 (플레이어 피격 등)
    /// </summary>
    public void SetText(float damage, Color color)
    {
        if (damageText == null) return;

        // 데미지 값 표시 (정수 변환)
        damageText.text = ((int)damage).ToString();
        damageText.color = color;

        // 크기 랜덤화 (자연스러운 연출)
        transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);

        // 새로 생성될 때마다 투명도 복원
        canvasGroup.alpha = 1f;
    }

    private void Update() //업데이트에서 실시간으로 텍스트 호출
    {
        FloatUp();
        FadeOut();
    }

    /// <summary>
    /// 텍스트가 위로 천천히 이동
    /// </summary>
    private void FloatUp()
    {
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 텍스트가 점점 투명해지며 제거
    /// </summary>
    private void FadeOut()
    {
        if (canvasGroup == null) return;

        canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
        if (canvasGroup.alpha <= 0f)
            Destroy(gameObject);
    }
}
