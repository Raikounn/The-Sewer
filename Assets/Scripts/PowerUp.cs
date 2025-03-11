using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp
{
    public string nombre;
    public float duration;

    public PowerUp(float last, string name)
    {
        nombre = name;
        duration = last;
    }
}