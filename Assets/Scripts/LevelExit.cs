using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
  [Header("Scene Navigation")]
  [Tooltip("The exact build name of your next scene layout.")]
  public string nextSceneName;

  [Header("End of Game Narration")]
  public bool isGameEndingExit = false;

  [TextArea(3, 5)]
  public string endingNarrationText = "";

  private Collider2D levelCollider;
  private SpriteRenderer levelSprite;
  private HoverObject hoverScript;

  void Awake()
  {
    levelCollider = GetComponent<Collider2D>();
    levelSprite = GetComponent<SpriteRenderer>();
    hoverScript = GetComponent<HoverObject>();

    HideExit();
  }

  private void HideExit()
  {
    if (levelCollider != null) levelCollider.enabled = false;
    if (levelSprite != null) levelSprite.enabled = false;
    if (hoverScript != null) hoverScript.enabled = false;
  }

  public void RevealExit()
  {
    Debug.Log("[Exit] Enemy cleared! Activating level exit portal.");

    if (levelCollider != null) levelCollider.enabled = true;
    if (levelSprite != null) levelSprite.enabled = true;

    // Turn on the smooth float mechanics we built earlier
    if (hoverScript != null)
    {
      hoverScript.enabled = true;
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      Time.timeScale = 1f;

      if (isGameEndingExit)
      {
        GameNarrationManager narrationManager = Object.FindAnyObjectByType<GameNarrationManager>();

        if (narrationManager != null)
        {
          Debug.Log("[Exit] Triggering final victory narration screen layout.");
          string[] lines = new string[] { endingNarrationText };
          narrationManager.StartPreFightDialogue(lines, LoadTargetScene);
          if (levelSprite != null) levelSprite.enabled = false;
          if (levelCollider != null) levelCollider.enabled = false;
        }
        else
        {
          LoadTargetScene();
        }
      }
      else
      {
        LoadTargetScene();
      }
    }
  }

  private void LoadTargetScene()
  {
    Debug.Log($"[Exit] Player completed step routing. Loading target layout: {nextSceneName}");
    Time.timeScale = 1f;
    SceneManager.LoadScene(nextSceneName);
  }
}