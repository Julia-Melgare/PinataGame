using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialPanel;

    [SerializeField]
    private AudioSource pop;

    public void ShowTutorial()
    {
        pop.Play();
        tutorialPanel.SetActive(true);
    }

    public void HideTutorial()
    {
        pop.Play();
        tutorialPanel.SetActive(false);
    }

    public void PlayButton()
    {
        pop.Play();
        SceneManager.LoadScene("Gameplay");
    }
}
