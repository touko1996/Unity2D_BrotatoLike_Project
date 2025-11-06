using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Bounds")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    [Header("Follow Settings")]
    public float smoothSpeed = 5f;

    [Header("Shake Settings")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.2f;

    private float camHalfHeight;
    private float camHalfWidth;
    private Vector3 originalPos;
    private bool isShaking = false;
    private Coroutine shakeRoutine;

    public static CameraFollow Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Camera cam = GetComponent<Camera>();
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;
        originalPos = transform.position;
    }

    private void LateUpdate()
    {
        if (isShaking) return;
        if (target == null) return;

        Vector3 desiredPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        float clampedX = Mathf.Clamp(desiredPos.x, minX + camHalfWidth, maxX - camHalfWidth);
        float clampedY = Mathf.Clamp(desiredPos.y, minY + camHalfHeight, maxY - camHalfHeight);
        Vector3 clampedPos = new Vector3(clampedX, clampedY, desiredPos.z);

        transform.position = Vector3.Lerp(transform.position, clampedPos, smoothSpeed * Time.deltaTime);
        originalPos = transform.position;
    }

    // 흔들림 시작
    public void ShakeCamera(float duration = -1f, float magnitude = -1f)
    {
        if (!gameObject.activeInHierarchy) return;
        if (isShaking) return;

        if (duration <= 0f) duration = shakeDuration;
        if (magnitude <= 0f) magnitude = shakeMagnitude;

        shakeRoutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    // 흔들림 즉시 중단 (사망 시 호출)
    public void StopShake()
    {
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
            shakeRoutine = null;
        }

        isShaking = false;
        transform.position = originalPos;
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 사망 중단 예외
            if (Time.timeScale == 0f) break;

            elapsed += Time.deltaTime;
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.position = originalPos + new Vector3(offsetX, offsetY, 0f);
            yield return null;
        }

        transform.position = originalPos;
        isShaking = false;
    }
}
