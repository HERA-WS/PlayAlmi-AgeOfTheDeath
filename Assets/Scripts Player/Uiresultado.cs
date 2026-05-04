using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Coloca este script en las escenas Final-Derrota y Final-Victoria
// Asigna los dos TextMeshPro en el Inspector

public class UIResultado : MonoBehaviour
{
    [Header("Textos de resultado")]
    public TextMeshProUGUI textoPuntuacion;
    public TextMeshProUGUI textoCronometro;

    void Start()
    {
        int pts = PlayerPrefs.GetInt("puntuacion_final", 0);
        float segundos = PlayerPrefs.GetFloat("tiempo_final", 0f);

        if (textoPuntuacion != null)
            textoPuntuacion.text = "Puntuacion: " + pts + " pts";

        if (textoCronometro != null)
        {
            int m = Mathf.FloorToInt(segundos / 60f);
            int s = Mathf.FloorToInt(segundos % 60f);
            textoCronometro.text = "Tiempo: " + string.Format("{0:00}:{1:00}", m, s);
        }
    }

    // Boton "Volver al menu" en las pantallas de resultado
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Pantalla Inicio");
    }

    // Boton "Reintentar"
    public void Reintentar()
    {
        SceneManager.LoadScene("Nivel1");
    }
}
