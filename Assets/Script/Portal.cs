using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform destinationArena;
    private bool justTeleported = false;

    private void ResetTeleportFlag()
    {
        justTeleported = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (justTeleported) return;
        if (Time.timeScale == 0f) return;

        if (other.CompareTag("Player"))
        {
            justTeleported = true;

            if (destinationArena != null)
            {
                CharacterController controller = other.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.enabled = false;
                    other.transform.position = destinationArena.position;
                    controller.enabled = true;

                    Arena arenaScript = destinationArena.GetComponent<Arena>();
                    if (arenaScript != null)
                    {
                        arenaScript.ActivateArena();

                        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                        PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();
                        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

                        if (playerHealth != null && playerAttack != null && playerMovement != null)
                        {
                            arenaScript.ApplyModifierToPlayer(playerHealth, playerAttack, playerMovement);
                        }

                        ModifierUI modifierUI = FindFirstObjectByType<ModifierUI>();
                        if (modifierUI != null)
                        {
                            modifierUI.ShowModifier(arenaScript.currentModifier);
                        }

                        ArenaManager manager = FindFirstObjectByType<ArenaManager>();
                        if (manager != null)
                        {
                            manager.SetCurrentArena(destinationArena.gameObject);
                        }
                    }
                }
                else
                {
                    other.transform.position = destinationArena.position;
                }
            }

            Invoke(nameof(ResetTeleportFlag), 1f);
        }
    }
}