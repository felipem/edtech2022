using Persistence;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class DialogueManager : MonoBehaviour
    {
        public TutorialStep[] tutorialSteps; // Scriptable Objects.
        public bool tutorialActive;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Button continueButton;
        [SerializeField] private GameObject dialogPanel;
        private int currentTutorialStep;
        private bool tutorialComplete = false;

        private string fullDescriptionText;
        public bool TutorialComplete => tutorialComplete;
        public int CurrentTutorialStep => currentTutorialStep;
        private PersistenceManager persistenceManager;
        private void Start()
        {            
            persistenceManager = FindObjectOfType<PersistenceManager>();

            if (!persistenceManager.SelectedWorld.world.IsTutorialCompleted)
            {
                StartTutorial(0);
            }
            else
            {
                tutorialActive = false;
                dialogPanel.SetActive(false);
            }
        }
        private void Update()
        {
            if (!tutorialActive || currentTutorialStep >= tutorialSteps.Length)
            {
                return;
            }

            TutorialStep currentStep = tutorialSteps[currentTutorialStep];
            if (!currentStep.StepCompleted)
            {
                currentStep.Update();

                if (currentStep.StepCompleted)
                {
                    AppendDialogue(currentStep.SuccessMessage);
                    InvalidateUI();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                NextStep();
            }
        }
        public bool TutorialActive => tutorialActive;
        public void StartTutorial(int startingStep)
        {
            tutorialComplete = false;
            tutorialActive = true;
            dialogPanel.SetActive(true);
            currentTutorialStep = startingStep;
            DrawStep(currentTutorialStep);
        }
        public void StartDialogue(string titleText, string descriptionText)
        {
            // populate dialogue panel
            Debug.Log("Starting conversation  " + titleText);            
            fullDescriptionText = descriptionText;
            ContinueInteractable = false;

            DisplayNextSentence(descriptionText);
        }
        public void NextStep()
        {
            TutorialStep currentStep = tutorialSteps[currentTutorialStep];
            currentStep.OnStepEnd();
            currentTutorialStep++;
            if (currentTutorialStep >= tutorialSteps.Length)
            {
                EndTutorial();
                return;
            }

            DrawStep(currentTutorialStep);
        }
        private void DrawStep(int step)
        {
            TutorialStep nextStep = tutorialSteps[step];
            StartDialogue(nextStep.Title, nextStep.Description);
            nextStep.OnStepBegin();
            InvalidateUI();
        }
        public void InvalidateUI()
        {
            TutorialStep currentStep = tutorialSteps[currentTutorialStep];

            ContinueInteractable = currentStep.StepCompleted;
        }
        public void EndTutorial()
        {
            tutorialActive = false;
            dialogPanel.SetActive(false);
            tutorialComplete = true;
            persistenceManager.SelectedWorld.world.IsTutorialCompleted = true;
        }

        public void AppendDialogue(string textToAppend)
        {
            descriptionText.text = fullDescriptionText;
            descriptionText.text += "\n" + textToAppend;
        }

        public void DisplayNextSentence(string description)
        {
            descriptionText.text = description;
        }

        public bool ContinueInteractable
        {
            get => continueButton.interactable;
            set => continueButton.interactable = value;
        }


        IEnumerator TypeSentence(string sentence)
        {
            // animate word typing in dialogue box
            foreach (var letter in sentence.ToCharArray())
            {
                descriptionText.text += letter;
                yield return null;
            }
        }

        public void FinishTyping()
        {
            // show the full description if skipped
            StopAllCoroutines();
            descriptionText.text = fullDescriptionText;
        }
    }
}