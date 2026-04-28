using UnityEngine;
using System.Collections;

// ─────────────────────────────────────────────────────────────────────────────
// IMPORTANTE: Borra o elimina del proyecto el archivo "Habilidadmanager.cs"
// (con H minúscula). Solo debe existir ESTE archivo: HabilidadManager.cs
// ─────────────────────────────────────────────────────────────────────────────

public class HabilidadManager : MonoBehaviour
{
    [Header("Prefabs de habilidades")]
    public GameObject prefabEscudo;
    public GameObject prefabCorazon;
    public GameObject prefabCrucifijo;
    public GameObject prefabBiblia;

    [Header("Probabilidad base de soltar algo (0-1)")]
    [Range(0f, 1f)] public float chanceDrop = 0.3f;

    public void IntentarSoltarHabilidad(Vector3 posicion)
    {
        float roll = Random.value;
        if (roll > chanceDrop) return;
        SoltarHabilidadAleatoria(posicion);
    }

    void SoltarHabilidadAleatoria(Vector3 pos)
    {
        // Escudo=1, Corazon=2, Crucifijo=2, Biblia=1 -> total 6
        int r = Random.Range(0, 6);
        GameObject prefab;

        if      (r == 0)           prefab = prefabEscudo;
        else if (r == 1 || r == 2) prefab = prefabCorazon;
        else if (r == 3 || r == 4) prefab = prefabCrucifijo;
        else                       prefab = prefabBiblia;

        if (prefab != null)
            Instantiate(prefab, pos, Quaternion.identity);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Cada habilidad es una clase separada — pon cada una en su PROPIO prefab
// ─────────────────────────────────────────────────────────────────────────────

public class Habilidad_Escudo : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(AplicarEscudo());
        // Ocultar el objeto mientras la corrutina corre en this
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 11f); // se destruye al acabar los 10s
    }

    IEnumerator AplicarEscudo()
    {
        movimientoJugador.Escudo = true;
        Debug.Log("[HABILIDAD] Escudo activo 10s");
        yield return new WaitForSeconds(10f);
        movimientoJugador.Escudo = false;
        Debug.Log("[HABILIDAD] Escudo terminado");
    }
}

public class Habilidad_Corazon : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        movimientoJugador pj = other.GetComponent<movimientoJugador>();
        if (pj != null) pj.RecuperarVida();
        Debug.Log("[HABILIDAD] Corazon recogido");
        Destroy(gameObject);
    }
}

public class Habilidad_Crucifijo : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(AplicarCrucifijo());
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 11f);
    }

    IEnumerator AplicarCrucifijo()
    {
        movimientoJugador.MultiAtaque = 2;
        Debug.Log("[HABILIDAD] Ataque x2 activo 10s");
        yield return new WaitForSeconds(10f);
        movimientoJugador.MultiAtaque = 1;
        Debug.Log("[HABILIDAD] Ataque x2 terminado");
    }
}

public class Habilidad_Biblia : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        MatarTodosLosEnemigos();
        Destroy(gameObject);
    }

    void MatarTodosLosEnemigos()
    {
        enemigo[] enemigos = FindObjectsByType<enemigo>(FindObjectsSortMode.None);
        GestorOleadas gestor = FindFirstObjectByType<GestorOleadas>();
        int muertos = 0;
        foreach (enemigo e in enemigos)
        {
            if (!e.esBoss)
            {
                movimientoJugador.SumarPuntos(e.puntos);
                if (gestor != null) gestor.EnemigoMuerto(false);
                Destroy(e.gameObject);
                muertos++;
            }
        }
        Debug.Log("[HABILIDAD] Biblia: eliminados " + muertos + " enemigos");
    }
}
