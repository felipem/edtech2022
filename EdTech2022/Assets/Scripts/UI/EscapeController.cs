using System.Collections.Generic;

using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using World;
using World.Tiles;

public class EscapeController : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private GameObject loader;
    [SerializeField] private GameObject escapeUIObj;

    private List<GameObject> elementsOff;

    //private PostProcessingBehaviour blurComponent;
    private bool gameIsPaused = false;
    public bool GameIsPaused => gameIsPaused;
    private GameBoard board;

    // Start is called before the first frame update
    private void Start()
    {
        escapeUIObj.SetActive(false);
        board = FindObjectOfType<GameBoard>();
        InvalidateSharingUI();
    }

    // Update is called once per frame
    private void Update()
    {
        // handle pause/resume on escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameIsPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    void Pause()
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

        //blurComponent.enabled = true;
        gameIsPaused = true;
        Time.timeScale = 0f;
        escapeUIObj.SetActive(true);
        Tile.highlightEnabled = false;
    }

    void Resume()
    {
        foreach (GameObject obj in elementsOff)
        {
            obj.SetActive(true);
        }

        elementsOff = new List<GameObject>();
        //blurComponent.enabled = false;
        gameIsPaused = false;
        Time.timeScale = 1f;
        escapeUIObj.SetActive(false);
        Tile.highlightEnabled = true;
    }

    public void SaveButtonOnClick()
    {
        board.SaveActiveWorld();
    }

    public void InvalidateSharingUI()
    {
        if (board.ActiveWorld == null)
        {
            return;
        }
    }

    public void BackButtonSurveyOnClick()
    {
        board.SaveActiveWorld();
        // load main scene
        Resume();
        board.ActiveWorld = null;
        SceneManager.LoadScene("EndSurveyScene", LoadSceneMode.Single);
        loader.SetActive(true);
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            loader.SetActive(false);
        };
    }

    public void BackButtonOnClick()
    {
        board.SaveActiveWorld();

        // load main scene
        Resume();
        board.ActiveWorld = null;
        SceneManager.LoadScene("LoginUIScene", LoadSceneMode.Single);
        loader.SetActive(true);
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (loader!= null)
                loader.SetActive(false);
        };
    }

    public void ResumeButtonOnClick()
    {
        Resume();
    }
}