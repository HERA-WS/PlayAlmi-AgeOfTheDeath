using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GestorOleadas : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefabEnemigo;   // zombie normal
    public GameObject prefabBoss;      // megazombie gordo

    [Header("Puntos de spawn (asigna en el Inspector)")]
    public Transform[] puntosSpawn;   // arrastra aqui varios GameObjects vacios repartidos por el nivel

    [Header("Oleadas")]
    // Numero de zombies normales por oleada (oleada 4 = boss + 10 zombies)
    int[] zombiesPorOleada = { 4, 6, 8, 10 };

    int oleadaActual = 0;
    int enemigosVivos = 0;
    bool bossMuerto = false;
    bool esperandoOleada = false;

    void Start()
    {
        StartCoroutine(IniciarOleada());
    }

    IEnumerator IniciarOleada()
    {
        esperandoOleada = false;
        int zombies = zombiesPorOleada[oleadaActual];
        bool esOleadaFinal = (oleadaActual == 3);

        Debug.Log("[OLEADAS] Oleada " + (oleadaActual + 1));

        // Pequeña pausa entre oleadas (excepto la primera)
        if (oleadaActual > 0)
            yield return new WaitForSeconds(3f);

        // Spawnear zombies normales
        for (int i = 0; i < zombies; i++)
        {
            SpawnEnemigo(prefabEnemigo);
            enemigosVivos++;
            yield return new WaitForSeconds(0.3f);
        }

        // En la oleada final spawnear también el boss
        if (esOleadaFinal)
        {
            SpawnEnemigo(prefabBoss);
            // El boss no cuenta en enemigosVivos, tiene su propio flag
        }
    }

    void SpawnEnemigo(GameObject prefab)
    {
        if (puntosSpawn == null || puntosSpawn.Length == 0)
        {
            Debug.LogWarning("[OLEADAS] No hay puntos de spawn asignados");
            return;
        }
        Transform punto = puntosSpawn[Random.Range(0, puntosSpawn.Length)];
        // Pequeño offset aleatorio para que no aparezcan todos en el mismo pixel
        Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
        Instantiate(prefab, punto.position + offset, Quaternion.identity);
    }

    // Llamado por enemigo.cs al morir
    public void EnemigoMuerto(bool eraBoss)
    {
        if (eraBoss)
        {
            bossMuerto = true;
            ComprobarFinOleada();
            return;
        }

        enemigosVivos = Mathf.Max(0, enemigosVivos - 1);
        ComprobarFinOleada();
    }

    void ComprobarFinOleada()
    {
        bool oleadaFinal = (oleadaActual == 3);

        // Oleada normal: todos los zombies muertos
        // Oleada final: todos los zombies Y el boss muertos
        bool oleadaLimpia = oleadaFinal
            ? (enemigosVivos <= 0 && bossMuerto)
            : (enemigosVivos <= 0);

        if (!oleadaLimpia || esperandoOleada) return;

        esperandoOleada = true;

        if (oleadaFinal)
        {
            // Guardar resultado y cargar victoria
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
