using Persistence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using World;

public class MenuController : MonoBehaviour
{
    public GameObject playGameButton;
    public GameObject playAnywaysGameButton;
    public GameObject loginPanel;
    public GameObject surveyButton;

    [SerializeField] private WorldManager worldManager;
    [SerializeField] private GameObject loader;
    private PersistenceManager persistenceManager;
    // Start is called before the first frame update
    void Start()
    {
        persistenceManager = FindObjectOfType<PersistenceManager>();
        if (!persistenceManager.InitialSurveyComplete)
        {
            playGameButton.GetComponentInChildren<Button>().interactable = false;
            playAnywaysGameButton.GetComponentInChildren<Button>().interactable = true;
            surveyButton.SetActive(true);
        }
        else
        {
            playGameButton.GetComponentInChildren<Button>().interactable = true;
            playAnywaysGameButton.GetComponentInChildren<Button>().interactable = false;
            surveyButton.SetActive(false);
        }
    }
    public void PlayButtonOnClick()
    {
        playGameButton.GetComponentInChildren<Text>().text = "Retrieving...";
        ServerWorld world = worldManager.CreateWorld("Test World");

        // initialise
        // load main scene
        persistenceManager.SelectedWorld = world;
        loader.SetActive(true);
        SceneManager.LoadScene("BoardScene", LoadSceneMode.Single);
        SceneManager.sceneLoaded += (arg0, mode) => { if (loader != null) loader.SetActive(false); };
    }

    public void SurveyButtonOnClick()
    {
        surveyButton.GetComponentInChildren<Text>().text = "Retrieving...";
 
        loader.SetActive(true);
        SceneManager.LoadScene("SurveyScene", LoadSceneMode.Single);
        SceneManager.sceneLoaded += (arg0, mode) => { if (loader != null) loader.SetActive(false); };
    }

    public void ExitButtonOnClick()
    {
        loginPanel.SetActive(true);
        loginPanel.GetComponent<LoginPanelController>().TriggerLogout();
    }
}
