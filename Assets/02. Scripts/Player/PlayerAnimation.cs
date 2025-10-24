using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private float scaleAmount = 0.05f; //�þ�� ����
    [SerializeField] private float speed = 2f; // ������ �ӵ�
    [SerializeField] private bool isRunning = false; //�⺻�� false�� �ΰ� �޸� �� true�� ��ȯ

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
