using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("대상 (플레이어)")]
    public Transform target; // 따라갈 플레이어

    [Header("맵 경계 설정")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    [Header("카메라 이동 속도")]
    public float smoothSpeed = 5f;

    private float camHalfHeight;
    private float camHalfWidth;

    private void Start()
    {
        // 카메라 크기 계산 (Orthographic)
        Camera cam = GetComponent<Camera>();
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 목표 위치 계산
        Vector3 desiredPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        // 맵 경계 안에서만 이동 (Clamp)
        float clampedX = Mathf.Clamp(desiredPos.x, minX + camHalfWidth, maxX - camHalfWidth);
        float clampedY = Mathf.Clamp(desiredPos.y, minY + camHalfHeight, maxY - camHalfHeight);

        Vector3 clampedPos = new Vector3(clampedX, clampedY, desiredPos.z);

        // 부드럽게 이동 (Lerp)
        transform.position = Vector3.Lerp(transform.position, clampedPos, smoothSpeed * Time.deltaTime);
    }
}
