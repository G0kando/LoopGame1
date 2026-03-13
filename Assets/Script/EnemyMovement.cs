using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    public float attackRange = 1.5f;
    public int attackDamage = 1;
    public float attackCooldown = 1f;

    private Transform player;
    private float lastAttackTime;
    private TestEnemy enemyHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemyHealth = GetComponent<TestEnemy>();

        if (player == null)
            Debug.LogError("Player not found! Make sure player has 'Player' tag.");
    }

    void Update()
    {
        if (player == null || enemyHealth == null || enemyHealth.health <= 0)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Двигаемся к игроку
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Опционально: поворачиваемся к игроку
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
        else
        {
            // Атакуем, если прошла перезарядка
            if (Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack()
    {
        Debug.Log("Enemy attacks!");

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
        else
        {
            Debug.LogError("PlayerHealth component not found on player!");
        }
    }
}