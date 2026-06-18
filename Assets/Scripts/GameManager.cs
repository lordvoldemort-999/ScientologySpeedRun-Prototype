using UnityEngine;
using TMPro; // Required for your UI TextMeshPro display

public class GameManager : MonoBehaviour
{
  [Header("Spawn Settings")]
  public GameObject enemyPrefab;
  public Transform playerTransform;
  public float spawnDistanceBehind = 3f;
  public float spaceBetweenEnemies = 1.5f;

  [Header("UI Canvas Connections")]
  public GameOverManager gameOverUiScript;

  private Transform enemy1;
  private Transform enemy2;
  private bool isGameOver = false;

  void OnEnable()
  {
    if (playerTransform != null && !playerTransform.gameObject.CompareTag("Player"))
    {
      playerTransform.gameObject.tag = "Player";
    }

    SpawnEnemies();
  }

  void Update()
  {
    if (isGameOver) return;

    UpdateDistanceUI();
    CheckForCatch();
  }

  void SpawnEnemies()
  {
    if (playerTransform == null || enemyPrefab == null) return;

    Vector3 spawnBasePos = new Vector3(playerTransform.position.x - spawnDistanceBehind, playerTransform.position.y, 0f);

    Vector3 rawPos1 = spawnBasePos + new Vector3(-spaceBetweenEnemies / 2, 0f, 0f);
    Vector3 rawPos2 = spawnBasePos + new Vector3(spaceBetweenEnemies / 2, 0f, 0f);

    GameObject e1 = Instantiate(enemyPrefab, rawPos1, Quaternion.identity);
    e1.name = "Chaser_1";
    enemy1 = e1.transform;

    GameObject e2 = Instantiate(enemyPrefab, rawPos2, Quaternion.identity);
    e2.name = "Chaser_2";
    enemy2 = e2.transform;
  }

  void CheckForCatch()
  {
    if (playerTransform == null) return;

    float distanceToE1 = enemy1 != null ? Vector2.Distance(playerTransform.position, enemy1.position) : float.MaxValue;
    float distanceToE2 = enemy2 != null ? Vector2.Distance(playerTransform.position, enemy2.position) : float.MaxValue;

    // If either enemy catches up and breaches your custom combat radius threshold
    if (distanceToE1 < 0.6f || distanceToE2 < 0.6f)
    {
      TriggerGameOver();
    }
  }

  public void TriggerGameOver()
  {
    if (isGameOver) return;
    isGameOver = true;

    Debug.Log("Game Over! They caught you.");

    Time.timeScale = 0f;

    if (gameOverUiScript != null)
    {
      gameOverUiScript.SetupGameOverDisplay();
    }
    else
    {
      Debug.LogError("GameManager is missing a reference to the GameOverManager UI script component!");
    }
  }

  void UpdateDistanceUI()
  {
    if (playerTransform == null) return;

    float distanceToE1 = enemy1 != null ? Vector2.Distance(playerTransform.position, enemy1.position) : float.MaxValue;
    float distanceToE2 = enemy2 != null ? Vector2.Distance(playerTransform.position, enemy2.position) : float.MaxValue;

    float closestDistance = Mathf.Min(distanceToE1, distanceToE2);

    if (closestDistance != float.MaxValue)
    {
      if (GameHUDManager.Instance != null)
      {
        GameHUDManager.Instance.UpdateDistanceDisplay(closestDistance);
      }
    }
  }
}