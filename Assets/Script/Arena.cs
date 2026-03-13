using UnityEngine;
using System.Collections.Generic;

public class Arena : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public ArenaManager manager;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private int aliveEnemies = 0;

    public ArenaModifier currentModifier = ArenaModifier.None;

    void Start()
    {
        if (manager == null)
            manager = FindFirstObjectByType<ArenaManager>();
    }

    private void RemoveAllPortals()
    {
        Portal[] allPortals = FindObjectsOfType<Portal>();
        foreach (Portal portal in allPortals)
        {
            Destroy(portal.gameObject);
        }
    }

    public void OnEnemyDied()
    {
        aliveEnemies--;
        Debug.Log($"Enemy died. Remaining: {aliveEnemies}");

        if (aliveEnemies <= 0)
        {
            Debug.Log("All enemies dead! Telling manager to spawn portals.");
            if (manager != null)
            {
                manager.ArenaCleared();
            }
        }
    }

    // Новый метод для установки модификатора извне
    public void SetModifier(ArenaModifier modifier)
    {
        currentModifier = modifier;
        Debug.Log($"Arena modifier set to: {currentModifier}");
    }

    public void ActivateArena()
    {
        RemoveAllPortals();

        Debug.Log($"ActivateArena called on {gameObject.name}, modifier: {currentModifier}");

        // Удаляем старых врагов
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        spawnedEnemies.Clear();

        aliveEnemies = 0;

        // Создаём новых врагов с учётом модификатора
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null) continue;

            if (enemyPrefabs != null && enemyPrefabs.Length > 0)
            {
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedEnemies.Add(newEnemy);
                aliveEnemies++;

                TestEnemy enemyScript = newEnemy.GetComponent<TestEnemy>();
                if (enemyScript != null)
                {
                    enemyScript.SetManager(manager);
                    enemyScript.SetArena(this);

                    ApplyModifierToEnemy(enemyScript);
                }
            }
        }

        Debug.Log($"Spawned {spawnedEnemies.Count} enemies on {gameObject.name}, alive: {aliveEnemies}");
    }

    void ApplyModifierToEnemy(TestEnemy enemy)
    {
        switch (currentModifier)
        {
            case ArenaModifier.FastEnemies:
                EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
                if (movement != null)
                    movement.speed *= 1.5f;
                break;

            case ArenaModifier.TankEnemies:
                enemy.health += 2;
                break;

            case ArenaModifier.GlassCannon:
                EnemyMovement enemyAttack = enemy.GetComponent<EnemyMovement>();
                if (enemyAttack != null)
                    enemyAttack.attackDamage *= 2;
                break;
        }
    }

    public void ApplyModifierToPlayer(PlayerHealth playerHealth, PlayerAttack playerAttack, PlayerMovement playerMovement)
    {
        switch (currentModifier)
        {
            case ArenaModifier.GlassCannon:
                if (playerAttack != null)
                    playerAttack.attackDamage *= 2;
                break;
        }
    }
}