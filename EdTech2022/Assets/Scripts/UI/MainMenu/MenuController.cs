using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject worldsPanel;

    public GameObject viewWorldPanel;

    public GameObject playGameButton;
    public GameObject loginPanel;

    // Start is called before the first frame update
    void Start()
    {
        worldsPanel.SetActive(false);
        viewWorldPanel.SetActive(false);
        
    }
    public void PlayButtonOnClick()
    {
        playGameButton.GetComponentInChildren<Text>().text = "Retrieving...";
        worldsPanel.GetComponent<WorldsPanelController>().PopulateWorldsList(() =>
        {
            viewWorldPanel.SetActive(false);
            worldsPanel.SetActive(true);
            playGameButton.GetComponentInChildren<Text>().text = "Play Game";
        });
    }
    public void OpenViewWorldModal()
    {
        worldsPanel.SetActive(false);
        viewWorldPanel.SetActive(true);
    }

    public void ExitButtonOnClick()
    {
        loginPanel.SetActive(true);
        loginPanel.GetComponent<LoginPanelController>().TriggerLogout();
    }
}
