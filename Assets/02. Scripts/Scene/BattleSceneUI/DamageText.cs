using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TMP_Text text;
    public float floatSpeed = 1f;
    public float fadeSpeed = 1.5f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (text == null)
            text = GetComponentInChildren<TMP_Text>();
    }

    // 기본: 흰색 데미지 (몬스터용)
    public void SetText(float damage)
    {
        SetText(damage, Color.white);
    }

    // 오버로드: 색상 지정 (플레이어용 빨강 등)
    public void SetText(float damage, Color color)
    {
        if (text == null) return;

        text.text = ((int)damage).ToString();
        text.color = color;
        transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);
    }

    private void Update()
    {
        // 위로 이동
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

        // 페이드아웃
        canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
        if (canvasGroup.alpha <= 0)
            Destroy(gameObject);
    }
}
