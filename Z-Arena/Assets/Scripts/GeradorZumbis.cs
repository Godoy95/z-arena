
using UnityEngine;

public class GeradorZumbis : MonoBehaviour {

    public GameObject Zumbi;
    private float contadorTempo = 0;
    public float TempoGerarZumbi;
    public float tempoAumentaSpawn;

    // Update is called once per frame
    void Update ()
    {
        contadorTempo += Time.deltaTime;
        if (contadorTempo >= TempoGerarZumbi)
        {
            Instantiate(Zumbi, transform.position, transform.rotation);
            contadorTempo = 0;
        }

        if(contadorTempo >= tempoAumentaSpawn)
        {
            TempoGerarZumbi -= TempoGerarZumbi * 0.4f;
            contadorTempo = 0;
        }
    }
}
