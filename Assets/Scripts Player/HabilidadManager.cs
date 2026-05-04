using UnityEngine;
using System.Collections;

// ─────────────────────────────────────────────────────────────────────────
// IMPORTANTE: Solo debe existir ESTE archivo. Borra Habilidadmanager.cs
// ─────────────────────────────────────────────────────────────────────────

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

        if (r == 0) prefab = prefabEscudo;
        else if (r == 1 || r == 2) prefab = prefabCorazon;
        else if (r == 3 || r == 4) prefab = prefabCrucifijo;
        else prefab = prefabBiblia;

        if (prefab != null)
        {
            // Spawnear en Z=0 para que esté en el plano del jugador
            Vector3 spawnPos = new Vector3(pos.x, pos.y, 0f);
            Instantiate(prefab, spawnPos, Quaternion.identity);
            Debug.Log("[HABILIDAD] Soltada: " + prefab.name + " en " + spawnPos);
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────
// Scripts de cada habilidad — uno por prefab
// Cada prefab necesita: SpriteRenderer + CircleCollider2D (Is Trigger ON)
// El CircleCollider2D del prefab de habilidad debe estar en la misma Layer
// que detecta al jugador (ver Physics 2D → Layer Collision Matrix)
// ─────────────────────────────────────────────────────────────────────────

public class Habilidad_Escudo : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[ESCUDO] Trigger con: " + other.name + " tag: " + other.tag);

        movimientoJugador pj = BuscarJugador(other);
        if (pj == null) return;

        StartCoroutine(AplicarEscudo());
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 11f);
    }

    IEnumerator AplicarEscudo()
    {
        movimientoJugador.Escudo = true;
        Debug.Log("[HABILIDAD] Escudo activo 10s");
        yield return new WaitForSeconds(10f);
        movimientoJugador.Escudo = false;
        Debug.Log("[HABILIDAD] Escudo terminado");
    }

    movimientoJugador BuscarJugador(Collider2D col)
    {
        movimientoJugador pj = col.GetComponent<movimientoJugador>();
        if (pj == null) pj = col.GetComponentInParent<movimientoJugador>();
        return pj;
    }
}

public class Habilidad_Corazon : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[CORAZON] Trigger con: " + other.name + " tag: " + other.tag);

        movimientoJugador pj = other.GetComponent<movimientoJugador>();
        if (pj == null) pj = other.GetComponentInParent<movimientoJugador>();
        if (pj == null) return;

        pj.RecuperarVida();
        Debug.Log("[HABILIDAD] Corazon recogido");
        Destroy(gameObject);
    }
}

public class Habilidad_Crucifijo : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[CRUCIFIJO] Trigger con: " + other.name + " tag: " + other.tag);

        movimientoJugador pj = other.GetComponent<movimientoJugador>();
        if (pj == null) pj = other.GetComponentInParent<movimientoJugador>();
        if (pj == null) return;

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
        Debug.Log("[BIBLIA] Trigger con: " + other.name + " tag: " + other.tag);

        movimientoJugador pj = other.GetComponent<movimientoJugador>();
        if (pj == null) pj = other.GetComponentInParent<movimientoJugador>();
        if (pj == null) return;

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