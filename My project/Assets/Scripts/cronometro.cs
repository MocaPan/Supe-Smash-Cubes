using UnityEngine;
using TMPro;             // ? para TextMeshPro
using CustomPhysics2D;   // si lo necesitas en tu proyecto

public class CronometroTMP : MonoBehaviour
{
    [Tooltip("Referencia al TMP_Text donde se mostrará el tiempo")]
    public TMP_Text timerText;

    [Tooltip("Tiempo inicial en segundos")]
    public float tiempoInicial = 90f;

    private float tiempoRestante;
    private bool corriendo;

    void Start()
    {
        tiempoRestante = tiempoInicial;
        corriendo = true;

        if (timerText == null)
        {
            Debug.LogError(
                name + ": falta asignar el TMP_Text de cronómetro");
        }
    }

    void Update()
    {
        if (!corriendo) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            corriendo = false;
            OnTimerEnd();
        }

        timerText.text = FormatearTiempo(tiempoRestante);
    }

    private string FormatearTiempo(float segundos)
    {
        int minutos = (int)(segundos / 60f);
        int secs = (int)(segundos % 60f);
        return minutos.ToString("00") + ":" + secs.ToString("00");
    }

    private void OnTimerEnd()
    {
        Debug.Log("¡Se acabó el tiempo!");
        // Aquí tu lógica de Game Over, pausa, etc.
    }

    /// <summary>Reinicia el cronómetro a su valor inicial.</summary>
    public void Reiniciar()
    {
        tiempoRestante = tiempoInicial;
        corriendo = true;
    }

    /// <summary>Detiene el cronómetro.</summary>
    public void Detener()
    {
        corriendo = false;
    }
}
