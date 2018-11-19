using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creditos : MonoBehaviour
{

    public GameObject menu;
    public GameObject background;

    // Use this for initialization
    void Start()
    {

    }

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
            menu.SetActive(true);
            background.SetActive(true);
        }
    }

    public void Voltar()
    {
        menu.SetActive(true);
        background.SetActive(true);
    }
}
