using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PortalIcon : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;

    public void Setup(ArenaModifier modifier, Sprite iconSprite)
    {
        Debug.Log($"PortalIcon.Setup called with modifier: {modifier}, iconSprite: {(iconSprite != null ? "yes" : "null")}");

        if (modifier == ArenaModifier.None)
        {
            gameObject.SetActive(false);
            return;
        }

        if (iconImage == null)
            iconImage = GetComponentInChildren<Image>();

        if (nameText == null)
            nameText = GetComponentInChildren<TMP_Text>();

        if (iconImage != null && iconSprite != null)
        {
            iconImage.sprite = iconSprite;
            iconImage.gameObject.SetActive(true);
        }

        if (nameText != null)
        {
            nameText.text = modifier.ToString();
            nameText.gameObject.SetActive(true);
        
    }
        else
        {
            Debug.LogWarning($"IconImage or iconSprite is null for {modifier}");
        }

        if (nameText != null)
        {
            nameText.text = modifier.ToString();
            nameText.gameObject.SetActive(true);
            Debug.Log($"Portal text set to: {modifier}");
        }
        else
        {
            Debug.LogWarning($"NameText is null for {modifier}");
        }
    }
}