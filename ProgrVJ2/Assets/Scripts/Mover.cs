
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Mover : MonoBehaviour
//{
//    // Variables a configurar desde el editor
//    [Header("Configuracion")]
//    [SerializeField] float velocidad = 5f;   //serialize field permite cambiar ese atributo en el inspector

//    // Variables de uso interno en el script
//    private float moverHorizontal; // determina en que direccion movernos dependiendo de la tecla presionada
//    private Vector2 direccion; //para hacer el calculo del movimiento en x e y

//    // Variable para referenciar otro componente del objeto
//    private Rigidbody2D miRigidbody2D;

//    // Codigo ejecutado cuando el objeto se activa en el nivel
//    private void OnEnable()  //cuando el objeto se activa, se carga en memoria
//    {
//        miRigidbody2D = GetComponent<Rigidbody2D>();  //para obtener la referencia al objeto rigidbody que tengo asociado en ese game object
//    }

//    // Codigo ejecutado en cada frame del juego (Intervalo variable)
//    private void Update()
//    {
//        moverHorizontal = Input.GetAxis("Horizontal"); //configuracion por defecto en unity que me devuelve si el usuario presiona A o D o flecha izq o der
//        direccion = new Vector2(moverHorizontal, 0f);
//    }
//    private void FixedUpdate()
//    {
//        miRigidbody2D.AddForce(direccion * velocidad);
//    }
//}


using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] float velocidad = 5f;

    private float moverHorizontal;
    private Rigidbody2D miRigidbody2D;

    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moverHorizontal = Input.GetAxis("Horizontal"); // A/D o flechas
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal controlado directamente con velocidad
        Vector2 nuevaVelocidad = new Vector2(moverHorizontal * velocidad, miRigidbody2D.linearVelocity.y);
        miRigidbody2D.linearVelocity = nuevaVelocidad;
    }
}


