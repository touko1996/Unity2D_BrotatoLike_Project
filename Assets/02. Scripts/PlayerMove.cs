using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("플레이어 이동설정")]
    [SerializeField] private float moveSpeed = 5.0f;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
    }
    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + moveInput*moveSpeed*Time.fixedDeltaTime);
    }

}
