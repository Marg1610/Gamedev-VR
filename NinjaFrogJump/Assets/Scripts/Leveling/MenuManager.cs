using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] levelScoreTexts;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button[] levelButtons;

    void Start()
    {
        ScoreManager.EnsureInitialization();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
        }

        resetButton.onClick.AddListener(ResetAllScores);
        UpdateScoresDisplay();
        UpdateLevelButtonsState();

    }

public void UpdateScoresDisplay()
    {
        for (int i = 0; i < levelScoreTexts.Length; i++)
        {
            int levelIndex = i + 1;
            int bestScore = ScoreManager.Instance.GetScore(levelIndex);
            levelScoreTexts[i].text = $"{bestScore}";
        }
    }

    private void UpdateLevelButtonsState()
    {
        levelButtons[0].interactable = true;

        for (int i = 1; i < levelButtons.Length; i++)
        {
            int previousLevelScore = ScoreManager.Instance.GetScore(i);
            bool isUnlocked = previousLevelScore > 0;

            levelButtons[i].interactable = isUnlocked;

        }
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene($"Level {levelIndex}");
    }

    private void ResetAllScores()
    {
        ScoreManager.Instance.ResetAllScores();
        UpdateScoresDisplay();
        UpdateLevelButtonsState();
        Debug.Log("Все рекорды сброшены!");
    }
}