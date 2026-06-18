using UnityEngine;

using UnityEngine.UIElements;
public class StartFight : MonoBehaviour
{
  [Header("Fight Setup")]
  public GameObject enemy;
  public Transform spawnPoint;
  public string enemyDisplayName = "Elite Guard";

  [Header("Arena Boundaries")]
  public GameObject boundaryWalls;

  [Header("End of Level Route")]
  public LevelExit levelExitPortal;

  private GameObject activeFighter;
  private EnemyController activeFighterHealth;
  private bool fightStarted = false;

  [Header("Pre-Fight Dialogue Strings")]
  [TextArea(2, 4)]
  public string[] combatDialogue = new string[]
    {
        "PLAYER: You're standing in my way.",
        "ELITE GUARD: Orders are orders. You aren't leaving this floor alive."
    };
  private bool dialoguePlayed = false;

  void Update()
  {
    if (fightStarted)
    {
      if (activeFighter == null)
      {
        EndFightArea();
      }
      else
      {
        UpdateEnemyHealthHUD();
      }
    }
  }

  void Start()
  {
    if (boundaryWalls != null) boundaryWalls.SetActive(false);
  }

  void OnTriggerEnter2D(Collider2D player)
  {
    if (dialoguePlayed) return;

    if (player.CompareTag("Player"))
    {
      dialoguePlayed = true;

      EnemyController[] chasers = Object.FindObjectsByType<EnemyController>();
      foreach (EnemyController chaser in chasers)
      {
        chaser.enabled = false;
        if (chaser.animator != null) chaser.animator.SetFloat("Speed", 0f);
      }

      if (boundaryWalls != null)
      {
        boundaryWalls.SetActive(true);
      }

      GameNarrationManager narrationManager = Object.FindAnyObjectByType<GameNarrationManager>();
      if (narrationManager != null && combatDialogue.Length > 0)
      {
        narrationManager.StartPreFightDialogue(combatDialogue, BeginActualCombat);
      }
      else
      {
        BeginActualCombat();
      }
    }
  }

  void BeginActualCombat()
  {
    fightStarted = true;

    if (enemy != null && spawnPoint != null)
    {
      Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f);
      activeFighter = Instantiate(enemy, spawnPos, Quaternion.identity);
      activeFighter.name = "Arena_Fighter";

      activeFighterHealth = activeFighter.GetComponent<EnemyController>();

      if (GameHUDManager.Instance != null)
      {
        GameHUDManager.Instance.playerHudContainer.style.display = DisplayStyle.Flex; // Show regular stats

        float currentHp = activeFighterHealth != null ? activeFighterHealth.currentHealth : 100;
        float maxHp = activeFighterHealth != null ? activeFighterHealth.maxHealth : 100;

        GameHUDManager.Instance.ShowEnemyHealthBar(enemyDisplayName, currentHp, maxHp);
      }
    }
  }

  void UpdateEnemyHealthHUD()
  {
    if (GameHUDManager.Instance != null && activeFighterHealth != null)
    {
      GameHUDManager.Instance.UpdateEnemyHealth(activeFighterHealth.currentHealth, activeFighterHealth.maxHealth);
    }
  }

  void EndFightArea()
  {
    Debug.Log("Arena enemy defeated! Lowering boundary gates.");

    if (GameHUDManager.Instance != null)
    {
      GameHUDManager.Instance.HideEnemyHealthBar();
    }

    if (boundaryWalls != null)
    {
      boundaryWalls.SetActive(false);
    }

    if (levelExitPortal != null)
    {
      levelExitPortal.RevealExit();
    }

    Destroy(gameObject);
  }
}