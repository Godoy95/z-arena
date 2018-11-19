
using UnityEngine;


public class ControlaJogador : MonoBehaviour
{

    private Vector3 direcao;
    public LayerMask MascaradoChao;
    public LayerMask mascaraInimigo;
    public GameObject TextoGameOver;
    private Rigidbody rigidbodyJogador;
    private Animator animatorJogador;
    public float velRotacao;
    private Transform pivot;

    public ControlaInterface scriptControlaInterface;
    public AudioClip somDanoMasc;
    public AudioClip somDanoFem;
    public float distanciaAlvo;
    public Status statusJogador;
    private int personagem;
    public float tempoEntreCoronhada;
    private float tempoProxCoronhada = 0f;

    private void Start()
    {

        rigidbodyJogador = GetComponent<Rigidbody>();
        animatorJogador = GetComponent<Animator>();
        pivot = GameObject.Find("Pivot").transform;
        statusJogador = GetComponent<Status>();
        GameObject persoAtivo = GameObject.FindGameObjectWithTag("Personagem");

        personagem = persoAtivo.transform.GetSiblingIndex();
        Debug.Log(personagem);
    }



    // Update is called once per frame
    void Update()
    {

        //Input do Jogador - guarda as teclas que são apertadas
        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");

        direcao = (transform.forward * eixoZ + transform.right * eixoX);
        direcao = direcao.normalized;

        //Animação do personagem
        if (direcao != Vector3.zero)
        {
            animatorJogador.SetBool("Movendo", true);
        }
        else
        {
            animatorJogador.SetBool("Movendo", false);
        }

        if (Input.GetKey(KeyCode.E) && Time.time >= tempoProxCoronhada)
        {
            Coronhada();
        }

    }

    void FixedUpdate()
    {

        //Movimentação do jogador por segundo com física, e sem o bug da camera
        rigidbodyJogador.MovePosition(rigidbodyJogador.position + (direcao * statusJogador.Velocidade * Time.deltaTime));

        //float horizontal = Input.GetAxis("Mouse X") * velRotacao;    // Lê a posição X do mouse
        //float vertical = Input.GetAxis("Mouse Y") * velRotacao;      // Lê a posição Y do mouse
        //transform.Rotate(0, horizontal, 0);                          // Rotaciona o personagem no eixo Y
        //pivot.Rotate(-vertical, 0, 0);                             // Rotaciona o pivot no eixo X

        CharacterLook();

        /*
        Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(raio.origin, raio.direction * 100, Color.red);

        RaycastHit impacto;
        if(Physics.Raycast(raio, out impacto, 100, MascaradoChao))
        {
            Vector3 posicaoMiraJogador = impacto.point - transform.position;
            //posicaoMiraJogador.y = 0;

            Quaternion novaRotacao = Quaternion.LookRotation(posicaoMiraJogador);

            rigidboodyJogador.MoveRotation(novaRotacao);
        }
        */
    }

    public void TomarDano(float dano)
    {
        statusJogador.Vida -= dano;
        if (personagem >= 19 && personagem <= 21)
        {
            ControlaAudio.instancia.PlayOneShot(somDanoFem);
        }
        else
        {
            ControlaAudio.instancia.PlayOneShot(somDanoMasc);
        }

        scriptControlaInterface.AtualizarSliderVidaJogador();
        if (statusJogador.Vida <= 0)
        {
            Morrer();
        }
    }

    void CharacterLook()
    {
        Transform mainCamT = Camera.main.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 lookTarget = mainCamPos + (mainCamT.forward * distanciaAlvo);
        Vector3 thisPosition = transform.position;
        Vector3 lookDir = lookTarget - thisPosition;
        Quaternion lookRot = Quaternion.LookRotation(lookDir);
        lookRot.x = 0;
        lookRot.z = 0;

        Quaternion newRotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * velRotacao);
        transform.rotation = newRotation;
    }

    public void Morrer()
    {
        Time.timeScale = 0;
        scriptControlaInterface.GameOver();
    }

    private void Coronhada()
    {
        Vector3 posHitBox = (transform.position + (transform.forward * 0.8f));
        Collider[] inimigosAcertados = Physics.OverlapSphere(posHitBox, 1.5f, mascaraInimigo);
        for(int i = 0; i < inimigosAcertados.Length; i++)
        {
            inimigosAcertados[i].GetComponent<ControlaInimigo>().ReceberImpacto(4);
        }
        tempoProxCoronhada = Time.time + tempoEntreCoronhada;
    }
}