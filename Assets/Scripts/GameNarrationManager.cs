using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class GameNarrationManager : MonoBehaviour
{
  [Header("Narration Settings")]
  [TextArea(3, 5)]
  public string objectiveText = "SCIENTOLOGY SPEED RUN\nThere is a new trend on the internet to run through and map Scientology buildings.\nEvade the pursuers and defeat the floor boss. Don't die.\n\nGood luck.";

  [Tooltip("Seconds per letter for a typewriter effect")]
  public float typeSpeed = 0.03f;

  [Header("Audio Settings")]
  [Tooltip("Sound effect that plays for every letter typed.")]
  public AudioClip typeSoundFX;
  [Range(0f, 1f)] public float typeVolume = 0.5f;

  [Header("Scripts to Wake Up Later")]
  public MonoBehaviour gameManagerScript;

  private UIDocument uiDocument;
  private VisualElement narrationScreen;
  private VisualElement playerHudContainer;
  private Label narrationLabel;
  private Button continueButton;
  private bool isTyping = false;

  // Dialogue State Arrays
  private string[] dialogueLines;
  private int currentLineIndex = 0;
  private System.Action onDialogueCompleteCallback;

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

    // Start the intro narration sequence
    if (narrationLabel != null) StartCoroutine(TypeGenericString(objectiveText));
  }

  void OnDisable()
  {
    if (continueButton != null)
    {
      continueButton.clicked -= StartGameplay;
      continueButton.clicked -= AdvanceDialogue;
    }
  }

  IEnumerator TypeGenericString(string fullText)
  {
    isTyping = true;
    narrationLabel.text = "";

    foreach (char letter in fullText.ToCharArray())
    {
      narrationLabel.text += letter;

      // Audio tick execution
      if (AudioManager.Instance != null && typeSoundFX != null)
      {
        AudioManager.Instance.PlaySFX(typeSoundFX, typeVolume);
      }

      yield return new WaitForSecondsRealtime(typeSpeed);
    }

    isTyping = false;

    if (continueButton != null)
    {
      continueButton.style.display = DisplayStyle.Flex;
    }
  }

  public void StartPreFightDialogue(string[] lines, System.Action onComplete)
  {
    Time.timeScale = 0f;

    dialogueLines = lines;
    currentLineIndex = 0;
    onDialogueCompleteCallback = onComplete;

    if (narrationScreen != null) narrationScreen.style.display = DisplayStyle.Flex;
    if (playerHudContainer != null) playerHudContainer.style.display = DisplayStyle.None;
    if (continueButton != null) continueButton.style.display = DisplayStyle.None;

    if (continueButton != null)
    {
      continueButton.clicked -= StartGameplay; // Safely strip menu routing
      continueButton.clicked += AdvanceDialogue; // Append script dialogue navigation
    }

    StartNextDialogueLine();
  }

  void StartNextDialogueLine()
  {
    if (continueButton != null) continueButton.style.display = DisplayStyle.None;
    StartCoroutine(TypeGenericString(dialogueLines[currentLineIndex]));
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
      if (continueButton != null) continueButton.clicked -= AdvanceDialogue;
      if (narrationScreen != null) narrationScreen.style.display = DisplayStyle.None;

      Time.timeScale = 1f;
      onDialogueCompleteCallback?.Invoke();
    }
  }

  void StartGameplay()
  {
    if (isTyping) return;

    if (continueButton != null) continueButton.clicked -= StartGameplay;
    if (narrationScreen != null) narrationScreen.style.display = DisplayStyle.None;
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