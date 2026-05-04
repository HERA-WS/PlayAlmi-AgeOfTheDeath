using UnityEngine;

// Coloca este script en el hijo HitboxAtaque de cada personaje
// El Collider2D de ese hijo debe tener Is Trigger = TRUE

public class Hitbox : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[HITBOX] Contacto con: " + other.gameObject.name + " | Tag: " + other.tag);

        // ── Golpea a un enemigo ──────────────────────────────────────
        // Buscamos el componente enemigo en el objeto golpeado
        // y también en su padre (por si la hitbox del jugador toca el cuerpo del enemigo)
        enemigo e = other.GetComponent<enemigo>();
        if (e == null) e = other.GetComponentInParent<enemigo>();

        if (e != null)
        {
            e.RecibirDanio(damage);
            return;
        }

        // ── Golpea al jugador ────────────────────────────────────────
        // Buscamos por tag "Player" primero (más fiable)
        if (other.CompareTag("Player"))
        {
            movimientoJugador pj = other.GetComponent<movimientoJugador>();
            if (pj == null) pj = other.GetComponentInParent<movimientoJugador>();
            if (pj != null)
            {
                pj.RecibirDanio(damage);
                return;
            }
        }

        // Fallback: buscar componente sin tag
        movimientoJugador pj2 = other.GetComponent<movimientoJugador>();
        if (pj2 == null) pj2 = other.GetComponentInParent<movimientoJugador>();
        if (pj2 != null)
        {
            pj2.RecibirDanio(damage);
        }
    }
}