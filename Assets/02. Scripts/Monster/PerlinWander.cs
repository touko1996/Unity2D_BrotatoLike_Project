using UnityEngine;
/*********************************************************************************************
 [Perlin Noise]
 - 펄린 노이즈는 부드럽고 자연스러운 랜덤 이동을 구현하기 위해 완전히 무작위한 값이 아니라
   이전 값과 서서히 이어지는 형태의 부드러운 난수를 생성하는 알고리즘이다.
   0부터1사이의 부드러운 값을 반환해 이를 -1~1 범위로 변환하고 x,y축으로 각각 적용해 매 프레임마다 살짝 달라지는 이동 방향 구현.
 *********************************************************************************************/
public class PerlinWander : MonoBehaviour //펄린노이즈 
{
    [Header("펄린 설정")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float noiseSpeed = 0.6f;
    [SerializeField] private float noiseStrength = 1f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float noiseOffsetX;
    private float noiseOffsetY;
    private bool isActive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        noiseOffsetX = Random.Range(0f, 1000f);
        noiseOffsetY = Random.Range(0f, 1000f);
    }

    private void Update()
    {
        if (!isActive) return;

        float t = Time.time * noiseSpeed;
        float nx = Mathf.PerlinNoise(noiseOffsetX + t, 0f) * 2f - 1f;
        float ny = Mathf.PerlinNoise(0f, noiseOffsetY + t) * 2f - 1f;

        Vector2 dir = new Vector2(nx, ny).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);

        if (sr != null)
        {
            if (dir.x < 0) sr.flipX = true;
            else if (dir.x > 0) sr.flipX = false;
        }
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void StartWander()
    {
        isActive = true;
    }

    public void StopWander()
    {
        isActive = false;
    }
}
