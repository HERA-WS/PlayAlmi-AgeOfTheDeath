using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class movimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;

    [Header("Vida (corazones)")]
    public int vidasMaximas = 3;
    int vidasActuales;

    [Header("Ataque")]
    public Collider2D hitboxAtaque;
    public float attackDuration = 0.2f;
    public int danioAtaque = 1;

    [Header("Invencibilidad tras daño")]
    public float tiempoInvencible = 1f;

    // Eventos para la UI
    public static event Action<int, int> OnVidasCambiadas;   // (actual, max)
    public static event Action<int> OnPuntuacionCambiada;
    public static event Action<float> OnTiempoCambiado;

    // Estado publico legible por otros scripts
    public static int Puntuacion { get; private set; }
    public static float TiempoTotal { get; private set; }
    public static bool Escudo { get; set; }   // lo activa HabilidadManager
    public static int MultiAtaque { get; set; } = 1; // x2 lo activa HabilidadManager

    Rigidbody2D rb;
    Vector2 moveInput;
    bool atacando = false;
    bool invencible = false;

    void Awake()
    {
        // Resetear statics al empezar nivel
        Puntuacion = 0;
        TiempoTotal = 0f;
        Escudo = false;
        MultiAtaque = 1;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidasActuales = vidasMaximas;
        hitboxAtaque.enabled = false;
        OnVidasCambiadas?.Invoke(vidasActuales, vidasMaximas);
        OnPuntuacionCambiada?.Invoke(Puntuacion);
    }

    void Update()
    {
        // Cronómetro
        TiempoTotal += Time.deltaTime;
        OnTiempoCambiado?.Invoke(TiempoTotal);

        // Movimiento
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;

        // Ataque con Espacio
        if (Input.GetKeyDown(KeyCode.Space) && !atacando)
            StartCoroutine(Ataque());
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    IEnumerator Ataque()
    {
        atacando = true;
        hitboxAtaque.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        hitboxAtaque.enabled = false;
        atacando = false;
    }

    // ---- Daño recibido (llamado desde Hitbox.cs del enemigo) ----
    public void RecibirDanio(int dmg)
    {
        if (invencible || Escudo) return;

        vidasActuales = Mathf.Max(0, vidasActuales - dmg);
        Debug.Log("[JUGADOR] Vidas: " + vidasActuales);
        OnVidasCambiadas?.Invoke(vidasActuales, vidasMaximas);

        if (vidasActuales <= 0) { Morir(); return; }

        StartCoroutine(Invencibilidad());
    }

    // ---- Recuperar un corazón (llamado desde HabilidadManager) ----
    public void RecuperarVida()
    {
        vidasActuales = Mathf.Min(vidasMaximas, vidasActuales + 1);
        OnVidasCambiadas?.Invoke(vidasActuales, vidasMaximas);
    }

    // ---- Sumar puntos (llamado desde enemigo / boss) ----
    public static void SumarPuntos(int pts)
    {
        Puntuacion += pts;
        OnPuntuacionCambiada?.Invoke(Puntuacion);
    }

    IEnumerator Invencibilidad()
    {
        invencible = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float t = 0f;
        while (t < tiempoInvencible)
        {
            if (sr != null) sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            t += 0.1f;
        }
        if (sr != null) sr.enabled = true;
        invencible = false;
    }

    void Morir()
    {
        Debug.Log("[JUGADOR] MUERTO");
        // Guardar resultados antes de cambiar escena
        PlayerPrefs.SetInt("puntuacion_final", Puntuacion);
        PlayerPrefs.SetFloat("tiempo_final", TiempoTotal);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Final-Derrota");
    }
}