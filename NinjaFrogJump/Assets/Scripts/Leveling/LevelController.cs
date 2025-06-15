using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private int levelIndex = 1;
    public static int LastCompletedLevel { get; private set; }
    public static bool IsNewRecord { get; private set; }

    void Start()
    {
        ScoreManager.EnsureInitialization();

        GameState.LivesOnGameOver = 0;
        GameState.ScoreOnGameOver = 0;

        if (PlayerExperience.Instance != null)
        {
            PlayerExperience.Instance.SetLevelIndex(levelIndex);
            PlayerExperience.Instance.ShowBestScore();
            PlayerExperience.Instance.ResetExperience();
        }
    }

    public void FinishLevel()
    {
        LastCompletedLevel = levelIndex;
        IsNewRecord = false;

        if (PlayerExperience.Instance != null)
        {
            int currentScore = PlayerExperience.Instance.GetExperience();
            int bestScore = ScoreManager.Instance.GetScore(levelIndex);

            if (currentScore > bestScore)
            {
                ScoreManager.Instance.SaveBestScore(levelIndex, currentScore);
                IsNewRecord = true;
            }
        }

        SceneManager.LoadScene("Result");
    }
}