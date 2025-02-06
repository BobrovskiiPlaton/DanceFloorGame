using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    public float Duration { get; private set; } // Длительность таймера
    public float ElapsedTime { get; private set; } // Прошедшее время
    public bool IsRunning { get; private set; } // Флаг, указывающий, работает ли таймер

    public event Action OnTimerEnd; // Событие, вызываемое при завершении таймера

    // Запуск таймера
    public void StartTimer(float duration)
    {
        Duration = duration;
        ElapsedTime = 0f;
        IsRunning = true;
    }

    // Остановка таймера
    public void StopTimer()
    {
        IsRunning = false;
    }

    private void Update()
    {
        if (IsRunning)
        {
            ElapsedTime += Time.deltaTime;

            // Если время вышло
            if (ElapsedTime >= Duration)
            {
                IsRunning = false;
                OnTimerEnd?.Invoke(); // Вызываем событие
            }
        }
    }
}