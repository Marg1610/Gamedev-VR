using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        // Находим LevelController в сцене
        LevelController levelController = FindObjectOfType<LevelController>();

        if (levelController != null)
        {
            Debug.Log("Уровень завершен! Сохраняем результат...");
            levelController.FinishLevel();
        }
        else
        {
            Debug.LogError("LevelController не найден на сцене! Добавьте его на любой объект уровня.");
        }
    }
}