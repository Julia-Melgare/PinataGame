using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneManager : MonoBehaviour
{
    private static int levelID = 1;

    void Start()
    {
        PlayerPrefs.DeleteAll();

        DontDestroyOnLoad(gameObject);

        levelID = PlayerPrefs.GetInt("Level", 1);
    }

    public void NextLevel()
    {
        levelID++;
        PlayerPrefs.SetInt("Level", levelID);
        if (levelID >= 3)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(levelID);
        }        
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(levelID);
    }
}
