using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  private float timeBtwAttacks;
  public float startTimeBtwAttacks;
  public Transform attackPos;
  public float attackRange;
  public LayerMask whatIsEnemies;
  public int damage;

  // Update is called once per frame
  void Update()
  {
    if (timeBtwAttacks <= 0)
    {
      //use "J" to attack
      if (Input.GetKey(KeyCode.J))
      {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
          // enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(damage);
        }
      }
      timeBtwAttacks = startTimeBtwAttacks;
    }
    else
    {
      timeBtwAttacks -= Time.deltaTime;
    }

    // void OnDrawGizmosSelected ()
    // {
    //   Gizmos.color = Color.red;
    //   Gizmos.DrawWireSphere(attackPos.position, attackRange);
    // }
  }

}
