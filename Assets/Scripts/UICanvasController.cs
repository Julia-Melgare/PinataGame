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

    private void OnDisable()
    {
        player.onHealthChange -= UpdatePlayerHealthIcons;
    }

    private void OnEnable()
    {
        player.onHealthChange += UpdatePlayerHealthIcons;
    }

    private void UpdatePlayerHealthIcons(int currentHealth)
    {
        for (int i = 0; i < playerLives.Count; i++)
        {
            playerLives[i].SetSprite(i >= currentHealth);
        }
    }
}
