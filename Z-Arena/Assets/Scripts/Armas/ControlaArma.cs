
using UnityEngine;

public class ControlaArma : MonoBehaviour
{

    public int armaAtual;
    public int totalArmas;
    public GameObject crosshairPrefab;
    public LineRenderer laser;
    public GameObject CanoDaArma;
    public GameObject Camera;
    public LayerMask mask;
    public float laserRange;


    // Use this for initialization
    void Start()
    {
        transform.GetChild(armaAtual).gameObject.SetActive(true);
        laser = GetComponent<LineRenderer>();
        laser.enabled = false;
        //Instancia o crosshair
        if (crosshairPrefab != null)
        {
            crosshairPrefab = Instantiate(crosshairPrefab);
            ToggleCrosshair(false);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.GetChild(armaAtual).gameObject.SetActive(false);
            if (armaAtual == 0)
            {
                armaAtual = totalArmas - 1;
            }
            else
            {
                armaAtual--;
            }
            transform.GetChild(armaAtual).gameObject.SetActive(true);
        }

    }

    private void FixedUpdate()
    {
        //  Mira
        if (Input.GetButton("Fire2"))
        {
            PositionCrosshair();
        }
        else
        {
            ToggleCrosshair(false);
        }
    }

    // Habilita ou desabilita o crosshair
    void ToggleCrosshair(bool enable)
    {
        if (crosshairPrefab != null && laser != null)
        {
            laser.enabled = enable;
            crosshairPrefab.SetActive(enable);
        }
    }

    // Posiciona o crosshair
    public void PositionCrosshair()
    {
        RaycastHit hit;
        Vector3 origem = CanoDaArma.transform.position;
        Vector3 direcao = CanoDaArma.transform.forward;
        laser.SetPosition(0, origem - direcao * 0.3f + CanoDaArma.transform.right * 0.05f);

        if (Physics.Raycast(origem, direcao, out hit, 100, mask, QueryTriggerInteraction.Ignore) && Vector3.Distance(origem, hit.point) < laserRange)
        {
            Debug.DrawLine(origem, hit.point, Color.red);
            if (crosshairPrefab != null && laser != null)
            {
                //ToggleCrosshair(true);;

                laser.enabled = true;
                crosshairPrefab.transform.position = hit.point;
                
                crosshairPrefab.transform.LookAt(Camera.transform);
                laser.SetPosition(1, hit.point);
            }
        }
        else
        {
            crosshairPrefab.SetActive(false);
            laser.enabled = true;
            laser.SetPosition(1, origem + direcao * laserRange);
        }
    }
}
