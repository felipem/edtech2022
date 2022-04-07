using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Persistence;

public class SubmittingBehaviour : UI_AbstractMenuBehaviour {

    [SerializeField] Text InfoText;
    [SerializeField] Text CloseButtonText;

    public MainMenuBehaviour MainBehaviour;
    private PersistenceManager persistenceManager;
    public bool Presurvey;

    
    // Use this for initialization
    void Start () {
        persistenceManager = FindObjectOfType<PersistenceManager>();
        CloseButtonText.text = MainMenuBehaviour.CurrentDictionary.GetKeyValue("cancel");
        //activate ths, iif you want to send  data
        InfoText.text = MainMenuBehaviour.CurrentDictionary.GetKeyValue("sending_data",false);
        Questionaire questionaire = MainBehaviour.GetCurrentQuestionaire();
        SaveSurveyToJson saveSurvey = new SaveSurveyToJson(questionaire);
        string survey = JsonUtility.ToJson(saveSurvey);

        //how many saves so far?
        int savedCount = 0;
        if (PlayerPrefs.HasKey("SavedCount"))
        {
            savedCount = PlayerPrefs.GetInt("SavedCount");
        }
        SaveLocal(survey, savedCount);

        if (Presurvey)
        {
            APIService.Instance.SendPresurvey(survey, (data) =>
            {
                savedCount++;
                PlayerPrefs.SetInt("SavedCount", savedCount);
                persistenceManager.InitialSurveyComplete = true;

                SceneManager.LoadScene("LoginUIScene", LoadSceneMode.Single);
            });
        }
        else
        {
            APIService.Instance.SendPostsurvey(survey, (data) =>
            {
                Application.Quit();
            });
        }
    }
        /// <summary>
        /// Save results as a json file  in persistant data path
        /// </summary>
        /// <param name="survey">Survey.</param>
        /// <param name="id">Identifier.</param>
    void SaveLocal(string survey, int id)
    {
        //save to disk 
        string fileName = "results" + id.ToString("D4");
        print("Filename: " + fileName);
        string filePath = Application.persistentDataPath + "/"+fileName + ".json";
        byte[] JsonStringBytes = Encoding.UTF8.GetBytes(survey);
        File.WriteAllBytes(filePath, JsonStringBytes);

        InfoText.text += "\n\n" + MainMenuBehaviour.CurrentDictionary.GetKeyValue("saved_locally", false);
       
    }

    /// <summary>
    /// enumerator, that pushes the data  to a server
    /// you need to configure that on your own, activate the coroutine in the start function
    /// </summary>
    /// <returns>The to server.</returns>
    /// <param name="survey">Survey.</param>
    /// <param name="id">Identifier.</param>
    IEnumerator PushToServer(string survey, int id)
    {
        survey = System.Uri.EscapeUriString(survey);
        WWWForm form = new WWWForm();
        form.AddField("key", "your authetication key");
        form.AddField("id", id);
        form.AddField("data", survey);

        string url = "http://your.webserver.com/rest";
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();
        print("response: " + www.downloadHandler.text);
        if (www.downloadHandler.text.Contains("success"))
        {
            InfoText.text += "\n\n" + MainMenuBehaviour.CurrentDictionary.GetKeyValue("sent_to_server", false);
            CloseButtonText.text = MainMenuBehaviour.CurrentDictionary.GetKeyValue("OK");
        }
        else
        {
            InfoText.text += "\n\n Sending Error"; 
        }

        MainBehaviour.ShowStartScreen();
    }
}
