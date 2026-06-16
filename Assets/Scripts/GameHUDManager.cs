using UnityEngine;
using UnityEngine.UIElements;

public class GameHUDManager : MonoBehaviour
{
  public static GameHUDManager Instance { get; private set; }
  public VisualElement playerHudContainer { get; private set; }

  [Header("Sprite Assets for Pause Button")]
  public Sprite pauseIcon;
  public Sprite playIcon;

  private UIDocument uiDocument;
  private Label distanceLabel;
  private ProgressBar playerHealthBar;
  private Button pauseButton;

  // Enemy HUD Elements
  private VisualElement enemyHudContainer;
  private ProgressBar enemyHealthBar;

  private bool isPaused = false;

  private VisualElement[] weaponSlots = new VisualElement[3];

  void Awake()
  {
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
  }

  void OnEnable()
  {
    uiDocument = GetComponent<UIDocument>();
    if (uiDocument == null) return;

    distanceLabel = uiDocument.rootVisualElement.Q<Label>("DistanceText");
    playerHealthBar = uiDocument.rootVisualElement.Q<ProgressBar>("PlayerHealthBar");
    pauseButton = uiDocument.rootVisualElement.Q<Button>("PauseButton");
    playerHudContainer = uiDocument.rootVisualElement.Q<VisualElement>("PlayerHUD");

    enemyHudContainer = uiDocument.rootVisualElement.Q<VisualElement>("EnemyHUD");
    enemyHealthBar = uiDocument.rootVisualElement.Q<ProgressBar>("EnemyHealthBar");

    weaponSlots[0] = uiDocument.rootVisualElement.Q<VisualElement>("WeaponSlot0");
    weaponSlots[1] = uiDocument.rootVisualElement.Q<VisualElement>("WeaponSlot1");
    weaponSlots[2] = uiDocument.rootVisualElement.Q<VisualElement>("WeaponSlot2");

    SetSelectedWeaponIndicator(0);

    if (pauseButton != null) pauseButton.clicked += TogglePause;
  }

  void OnDisable()
  {
    if (pauseButton != null) pauseButton.clicked -= TogglePause;
  }

  public void SetSelectedWeaponIndicator(int activeIndex)
  {
    for (int i = 0; i < weaponSlots.Length; i++)
    {
      if (weaponSlots[i] == null) continue;

      if (i == activeIndex)
      {
        weaponSlots[i].AddToClassList("activeWeapon");
      }
      else
      {
        weaponSlots[i].RemoveFromClassList("activeWeapon");
      }
    }
  }

  public void UpdateDistanceDisplay(float distance)
  {
    if (distanceLabel != null) distanceLabel.text = $"Threat Distance: {distance:F1}m";
  }

  public void UpdatePlayerHealth(float currentHealth, float maxHealth)
  {
    if (playerHealthBar == null) return;
    playerHealthBar.highValue = maxHealth;
    playerHealthBar.value = currentHealth;
  }

  public void ShowEnemyHealthBar(string enemyName, float currentHealth, float maxHealth)
  {
    if (enemyHudContainer == null) return;

    enemyHudContainer.style.display = DisplayStyle.Flex;
    if (enemyHealthBar != null) enemyHealthBar.title = enemyName;
    UpdateEnemyHealth(currentHealth, maxHealth);
  }

  public void UpdateEnemyHealth(float currentHealth, float maxHealth)
  {
    if (enemyHealthBar != null) enemyHealthBar.value = currentHealth / maxHealth * 100;
  }

  public void HideEnemyHealthBar()
  {
    if (enemyHudContainer != null) enemyHudContainer.style.display = DisplayStyle.None;
  }

  void TogglePause()
  {
    if (Time.timeScale == 0f && !isPaused) return;

    isPaused = !isPaused;

    if (isPaused)
    {
      Time.timeScale = 0f;

      if (pauseButton != null && playIcon != null)
      {
        pauseButton.style.backgroundImage = new StyleBackground(playIcon);
      }
      Debug.Log("Game Paused");
    }
    else
    {
      Time.timeScale = 1f;

      if (pauseButton != null && pauseIcon != null)
      {
        pauseButton.style.backgroundImage = new StyleBackground(pauseIcon);
      }
      Debug.Log("Game Resumed");
    }
  }
}