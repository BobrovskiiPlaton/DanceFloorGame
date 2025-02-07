using TMPro;
using UnityEngine;

public class Picker : MonoBehaviour
{
    public Camera renderCamera;
    [HideInInspector]
    public Color pointedColor;
    [HideInInspector]
    public string pointedShape;

    void Start()
    {
        if (renderCamera == null)
        {
            renderCamera = Camera.main;
        }
    }

    void Update()
    {
#if UNITY_ANDROID
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                GetColor();
#elif UNITY_IOS
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                GetColor();
#else
        if (Input.GetMouseButtonDown(0))
        {
            GetColor();
            GetShape();
        }
#endif
    }

    private void GetColor()
    {
        
        Vector3 mousePos = renderCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Проверяем, находится ли курсор над объектом
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            // Получаем компонент SpriteRenderer
            SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // Устанавливаем pointedColor на основе цвета спрайта
                pointedColor = spriteRenderer.color;
            }
            else
            {
                pointedColor = Color.black;
            }
        }
    }

    private void GetShape()
    {
        Vector3 mousePos = renderCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Проверяем, находится ли курсор над объектом
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            pointedShape = hit.collider.name;
            Debug.Log(pointedShape);
        }
        else
        {
            pointedShape = "none";
        }
    }
}