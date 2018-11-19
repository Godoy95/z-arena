
using UnityEngine;

public class Bala : MonoBehaviour
{

    public float dano;
    public float Velocidade;

    private void Start()
    {
        transform.Rotate(90, 0, 0);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.up * Velocidade * Time.deltaTime);
    }

    void OnTriggerEnter(Collider objetoDeColisao)
    {
        if (objetoDeColisao.tag != "Inimigo")
        {
            Destroy(gameObject);
        }
    }
}
