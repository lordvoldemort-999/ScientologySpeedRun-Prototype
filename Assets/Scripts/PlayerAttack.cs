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
  public Transform punchPos;
  public Transform frontKickPos;
  public Transform upPunchPos;
  public Transform backKickPos;
  public Transform downKickPos;
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
        if (vInput > 0) TriggerAttack("UpPunch", upPunchRange, upPunchDamage, upPunchPos);
        else if (!(vInput < 0) && !(vInput > 0)) TriggerAttack("Punch", punchRange, punchDamage, punchPos);
        break;
      case WeaponType.Sword:
        // sword attack code here
        if (vInput > 0) TriggerAttack("UpSwordSwing", upPunchRange, upPunchDamage, upPunchPos);
        else if (!(vInput < 0) && !(vInput > 0)) TriggerAttack("SwordSwing", frontKickRange, frontKickDamage, frontKickPos); 
        break;
      case WeaponType.Staff:
        if (vInput > 0) TriggerAttack("StaffUpDownSwing", upPunchRange, upPunchDamage + 1, upPunchPos);
        else if (!(vInput < 0) && !(vInput > 0)) TriggerAttack("StaffSwing", punchRange, punchDamage + 1, punchPos);
        break;
    }
  }

  void HandleKAttack()
  {
    float vInput = Input.GetAxisRaw("Vertical");
    switch (currentWeapon)
    {
      case WeaponType.Fist:
        if (vInput > 0) TriggerAttack("BackKick", backKickRange, backKickDamage, backKickPos);
        else if (vInput < 0) TriggerAttack("DownKick", downKickRange, downKickDamage, downKickPos);
        else TriggerAttack("FrontKick", frontKickRange, frontKickDamage, frontKickPos);
        break;
      case WeaponType.Sword:
        // sword attack code here
        break;
      case WeaponType.Staff:
        // staff attack code here
        break;
    }
  }

  void TriggerAttack(string animationTriggerName, float range, int damageValue, Transform attackPos)
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
    Gizmos.DrawWireSphere(punchPos.position, punchRange);
    Gizmos.DrawWireSphere(frontKickPos.position, frontKickRange);
    Gizmos.DrawWireSphere(upPunchPos.position, upPunchRange);
    Gizmos.DrawWireSphere(frontKickPos.position, frontKickRange);
    Gizmos.DrawWireSphere(backKickPos.position, backKickRange);
    Gizmos.DrawWireSphere(downKickPos.position, downKickRange);
  }

}
