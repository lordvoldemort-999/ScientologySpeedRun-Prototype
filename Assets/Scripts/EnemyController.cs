using UnityEngine;

public class EnemyController : MonoBehaviour
{
  [Header("Health Settings")]
  public int maxHealth = 100;
  public int currentHealth;

  [Header("AI Movement")]
  private Transform playerTransform;
  public float moveSpeed = 3f;
  public float stoppingDistance = 1.2f;

  [Header("References")]
  public Animator animator;
  public LayerMask whatIsPlayer;

  [Header("Hitbox Positions")]
  public Transform punchPos;
  public Transform frontKickPos;
  public Transform upPunchPos;
  public Transform backKickPos;
  public Transform downKickPos;

  [Header("Attack Settings")]
  public float punchRange;
  public int punchDamage;

  public float frontKickRange;
  public int frontKickDamage;

  public float upPunchRange;
  public int upPunchDamage;

  public float backKickRange;
  public int backKickDamage;

  public float downKickRange;
  public int downKickDamage;

  public float startTimeBtwAttacks;
  private float timeBtwAttacks;

  private bool isDead = false;

  void Start()
  {
    currentHealth = maxHealth;

    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    if (playerObj != null)
    {
      playerTransform = playerObj.transform;
    }
  }

  void Update()
  {
    if (isDead || playerTransform == null) return;

    float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

    if (timeBtwAttacks <= 0)
    {
      // If inside combat/striking distance, execute an attack
      if (distanceToPlayer <= stoppingDistance)
      {
        ChooseRandomAttack();
      }
      else
      {
        ChasePlayer();
      }
    }
    else
    {
      timeBtwAttacks -= Time.deltaTime;
      FacePlayer();
      animator.SetFloat("Speed", 0f);
    }
  }

  void ChasePlayer()
  {
    FacePlayer();

    transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

    animator.SetFloat("Speed", 1f);
  }

  void FacePlayer()
  {
    if (playerTransform.position.x > transform.position.x)
    {
      transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    else
    {
      transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
  }

  void ChooseRandomAttack()
  {
    int randomMove = Random.Range(0, 5);

    switch (randomMove)
    {
      case 0:
        EnemyTriggerAttack("Punch", punchRange, punchDamage, punchPos);
        break;
      case 1:
        EnemyTriggerAttack("FrontKick", frontKickRange, frontKickDamage, frontKickPos);
        break;
      case 2:
        EnemyTriggerAttack("UpPunch", upPunchRange, upPunchDamage, upPunchPos);
        break;
      case 3:
        EnemyTriggerAttack("BackKick", backKickRange, backKickDamage, backKickPos);
        break;
      case 4:
        EnemyTriggerAttack("DownKick", downKickRange, downKickDamage, downKickPos);
        break;
    }

    timeBtwAttacks = startTimeBtwAttacks;
  }

  void EnemyTriggerAttack(string animationTriggerName, float range, int damageValue, Transform attackPos)
  {
    animator.SetTrigger(animationTriggerName);

    Collider2D[] playersToDamage = Physics2D.OverlapCircleAll(attackPos.position, range, whatIsPlayer);

    for (int i = 0; i < playersToDamage.Length; i++)
    {
      PlayerHealth player = playersToDamage[i].GetComponent<PlayerHealth>();
      if (player != null)
      {
        player.TakeDamage(damageValue);
      }
    }
  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;
    print(gameObject.name + " has taken " + damage + " damage. Health remaining: " + currentHealth);
    animator.SetTrigger("Damaged");

    if (currentHealth <= 0)
    {
      Die();
    }
  }

  void Die()
  {
    isDead = true;
    print(gameObject.name + " has died.");
    animator.SetTrigger("Defeat");
    Destroy(gameObject);
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.yellow;
    if (punchPos != null) Gizmos.DrawWireSphere(punchPos.position, punchRange);
    if (frontKickPos != null) Gizmos.DrawWireSphere(frontKickPos.position, frontKickRange);
    if (upPunchPos != null) Gizmos.DrawWireSphere(upPunchPos.position, upPunchRange);
    if (backKickPos != null) Gizmos.DrawWireSphere(backKickPos.position, backKickRange);
    if (downKickPos != null) Gizmos.DrawWireSphere(downKickPos.position, downKickRange);
  }
}