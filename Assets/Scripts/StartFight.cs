using UnityEngine;

public class StartFight : MonoBehaviour
{
  [Header("Fight Setup")]
  public GameObject enemy;
  public Transform spawnPoint;

  [Header("Arena Boundaries")]
  public GameObject boundaryWalls;

  private GameObject activeFighter;
  private bool fightStarted = false;

  void Update()
  {
    if (fightStarted)
    {
      if (activeFighter == null)
      {
        EndFightArea();
      }
    }
  }

  void Start()
  {
    if (boundaryWalls != null)
    {
      boundaryWalls.SetActive(false);
    }
  }

  void OnTriggerEnter2D(Collider2D player)
  {
    if (fightStarted) return;

    if (player.CompareTag("Player"))
    {
      fightStarted = true;

      EnemyController[] chasers = Object.FindObjectsByType<EnemyController>();
      foreach (EnemyController chaser in chasers)
      {
        chaser.enabled = false;

        if (chaser.animator != null)
        {
          chaser.animator.SetFloat("Speed", 0f);
        }
      }

      if (boundaryWalls != null)
      {
        boundaryWalls.SetActive(true);
      }

      if (enemy != null && spawnPoint != null)
      {
        Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f);
        activeFighter = Instantiate(enemy, spawnPos, Quaternion.identity);
        activeFighter.name = "Arena_Fighter";
      }
    }
  }

  void EndFightArea()
  {
    Debug.Log("Arena enemy defeated! Lowering boundary gates.");

    if (boundaryWalls != null)
    {
      boundaryWalls.SetActive(false);
    }

    Destroy(gameObject);
  }
}
