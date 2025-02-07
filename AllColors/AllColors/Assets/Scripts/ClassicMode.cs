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
    [SerializeField] protected TMP_Text colorRule;
    [SerializeField] protected TMP_Text score;
    [SerializeField] protected Slider timeSlider;
    [SerializeField] protected GameObject overScreen;

    protected float waitTime = 6f;
    protected Color currentColor;
    protected Picker picker;
    protected Timer timer;
    
    public FieldGenerator fieldGenerator;
    public AudioSource scoreAudio;   // Ссылка на источник аудио
    
    void Start()
    {
        picker = gameObject.AddComponent<Picker>();
        timer = gameObject.AddComponent<Timer>(); // Добавляем таймер как компонент
        timer.OnTimerEnd += GameOver;
        StartCoroutine(CycleRounds());
    }
    
    

    protected virtual IEnumerator CycleRounds()
    {
        while (true)
        {
            picker.pointedColor = Color.black;

            fieldGenerator.GenerateField();
            UpdateRule();

            // Запускаем таймер
            timer.StartTimer(waitTime);

            // Ждем, пока таймер работает или игрок выберет цвет
            while (picker.pointedColor == Color.black)
            {
                timeSlider.value = timer.ElapsedTime / timer.Duration; // Обновляем слайдер
                yield return null;
            }

            // Если таймер завершился или игрок выбрал неправильный цвет
            if (!timer.IsRunning || picker.pointedColor != currentColor)
            {
                GameOver();
                yield break;
            }

            AddPoint();
            DecreaseTimer();
            fieldGenerator.DeleteField();
        }
    }

    protected void AddPoint()
    {
        score.text = (int.Parse(score.text) + 1).ToString();
        scoreAudio.Play(); // Воспроизводим звук при наборе очков
    }

    protected virtual void UpdateRule()
    {

        int currentScore = int.Parse(score.text);
        currentColor = fieldGenerator.figures[Random.Range(0, fieldGenerator.figures.Count)].Color;
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

    protected virtual void AddDifficulty(int score)
    {
        if (score % 3 == 0 && score != 0)
            fieldGenerator.difficulty += 1;
    }

    protected void DecreaseTimer()
    {
        if (waitTime > 1f)
            waitTime -= 0.05f;
        else
            waitTime = 1f;
    }

    
    protected void GameOver()
    {
        ShowCorrectColor();
        Time.timeScale = 0f;
        overScreen.SetActive(true);
    }

    protected virtual void ShowCorrectColor()
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
