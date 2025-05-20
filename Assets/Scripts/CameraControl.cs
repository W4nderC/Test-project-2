using Unity.Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private CinemachineCamera cineCamera;
    private PlayerController player;

    void Start()
    {
        
    }

    public static void SetCameraFollow(Transform target)
    {
        CinemachineCamera cineCamera = FindObjectOfType<CinemachineCamera>();
        if (cineCamera != null)
        {
            cineCamera.Follow = target;
            cineCamera.LookAt = target;
        }
    }
}
