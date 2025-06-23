using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Camera : MonoBehaviour
{
    // 카메라 경계 설정
    public Vector2 minCamLimit = new Vector2(0,0);
    public Vector2 maxCamLimit = new Vector2(13,0);

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        transform.position = new Vector3(
        Mathf.Clamp(pos.x, minCamLimit.x, maxCamLimit.x),
        Mathf.Clamp(pos.y, minCamLimit.y + 1, maxCamLimit.y + 1), -10);
    }
}
