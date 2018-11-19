using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public GameObject creditos;
    public GameObject comandos;
    public GameObject background2;

    private void OnEnable()
    {
        creditos.SetActive(false);
        background2.SetActive(false);
        comandos.SetActive(false);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void NovoJogo()
    {
        comandos.SetActive(true);
        background2.SetActive(true);
    }

    public void Creditos()
    {
        creditos.SetActive(true);
        background2.SetActive(true);
    }

    public void Sair()
    {
        Application.Quit();
    }

}
