using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CronometroTMP : MonoBehaviour
{
    [Tooltip("Referencia al TMP_Text donde se mostrará el tiempo")]
    public TMP_Text timerText;

    [Tooltip("Tiempo inicial en segundos")]
    public float tiempoInicial = 90f;

    [Header("Game Over UI")]
    [Tooltip("Panel que contiene el mensaje de fin de juego")]
    public GameObject gameOverPanel;
    [Tooltip("Texto donde se mostrará el ganador o empate")]
    public TMP_Text resultText;

    [Header("Puntajes por jugador")]
    [Tooltip("Array con los componentes Score de cada jugador")]
    public Score[] playerScores;

    [Tooltip("Segundos a esperar tras Game Over para volver al menú")]
    public float delayToMainMenu = 5f;

    // Flag estático para indicar que el juego terminó
    public static bool GameIsOver { get; private set; }

    private float tiempoRestante;
    private bool corriendo;

    void Start()
    {
        tiempoRestante = tiempoInicial;
        corriendo = true;
        GameIsOver = false;

        if (timerText == null)
            Debug.LogError(name + ": falta asignar el TMP_Text de cronómetro");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
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
        // Indicamos que el juego ha terminado
        GameIsOver = true;
        // Pausamos toda la simulación
        Time.timeScale = 0f;

        // Determinar el puntaje más alto
        int maxScore = int.MinValue;
        bool tie = false;
        int winnerIdx = -1;

        for (int i = 0; i < playerScores.Length; i++)
        {
            int s = playerScores[i].GetScore();
            if (s > maxScore)
            {
                maxScore = s;
                winnerIdx = i;
                tie = false;
            }
            else if (s == maxScore)
            {
                tie = true;
            }
        }

        // Mostrar resultado
        if (resultText != null && gameOverPanel != null)
        {
            if (tie)
                resultText.text = $"Empate con {maxScore} puntos";
            else
                resultText.text = $"Jugador {winnerIdx + 1} gana con {maxScore} puntos";

            gameOverPanel.SetActive(true);
        }

        // Iniciar coroutine para esperar y volver al menú principal
        StartCoroutine(GoToMainMenuAfterDelay());
    }

    private System.Collections.IEnumerator GoToMainMenuAfterDelay()
    {
        // Espera real, independiente de Time.timeScale
        yield return new WaitForSecondsRealtime(delayToMainMenu);
        SceneManager.LoadScene(0);
    }

    public void Reiniciar()
    {
        tiempoRestante = tiempoInicial;
        corriendo = true;
    }

    public void Detener()
    {
        corriendo = false;
    }
}
