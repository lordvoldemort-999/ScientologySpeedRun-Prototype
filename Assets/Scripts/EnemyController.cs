using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("AI Movement")]
    private Transform playerTransform;
    public float moveSpeed = 3f;
    public float stoppingDistance = 1.2f; // Keeps enemy from walking inside the player

    [Header("References")]
    public Animator animator;
    public LayerMask whatIsPlayer;       // Set this to your "Player" layer

    // Matching the player's specific hitbox structure!
    [Header("Hitbox Positions")]
    public Transform punchPos;
    public Transform frontKickPos;
    public Transform upPunchPos;
    public Transform backKickPos;
    public Transform downKickPos;

    [Header("Attack Settings")]
    public float punchRange = 0.5f;
    public int punchDamage = 10;
    
    public float frontKickRange = 0.7f;
    public int frontKickDamage = 15;

    public float upPunchRange = 0.6f;
    public int upPunchDamage = 12;

    public float startTimeBtwAttacks = 1.2f;
    private float timeBtwAttacks;

    void Start()
    {
        currentHealth = maxHealth;

        // Locate the player automatically in the scene
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (timeBtwAttacks <= 0)
        {
            // If inside kicking/punching distance, strike
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
            animator.SetFloat("Speed", 0f); // Stand still on cooldown
        }
    }

    void ChasePlayer()
    {
        FacePlayer();
        
        Vector2 targetPos = new Vector2(playerTransform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        
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
        int randomMove = Random.Range(0, 3); // Picks 0, 1, or 2

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
        }

        timeBtwAttacks = startTimeBtwAttacks;
    }

    // Mirrors the player's TriggerAttack method, but hunts for the Player's component instead
    void EnemyTriggerAttack(string animationTriggerName, float range, int damageValue, Transform attackPos)
    {
        animator.SetTrigger(animationTriggerName);

        Collider2D[] playersToDamage = Physics2D.OverlapCircleAll(attackPos.position, range, whatIsPlayer);
        
        for (int i = 0; i < playersToDamage.Length; i++)
        {
            // Looks for the script where your player's TakeDamage function lives
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
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        print(gameObject.name + " has died.");
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draws the enemy hit circles in Yellow so they don't get confused with player red hit circles
        Gizmos.color = Color.yellow;
        if (punchPos != null) Gizmos.DrawWireSphere(punchPos.position, punchRange);
        if (frontKickPos != null) Gizmos.DrawWireSphere(frontKickPos.position, frontKickRange);
        if (upPunchPos != null) Gizmos.DrawWireSphere(upPunchPos.position, upPunchRange);
    }
}