using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Arma : MonoBehaviour
{

    public GameObject balaPrefab;
    public GameObject CanoDaArma;
    public AudioClip SomDeTiro;
    public Text currentAmmoText;
    public Text ammoLeftText;
    public GameObject textoReload;
    public GameObject textoNoAmmo;

    public float fireRate;
    public int maxAmmo;
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
            ammoLeft = maxAmmo;
        }

    }

    private void OnEnable()
    {
        isReloading = false;
        textoReload.SetActive(false);
        textoNoAmmo.SetActive(false);
        balaPrefab.GetComponent<Bala>().dano = dano;
    }

    // Update is called once per frame
    void Update()
    {
        currentAmmoText.text = (currentAmmo.ToString() + "/");
        ammoLeftText.text = (ammoLeft.ToString());

        if (currentAmmo <= 0 && ammoLeft <= 0)
        {
            textoNoAmmo.SetActive(true);
        }
        else
        {
            textoNoAmmo.SetActive(false);
        }

        if (isReloading)
        {
            return;
        }
            

        if ((currentAmmo <= 0 || (Input.GetKeyDown("r") && currentAmmo < magSize) && ammoLeft > 0))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= tempoDeTiro)
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

        if (currentAmmo <= 0)
        {
            if (ammoLeft >= magSize)
            {
                currentAmmo = magSize;
                ammoLeft -= magSize;
            }
            else if (ammoLeft > 0)
            {
                currentAmmo = ammoLeft;
                ammoLeft = 0;
            }
        }
        else
        {
            if (ammoLeft >= magSize - currentAmmo)
            {
                ammoLeft -= magSize - currentAmmo;
                currentAmmo = magSize;
            }
            else if (ammoLeft > 0)
            {
                currentAmmo += ammoLeft;
                ammoLeft = 0;
            }
        }
        
        isReloading = false;
        textoReload.SetActive(false);

    }

    public void EncherMunicao()
    {
        ammoLeft = maxAmmo;
    }
}

