using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  [Header("Player Health Settings")]
  public int maxHealth = 100;
  public int currentHealth;
  void Start()
  {
    currentHealth = maxHealth;
  }

  public void TakeDamage (int damage)
  {
    currentHealth -= damage;
    print("Player took " + damage + " damage! Remaining health: " + currentHealth);
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
