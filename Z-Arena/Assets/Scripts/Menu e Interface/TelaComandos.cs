using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaComandos : MonoBehaviour
{

    public GameObject menu;
    public GameObject background;


    private void OnEnable()
    {
        menu.SetActive(false);
        background.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Voltar();
        }
    }

    public void Voltar()
    {
        menu.SetActive(true);
        background.SetActive(true);
    }

    public void Comecar()
    {
        SceneManager.LoadScene("Game");
    }
}
