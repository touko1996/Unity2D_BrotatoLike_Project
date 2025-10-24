using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private float scaleAmount = 0.05f; //�þ�� ����
    [SerializeField] private float speed = 8f; // ������ �ӵ�

    private Vector3 originScale;

    private void Start()
    {
        originScale = transform.localScale;

    }
    private void Update()
    {
        
        float newY = originScale.y + Mathf.Sin(Time.time * speed) * scaleAmount;
        transform.localScale = new Vector3(originScale.x, newY, originScale.z);
    }
    
}
