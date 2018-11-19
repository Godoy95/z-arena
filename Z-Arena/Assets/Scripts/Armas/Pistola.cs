using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pistola : MonoBehaviour
{

    public GameObject balaPrefab;
    public GameObject CanoDaArma;
    public AudioClip SomDeTiro;
    public Text currentAmmoText;
    public Text ammoLeftText;
    public GameObject textoReload;
    public GameObject textoNoAmmo;

    public float fireRate;
    private int ammoLeft;
    public int magSize;
    private int currentAmmo = -1;
    public float reloadTime;
    private bool isReloading = false;
    public float dano;

    private float tempoDeTiro = 0f;

    // Use this for initialization
    void Start()
    {
        CanoDaArma = GameObject.FindWithTag("CanoDeArma");

        if (currentAmmo == -1)
        {
            currentAmmo = magSize;
        }
        ;
    }

    private void OnEnable()
    {
        isReloading = false;
        textoReload.SetActive(false);
        textoNoAmmo.SetActive(false);
        ammoLeftText.text = ("--");
        balaPrefab.GetComponent<Bala>().dano = dano;
    }

    // Update is called once per frame
    void Update()
    {
        currentAmmoText.text = (currentAmmo.ToString() + "/");     

        if (isReloading)
            return;

        if (currentAmmo <= 0 || (Input.GetKeyDown("r") && currentAmmo < magSize))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= tempoDeTiro)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(balaPrefab, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
        tempoDeTiro = Time.time + (1 / fireRate);
        ControlaAudio.instancia.PlayOneShot(SomDeTiro);
        currentAmmo--;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        textoReload.SetActive(true);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magSize;

        isReloading = false;
        textoReload.SetActive(false);

    }
}

