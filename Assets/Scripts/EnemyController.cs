using System.Xml.Serialization;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public int maxHealth = 100;
  private int currentHealth;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    currentHealth = maxHealth;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;
    print("Damage taken!");
    if (currentHealth <= 0)
    {
      Die();
    }
  }

  void Die()
  {
    print(gameObject.name + "has died.");
    Destroy(gameObject);
  }
}
