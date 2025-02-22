using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorToText : MonoBehaviour
{
    private static Dictionary<Color, string> colorNames = new Dictionary<Color, string>
    {
        { new Color(200 / 255f, 0, 0, 1), "Red" },
        { new Color(255 / 255f, 241 / 255f, 107 / 255f, 1), "Yellow" },
        { new Color(35 / 255f, 75 / 255f, 0, 1), "Green" },
        { new Color(6 / 255f, 0, 195 / 255f, 1), "Blue" },
        { new Color(255 / 255f, 148 / 255f, 197 / 255f, 1), "Pink" },
        { new Color(146 / 255f, 47 / 255f, 255 / 255f, 1), "Violet" },
        { new Color(214 / 255f, 87 / 255f, 0, 1), "Orange" },
        { new Color(118 / 255f, 0, 36 / 255f, 1), "Maroon" },
        { new Color(214 / 255f, 0, 135 / 255f, 1), "Magenta" },
        { new Color(0, 250 / 255f, 255 / 255f, 1), "Cyan" },
        { new Color(255 / 255f, 255 / 255f, 255 / 255f, 1), "White" },
        { new Color(255 / 255f, 180 / 255f, 129 / 255f, 1), "Peach" },
        { new Color(45 / 255f, 255 / 255f, 0, 1), "Lime" },
    };

    public static string ColorToName(Color color)
    {
        if (colorNames.TryGetValue(color, out string name))
        {
            return name;
        }
        else
        {
            return FindClosestColorName(color);
        }
    }

    public static TMP_FontAsset ColorToFont(Color color)
    {
        string fontPath = "Fonts/NeonFont/";
        switch (FindClosestColorName(color))
        {
            case "Red":
                return Resources.Load<TMP_FontAsset>(fontPath + "RedNeonFont");
            case "Yellow":
                return Resources.Load<TMP_FontAsset>(fontPath + "YellowNeonFont");
            case "Green":
                return Resources.Load<TMP_FontAsset>(fontPath + "GreenNeonFont");
            case "Blue":
                return Resources.Load<TMP_FontAsset>(fontPath + "BlueNeonFont");
            case "Pink":
                return Resources.Load<TMP_FontAsset>(fontPath + "PinkNeonFont");
            case "Violet":
                return Resources.Load<TMP_FontAsset>(fontPath + "VioletNeonFont");
            case "Orange":
                return Resources.Load<TMP_FontAsset>(fontPath + "OrangeNeonFont");
            case "Maroon":
                return Resources.Load<TMP_FontAsset>(fontPath + "MaroonNeonFont");
            case "Magenta":
                return Resources.Load<TMP_FontAsset>(fontPath + "MagentaNeonFont");
            case "Cyan":
                return Resources.Load<TMP_FontAsset>(fontPath + "CyanNeonFont");
            case "Lavender":
                return Resources.Load<TMP_FontAsset>(fontPath + "LavenderNeonFont");
            case "Peach":
                return Resources.Load<TMP_FontAsset>(fontPath + "PeachNeonFont");
            case "Lime":
                return Resources.Load<TMP_FontAsset>(fontPath + "LimeNeonFont");
            default:
                return Resources.Load<TMP_FontAsset>(fontPath + "WhiteNeonFont");
            
        }

        
        


    }

    private static string FindClosestColorName(Color color)
    {
        Color closestColor = Color.black;
        float minDistance = float.MaxValue;

        foreach (var kvp in colorNames)
        {
            float distance = GetColorDistance(color, kvp.Key);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestColor = kvp.Key;
            }
        }

        return colorNames[closestColor];
    }

    private static float GetColorDistance(Color a, Color b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.r - b.r, 2) + Mathf.Pow(a.g - b.g, 2) + Mathf.Pow(a.b - b.b, 2) + Mathf.Pow(a.a - b.a, 2));
    }

    public static string GetRandomColorName()
    {
        List<string> names = new List<string>(colorNames.Values);
        return names[Random.Range(0, names.Count)];
    }
}
