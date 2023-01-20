using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject escapeMenu;
    public GameObject tutorial;
    public TrainUIController trainMenu;
    public Camera camera;
    public string mainScene = "MainMenu";

    private void Start()
    {
        escapeMenu.SetActive(false);
        tutorial.SetActive(false);
        trainMenu.gameObject.SetActive(false);
        trainMenu.trainIDText.text = "Vous êtes en train de contrôler:\nAucun train";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleEscapeMenu();

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hit = Physics2D.CircleCast(camera.ScreenToWorldPoint(Input.mousePosition), 0.5f, Vector2.right, 0.05f);

            if (hit.collider == null)
            {
                SetTrainMenu(false, null);
            } else if (hit.collider.gameObject.GetComponent<Train>() != null)
            {
                SetTrainMenu(true, hit.collider.gameObject.GetComponent<Train>());
            } else if (hit.collider.gameObject.GetComponent<Switch>() != null) 
            {
                hit.collider.gameObject.GetComponent<Switch>().TogglePoints();
            }
        }
    }

    public void ToggleEscapeMenu()
    {
        escapeMenu.SetActive(!escapeMenu.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(mainScene, LoadSceneMode.Single);
    }

    public void SetTrainMenu(bool status, Train trainBeingUpdated)
    {
        trainMenu.gameObject.SetActive(status);
        if (status)
        {
            trainMenu.UpdateTrainMenu(trainBeingUpdated.parentTrain);
        } else
        {
            trainMenu.trainIDText.text = "Vous êtes en train de contrôler:\nAucun train";
        }
    }

    public void onTutorial()
    {
        escapeMenu.SetActive(false);
        tutorial.SetActive(true);
    }

    public void onQuitTutorial()
    {
        escapeMenu.SetActive(true);
        tutorial.SetActive(false);
    }
}
