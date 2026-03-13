using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 3f;
    public int attackDamage = 1;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    private float lastAttackTime;
    private WeaponVisual weaponVisual;

    void Start()
    {
        weaponVisual = GetComponent<WeaponVisual>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        Vector3 rayStart = transform.position;
        Vector3 rayDirection = transform.forward;
        float step = 0.5f; // Шаг проверки

        Debug.Log("Attack triggered!");

        bool hitDetected = false;

        // Проходим лучом с шагом, чтобы не проскочить сквозь врага
        for (float distance = 0; distance < attackRange; distance += step)
        {
            Vector3 checkPoint = rayStart + rayDirection * distance;

            // Проверяем сферой маленького радиуса
            Collider[] hits = Physics.OverlapSphere(checkPoint, 0.3f, enemyLayer);

            foreach (Collider hit in hits)
            {
                TestEnemy enemy = hit.GetComponent<TestEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                    hitDetected = true;

                    // Визуализация
                    if (weaponVisual != null)
                    {
                        weaponVisual.ShowAttack(rayStart, hit.transform.position);
                    }

                    Debug.Log("Enemy hit with sphere check!");
                    break;
                }
            }

            if (hitDetected) break;
        }

        if (!hitDetected)
        {
            // Промах — показываем луч до конца
            if (weaponVisual != null)
            {
                weaponVisual.ShowAttack(rayStart, rayStart + rayDirection * attackRange);
            }
            Debug.Log("Missed!");
        }
    }
}