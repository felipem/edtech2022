using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelController : MonoBehaviour
{
    public Button LoginButton;
    public GameObject MenuPanel;
    private AuthHandler authHandler;

    // Start is called before the first frame update
    void Start()
    {
        authHandler = FindObjectOfType<AuthHandler>();

        InvalidateUI();
    }

    // Update is called once per frame
    private void InvalidateUI()
    {
        if (authHandler.CurrentAuth != null)
        {
            LoginButton.interactable = true;
            MenuPanel.SetActive(true);

            gameObject.SetActive(false);
        }
        else
        {
            LoginButton.interactable = true;
            MenuPanel.SetActive(false);
        }
    }

    public void TriggerLogout()
    {
        authHandler.Logout();
        InvalidateUI();
    }

    public void LoginAnonymousButtonClicked()
    {
        LoginButton.interactable = false;

        Text buttonText = LoginButton.GetComponentInChildren<Text>();

        string oldButtonText = buttonText.text;
        buttonText.text = "Logging in...";
        authHandler.LoginAnonymousUser((auth) =>
        {
            buttonText.text = oldButtonText;
            APIService.Instance.access_token = auth.IdToken;
            InvalidateUI();
        }, (error) =>
        {
            LoginButton.interactable = true;
            buttonText.text = "Login Failed. Try Again?";
        });
    }
}
