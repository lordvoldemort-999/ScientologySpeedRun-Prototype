using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  [Header("Player Health Settings")]
  public int maxHealth = 100;
  public Animator animator;
  public int currentHealth;

  void Start()
  {
    currentHealth = maxHealth;
    if (GameHUDManager.Instance != null) GameHUDManager.Instance.UpdatePlayerHealth(currentHealth, maxHealth);
  }

  public void TakeDamage (int damage)
  {
    currentHealth -= damage;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    print("Player took " + damage + " damage! Remaining health: " + currentHealth);
    animator.SetTrigger("Damaged");
    if (GameHUDManager.Instance != null) GameHUDManager.Instance.UpdatePlayerHealth(currentHealth, maxHealth);
    if (currentHealth <= 0) Die();
  }

  void Die()
  {
    print(gameObject.name + " has died.");
    animator.SetTrigger("Defeat");
  }

  public void DestroyCharacter()
  {
    Destroy(gameObject);
  }
}
