using UnityEngine;

public class PortalSpin : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float floatSpeed = 1f;
    public float floatHeight = 0.5f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Вращение
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Покачивание вверх-вниз
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}