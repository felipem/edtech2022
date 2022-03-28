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
    public GameObject loginPanel;
    [SerializeField] private WorldManager worldManager;
    [SerializeField] private GameObject loader;
    private PersistenceManager persistenceManager;
    // Start is called before the first frame update
    void Start()
    {
        persistenceManager = FindObjectOfType<PersistenceManager>();
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

    public void ExitButtonOnClick()
    {
        loginPanel.SetActive(true);
        loginPanel.GetComponent<LoginPanelController>().TriggerLogout();
    }
}
