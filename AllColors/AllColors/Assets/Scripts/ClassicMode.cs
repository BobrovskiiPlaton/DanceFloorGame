using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ClassicMode: MonoBehaviour
{
    [SerializeField] private TMP_Text colorRule;
    [SerializeField] private TMP_Text score;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private GameObject overScreen;

    private float waitTime = 6f;
    private Color currentColor;
    private ColorPicker2D _colorPicker2D;
    private Timer timer;
    
    public FieldGenerator fieldGenerator;
    public AudioSource scoreAudio;   // Ссылка на источник аудио
    
    void Start()
    {
        _colorPicker2D = gameObject.AddComponent<ColorPicker2D>();
        timer = gameObject.AddComponent<Timer>(); // Добавляем таймер как компонент
        timer.OnTimerEnd += GameOver;
        StartCoroutine(CycleRounds());
    }
    
    

    IEnumerator CycleRounds()
    {
        while (true)
        {
            _colorPicker2D.pointedColor = Color.black;

            fieldGenerator.GenerateField();
            UpdateRule();

            // Запускаем таймер
            timer.StartTimer(waitTime);

            // Ждем, пока таймер работает или игрок выберет цвет
            while (_colorPicker2D.pointedColor == Color.black)
            {
                timeSlider.value = timer.ElapsedTime / timer.Duration; // Обновляем слайдер
                yield return null;
            }

            // Если таймер завершился или игрок выбрал неправильный цвет
            if (!timer.IsRunning || _colorPicker2D.pointedColor != currentColor)
            {
                GameOver();
                yield break;
            }

            AddPoint();
            DecreaseTimer();
            fieldGenerator.DeleteField();
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
        currentColor = fieldGenerator.existingColors[Random.Range(0, fieldGenerator.existingColors.Count)];
        AddDifficulty(currentScore);
        
        if (currentScore >= 10)
        {
            
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
            fieldGenerator.difficulty += 1;
    }

    private void DecreaseTimer()
    {
        if (waitTime > 1f)
            waitTime -= 0.05f;
        else
            waitTime = 1f;
    }

    
    private void GameOver()
    {
        ShowCorrectColor();
        Time.timeScale = 0f;
        overScreen.SetActive(true);
    }

    private void ShowCorrectColor()
    {
        foreach (GameObject figure in fieldGenerator.currentField)
        {
            if (currentColor != figure.GetComponent<SpriteRenderer>().color)
            {
                Destroy(figure);
            }
        }
        fieldGenerator.currentField.RemoveAll(s => s == null || s.GetComponent<SpriteRenderer>().color != currentColor);
    }
}
