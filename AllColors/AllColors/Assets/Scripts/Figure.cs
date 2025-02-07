using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure
{
    public Color Color { get; set; }
    public string Shape { get; set; }

    public Figure(Color color, string shape)
    {
        Color = color;
        Shape = shape;
    }
}
