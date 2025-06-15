using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text newRecordText;
    [SerializeField] private TMP_Text bestScoreText;

    void Start()
    {
        int levelIndex = LevelController.LastCompletedLevel;
        int currentScore = PlayerExperience.Instance.GetExperience();
        int bestScore = ScoreManager.Instance.GetScore(levelIndex);

        scoreText.text = $"Ваш счёт: {currentScore}";
        levelText.text = $"Уровень {levelIndex} пройден!";
        bestScoreText.text = $"Лучший: {bestScore}";

        newRecordText.gameObject.SetActive(LevelController.IsNewRecord);

        ScoreManager.Instance.UpdateScores();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}