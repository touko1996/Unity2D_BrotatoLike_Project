using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private PlayerStats playerStats;
    private SpriteRenderer playerSr;
    private PlayerAnimation playerAnim; 

    private Vector2 moveInput;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerSr = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();
        playerAnim = GetComponent<PlayerAnimation>(); 
    }

    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // 좌우 반전 처리
        if (moveInput.x > 0) playerSr.flipX = false;
        else if (moveInput.x < 0) playerSr.flipX = true;

        // 이동 중 여부 계산
        bool isMoving = moveInput.sqrMagnitude > 0.01f; //벡터길이의 제곱이 0.01보다 크면 이동중으로 간주

        // PlayerAnimation에 이동 여부 전달
        if (playerAnim != null)
            playerAnim.SetRunning(isMoving);

       
    }

    private void FixedUpdate()
    {
        if (playerStats == null) return;

        float currentSpeed = playerStats.currentMoveSpeed;

        playerRb.MovePosition( playerRb.position + moveInput * currentSpeed * Time.fixedDeltaTime);
    }
}
