using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICanvasController : MonoBehaviour
{
    [SerializeField]
    private PlayerLogic player;

    [SerializeField]
    private List<UIHealthIcon> playerLives;

    [SerializeField]
    private EnemyLogic enemy;

    [SerializeField]
    private List<UIHealthIcon> enemyLives;

    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject victoryPanel;

    private void OnDisable()
    {
        player.onHealthChange -= UpdatePlayerHealthIcons;
        enemy.onHealthChange -= UpdateEnemyHealthIcons;
    }

    private void OnEnable()
    {
        player.onHealthChange += UpdatePlayerHealthIcons;
        enemy.onHealthChange += UpdateEnemyHealthIcons;
    }

    private void UpdatePlayerHealthIcons(int currentPlayerHealth)
    {
        for (int i = 0; i < playerLives.Count; i++)
        {
            playerLives[i].SetSprite(i >= currentPlayerHealth);
        }

        if (currentPlayerHealth <= 0)
        {
            ShowGameOverScreen();
        }
    }

    private void UpdateEnemyHealthIcons(int currentEnemyHealth)
    {
        for (int i = 0; i < enemyLives.Count; i++)
        {
            enemyLives[i].SetSprite(i >= currentEnemyHealth);
        }

        if (currentEnemyHealth <= 0)
        {
            ShowVictoryScreen();
        }
    }

    private void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
    }

    private void ShowVictoryScreen()
    {
        victoryPanel.SetActive(true);
    }

    public void RetryButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
