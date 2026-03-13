using UnityEngine;
using System.Collections.Generic;

public class ArenaManager : MonoBehaviour
{
    public GameObject[] arenaPrefabs;
    public GameObject portalPrefab;
    public GameObject upgradeSpherePrefab;
    public Transform player;

    private GameObject currentArena;
    private bool isSpawningPortals = false;

    void Start()
    {
        Debug.Log("ArenaManager Start called");
        if (arenaPrefabs.Length > 0)
        {
            LoadArena(0);
        }
        else
        {
            Debug.LogError("Нет префабов арен в массиве!");
        }
    }

    public void SetCurrentArena(GameObject newArena)
    {
        if (newArena == null)
        {
            Debug.LogError("SetCurrentArena получил null!");
            return;
        }
        currentArena = newArena;
        Debug.Log($"Current arena updated to: {currentArena.name}");
    }

    public void ArenaCleared()
    {
        Debug.Log($"ArenaCleared called! Current arena: {currentArena?.name}");

        UpgradeUI upgradeUI = FindFirstObjectByType<UpgradeUI>();
        if (upgradeUI != null)
        {
            Debug.Log("UpgradeUI found, showing choices...");
            upgradeUI.ShowUpgradeChoices();
        }
        else
        {
            Debug.LogError("UpgradeUI not found! Make sure UpgradeCanvas is in the scene with UpgradeUI script.");
        }
    }

    public void UpgradeChosen()
    {
        if (isSpawningPortals)
        {
            Debug.LogWarning("UpgradeChosen called while already spawning portals - ignoring");
            return;
        }

        if (currentArena == null)
        {
            Debug.LogError("currentArena is null in UpgradeChosen!");
            isSpawningPortals = false;
            return;
        }

        isSpawningPortals = true;
        Debug.Log("Upgrade chosen, spawning portals...");

        int currentIndex = GetCurrentArenaIndex();

        List<int> availableIndices = new List<int>();
        for (int i = 0; i < arenaPrefabs.Length; i++)
        {
            if (i != currentIndex)
                availableIndices.Add(i);
        }

        if (availableIndices.Count < 2)
        {
            Debug.LogError("Нужно минимум 3 арены для случайного выбора!");
            isSpawningPortals = false;
            return;
        }

        int firstIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        availableIndices.Remove(firstIndex);
        int secondIndex = availableIndices[Random.Range(0, availableIndices.Count)];

        SpawnPortal(new Vector3(5, 1.5f, 0), firstIndex);
        SpawnPortal(new Vector3(-5, 1.5f, 0), secondIndex);

        Invoke(nameof(ResetSpawningFlag), 1f);
    }

    private void ResetSpawningFlag()
    {
        isSpawningPortals = false;
        Debug.Log("Spawning flag reset");
    }

    void SpawnPortal(Vector3 localPosition, int arenaIndex)
    {
        if (arenaIndex < 0 || arenaIndex >= arenaPrefabs.Length)
        {
            Debug.LogError($"Ошибка: индекс арены {arenaIndex} вне диапазона!");
            return;
        }

        if (currentArena == null)
        {
            Debug.LogError("currentArena is null! Негде создавать портал.");
            return;
        }

        if (currentArena.transform == null)
        {
            Debug.LogError("currentArena.transform is null!");
            return;
        }

        Debug.Log($"Spawning portal to arena {arenaIndex}");

        GameObject portal = Instantiate(portalPrefab);
        Vector3 worldPosition = currentArena.transform.position + localPosition;
        portal.transform.position = worldPosition;

        Portal portalScript = portal.GetComponent<Portal>();
        if (portalScript == null)
        {
            Debug.LogError("На префабе портала нет компонента Portal!");
            Destroy(portal);
            return;
        }

        float distance = 500f;
        float angle = arenaIndex * (360f / arenaPrefabs.Length);
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

        Vector3 arenaPosition = new Vector3(x, 0, z);
        GameObject destinationArena = Instantiate(arenaPrefabs[arenaIndex], arenaPosition, Quaternion.identity);

        portalScript.destinationArena = destinationArena.transform;

        Debug.Log($"Portal {arenaIndex} created at {worldPosition} -> arena at {arenaPosition}");
    }

    void LoadArena(int index)
    {
        Debug.Log($"Loading arena {index}");

        Portal[] allPortals = FindObjectsOfType<Portal>();
        foreach (Portal portal in allPortals)
        {
            Destroy(portal.gameObject);
        }

        if (currentArena != null)
            Destroy(currentArena);

        currentArena = Instantiate(arenaPrefabs[index], Vector3.zero, Quaternion.identity);

        Arena arenaScript = currentArena.GetComponent<Arena>();
        if (arenaScript != null)
        {
            arenaScript.ActivateArena();
            Debug.Log($"Start arena activated: {currentArena.name}");
        }
        else
        {
            Debug.LogError("На стартовой арене нет скрипта Arena!");
        }

        if (player != null)
        {
            player.position = Vector3.zero;
            Debug.Log($"Player moved to {player.position}");
        }
        else
        {
            Debug.LogError("Player не назначен в ArenaManager!");
        }
    }

    private int GetCurrentArenaIndex()
    {
        if (currentArena == null) return 0;

        string currentName = currentArena.name.Replace("(Clone)", "");

        for (int i = 0; i < arenaPrefabs.Length; i++)
        {
            if (arenaPrefabs[i] != null && arenaPrefabs[i].name == currentName)
                return i;
        }

        Debug.LogWarning($"Could not find index for arena {currentName}, defaulting to 0");
        return 0;
    }
}