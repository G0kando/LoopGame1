using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ModifierUI : MonoBehaviour
{
    public GameObject iconPrefab;
    public Transform iconContainer;

    private Dictionary<ArenaModifier, Sprite> modifierIcons;

    void Start()
    {
        GenerateIcons();

        // Смещаем контейнер в правый верхний угол (опционально)
        RectTransform rect = iconContainer.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchorMin = new Vector2(1, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(1, 1);
            rect.anchoredPosition = new Vector2(-20, -20);
        }
    }

    void GenerateIcons()
    {
        modifierIcons = new Dictionary<ArenaModifier, Sprite>();

        modifierIcons[ArenaModifier.FastEnemies] = CreateColoredSprite(Color.red, ">>");
        modifierIcons[ArenaModifier.TankEnemies] = CreateColoredSprite(Color.blue, "🛡️");
        modifierIcons[ArenaModifier.GlassCannon] = CreateColoredSprite(Color.yellow, "⚔️");
        modifierIcons[ArenaModifier.Vampirism] = CreateColoredSprite(Color.magenta, "🧛");
        modifierIcons[ArenaModifier.Darkness] = CreateColoredSprite(Color.gray, "🌑");
        modifierIcons[ArenaModifier.ExplosiveEnemies] = CreateColoredSprite(new Color(1f, 0.5f, 0f), "💥");
    }

    Sprite CreateColoredSprite(Color color, string symbol)
    {
        Texture2D texture = new Texture2D(64, 64);

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                texture.SetPixel(x, y, color);
            }
        }

        for (int x = 0; x < 64; x++)
        {
            texture.SetPixel(x, 0, Color.white);
            texture.SetPixel(x, 63, Color.white);
            texture.SetPixel(0, x, Color.white);
            texture.SetPixel(63, x, Color.white);
        }

        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
    }

    public void ShowModifier(ArenaModifier modifier)
    {
        foreach (Transform child in iconContainer)
        {
            Destroy(child.gameObject);
        }

        if (modifier == ArenaModifier.None) return;

        GameObject iconObj = Instantiate(iconPrefab, iconContainer);

        Image iconImage = iconObj.transform.Find("IconImage")?.GetComponent<Image>();
        if (iconImage != null && modifierIcons.ContainsKey(modifier))
        {
            iconImage.sprite = modifierIcons[modifier];
        }

        TMP_Text nameText = iconObj.GetComponentInChildren<TMP_Text>();
        if (nameText != null)
        {
            nameText.text = modifier.ToString();
        }
    }

    // НОВЫЙ МЕТОД
    public Sprite GetIconForModifier(ArenaModifier modifier)
    {
        if (modifierIcons != null && modifierIcons.ContainsKey(modifier))
        {
            return modifierIcons[modifier];
        }
        return null;
    }
}