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
  private VisualElement playerHudContainer;
  private Label narrationLabel;
  private Button continueButton;
  private bool isTyping = false;

  void Awake()
  {
    Time.timeScale = 0f;

    if (gameManagerScript != null)
    {
      gameManagerScript.enabled = false;
      Debug.Log("[Narration] Successfully put GameManager to sleep for the intro.");
    }
    else
    {
      Debug.LogError("[Narration] Game Manager Script slot is empty! Please link it in the inspector.");
    }
  }
  void OnEnable()
  {
    uiDocument = GetComponent<UIDocument>();
    if (uiDocument == null) return;

    narrationScreen = uiDocument.rootVisualElement.Q<VisualElement>("NarrationScreen");
    playerHudContainer = uiDocument.rootVisualElement.Q<VisualElement>("PlayerHUD");
    narrationLabel = uiDocument.rootVisualElement.Q<Label>("NarrationText");
    continueButton = uiDocument.rootVisualElement.Q<Button>("ContinueButton");

    VisualElement enemyHudContainer = uiDocument.rootVisualElement.Q<VisualElement>("EnemyHUD");
    if (enemyHudContainer != null) enemyHudContainer.style.display = DisplayStyle.None;

    if (playerHudContainer != null) playerHudContainer.style.display = DisplayStyle.None;

    if (continueButton != null)
    {
      continueButton.style.display = DisplayStyle.None;
      continueButton.clicked += StartGameplay;
    }

    if (narrationLabel != null) StartCoroutine(TypeText(objectiveText));
  }

  void OnDisable()
  {
    if (continueButton != null) continueButton.clicked -= StartGameplay;
  }

  IEnumerator TypeText(string fullText)
  {
    isTyping = true;
    narrationLabel.text = "";

    foreach (char letter in fullText.ToCharArray())
    {
      narrationLabel.text += letter;
      yield return new WaitForSecondsRealtime(typeSpeed);
    }

    isTyping = false;

    if (continueButton != null)
    {
      continueButton.style.display = DisplayStyle.Flex;
    }
  }

  // --- ADDED FOR PRE-FIGHT DIALOGUE ---
  private string[] dialogueLines;
  private int currentLineIndex = 0;
  private System.Action onDialogueCompleteCallback;

  public void StartPreFightDialogue(string[] lines, System.Action onComplete)
  {
    // Freeze gameplay vectors
    Time.timeScale = 0f;

    dialogueLines = lines;
    currentLineIndex = 0;
    onDialogueCompleteCallback = onComplete;

    // Show the narration overlay screen canvas box
    if (narrationScreen != null) narrationScreen.style.display = DisplayStyle.Flex;
    if (playerHudContainer != null) playerHudContainer.style.display = DisplayStyle.None;
    if (continueButton != null) continueButton.style.display = DisplayStyle.None;

    // Hook into a modified continue loop or handle the clicks locally
    if (continueButton != null)
    {
      continueButton.clicked -= StartGameplay; // Unhook standard intro hook
      continueButton.clicked += AdvanceDialogue;
    }

    StartNextDialogueLine();
  }

  void StartNextDialogueLine()
  {
    if (continueButton != null) continueButton.style.display = DisplayStyle.None;
    StartCoroutine(TypeDialogueLine(dialogueLines[currentLineIndex]));
  }

  IEnumerator TypeDialogueLine(string line)
  {
    isTyping = true;
    narrationLabel.text = "";

    foreach (char letter in line.ToCharArray())
    {
      narrationLabel.text += letter;
      yield return new WaitForSecondsRealtime(typeSpeed);
    }

    isTyping = false;
    if (continueButton != null) continueButton.style.display = DisplayStyle.Flex;
  }

  void AdvanceDialogue()
  {
    if (isTyping) return;

    currentLineIndex++;

    if (currentLineIndex < dialogueLines.Length)
    {
      StartNextDialogueLine();
    }
    else
    {
      // Clean up conversation hooks
      if (continueButton != null) continueButton.clicked -= AdvanceDialogue;
      if (narrationScreen != null) narrationScreen.style.display = DisplayStyle.None;

      // Resume engine timeline and trigger our callback
      Time.timeScale = 1f;
      onDialogueCompleteCallback?.Invoke();
    }
  }

  void StartGameplay()
  {
    if (isTyping) return;

    if (narrationScreen != null)
    {
      narrationScreen.style.display = DisplayStyle.None;
    }

    if (playerHudContainer != null) playerHudContainer.style.display = DisplayStyle.Flex;

    Time.timeScale = 1f;

    if (gameManagerScript != null)
    {
      gameManagerScript.enabled = true;
      Debug.Log("GameManager flipped on. OnEnable() spawning sequence engaged!");
    }

    enabled = false;
  }
}