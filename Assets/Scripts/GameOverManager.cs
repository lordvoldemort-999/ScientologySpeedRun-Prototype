using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
  [Header("Scene Routing")]
  public string mainMenuSceneName = "MainMenu";

  private UIDocument uiDocument;
  private VisualElement gameOverScreen;
  private VisualElement playerHudContainer;
  private Button retryButton;
  private Button menuButton;

  void OnEnable()
  {
    uiDocument = GetComponent<UIDocument>();
    if (uiDocument == null) return;

    gameOverScreen = uiDocument.rootVisualElement.Q<VisualElement>("GameOverScreen");
    playerHudContainer = uiDocument.rootVisualElement.Q<VisualElement>("PlayerHUD");
    retryButton = uiDocument.rootVisualElement.Q<Button>("RetryButton");
    menuButton = uiDocument.rootVisualElement.Q<Button>("MainMenuButton");

    if (gameOverScreen != null)
    {
      gameOverScreen.style.display = DisplayStyle.None;
    }

    if (retryButton != null) retryButton.clicked += ReloadLevel;
    if (menuButton != null) menuButton.clicked += LoadMainMenu;
  }

  void OnDisable()
  {
    if (retryButton != null) retryButton.clicked -= ReloadLevel;
    if (menuButton != null) menuButton.clicked -= LoadMainMenu;
  }

  // method to be called when player gets caught
  public void SetupGameOverDisplay()
  {
    if (playerHudContainer != null) playerHudContainer.style.display = DisplayStyle.None;
    if (gameOverScreen != null) gameOverScreen.style.display = DisplayStyle.Flex;
  }

  void ReloadLevel()
  {
    Debug.Log("Restarting office encounter...");
    Time.timeScale = 1f;

    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  void LoadMainMenu()
  {
    Debug.Log("Returning to main menu layout...");
    SceneManager.LoadScene(mainMenuSceneName);
    Time.timeScale = 1f;
  }
}