using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
  [Header("Scene Configuration")]
  public string gameSceneName = "MainGameLevel";

  private UIDocument uiDocument;
  private Button playButton;
  private Button quitButton;
  private Button controlsButton;

  private VisualElement controlsScreen;
  private Button controlsBackButton;

  void OnEnable()
  {
    uiDocument = GetComponent<UIDocument>();

    if (uiDocument == null) return;

    playButton = uiDocument.rootVisualElement.Q<Button>("PlayButton");
    quitButton = uiDocument.rootVisualElement.Q<Button>("QuitButton");
    controlsButton = uiDocument.rootVisualElement.Q<Button>("ControlsButton");

    controlsScreen = uiDocument.rootVisualElement.Q<VisualElement>("ControlsScreen");
    controlsBackButton = uiDocument.rootVisualElement.Q<Button>("ControlsBackButton");

    if (playButton != null) playButton.clicked += PlayGame;
    if (quitButton != null) quitButton.clicked += QuitGame;
    if (controlsButton != null) controlsButton.clicked += OpenControls;
    if (controlsBackButton != null) controlsBackButton.clicked += CloseControls;
  }

  void OnDisable()
  {
    if (playButton != null) playButton.clicked -= PlayGame;
    if (quitButton != null) quitButton.clicked -= QuitGame;
  }

  void PlayGame()
  {
    Debug.Log("UI Toolkit: Loading game level...");
    Time.timeScale = 1f;
    SceneManager.LoadScene(gameSceneName);
  }

  void QuitGame()
  {
    Debug.Log("UI Toolkit: Exiting application.");
    Application.Quit();
  }

  void OpenControls()
  {
    if (controlsScreen != null)
    {
      controlsScreen.style.display = DisplayStyle.Flex;
    }
  }

  void CloseControls()
  {
    if (controlsScreen != null)
    {
      controlsScreen.style.display = DisplayStyle.None;
    }
  }
}
