using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        int currentScore = PlayerExperience.Instance.GetExperience();
        scoreText.text = $"¬аш счЄт: {currentScore}";
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}