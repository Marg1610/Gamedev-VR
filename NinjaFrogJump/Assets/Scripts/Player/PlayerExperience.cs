using TMPro;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public static PlayerExperience Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    private int experience = 0;
    private int levelIndex = 1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        ScoreManager.EnsureInitialization();
    }

    void Start()
    {
        ResetExperienceForNewRun();
    }

    public void ResetExperienceForNewRun()
    {
        experience = 0;
        UpdateScoreUI();
        Debug.Log("Счёт сброшен для нового прохождения уровня");
    }
    public void SaveCurrentScore()
    {
        int currentScore = GetExperience();
        int bestScore = ScoreManager.Instance.GetScore(levelIndex);

        if (currentScore > bestScore)
        {
            ScoreManager.Instance.SaveBestScore(levelIndex, currentScore);
        }
    }
    public void ResetExperience()
    {
        experience = 0;
        UpdateScoreUI();
    }

    public int GetExperience() => experience;

    public void AddExperience(int points)
    {
        experience += points;
        UpdateScoreUI();
        Debug.Log($"Опыт добавлен: +{points} (текущий: {experience})");
    }

    public void SetLevelIndex(int index)
    {
        levelIndex = index;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Опыт: " + experience.ToString();
        }
        else
        {
            Debug.LogWarning("ScoreText is not assigned in PlayerExperience!");
        }
    }

    public void ShowBestScore()
    {
        int bestScore = ScoreManager.Instance.GetScore(levelIndex);
        Debug.Log($"Лучший счёт для уровня {levelIndex}: {bestScore}");
    }
}