using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();

        if (playerAttack == null) Debug.LogError("PlayerAttack not found on player!");
        if (playerHealth == null) Debug.LogError("PlayerHealth not found on player!");
        if (playerMovement == null) Debug.LogError("PlayerMovement not found on player!");
    }

    public void ApplyUpgrade(string upgradeName)
    {
        Debug.Log($"Applied upgrade: {upgradeName}");

        switch (upgradeName)
        {
            case "Damage +1":
                if (playerAttack != null)
                    playerAttack.attackDamage += 1;
                else
                    Debug.LogError("playerAttack is null!");
                break;

            case "Health +2":
                if (playerHealth != null)
                {
                    playerHealth.maxHealth += 2;
                    playerHealth.Heal(2);
                }
                else
                    Debug.LogError("playerHealth is null!");
                break;

            case "Attack Speed":
                if (playerAttack != null)
                    playerAttack.attackCooldown *= 0.8f;
                else
                    Debug.LogError("playerAttack is null!");
                break;

            case "Speed +20%":
                if (playerMovement != null)
                    playerMovement.ModifySpeed(1.2f);
                else
                    Debug.LogError("playerMovement is null!");
                break;

            case "Critical Hit":
                Debug.Log("Critical Hit not implemented yet");
                break;

            case "Vampire":
                Debug.Log("Vampire not implemented yet");
                break;

            default:
                Debug.LogError($"Unknown upgrade: {upgradeName}");
                break;
        }
    }
}