using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float scaleAmount = 0.05f; //늘어나는 정도
    [SerializeField] private float speed = 8f; // 숨쉬는 속도

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
