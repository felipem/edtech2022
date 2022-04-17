using Persistence;
using UnityEngine;
using World;
using World.Entities;

namespace Tutorial
{
    /**
        Wrapper manager class that can run a tutorial flow through the game utilising a dialoguemanager
     */
    public class TutorialManager : MonoBehaviour
    {
        public TutorialStep[] tutorialSteps; // Scriptable Objects.
        public bool tutorialActive;
        public GameObject tutorialCanvas;
        public GameBoard board;
        private DialogueManager dialogueManager;
        private int currentTutorialStep;
        private bool tutorialComplete = false;

        public bool TutorialComplete => tutorialComplete;
        public int CurrentTutorialStep => currentTutorialStep;
        private PersistenceManager persistenceManager;

        // Start is called before the first frame update
        private void Start()
        {
            dialogueManager = tutorialCanvas.GetComponent<DialogueManager>();
            persistenceManager = FindObjectOfType<PersistenceManager>();

            if (!persistenceManager.SelectedWorld.world.IsTutorialCompleted)
            {
                StartTutorial(0);
            }
            else
            {
                tutorialActive = false;
                tutorialCanvas.SetActive(false);
            }
        }

        // Update is called once per frame
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
                    dialogueManager.AppendDialogue(currentStep.SuccessMessage);
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
            tutorialCanvas.SetActive(true);
            currentTutorialStep = startingStep;
            DrawStep(currentTutorialStep);
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
            dialogueManager.StartDialogue(nextStep.Title, nextStep.Description);
            nextStep.OnStepBegin();
            InvalidateUI();
        }

        public void InvalidateUI()
        {
            TutorialStep currentStep = tutorialSteps[currentTutorialStep];

            dialogueManager.ContinueInteractable = currentStep.StepCompleted;
        }

        public void EndTutorial()
        {
            tutorialActive = false;
            tutorialCanvas.SetActive(false);
            tutorialComplete = true;
            persistenceManager.SelectedWorld.world.IsTutorialCompleted = true;
        }
    }
}