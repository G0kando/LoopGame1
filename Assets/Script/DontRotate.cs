using UnityEngine;

public class DontRotate : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        Debug.Log($"DontRotate started on {gameObject.name}, initial rotation: {initialRotation.eulerAngles}");
    }

    void LateUpdate()
    {
        if (transform.rotation != initialRotation)
        {
            transform.rotation = initialRotation;
            Debug.Log("Rotation reset");
        }
    }
}