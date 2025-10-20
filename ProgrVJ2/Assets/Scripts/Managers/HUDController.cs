using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI miTexto;


    [Header("Vidas")]
    [SerializeField] GameObject iconoVida;
    [SerializeField] GameObject contenedorVida;

    [Header("Estrellas")]
    [SerializeField] GameObject iconoEstrella;
    [SerializeField] GameObject contenedorEstrellas;

    [Header("Experiencia")]
    [SerializeField] private TextMeshProUGUI textoExperiencia;


    private void OnEnable()
    {
        GameEvents.OnPause += Pausar;  //con += asignamos el metodo que va a responder (no poner parentesis)
        GameEvents.OnResume += Reanudar;
    }

    private void OnDisable()
    {
        //buena practica: desuscribirse a los eventos
        GameEvents.OnPause -= Pausar;
        GameEvents.OnResume -= Reanudar;
    }

    private void Pausar()
    {
        ActualizarTextoHUD("PAUSADO");
    }

    private void Reanudar()
    {
        ActualizarTextoHUD("");
    }

    // ------------------------------
    // TEXTO HUD
    // ------------------------------

    public void ActualizarTextoHUD(string nuevoTexto)
    {
        miTexto.text = nuevoTexto;
    }


    // ------------------------------
    // VIDAS
    // ------------------------------
    public void ActualizarVidasHUD(int vidas)
    {
        int cantidadActual = contenedorVida.transform.childCount;

        if (cantidadActual < vidas)
        {
            for (int i = cantidadActual; i < vidas; i++)
                Instantiate(iconoVida, contenedorVida.transform);
        }
        else if (cantidadActual > vidas)
        {
            for (int i = cantidadActual - 1; i >= vidas; i--)
                Destroy(contenedorVida.transform.GetChild(i).gameObject);
        }
    }

    // ------------------------------
    // ESTRELLAS
    // ------------------------------
    public void ActualizarEstrellasHUD(int cantidad)
    {
        int cantidadActual = contenedorEstrellas.transform.childCount;

        if (cantidadActual < cantidad)
        {
            for (int i = cantidadActual; i < cantidad; i++)
                Instantiate(iconoEstrella, contenedorEstrellas.transform);
        }
        else if (cantidadActual > cantidad)
        {
            for (int i = cantidadActual - 1; i >= cantidad; i--)
                Destroy(contenedorEstrellas.transform.GetChild(i).gameObject);
        }
    }

    // ------------------------------
    // EXPERIENCIA (EXP)
    // ------------------------------
    public void ActualizarExperienciaHUD(int experiencia)
    {
        if (textoExperiencia != null)
        {
            textoExperiencia.text = "EXP: " + experiencia;
        }
    }

    // Opcional: mostrar bonus de EXP temporalmente
    public void MostrarXPBonus(int cantidadGanada, int experienciaTotal)
    {
        if (textoExperiencia != null)
        {
            textoExperiencia.text = "EXP: " + experienciaTotal + " (+" + cantidadGanada + ")";
            // Restaurar después de 1 segundo
            StartCoroutine(RestaurarTextoXP(experienciaTotal, 1f));
        }
    }

    private System.Collections.IEnumerator RestaurarTextoXP(int experienciaTotal, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (textoExperiencia != null)
            textoExperiencia.text = "EXP: " + experienciaTotal;
    }
}
