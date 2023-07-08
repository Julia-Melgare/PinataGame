using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void UpdatePlayerHealthIcons(int currentHealth)
    {
        for (int i = 0; i < playerLives.Count; i++)
        {
            playerLives[i].SetSprite(i >= currentHealth);
        }
    }

    private void UpdateEnemyHealthIcons(int currentHealth)
    {
        for (int i = 0; i < enemyLives.Count; i++)
        {
            enemyLives[i].SetSprite(i >= currentHealth);
        }
    }
}
