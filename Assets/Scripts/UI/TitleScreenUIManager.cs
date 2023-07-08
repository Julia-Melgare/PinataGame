using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialPanel;

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
