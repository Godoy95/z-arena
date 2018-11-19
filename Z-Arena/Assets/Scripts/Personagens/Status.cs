using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public float VidaInicial = 100.0f;
    [HideInInspector]
    public float Vida;
    public float Velocidade = 5.0f;

    void Awake()
    {
        Vida = VidaInicial;
    }
}
