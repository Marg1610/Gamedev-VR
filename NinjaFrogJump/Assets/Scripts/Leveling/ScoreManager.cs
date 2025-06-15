using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private const string ScorePrefix = "Level_";

    void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Сохраняем только если счёт лучше предыдущего
    public void SaveBestScore(int levelIndex, int newScore)
    {
        int currentBest = GetScore(levelIndex);

        // Только строго больше предыдущего рекорда
        if (newScore > currentBest)
        {
            PlayerPrefs.SetInt(ScorePrefix + levelIndex, newScore);
            PlayerPrefs.Save();
            Debug.Log($"Новый рекорд для уровня {levelIndex}: {newScore} (предыдущий: {currentBest})");
        }
        else
        {
            Debug.Log($"Текущий счёт {newScore} не превышает рекорд {currentBest} для уровня {levelIndex}. Рекорд не обновлён.");
        }
    }

    public int GetScore(int levelIndex)
    {
        return PlayerPrefs.GetInt(ScorePrefix + levelIndex, 0);
    }

    public void ResetAllScores()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Все рекорды сброшены!");
    }
    public void UpdateScores()
    {
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        menuManager?.UpdateScoresDisplay();
    }

    public static void EnsureInitialization()
    {
        if (Instance == null)
        {
            GameObject scoreManager = new GameObject("ScoreManager");
            scoreManager.AddComponent<ScoreManager>();
            Debug.Log("ScoreManager создан динамически");
        }
    }
}