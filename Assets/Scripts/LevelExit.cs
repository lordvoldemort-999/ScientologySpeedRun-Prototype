using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
  [Header("Scene Navigation")]
  [Tooltip("The exact build name of your next scene layout.")]
  public string nextSceneName;

  private Collider2D levelCollider;
  private SpriteRenderer levelSprite;
  private HoverObject hoverScript; // Pulls the script we just made!

  void Awake()
  {
    levelCollider = GetComponent<Collider2D>();
    levelSprite = GetComponent<SpriteRenderer>();
    hoverScript = GetComponent<HoverObject>();

    // Start hidden and inactive
    HideExit();
  }

  private void HideExit()
  {
    if (levelCollider != null) levelCollider.enabled = false;
    if (levelSprite != null) levelSprite.enabled = false;
    if (hoverScript != null) hoverScript.enabled = false;
  }

  // This method will be called by your StartFight script when the boss dies
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
      Debug.Log($"[Exit] Player entered exit trigger. Loading: {nextSceneName}");

      // Make sure time flows normally in case anything was paused
      Time.timeScale = 1f;

      SceneManager.LoadScene(nextSceneName);
    }
  }
}