using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("�÷��̾� �̵�����")]
    [SerializeField] private float moveSpeed = 5.0f;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private SpriteRenderer sr; // �ø��� ��������Ʈ ������

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ ����
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // --- �¿� �̵��� �ø� ó�� ---
        if (moveInput.x > 0)
        {
            sr.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            sr.flipX = true;
        }

        bool isMoving = Mathf.Abs(moveInput.x) > 0.1f || Mathf.Abs(moveInput.y) > 0.1f;
        GetComponent<PlayerAnimation>().SetRunning(isMoving);
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}