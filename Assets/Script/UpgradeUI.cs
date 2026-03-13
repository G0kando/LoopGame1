using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
    public GameObject upgradePanel;
    public GameObject cardPrefab;
    public Transform cardsContainer;

    private List<UpgradeData> availableUpgrades = new List<UpgradeData>();
    private ArenaManager arenaManager;
    private PlayerMovement playerMovement;

    void Start()
    {
        arenaManager = FindFirstObjectByType<ArenaManager>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        Debug.Log($"UpgradeUI started. arenaManager: {arenaManager != null}, playerMovement: {playerMovement != null}");
    }

    public void ShowUpgradeChoices()
    {
        Debug.Log("=== ShowUpgradeChoices START ===");

        if (upgradePanel == null)
        {
            Debug.LogError("upgradePanel is null!");
            return;
        }

        if (cardPrefab == null)
        {
            Debug.LogError("cardPrefab is null!");
            return;
        }

        if (cardsContainer == null)
        {
            Debug.LogError("cardsContainer is null!");
            return;
        }

        if (playerMovement != null)
        {
            playerMovement.LockControls(true);
            Debug.Log("Player movement locked");
        }

        foreach (Transform child in cardsContainer)
        {
            Destroy(child.gameObject);
        }

        List<UpgradeData> choices = GetRandomUpgrades(3);

        foreach (UpgradeData upgrade in choices)
        {
            GameObject card = Instantiate(cardPrefab, cardsContainer);

            TextMeshProUGUI[] tmpTexts = card.GetComponentsInChildren<TextMeshProUGUI>();

            if (tmpTexts.Length >= 2)
            {
                tmpTexts[0].text = upgrade.upgradeName;
                tmpTexts[1].text = upgrade.description;
            }
            else
            {
                Debug.LogError("Card prefab must have at least 2 TextMeshProUGUI components!");
                Destroy(card);
                continue;
            }

            Button button = card.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnUpgradeSelected(upgrade));
            }
            else
            {
                Debug.LogError("Card prefab must have a Button component!");
                Destroy(card);
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        upgradePanel.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("=== ShowUpgradeChoices END ===");
    }

    void OnUpgradeSelected(UpgradeData selectedUpgrade)
    {
        Debug.Log($"=== OnUpgradeSelected START ===");

        if (selectedUpgrade == null)
        {
            Debug.LogError("selectedUpgrade is null!");
            return;
        }

        Debug.Log($"Selected upgrade: {selectedUpgrade.upgradeName}");

        PlayerUpgrades playerUpgrades = FindFirstObjectByType<PlayerUpgrades>();
        if (playerUpgrades != null)
        {
            playerUpgrades.ApplyUpgrade(selectedUpgrade.upgradeName);
        }
        else
        {
            Debug.LogError("PlayerUpgrades not found!");
        }

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMovement != null)
        {
            playerMovement.LockControls(false);
        }

        if (arenaManager != null)
        {
            arenaManager.UpgradeChosen();
        }
        else
        {
            Debug.LogError("arenaManager is null!");
        }

        Debug.Log("=== OnUpgradeSelected END ===");
    }

    List<UpgradeData> GetRandomUpgrades(int count)
    {
        List<UpgradeData> allUpgrades = new List<UpgradeData>
        {
            new UpgradeData("Damage +1", "Увеличивает урон на 1"),
            new UpgradeData("Health +2", "Увеличивает максимум здоровья на 2"),
            new UpgradeData("Attack Speed", "Уменьшает перезарядку атаки на 20%"),
            new UpgradeData("Speed +20%", "Увеличивает скорость передвижения"),
            new UpgradeData("Critical Hit", "5% шанс двойного урона"),
            new UpgradeData("Vampire", "Восстанавливает 1 HP за убийство")
        };

        List<UpgradeData> shuffled = new List<UpgradeData>(allUpgrades);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int rnd = Random.Range(i, shuffled.Count);
            UpgradeData temp = shuffled[i];
            shuffled[i] = shuffled[rnd];
            shuffled[rnd] = temp;
        }

        return shuffled.GetRange(0, Mathf.Min(count, shuffled.Count));
    }
}