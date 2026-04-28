using UnityEngine;

// Coloca este script en el GameObject hijo con Collider2D -> Is Trigger = TRUE
// Vale tanto para la hitbox del jugador como para la del enemigo

public class Hitbox : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[HITBOX] Contacto con: " + other.gameObject.name);

        enemigo e = other.GetComponent<enemigo>();
        if (e != null) { e.RecibirDanio(damage); return; }

        movimientoJugador pj = other.GetComponent<movimientoJugador>();
        if (pj != null) { pj.RecibirDanio(damage); }
    }
}