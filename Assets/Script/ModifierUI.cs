using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ModifierUI : MonoBehaviour
{
    public GameObject iconPrefab;
    public Transform iconContainer;

    private Dictionary<ArenaModifier, Sprite> modifierIcons;

    void Start()
    {
        // Генерируем иконки при старте
        modifierIcons = ModifierIconsGenerator.GenerateIcons();
    }

    public void ShowModifier(ArenaModifier modifier)
    {
        // Очищаем старые иконки
        foreach (Transform child in iconContainer)
        {
            Destroy(child.gameObject);
        }

        if (modifier == ArenaModifier.None) return;

        // Создаём иконку
        GameObject iconObj = Instantiate(iconPrefab, iconContainer);
        Image iconImage = iconObj.GetComponent<Image>();

        if (modifierIcons.ContainsKey(modifier))
        {
            iconImage.sprite = modifierIcons[modifier];
        }

        // Добавляем текст с названием (опционально)
        Text text = iconObj.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = modifier.ToString();
        }
    }
}