using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenúPausa : MonoBehaviour
{
    [SerializeField] GameObject botonPausa;
    [SerializeField] GameObject menuPausa;
    [SerializeField] GameObject Cronometro;
    private bool isPaused = false;
    private void Update()
    {
        if (CronometroTMP.GameIsOver)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Reanudar();
            }
            else
            {
                Pausa();
            }
        }
    }
    public void Pausa()
    {
        isPaused = true;
        Time.timeScale = 0;
        botonPausa.SetActive(false);
        Cronometro.SetActive(false);
        menuPausa.SetActive(true);
    }
    public void Reanudar()
    {
        isPaused = false;
        Time.timeScale = 1;
        botonPausa.SetActive(true);
        Cronometro.SetActive(true);
        menuPausa.SetActive(false);
    }
    public void Reiniciar()
    {
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Salir()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
}
