using UnityEngine;
/*********************************************************************************************
 [Perlin Noise]
 - �޸� ������� �ε巴�� �ڿ������� ���� �̵��� �����ϱ� ���� ������ �������� ���� �ƴ϶�
   ���� ���� ������ �̾����� ������ �ε巯�� ������ �����ϴ� �˰����̴�.
   0����1������ �ε巯�� ���� ��ȯ�� �̸� -1~1 ������ ��ȯ�ϰ� x,y������ ���� ������ �� �����Ӹ��� ��¦ �޶����� �̵� ���� ����.
 *********************************************************************************************/
public class PerlinWander : MonoBehaviour //�޸������� 
{
    [Header("�޸� ����")]
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
