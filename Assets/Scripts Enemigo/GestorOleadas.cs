using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GestorOleadas : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefabEnemigo;
    public GameObject prefabBoss;

    [Header("Puntos de spawn")]
    public Transform[] puntosSpawn;

    [Header("UI - Anuncio de oleada")]
    public TextMeshProUGUI textoOleada;   // texto grande en el centro de pantalla
    public float duracionAnuncio = 2.5f;  // segundos que se muestra

    [Header("Puntuacion flotante")]
    public GameObject prefabTextoPuntos;  // prefab con TextMeshPro para mostrar puntos

    // Nombres de oleada en inglés
    static readonly string[] nombresOleada = { "WAVE ONE", "WAVE TWO", "WAVE THREE", "FINAL WAVE" };

    int[] zombiesPorOleada = { 4, 6, 8, 10 };

    int oleadaActual = 0;
    int enemigosVivos = 0;
    bool bossMuerto = false;
    bool esperando = false;

    void Start()
    {
        if (textoOleada != null) textoOleada.gameObject.SetActive(false);
        StartCoroutine(IniciarOleada());
    }

    IEnumerator IniciarOleada()
    {
        esperando = false;

        // ── Anuncio de oleada ──────────────────────────────────────
        yield return StartCoroutine(MostrarAnuncio(oleadaActual));

        // ── Pausa extra entre oleadas (no en la primera) ───────────
        if (oleadaActual > 0)
            yield return new WaitForSeconds(1f);

        // ── Spawn ──────────────────────────────────────────────────
        int zombies = zombiesPorOleada[oleadaActual];
        bool esUltima = (oleadaActual == 3);

        enemigosVivos = 0;

        for (int i = 0; i < zombies; i++)
        {
            SpawnEnemigo(prefabEnemigo);
            enemigosVivos++;
            yield return new WaitForSeconds(0.4f);
        }

        if (esUltima && prefabBoss != null)
        {
            bossMuerto = false;
            SpawnEnemigo(prefabBoss);
        }
    }

    IEnumerator MostrarAnuncio(int indice)
    {
        if (textoOleada == null) yield break;

        string nombre = indice < nombresOleada.Length ? nombresOleada[indice] : "WAVE " + (indice + 1);
        textoOleada.text = nombre;
        textoOleada.gameObject.SetActive(true);

        // Fade in
        textoOleada.alpha = 0f;
        float t = 0f;
        while (t < 0.4f)
        {
            t += Time.deltaTime;
            textoOleada.alpha = Mathf.Clamp01(t / 0.4f);
            yield return null;
        }

        yield return new WaitForSeconds(duracionAnuncio);

        // Fade out
        t = 0f;
        while (t < 0.4f)
        {
            t += Time.deltaTime;
            textoOleada.alpha = 1f - Mathf.Clamp01(t / 0.4f);
            yield return null;
        }

        textoOleada.gameObject.SetActive(false);
    }

    void SpawnEnemigo(GameObject prefab)
    {
        if (puntosSpawn == null || puntosSpawn.Length == 0)
        {
            Debug.LogWarning("[OLEADAS] No hay puntos de spawn asignados");
            return;
        }
        Transform punto = puntosSpawn[Random.Range(0, puntosSpawn.Length)];
        Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
        Instantiate(prefab, punto.position + offset, Quaternion.identity);
    }

    // ── Llamado por enemigo.cs al morir ───────────────────────────────────
    public void EnemigoMuerto(bool eraBoss)
    {
        if (eraBoss) { bossMuerto = true; }
        else { enemigosVivos = Mathf.Max(0, enemigosVivos - 1); }
        ComprobarFinOleada();
    }

    // ── Muestra los puntos flotando donde murió el enemigo ───────────────
    public void MostrarPuntosFlotantes(int puntos, Vector3 posicion)
    {
        if (prefabTextoPuntos == null) return;

        GameObject go = Instantiate(prefabTextoPuntos,
            new Vector3(posicion.x, posicion.y + 0.5f, posicion.z),
            Quaternion.identity);

        TextMeshPro tmp = go.GetComponent<TextMeshPro>();
        if (tmp != null)
        {
            tmp.text = "+" + puntos;
        }
    }

    void ComprobarFinOleada()
    {
        bool esUltima = (oleadaActual == 3);
        bool oleadaLimpia = esUltima
            ? (enemigosVivos <= 0 && bossMuerto)
            : (enemigosVivos <= 0);

        if (!oleadaLimpia || esperando) return;
        esperando = true;

        if (esUltima)
        {
            PlayerPrefs.SetInt("puntuacion_final", movimientoJugador.Puntuacion);
            PlayerPrefs.SetFloat("tiempo_final", movimientoJugador.TiempoTotal);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Final-Victoria");
        }
        else
        {
            oleadaActual++;
            StartCoroutine(IniciarOleada());
        }
    }
}