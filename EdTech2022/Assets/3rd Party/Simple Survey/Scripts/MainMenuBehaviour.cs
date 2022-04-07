﻿
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBehaviour : MonoBehaviour {

    /// <summary>
    /// The two questionaires as Text asset in json
    /// </summary>
    [SerializeField] TextAsset MainQuestionaire;

    [SerializeField] TextAsset LanguageDictionary;

    //Containers to put the questins in
    /// <summary>
    /// Put in the QuestionTask here
    /// </summary>
    [SerializeField] Text QuestionTaskTextElement;

    /// <summary>
    /// put all elements in here
    /// </summary>
    [SerializeField] RectTransform QuestionElementsContainer;

    //Question Buttons
    [SerializeField] GameObject NextButton;
    [SerializeField] GameObject PreviousButton;
    [SerializeField] GameObject SubmitButton;
    [SerializeField] GameObject CancelButton;
    [SerializeField] GameObject MainCancelButton;

    // Panels
    [SerializeField] GameObject MainPanel;
    [SerializeField] GameObject QuestionPanel;

    [SerializeField] QuestionBehaviour questionBehaviour;

    [SerializeField] GameObject SubmitPanelPrefab;
    [SerializeField] GameObject QuestionNotAnsweredPanelPrefab;

    [SerializeField] bool Presurvey;

    Questionaire CurrentQuestionaire = new Questionaire();

    public static LanguageDictionary CurrentDictionary;

    public Questionaire GetCurrentQuestionaire()
    {
        return CurrentQuestionaire;
    }
    int QuestionIndex;

    // Use this for initialization
    void Start () {

        SetLanguage(LanguageDictionary.text);
        ShowStartScreen();       
    }

    public void ShowStartScreen()
    {
        MainPanel.SetActive(true);
        QuestionPanel.SetActive(false);
        MainCancelButton.SetActive(false);
    }

    void StartQuestionaire()
    {
        //get Questionaire through serializer
        CurrentQuestionaire = JsonUtility.FromJson<Questionaire>(MainQuestionaire.text);
        ////Set question Index to zero
        QuestionIndex = 0;

        //Load first Question
        LoadQuestion(0);

        //Switch visibility of Panels

        MainPanel.SetActive(false);
        QuestionPanel.SetActive(true);
        MainCancelButton.SetActive(true);
    }

    void LoadQuestion(int index)
    {
   
        Question question= CurrentQuestionaire.GetQuestion(index);
        if (question!= null) {
            questionBehaviour.LoadQuestion(question);
        }
        else
        {
            print("Question not found! " + index);
            ShowStartScreen();
        }
        ButtonVisibility();
    }

    public void NextQuestion()
    {
        if (CheckQuestionSolved(CurrentQuestionaire.GetQuestion(QuestionIndex)))
        {
            QuestionIndex++;
           LoadQuestion(QuestionIndex);
        }
        else
        {
            Instantiate(QuestionNotAnsweredPanelPrefab);
        }
    }

    bool CheckQuestionSolved(Question question)
    {
        return question.IsQuestionSolved();
    }

    public void PreviousQuestion()
    {
        QuestionIndex--;
        LoadQuestion(QuestionIndex);
    }

    void ButtonVisibility()
    {
        //Check if this is the last question, then dispay submit button!
        bool chancelQuestionaire = QuestionIndex == 0;
        CancelButton.SetActive(chancelQuestionaire);
        PreviousButton.SetActive(!chancelQuestionaire);

        //Check if this is the last question, then dispay submit button!
        bool submit = QuestionIndex == CurrentQuestionaire.GetQuestionCount() - 1;
        SubmitButton.SetActive(submit);
        NextButton.SetActive(!submit);
    }

    public void ChancelQuestionaire()
    {
        ShowStartScreen();
    }

    public void SubmitQuestionaire()
    {
        if (CheckQuestionSolved(CurrentQuestionaire.GetQuestion(QuestionIndex)))
        {
            GameObject SubmitPanel = Instantiate(SubmitPanelPrefab);
            SubmittingBehaviour submittingBehaviour = SubmitPanel.GetComponentInChildren<SubmittingBehaviour>();
            submittingBehaviour.Presurvey = Presurvey;
            submittingBehaviour.MainBehaviour = this;
        }
        else
        {
            Instantiate(QuestionNotAnsweredPanelPrefab);
        }
    }

  public  void StartSurvey()
    {
        StartQuestionaire();
    }

    void SetLanguage(string languageDictionary)
    {
        CurrentDictionary = JsonUtility.FromJson<LanguageDictionary>(languageDictionary);
        SubmitButton.GetComponentInChildren<Text>().text = CurrentDictionary.GetKeyValue("submit");
        NextButton.GetComponentInChildren<Text>().text = CurrentDictionary.GetKeyValue("next");
        PreviousButton.GetComponentInChildren<Text>().text = CurrentDictionary.GetKeyValue("previous");
        CancelButton.GetComponentInChildren<Text>().text = CurrentDictionary.GetKeyValue("cancel");
    }

}
