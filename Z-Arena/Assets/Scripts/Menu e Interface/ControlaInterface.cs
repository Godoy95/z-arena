using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlaInterface : MonoBehaviour {
    private ControlaJogador scriptControlaJogador;
    public Slider SliderVidaJogador;
    public GameObject PainelDeGameOver;
    public Text TextoTempoDeSobrevivencia;
    public Text TextoPontuacaoMaxima;
    private float tempoPontuacaoSalvo;
    private int pontosMax;


    void Start()
    {
        scriptControlaJogador = GameObject.FindWithTag("Jogador").GetComponent<ControlaJogador>();
        SliderVidaJogador.maxValue = scriptControlaJogador.statusJogador.Vida;
        AtualizarSliderVidaJogador();
        tempoPontuacaoSalvo = PlayerPrefs.GetFloat("PontuacaoMaxima");
       
    }
    


    public void AtualizarSliderVidaJogador()
        {
            SliderVidaJogador.value = scriptControlaJogador.statusJogador.Vida;
        }

    public void GameOver()
    {
        PainelDeGameOver.SetActive(true);
        int minutos = (int)Time.timeSinceLevelLoad / 60;
        int segundos = (int)Time.timeSinceLevelLoad % 60;
        int pontuacao = (int)(Time.timeSinceLevelLoad * 4.78);
        TextoTempoDeSobrevivencia.text = "Você sobreviveu por " + minutos + "min e " + segundos + "s  \n  E sua pontuação foi: " + pontuacao ;
        AjustarPontuacaoMaxima(minutos, segundos);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void AjustarPontuacaoMaxima(int min, int seg)
    {

        if (Time.timeSinceLevelLoad > tempoPontuacaoSalvo)
        {
            tempoPontuacaoSalvo = Time.timeSinceLevelLoad;
            pontosMax = (int)(Time.timeSinceLevelLoad * 4.78);
            TextoPontuacaoMaxima.text = string.Format("Seu melhor tempo é {0}min e {1}s com {2} pontos", min, seg, pontosMax);
            PlayerPrefs.SetFloat("PontuacaoMaxima", tempoPontuacaoSalvo);
            
            
        }
        if (TextoPontuacaoMaxima.text == "")
        {
            min = (int)tempoPontuacaoSalvo / 60;
            seg = (int)tempoPontuacaoSalvo % 60;
            pontosMax = (int)(tempoPontuacaoSalvo * 4.78);
            TextoPontuacaoMaxima.text = string.Format("Seu melhor tempo é {0}min e {1}s com {2} pontos", min, seg, pontosMax);
        }
    }
    public void Reiniciar()
    {
        SceneManager.LoadScene("Menu Principal");
    }

   
}
