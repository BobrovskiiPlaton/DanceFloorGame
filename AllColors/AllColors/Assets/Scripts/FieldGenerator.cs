using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    [HideInInspector] public int difficulty = 4;
    [HideInInspector] public List<Figure> figures = new List<Figure>();
    [HideInInspector] public List<GameObject> currentField = new List<GameObject>();
    
    public Color[] colors;
    public List<GameObject> shapes;

    
    public void GenerateField()
    {
        figures.Clear();
        float gridLength = 8f;
        float prefabSize = Mathf.Round(gridLength / Mathf.Min(difficulty, 10) * 100) / 100;

        int prefabsX = Mathf.FloorToInt(gridLength / prefabSize);
        int prefabsY = Mathf.FloorToInt(gridLength / prefabSize);

        float startX = -gridLength / 2 + (gridLength - prefabsX * prefabSize) / 2 + prefabSize / 2;
        float startY = -gridLength / 2 + (gridLength - prefabsY * prefabSize) / 2 + prefabSize / 2;

        for (int x = 0; x < prefabsX; x++)
        {
            for (int y = 0; y < prefabsY; y++)
            {
                Vector2 position = new Vector2(startX + x * prefabSize, startY + y * prefabSize);

                GameObject newShape = Instantiate(shapes[Random.Range(0, shapes.Count)], position, Quaternion.identity);
                newShape.transform.localScale = new Vector3(prefabSize, prefabSize, 1);
                currentField.Add(newShape);

                AdjustPosition(newShape, position, prefabSize);

                if (newShape.GetComponent<SpriteRenderer>() != null && colors.Length > 0)
                {
                    var newColor = colors[Random.Range(0, colors.Length)];
                    figures.Add(new Figure(newColor, newShape.name));
                    newShape.GetComponent<SpriteRenderer>().color = newColor;
                }
            }
        }
    }

    private static void AdjustPosition(GameObject newShape, Vector2 position, float prefabSize)
    {
        if (newShape.name.Contains("Triangle")) // Проверяем имя префаба
        {
            position.y -= prefabSize * 0.2f; // Подберите подходящее значение смещения
            newShape.transform.position = position;
        }

        if (newShape.name.Contains("Hexagon")) // Проверяем имя префаба
        {
            position.y -= prefabSize * 0.05f; // Подберите подходящее значение смещения
            newShape.transform.position = position;
        }
    }

    public void DeleteField()
    {
        foreach (GameObject prefab in currentField)
        {
            if (prefab != null)
                Destroy(prefab);
        }
        currentField.Clear();
    }
}