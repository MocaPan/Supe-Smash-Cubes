// Score.cs
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class Score : MonoBehaviour
{
    private TMP_Text scoreText;
    private int currentScore = 0;

    void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = currentScore.ToString();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateUI();
    }

    public void SetScore(int amount)
    {
        currentScore = amount;
        UpdateUI();
    }

    public int GetScore() => currentScore;
}
