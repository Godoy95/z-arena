using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaMunicao : MonoBehaviour
{
    public ControlaArma controlaArma;

    // Use this for initialization
    void Start()
    {
      
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Jogador")
        {
            Arma arma = controlaArma.gameObject.GetComponentInChildren<Arma>();
            arma.EncherMunicao();
        }
    }
}
