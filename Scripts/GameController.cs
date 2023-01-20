using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int score;
    public int time;
    public TrainUIController trainUIController;
    public UIManager uiManager;
    public RouteManager routeManager;
    public CameraController cameraController;
    public string mainScene;
    public string nextScene;
    public GameObject escapeMenu;
    public GameUI gameMenu;
    public EndGameManager endMenu;

    private void Start()
    {
        endMenu.gameObject.SetActive(false);
    }

    public void EndGame(float time)
    {
        escapeMenu.SetActive(false);
        gameMenu.gameObject.SetActive(false);
        trainUIController.gameObject.SetActive(false);

        endMenu.score.text = "Score: " + gameMenu.score;
        endMenu.time.text = "Temps: " + Mathf.Floor(time / 60) + ":" + Mathf.Floor(time % 60);

        endMenu.gameObject.SetActive(true);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(mainScene, LoadSceneMode.Single);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
