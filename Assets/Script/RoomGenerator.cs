using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [Header("Настройки генерации")]
    public GameObject floorPrefab;
    public int numberOfFloors = 40;
    public Vector2 roomSize = new Vector2(90, 90);

    [Header("Настройки масштаба")]
    public float floorWidth = 2.5f; // Ширина плитки (под твой размер)
    public float gapBetweenFloors = 1.5f; // Зазор между плитками

    private List<Vector3> usedPositions = new List<Vector3>();
    private float minDistance;

    void Start()
    {
        minDistance = floorWidth + gapBetweenFloors;
        GenerateRoom();
    }

    void GenerateRoom()
    {
        int floorsCreated = 0;
        int attempts = 0;
        int maxAttempts = 5000;

        while (floorsCreated < numberOfFloors && attempts < maxAttempts)
        {
            Vector3 randomPos = GetRandomPosition();
            if (IsPositionValid(randomPos))
            {
                GameObject newFloor = Instantiate(floorPrefab, randomPos, Quaternion.identity);
                newFloor.transform.localScale = new Vector3(floorWidth, 0.5f, floorWidth);
                usedPositions.Add(randomPos);
                floorsCreated++;
            }
            attempts++;
        }

        Debug.Log($"Создано плиток: {floorsCreated} из {numberOfFloors}");
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-roomSize.x / 2, roomSize.x / 2);
        float z = Random.Range(-roomSize.y / 2, roomSize.y / 2);
        return new Vector3(x, 0, z);
    }

    bool IsPositionValid(Vector3 pos)
    {
        foreach (Vector3 used in usedPositions)
        {
            if (Vector3.Distance(pos, used) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
}