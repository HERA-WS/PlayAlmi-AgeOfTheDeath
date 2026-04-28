using UnityEngine;
using UnityEngine.SceneManagement;

// Pon este script en cualquier boton y llama al metodo que necesites
// desde el evento OnClick() del componente Button en el Inspector

public class GestorEscenas : MonoBehaviour
{
    // -------------------------------------------------------
    // Navegar a una escena por nombre
    // Uso: arrastrar este script al boton, OnClick -> IrA("NombreEscena")
    // -------------------------------------------------------
    public void IrA(string nombreEscena)
    {
        // Guarda la escena actual antes de salir para poder volver
        PlayerPrefs.SetString("escenaAnterior", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        SceneManager.LoadScene(nombreEscena);
    }

    // -------------------------------------------------------
    // Volver a la escena anterior (usa la que guardamos con IrA)
    // Uso: OnClick -> Volver()
    // -------------------------------------------------------
    public void Volver()
    {
        string anterior = PlayerPrefs.GetString("escenaAnterior", "Pantalla Inicio");
        SceneManager.LoadScene(anterior);
    }

    // -------------------------------------------------------
    // Salir del juego (para un boton Salir en el futuro)
    // -------------------------------------------------------
    public void Salir()
    {
        Application.Quit();
        // En el editor Unity no cierra la app, esto lo simula:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
