using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScreenFillingSquares : MonoBehaviour
{
    public GameObject squarePrefab;  // Префаб квадрата
    public Color[] colors;           // Массив цветов
    public ColorPicker2D _colorPicker2D;
    public AudioSource scoreAudio;   // Ссылка на источник аудио

    [SerializeField] private TMP_Text colorRule;
    [SerializeField] private TMP_Text score;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private GameObject overScreen;

    private float waitTime = 6f;
    private List<GameObject> squares = new List<GameObject>();
    private Color currentColor;
    private bool squaresFloating = false;

    private List<Color> existingColors = new List<Color>();
    private int difficulty = 4;

    void Start()
    {
        StartCoroutine(CycleSquares());
    }
    
    

    IEnumerator CycleSquares()
    {
        while (true)
        {
            _colorPicker2D.pointedColor = Color.black;
            
            GenerateField();
            UpdateRule();

            float elapsedTime = 0f;
            while (elapsedTime < waitTime && _colorPicker2D.pointedColor == Color.black)
            {
                elapsedTime += Time.deltaTime;
                timeSlider.value = elapsedTime / waitTime;
                yield return null;
            }

            if (_colorPicker2D.pointedColor == Color.black || _colorPicker2D.pointedColor != currentColor)
            {
                GameOver();
                yield break;
            }

            AddPoint();
            DecreaseTimer();
            DeleteField();
        }
    }

    private void AddPoint()
    {
        score.text = (int.Parse(score.text) + 1).ToString();
        scoreAudio.Play(); // Воспроизводим звук при наборе очков
    }

    private void UpdateRule()
    {

        int currentScore = int.Parse(score.text);
        currentColor = existingColors[Random.Range(0, existingColors.Count)];
        AddDifficulty(currentScore);
        
        if (currentScore >= 10)
        {
            //squaresFloating = true;
        }

        if (currentScore >= 5)
        {
            var randomColorName = ColorToText.GetRandomColorName();
            //colorRule.color = currentColor;
            colorRule.text = randomColorName;
            colorRule.font = ColorToText.ColorToFont(currentColor);
        }
        else
        {
            //colorRule.color = currentColor;
            colorRule.text = ColorToText.ColorToName(currentColor);
            colorRule.font = ColorToText.ColorToFont(currentColor);
        }
    }

    private void AddDifficulty(int score)
    {
        if (score % 3 == 0 && score != 0)
            difficulty += 1;
    }

    private void DecreaseTimer()
    {
        if (waitTime > 1f)
            waitTime -= 0.05f;
        else
            waitTime = 1f;
    }

    
    void GenerateField()
    {
        existingColors.Clear();
        float gridLength = 8f;
        float squareSize = Mathf.Round(gridLength / difficulty * 100) / 200;
        Debug.Log(gridLength + " " + squareSize);

        int squaresX = Mathf.FloorToInt(gridLength / squareSize);
        int squaresY = Mathf.FloorToInt(gridLength / squareSize);

        float startX = -gridLength / 2 + (gridLength - squaresX * squareSize) / 2 + squareSize / 2;
        float startY = -gridLength / 2 + (gridLength - squaresY * squareSize) / 2 + squareSize / 2;

        for (int x = 0; x < squaresX; x++)
        {
            for (int y = 0; y < squaresY; y++)
            {
                Vector2 position = new Vector2(startX + x * squareSize, startY + y * squareSize);

                GameObject newSquare = Instantiate(squarePrefab, position, Quaternion.identity);
                newSquare.transform.localScale = new Vector3(squareSize, squareSize, 1);
                squares.Add(newSquare);

                if (newSquare.GetComponent<SpriteRenderer>() != null && colors.Length > 0)
                {
                    var newColor = colors[Random.Range(0, colors.Length)];
                    if(!existingColors.Contains(newColor))
                        existingColors.Add(newColor);
                    newSquare.GetComponent<SpriteRenderer>().color = newColor;
                }

                if (squaresFloating)
                {
                    Vector2 randomDirection = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                    newSquare.GetComponent<Rigidbody2D>().velocity = randomDirection;
                }
            }
        }
    }
    

    private void DeleteField()
    {
        foreach (GameObject square in squares)
        {
            if (square != null)
                Destroy(square);
        }
        squares.Clear();
    }

    private void GameOver()
    {
        ShowCorrectColor();
        Time.timeScale = 0f;
        overScreen.SetActive(true);
    }

    private void ShowCorrectColor()
    {
        foreach (GameObject square in squares)
        {
            if (currentColor != square.GetComponent<SpriteRenderer>().color)
            {
                Destroy(square);
            }
        }
        squares.RemoveAll(s => s == null || s.GetComponent<SpriteRenderer>().color != currentColor);
    }
}
