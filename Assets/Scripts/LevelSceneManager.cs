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

    }

    void Update()
    {
        if(Input.GetKey(KeyCode.R)) LoadScene(levelID);
    }

    public void NextLevel()
    {
        levelID++;
        PlayerPrefs.SetInt("Level", levelID);
        PlayerPrefs.Save();
        if (levelID >= 4)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log(levelID);
            SceneManager.LoadScene(levelID);
        }        
    }

    public void LoadScene(int ID)
    {
        SceneManager.LoadScene(ID);
        levelID = ID;
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level", levelID));
    }
}
