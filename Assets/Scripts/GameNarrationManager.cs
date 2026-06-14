using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class GameNarrationManager : MonoBehaviour
{
  [Header("Narration Settings")]
  [TextArea(3, 5)]
  public string objectiveText = "SCIENTOLOGY SPEED RUN\nThere is a new trend on the internet to run through and map Scientology buildings.\nEvade the pursuers and defeat the floor boss. Don't die.\n\nGood luck.";

  public float typeSpeed = 0.03f; // Seconds per letter for a typewriter effect

  [Header("Scripts to Wake Up Later")]
  public MonoBehaviour gameManagerScript; // Drag your main GameManager script here

  private UIDocument uiDocument;
  private VisualElement narrationScreen;
  private Label narrationLabel;
  private Button continueButton;
  private bool isTyping = false;

  void Awake()
  {
    // 1. Freeze time completely before Awake/Start runs on other game entities
    Time.timeScale = 0f;
  }

  void OnEnable()
  {
    uiDocument = GetComponent<UIDocument>();
    if (uiDocument == null) return;

    // 2. Query the document structure using our UI Builder Name IDs
    narrationScreen = uiDocument.rootVisualElement.Q<VisualElement>("NarrationScreen");
    narrationLabel = uiDocument.rootVisualElement.Q<Label>("NarrationText");
    continueButton = uiDocument.rootVisualElement.Q<Button>("ContinueButton");

    // Hide the continue button initially so they have to read
    if (continueButton != null)
    {
      continueButton.style.display = DisplayStyle.None;
      continueButton.clicked += StartGameplay;
    }

    // 3. Start displaying the objectives
    if (narrationLabel != null)
    {
      StartCoroutine(TypeText(objectiveText));
    }
  }

  void OnDisable()
  {
    if (continueButton != null) continueButton.clicked -= StartGameplay;
  }

  IEnumerator TypeText(string fullText)
  {
    isTyping = true;
    narrationLabel.text = "";

    // Build the objective sequence letter by letter smoothly
    foreach (char letter in fullText.ToCharArray())
    {
      narrationLabel.text += letter;
      // Use Realtime since normal Time.timeScale is frozen at 0
      yield return new WaitForSecondsRealtime(typeSpeed);
    }

    isTyping = false;

    // Reveal the start button once text finishes printing
    if (continueButton != null)
    {
      continueButton.style.display = DisplayStyle.Flex;
    }
  }

  void StartGameplay()
  {
    if (isTyping) return;

    // 1. Clear the overlay text container
    if (narrationScreen != null)
    {
      narrationScreen.style.display = DisplayStyle.None;
    }

    // 2. Unfreeze physics time step FIRST
    Time.timeScale = 1f;

    // 3. Flip the switch. Waking this up automatically triggers its OnEnable()!
    if (gameManagerScript != null)
    {
      gameManagerScript.enabled = true;
      Debug.Log("GameManager flipped on. OnEnable() spawning sequence engaged!");
    }
    else
    {
      Debug.LogError("GameNarrationManager is missing the reference to the GameManager script component!");
    }

    enabled = false;
  }
}