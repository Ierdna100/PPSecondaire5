using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startScene = "Level1";
    public GameObject tutorial;
    public GameObject thisMenu;

    private void Start()
    {
        tutorial.SetActive(false);
    }

    public void onQuit()
    {
        Application.Quit();
    }

    public void onStart()
    {
        SceneManager.LoadScene(startScene, LoadSceneMode.Single);
    }

    public void onTutorial()
    {
        thisMenu.SetActive(false);
        tutorial.SetActive(true);

    }

    public void onQuitTutorial()
    {
        thisMenu.SetActive(true);
        tutorial.SetActive(false);
    }
}
