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

  void OnEnable()
  {
    uiDocument = GetComponent<UIDocument>();

    if (uiDocument == null) return;

    playButton = uiDocument.rootVisualElement.Q<Button>("PlayButton");
    quitButton = uiDocument.rootVisualElement.Q<Button>("QuitButton");

    if (playButton != null) playButton.clicked += PlayGame;
    if (quitButton != null) quitButton.clicked += QuitGame;
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
}
