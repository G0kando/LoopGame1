using UnityEngine;
using System.Collections;

public class TestEnemy : MonoBehaviour
{
    public int health = 3;
    private ArenaManager manager;
    private Arena myArena;

    public void SetManager(ArenaManager newManager)
    {
        manager = newManager;
    }

    public void SetArena(Arena arena)
    {
        myArena = arena;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Enemy took damage, health: {health}");

        EnemyDamageFlash flash = GetComponent<EnemyDamageFlash>();
        if (flash != null)
        {
            flash.Flash();
        }

        if (health <= 0)
        {
            StartCoroutine(DieAfterFlash());
        }
    }

    IEnumerator DieAfterFlash()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);

        if (myArena != null)
        {
            myArena.OnEnemyDied();
        }
        else if (manager != null)
        {
            manager.ArenaCleared();
        }
    }
}