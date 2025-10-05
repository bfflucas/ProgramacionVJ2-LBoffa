using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("Vidas")]
    [SerializeField] GameObject iconoVida;
    [SerializeField] GameObject contenedorVida;

    [Header("Estrellas")]
    [SerializeField] GameObject iconoEstrella;
    [SerializeField] GameObject contenedorEstrellas;

    [Header("Experiencia")]
    [SerializeField] private TextMeshProUGUI textoExperiencia;



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
