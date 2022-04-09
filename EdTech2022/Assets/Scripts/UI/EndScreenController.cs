using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using World.Tiles;

public class EndScreenController : MonoBehaviour
{
    [SerializeField] private GameObject winMenuUI;
    [SerializeField] private GameObject loseMenuUI;
    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private GameObject loader;

    private EscapeController escapeController;

    private List<GameObject> elementsOff;

    // Start is called before the first frame update
    private void Start()
    {
        escapeController = gameObject.GetComponent<EscapeController>();

        winMenuUI.SetActive(false);
    }

    public void EnableWinScreen()
    {
        elementsOff = new List<GameObject>();
        foreach (GameObject obj in uiElements)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                elementsOff.Add(obj);
            }
        }

        escapeController.enabled = false;
        winMenuUI.SetActive(true);
        Tile.highlightEnabled = false;
    }

    public void DisableWinScreen()
    {
        foreach (GameObject obj in elementsOff)
        {
            obj.SetActive(true);
        }

        elementsOff = new List<GameObject>();
        //blurComponent.enabled = false;
        escapeController.enabled = true;
        winMenuUI.SetActive(false);
        Tile.highlightEnabled = true;
    }

    public void EnableLoseScreen()
    {
        elementsOff = new List<GameObject>();
        foreach (GameObject obj in uiElements)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                elementsOff.Add(obj);
            }
        }

        escapeController.enabled = false;
        loseMenuUI.SetActive(true);
        //blurComponent.enabled = true;
        Tile.highlightEnabled = false;
    }

    public void DisableLoseScreen()
    {
        // redisplay hidden ui elements
        foreach (GameObject obj in elementsOff)
        {
            obj.SetActive(true);
        }

        elementsOff = new List<GameObject>();
        //blurComponent.enabled = false;
        escapeController.enabled = true;
        loseMenuUI.SetActive(false);
        Tile.highlightEnabled = true;
    }

    public void WinContinueButtonOnClick()
    {
        // redisplay hidden ui elements
        foreach (GameObject obj in elementsOff)
        {
            obj.SetActive(true);
        }

        elementsOff = new List<GameObject>();
        DisableWinScreen();
    }

    public void MainMenuButtonOnClick()
    {
        // show spinner/loader and switch scenes
        SceneManager.LoadScene("LoginUIScene", LoadSceneMode.Single);
        loader.SetActive(true);
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            loader.SetActive(false);
        };
    }

    public void MainMenuButtonSurveyOnClick()
    {
        // show spinner/loader and switch scenes
        SceneManager.LoadScene("EndSurveyScene", LoadSceneMode.Single);
        loader.SetActive(true);
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            loader.SetActive(false);
        };
    }
}