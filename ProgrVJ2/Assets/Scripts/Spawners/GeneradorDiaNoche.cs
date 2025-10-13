using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GeneradorDiaNoche : MonoBehaviour
{
    [SerializeField] private Camera camara;
    [SerializeField] private Color nocheColor;
    [SerializeField] private Light2D luz2D;


    [SerializeField][Range(4,128)] private float duracionDia;
    [SerializeField][Range(4, 24)] private float dias;


    private Color diaColor;


    void Start()
    {
        diaColor = camara.backgroundColor;
        StartCoroutine(CambiarColor(duracionDia));

    }


    IEnumerator CambiarColor(float tiempo)
    {
        Color colorDestinoFondo = camara.backgroundColor == diaColor ? nocheColor : diaColor;
        Color colorDestinoLuz = luz2D.color != Color.white ? Color.white : nocheColor; //en el caso de las luces, el color origen es gralmente blanco
        float duracionCiclo = tiempo * 0.6f;
        float duracionCambio = tiempo * 0.4f;

        for(int i = 0; i < duracionDia; i++)
        {
            yield return new WaitForSeconds(duracionCiclo);

            float tiempoTranscurrido = 0;

            while (tiempoTranscurrido < duracionCambio)
            {
                tiempoTranscurrido += Time.deltaTime;
                float t = tiempoTranscurrido / duracionCambio;

                float smoothT = Mathf.SmoothStep(0f, 1f, t); //suavizar el valor

                camara.backgroundColor = Color.Lerp(camara.backgroundColor, colorDestinoFondo, smoothT);
                luz2D.color=Color.Lerp(luz2D.color, colorDestinoLuz, smoothT);

                yield return null; //le decimos que no vamos a esperar, para que se ejecute frame a frame
            }

            colorDestinoFondo = camara.backgroundColor == diaColor ? nocheColor : diaColor;
            colorDestinoLuz = luz2D.color != Color.white ? Color.white : nocheColor;
        }

    }

    
}
