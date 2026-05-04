using UnityEngine;
using System.Collections;

public class enemigo : MonoBehaviour
{
    [Header("Stats")]
    public int vida = 3;
    public float velocidad = 2f;
    public int danio = 1;
    public int puntos = 10;
    public bool esBoss = false;

    [Header("Rangos")]
    public float distanciaDeteccion = 5f;
    public float distanciaAtaque = 0.7f;

    [Header("Ataque")]
    public float attackDuration = 0.15f;
    public float attackCooldown = 1.2f;

    [Header("Referencias")]
    public Collider2D hitbox;   // hijo HitboxAtaque, Is Trigger = true

    Transform jugador;
    Rigidbody2D rb;

    enum Estado { Idle, Perseguir, Atacar }
    Estado estado = Estado.Idle;
    bool enCooldown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox.enabled = false;

        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) jugador = go.transform;
        else Debug.LogWarning("[ENEMIGO] No hay objeto con tag 'Player'");
    }

    void Update()
    {
        if (jugador == null) return;

        float dist = Vector2.Distance(transform.position, jugador.position);

        switch (estado)
        {
            case Estado.Idle:
                if (dist < distanciaDeteccion) CambiarEstado(Estado.Perseguir);
                break;
            case Estado.Perseguir:
                if (dist <= distanciaAtaque && !enCooldown) CambiarEstado(Estado.Atacar);
                else if (dist > distanciaDeteccion) CambiarEstado(Estado.Idle);
                break;
            case Estado.Atacar:
                break;
        }
    }

    void FixedUpdate()
    {
        if (jugador == null) return;

        if (estado == Estado.Perseguir)
        {
            Vector2 dir = (jugador.position - transform.position).normalized;
            rb.linearVelocity = dir * velocidad;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void CambiarEstado(Estado nuevo)
    {
        estado = nuevo;
        if (nuevo == Estado.Atacar) StartCoroutine(Ataque());
    }

    IEnumerator Ataque()
    {
        Debug.Log("[ENEMIGO] Atacando a " + (jugador != null ? jugador.name : "?"));
        hitbox.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        hitbox.enabled = false;

        enCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        enCooldown = false;

        CambiarEstado(Estado.Perseguir);
    }

    // Llamado desde Hitbox.cs del jugador


    void Morir()
    {
        movimientoJugador.SumarPuntos(puntos);

        // Mostrar puntos flotantes donde murió
        GestorOleadas gestor = FindFirstObjectByType<GestorOleadas>();
        if (gestor != null)
        {
            gestor.MostrarPuntosFlotantes(puntos, transform.position);
            gestor.EnemigoMuerto(esBoss);
        }

        // Soltar habilidad solo si no es boss
        if (!esBoss)
        {
            HabilidadManager hm = FindFirstObjectByType<HabilidadManager>();
            if (hm != null) hm.IntentarSoltarHabilidad(transform.position);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }
}