using UnityEngine;

public class StartFight : MonoBehaviour
{
  // Update is called once per frame
  void Update()
  {

  }
  public GameObject enemy;
  public Transform spawnPoint;
  void OnTriggerEnter2D(Collider2D player)
  {
    if (player.CompareTag("Player"))
    {
      Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y);
      Instantiate(enemy, spawnPos, Quaternion.identity);
      Destroy(gameObject);
    }
  }
}
