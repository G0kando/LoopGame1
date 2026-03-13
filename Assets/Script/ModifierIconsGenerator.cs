using UnityEngine;
using System.Collections.Generic;

public static class ModifierIconsGenerator
{
    public static Dictionary<ArenaModifier, Sprite> GenerateIcons()
    {
        var icons = new Dictionary<ArenaModifier, Sprite>();

        icons[ArenaModifier.FastEnemies] = CreateColoredSprite(Color.red, ">>");
        icons[ArenaModifier.TankEnemies] = CreateColoredSprite(Color.blue, "🛡️");
        icons[ArenaModifier.GlassCannon] = CreateColoredSprite(Color.yellow, "⚔️");
        icons[ArenaModifier.Vampirism] = CreateColoredSprite(Color.magenta, "🧛");
        icons[ArenaModifier.Darkness] = CreateColoredSprite(Color.gray, "🌑");
        icons[ArenaModifier.ExplosiveEnemies] = CreateColoredSprite(new Color(1f, 0.5f, 0f), "💥");

        return icons;
    }

    private static Sprite CreateColoredSprite(Color color, string symbol)
    {
        // Создаём текстуру 64x64
        Texture2D texture = new Texture2D(64, 64);

        // Заливаем цветом
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                texture.SetPixel(x, y, color);
            }
        }

        // Рисуем рамку
        for (int x = 0; x < 64; x++)
        {
            texture.SetPixel(x, 0, Color.white);
            texture.SetPixel(x, 63, Color.white);
            texture.SetPixel(0, x, Color.white);
            texture.SetPixel(63, x, Color.white);
        }

        texture.Apply();

        // Создаём спрайт
        return Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
    }
}