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

public class ShapesMode: ClassicMode
{
    private string currentShape;

    protected override IEnumerator CycleRounds()
    {
        while (true)
        {
            picker.pointedColor = Color.black;
            picker.pointedShape = "none";

            fieldGenerator.GenerateField();
            UpdateRule();

            // Запускаем таймер
            timer.StartTimer(waitTime);

            // Ждем, пока таймер работает или игрок выберет цвет
            while (picker.pointedColor == Color.black || picker.pointedShape == "none")
            {
                timeSlider.value = timer.ElapsedTime / timer.Duration; // Обновляем слайдер
                yield return null;
            }

            // Если таймер завершился или игрок выбрал неправильный цвет
            if (!timer.IsRunning || picker.pointedColor != currentColor || picker.pointedShape != currentShape)
            {
                GameOver();
                yield break;
            }

            AddPoint();
            DecreaseTimer();
            fieldGenerator.DeleteField();
        }
    }
    

    protected override void UpdateRule()
    {

        int currentScore = int.Parse(score.text);
        currentColor = fieldGenerator.figures[Random.Range(0, fieldGenerator.figures.Count)].Color;
        List<string> shapesForColor = new List<string>();
        
        foreach (var combination in fieldGenerator.figures)
        {
            if (combination.Color == currentColor)
            {
                shapesForColor.Add(combination.Shape);
            }
        }

        currentShape = shapesForColor[Random.Range(0, shapesForColor.Count)];
        
        AddDifficulty(currentScore);
        
        if (currentScore >= 10)
        {
            
        }

        if (currentScore >= 5)
        {
            var randomColorName = ColorToText.GetRandomColorName();
            colorRule.text = randomColorName + " " + currentShape;
            colorRule.font = ColorToText.ColorToFont(currentColor);
        }
        else
        {
            colorRule.text = ColorToText.ColorToName(currentColor) + " " + currentShape;
            colorRule.font = ColorToText.ColorToFont(currentColor);
        }
    }
    

    protected override void ShowCorrectColor()
    {
        foreach (GameObject figure in fieldGenerator.currentField)
        {
            if (currentColor != figure.GetComponent<SpriteRenderer>().color || currentShape != figure.name)
            {
                Destroy(figure);
            }
        }
        fieldGenerator.currentField.RemoveAll(s => s == null || s.GetComponent<SpriteRenderer>().color != currentColor && s.name != currentShape);
    }
}
