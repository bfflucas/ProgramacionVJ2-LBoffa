using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{

    [SerializeField] GameObject iconoVida;
    [SerializeField] GameObject contenedorVida;


    public void ActualizarVidasHUD(int vidas)
    {
       

        int cantidadActual = contenedorVida.transform.childCount;

        if (cantidadActual < vidas)
        {
            // Agregar iconos faltantes
            for (int i = cantidadActual; i < vidas; i++)
            {
                Instantiate(iconoVida, contenedorVida.transform);
            }
        }
        else if (cantidadActual > vidas)
        {
            // Remover iconos sobrantes
            for (int i = cantidadActual - 1; i >= vidas; i--)
            {
                Destroy(contenedorVida.transform.GetChild(i).gameObject);
            }
        }
    }

}
