using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyMode : ClassicMode
{
    protected override IEnumerator CycleRounds()
    {
        while (true)
        {
            picker.pointedColor = Color.black;

            fieldGenerator.GenerateField();
            
            foreach (var figure in fieldGenerator.currentField)
            {
                figure.transform.localScale =
                    new Vector3(1f, 1f);
                figure.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                figure.GetComponent<Rigidbody2D>().gravityScale = 0;
                Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                float speed = Random.Range(3f, 5f); // Скорость движения
                figure.GetComponent<Rigidbody2D>().velocity = randomDirection * speed;
            }
            
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
    protected override void AddDifficulty(int score)
    {
        if (score % 5 == 0 && score != 0 && fieldGenerator.difficulty < 5)
            fieldGenerator.difficulty += 1;
    }
}
