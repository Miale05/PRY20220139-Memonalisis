using UnityEngine;

/// <summary>
/// Clase agregada a todos los tableros para rotarlos hacia la camara la primera vez que son invocados
/// </summary>
public class BoardFaceCamera : MonoBehaviour
{
    float tempRotX;
    float tempRotZ;

    void Start() {
        //Guardamos la rotacion actual en X y Z
        tempRotX = transform.eulerAngles.x;
        tempRotZ = transform.eulerAngles.z;
        //Forzamos al tablero a que mire a la camara
        transform.LookAt(Camera.main.transform);
        //Asignamos los viejos valores de rotacion en X y Z para que el tablero no se incline
        transform.eulerAngles = new Vector3(tempRotX, transform.eulerAngles.y, tempRotZ);
    }
}
