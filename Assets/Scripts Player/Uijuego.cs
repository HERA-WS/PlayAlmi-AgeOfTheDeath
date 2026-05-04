using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Coloca este script en un GameObject vacio llamado "UI"
// Asigna los Image[] corazones, el TextMeshPro de puntuacion y el de tiempo

public class UIJuego : MonoBehaviour
{
    [Header("Corazones (arrastra las 3 imagenes en orden)")]
    public Image[] iconosCorazon;   // 3 imagenes de corazon en la HUD

    [Header("Textos")]
    public TextMeshProUGUI textoPuntuacion;
    public TextMeshProUGUI textoCronometro;

    void OnEnable()
    {
        movimientoJugador.OnVidasCambiadas += ActualizarCorazones;
        movimientoJugador.OnPuntuacionCambiada += ActualizarPuntuacion;
        movimientoJugador.OnTiempoCambiado += ActualizarCronometro;
    }

    void OnDisable()
    {
        movimientoJugador.OnVidasCambiadas -= ActualizarCorazones;
        movimientoJugador.OnPuntuacionCambiada -= ActualizarPuntuacion;
        movimientoJugador.OnTiempoCambiado -= ActualizarCronometro;
    }

    void ActualizarCorazones(int actual, int max)
    {
        for (int i = 0; i < iconosCorazon.Length; i++)
            iconosCorazon[i].enabled = (i < actual);
    }

    void ActualizarPuntuacion(int pts)
    {
        if (textoPuntuacion != null)
            textoPuntuacion.text = "Puntos: " + pts;
    }

    void ActualizarCronometro(float segundos)
    {
        if (textoCronometro == null) return;
        int m = Mathf.FloorToInt(segundos / 60f);
        int s = Mathf.FloorToInt(segundos % 60f);
        textoCronometro.text = string.Format("{0:00}:{1:00}", m, s);
    }
}
