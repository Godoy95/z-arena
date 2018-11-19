using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMessage : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject proximoBT;
    public GameObject comecarBT;
    public GameObject controlaAudio;

    public static bool isPaused = false;
    private int mensagemAtual;

    private void Start()
    {
        mensagemAtual = -1;
        Pause();
        Proximo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controlaAudio.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Proximo()
    {

        if (mensagemAtual != -1)
        {
            pauseMenuUI.transform.GetChild(mensagemAtual).gameObject.SetActive(false);
        }

        mensagemAtual++;
        pauseMenuUI.transform.GetChild(mensagemAtual).gameObject.SetActive(true);

        if (mensagemAtual == 4)
        {
            proximoBT.SetActive(false);
            comecarBT.SetActive(true);
        }
        
    }
}
