using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    [HideInInspector] public int difficulty = 4;
    [HideInInspector] public List<Color> existingColors = new List<Color>();
    [HideInInspector] public List<GameObject> currentField = new List<GameObject>();
    public Color[] colors;

    public List<GameObject> figures;

    
    public void GenerateField()
    {
        existingColors.Clear();
        float gridLength = 8f;
        float prefabSize = Mathf.Round(gridLength / difficulty * 100) / 100;

        int prefabsX = Mathf.FloorToInt(gridLength / prefabSize);
        int prefabsY = Mathf.FloorToInt(gridLength / prefabSize);

        float startX = -gridLength / 2 + (gridLength - prefabsX * prefabSize) / 2 + prefabSize / 2;
        float startY = -gridLength / 2 + (gridLength - prefabsY * prefabSize) / 2 + prefabSize / 2;

        for (int x = 0; x < prefabsX; x++)
        {
            for (int y = 0; y < prefabsY; y++)
            {
                Vector2 position = new Vector2(startX + x * prefabSize, startY + y * prefabSize);

                GameObject newFigure = Instantiate(figures[Random.Range(0, figures.Count)], position, Quaternion.identity);
                newFigure.transform.localScale = new Vector3(prefabSize, prefabSize, 1);
                currentField.Add(newFigure);
                
                if (newFigure.name.Contains("Triangle")) // Проверяем имя префаба
                {
                    position.y -= prefabSize * 0.2f; // Подберите подходящее значение смещения
                    newFigure.transform.position = position;
                }
                if (newFigure.name.Contains("Hexagon")) // Проверяем имя префаба
                {
                    position.y -= prefabSize * 0.05f; // Подберите подходящее значение смещения
                    newFigure.transform.position = position;
                }
                
                if (newFigure.GetComponent<SpriteRenderer>() != null && colors.Length > 0)
                {
                    var newColor = colors[Random.Range(0, colors.Length)];
                    if(!existingColors.Contains(newColor))
                        existingColors.Add(newColor);
                    newFigure.GetComponent<SpriteRenderer>().color = newColor;
                }
                /*if (squaresFloating)
                {
                    Vector2 randomDirection = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                    newSquare.GetComponent<Rigidbody2D>().velocity = randomDirection;
                }*/
            }
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
