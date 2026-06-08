using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }
  public float startTimeBtwAttacks;
  private float timeBtwAttacks;

  public enum WeaponType {Fist, Sword, Staff}

  [Header("Current Equipment")]
  public WeaponType currentWeapon = WeaponType.Fist;

  [Header("References")]
  public Animator animator;
  public Transform attackPos;
  public LayerMask whatIsEnemies;

  [Header("Punch Settings")]
  public float punchRange;
  public int punchDamage;

  [Header("Front Kick Settings")]
  public float frontKickRange;
  public int frontKickDamage;

  [Header("Up Punch Settings")]
  public float upPunchRange;
  public int upPunchDamage;

  [Header("Back Kick Settings")]
  public float backKickRange;
  public int backKickDamage;

  [Header("Down Kick Settings")]
  public float downKickRange;
  public int downKickDamage;


  // Update is called once per frame
  void Update()
  {
    HandleWeaponSwitching();
    if (timeBtwAttacks <= 0)
    {
      // "J" is for punching
      if (Input.GetKey(KeyCode.J))
      {
        HandleJAttack();
      }
      // "K" is for kicking/heavy attacks
      else if (Input.GetKeyDown(KeyCode.K))
      {
        HandleKAttack();
      }
    }
    else
    {
      timeBtwAttacks -= Time.deltaTime;
    }

  }

  void HandleWeaponSwitching()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      currentWeapon = WeaponType.Fist;
      animator.SetInteger("WeaponID", 0);
      print("Equipped:" + currentWeapon);
    } else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      currentWeapon = WeaponType.Sword;
      animator.SetInteger("WeaponID", 1);
      print("Equipped:" + currentWeapon);
    } else if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      currentWeapon = WeaponType.Staff;
      animator.SetInteger("WeaponID", 2);
      print("Equipped:" + currentWeapon);
    }
  }

  void HandleJAttack()
  {
    float vInput = Input.GetAxisRaw("Vertical");
    switch (currentWeapon)
    {
      case WeaponType.Fist:
        if (vInput > 0) TriggerAttack("UpPunch", upPunchRange, upPunchDamage);
        else if (!(vInput < 0) && !(vInput > 0)) TriggerAttack("Punch", punchRange, punchDamage);
        break;
      case WeaponType.Sword:
        // sword attack code here
        break;
      case WeaponType.Staff:
        // staff attack code here
        break;
    }
  }

  void HandleKAttack()
  {
    float vInput = Input.GetAxisRaw("Vertical");
    switch (currentWeapon)
    {
      case WeaponType.Fist:
        if (vInput > 0) TriggerAttack("BackKick", backKickRange, backKickDamage);
        else if (vInput < 0) TriggerAttack("DownKick", downKickRange, downKickDamage);
        else TriggerAttack("FrontKick", frontKickRange, frontKickDamage);
        break;
      case WeaponType.Sword:
        // sword attack code here
        break;
      case WeaponType.Staff:
        // staff attack code here
        break;
    }
  }

  void TriggerAttack(string animationTriggerName, float range, int damageValue)
  {
    animator.SetTrigger(animationTriggerName);
    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, range, whatIsEnemies);
    for (int i = 0; i < enemiesToDamage.Length; i++)
    {
      EnemyController enemy = enemiesToDamage[i].GetComponent<EnemyController>();
      if (enemy != null)
      {
        enemy.TakeDamage(damageValue);
      }
    }
    timeBtwAttacks = startTimeBtwAttacks;
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(attackPos.position, punchRange);
    Gizmos.DrawWireSphere(attackPos.position, frontKickRange);
  }

}
