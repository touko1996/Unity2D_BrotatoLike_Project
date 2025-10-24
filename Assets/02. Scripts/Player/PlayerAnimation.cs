using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float scaleAmount = 0.05f; //늘어나는 정도
    [SerializeField] private float speed = 2f; // 숨쉬는 속도
    [SerializeField] private bool isRunning = false; //기본을 false로 두고 달릴 때 true로 전환

    private Vector3 originScale;

    private void Start()
    {
        originScale = transform.localScale;
       
    }
    private void Update()
    {
        float currentSpeed = isRunning ? speed * 2f : speed;
        float newY = originScale.y + Mathf.Sin(Time.time * currentSpeed)*scaleAmount;
        transform.localScale = new Vector3(originScale.x, newY, originScale.z);
    }
    public void SetRunning(bool running)
    {
        isRunning = running;
    }
}
