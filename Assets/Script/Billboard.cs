using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Находим камеру с тегом MainCamera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Billboard: Main Camera not found! Make sure your camera has the 'MainCamera' tag.");
        }
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.forward);
        }
    }
}