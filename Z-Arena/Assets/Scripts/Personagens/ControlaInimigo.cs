
using UnityEngine;

public class ControlaInimigo : MonoBehaviour
{
    public GameObject Jogador;

    private Rigidbody rigidboodyInimigo;
    private Animator animatorInimigo;
    public AudioClip SomDaMorte;
    private Status statusInimigo;
    private MovimentoPersonagem movimentaInimigo;
    private AnimacaoPersonagem animacaoInimigo;
    private Vector3 posicaoAleatoria;
    private Vector3 direcao;
    private float contadorVagar;
    private float tempoEntrePosicoesAleatorias = 4;


    // Use this for initialization
    void Start()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        Jogador = GameObject.FindWithTag("Jogador");
        int geraTipoZumbi = Random.Range(1, 28);

        animacaoInimigo = GetComponent<AnimacaoPersonagem>();
        movimentaInimigo = GetComponent<MovimentoPersonagem>();
        statusInimigo = GetComponent<Status>();

        if(geraTipoZumbi == 6 || geraTipoZumbi == 11)
        {
            geraTipoZumbi++;
        }

        transform.GetChild(geraTipoZumbi).gameObject.SetActive(true);
    }

    
    void FixedUpdate()
    {
        float distancia = Vector3.Distance(transform.position, Jogador.transform.position);

        


        movimentaInimigo.Rotacionar(direcao);
        animacaoInimigo.Movimentar(direcao.magnitude);

        if (distancia > 30)
        {
            Vagar();
        }
        else if (distancia > 2.5)
        {
            direcao = Jogador.transform.position - transform.position;

            movimentaInimigo.Movimentar(direcao, statusInimigo.Velocidade);

            animacaoInimigo.Atacar(false);
        }
        else
        {
            animacaoInimigo.Atacar(true);
        }


    }
    void Vagar()
    {
        contadorVagar -= Time.deltaTime;
        if (contadorVagar <= 0)
        {
            posicaoAleatoria = AleatorizarPosicao();
            contadorVagar += tempoEntrePosicoesAleatorias;
        }

        bool ficouPertoOSuficiente = Vector3.Distance(transform.position, posicaoAleatoria) <= 0.2;
        if (ficouPertoOSuficiente == false)
        {
            direcao = posicaoAleatoria - transform.position;
            direcao.y = 0;
            movimentaInimigo.Rotacionar(direcao);
            
            movimentaInimigo.Movimentar(transform.forward, statusInimigo.Velocidade);
            

        }
    }

    Vector3 AleatorizarPosicao()
    {
        Vector3 posicao = Random.insideUnitSphere * 10;
        posicao += transform.position;
        posicao.y = transform.position.y;

        return posicao;
    }


    void AtacaJogador()
    {
        float dano = Random.Range(20.0f, 30.0f);
        Jogador.GetComponent<ControlaJogador>().TomarDano(dano);
    }

    void OnTriggerEnter(Collider objetoDeColisao)
    {
        if (objetoDeColisao.tag == "Projetil")
        {
            float dano = objetoDeColisao.GetComponent<Bala>().dano;
            Destroy(objetoDeColisao.gameObject);
            TomarDano(dano);
        }
    }

    public void TomarDano(float dano)
    {
        ReceberImpacto(2f);
        statusInimigo.Vida -= dano;
        if (statusInimigo.Vida <= 0)
        {
            Morrer();
        }
    }

    public void ReceberImpacto(float magnitude)
    {
        Vector3 direction = Jogador.transform.position - transform.position;
        transform.position -= direction.normalized * magnitude;
    }

    public void Morrer()
    {
        
        Destroy(gameObject);
        ControlaAudio.instancia.PlayOneShot(SomDaMorte);
    }
}

