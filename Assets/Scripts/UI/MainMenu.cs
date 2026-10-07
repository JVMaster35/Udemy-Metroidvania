using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startScene;

    // Update is called once per frame
    void Update()
    {
        ResetLevels();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void ResetLevels()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Levels Reseted!");
        }
    }
}
