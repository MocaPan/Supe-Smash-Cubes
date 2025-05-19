using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject MenúInfo;
    [SerializeField] GameObject MenúPrincipal;
    private bool inInfo = false;
    //private int nivelRandom = Random.Range(1, 4);
    public void Jugar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
    public void Information()
    {
        inInfo = true;
        MenúInfo.SetActive(true);
        MenúPrincipal.SetActive(false);

    }
    public void Volver()
    {
        inInfo = false;
        MenúInfo.SetActive(false);
        MenúPrincipal.SetActive(true);
    }
}
