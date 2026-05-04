using UnityEngine;
using TMPro;
using System.Collections;

// Coloca este script en el prefab "TextoPuntos"
// El prefab necesita: TextMeshPro (no UI, el de mundo 3D)
// Tamaño de fuente recomendado: 3-4, color blanco o amarillo, sorting order alto

public class TextoPuntos : MonoBehaviour
{
    [Header("Animación")]
    public float duracion = 1.2f;       // tiempo hasta desaparecer
    public float velocidadSubida = 1.5f; // unidades por segundo que sube

    TextMeshPro tmp;

    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        StartCoroutine(Animar());
    }

    IEnumerator Animar()
    {
        float t = 0f;
        Vector3 posInicial = transform.position;
        Color colorInicial = tmp != null ? tmp.color : Color.white;

        while (t < duracion)
        {
            t += Time.deltaTime;
            float progreso = t / duracion;

            // Subir
            transform.position = posInicial + Vector3.up * velocidadSubida * t;

            // Fade out en la segunda mitad
            if (tmp != null && progreso > 0.5f)
            {
                float alpha = 1f - ((progreso - 0.5f) / 0.5f);
                tmp.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}