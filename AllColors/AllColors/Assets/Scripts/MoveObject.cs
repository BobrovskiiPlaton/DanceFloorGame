using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public RectTransform[] targetTransforms; // Массив целевых объектов
    public Vector3 moveDistance; // Расстояние, на которое нужно переместить объекты
    public float speed = 2.0f; // Скорость перемещения
    
    private Vector3[] initialPositions;

    void Start()
    {
        initialPositions = new Vector3[targetTransforms.Length];
        for (int i = 0; i < targetTransforms.Length; i++)
        {
            initialPositions[i] = targetTransforms[i].anchoredPosition;
        }
    }

    // Метод для начала перемещения объектов
    public void StartMoving()
    {
        for (int i = 0; i < targetTransforms.Length; i++)
        {
            targetTransforms[i].anchoredPosition = Vector3.Lerp(targetTransforms[i].anchoredPosition, initialPositions[i] + moveDistance, speed * Time.deltaTime);
        }
    }
}
