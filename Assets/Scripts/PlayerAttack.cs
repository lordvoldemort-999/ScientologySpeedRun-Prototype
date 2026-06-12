using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAttack : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }
  public float startTimeBtwAttacks;
  private float timeBtwAttacks;

  public enum WeaponType { Fist, Sword, Staff }

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

  [Header("")]
  private string bufferedInput = "";
  private float inputBufferTimer = 0f;
  public float maxBufferTime = 0.2f;
  private bool isCurrentlyAttacking = false;

  // Update is called once per frame
  void Update()
  {
    HandleWeaponSwitching();
    ProcessInputWithBuffer();

    if (inputBufferTimer > 0)
    {
      inputBufferTimer -= Time.deltaTime;
      if (inputBufferTimer <= 0)
      {
        bufferedInput = "";         // Timer expired, clear the stored input
      }
    }

    if (timeBtwAttacks <= 0)
    {
      isCurrentlyAttacking = false;
      animator.SetBool("IsAttacking", isCurrentlyAttacking);
      if (bufferedInput != "")
      {
        ExecuteBufferedAttack();
      }
    }
    else
    {
      timeBtwAttacks -= Time.deltaTime;
    }

  }

  void ProcessInputWithBuffer()
  {
    // "J" processing input
    if (Input.GetKeyDown(KeyCode.J))
    {
      string attackToBuffer = DetermineJAttackName();
      BufferAction(attackToBuffer);
    }
    else if (Input.GetKeyDown(KeyCode.K))
    {
      string attackToBuffer = DetermineKAttackName();
      BufferAction(attackToBuffer);
    }
  }

  void BufferAction(string AttackTriggerName)
  {
    bufferedInput = AttackTriggerName;
    inputBufferTimer = maxBufferTime;
  }

  void ExecuteBufferedAttack()
  {
    string attackToPlay = bufferedInput;

    bufferedInput = "";
    inputBufferTimer = 0f;
    ExecuteAttackProperties(attackToPlay);
  }

  string DetermineJAttackName()
  {
    float vInput = Input.GetAxisRaw("Vertical");
    switch (currentWeapon)
    {
      case WeaponType.Fist:
        if (vInput > 0) return "UpPunch";
        if (Input.GetKey(KeyCode.LeftShift)) return "TaijutsuCombo";
        return "Punch";
      case WeaponType.Sword:
        if (vInput > 0) return "UpSwordSwing";
        return "SwordSwing";
      case WeaponType.Staff:
        return "StaffSwing";
      default:
        return "";
    }
  }

  string DetermineKAttackName()
  {
    float vInput = Input.GetAxisRaw("Vertical");
    switch (currentWeapon)
    {
      case WeaponType.Fist:
        if (vInput > 0) return "BackKick";
        if (vInput < 0) return "DownKick";
        return "FrontKick";
      case WeaponType.Sword:
        if (Input.GetKey(KeyCode.LeftShift)) return "Finisher";
        return "Combo";
      case WeaponType.Staff:
        return "StaffUpDownSwing";
      default:
        return "";
    }
  }

  void ExecuteAttackProperties(string triggerName)
  {
    isCurrentlyAttacking = true;
    animator.SetBool("IsAttacking", isCurrentlyAttacking);
    animator.SetTrigger(triggerName);
    timeBtwAttacks = startTimeBtwAttacks;

    Transform targetPos = punchPos;
    float targetRange = punchRange;
    int targetDamage = punchDamage;

    if (triggerName == "Punch") { targetPos = punchPos; targetRange = punchRange; targetDamage = punchDamage; }
    else if (triggerName == "UpPunch") { targetPos = upPunchPos; targetRange = upPunchRange; targetDamage = upPunchDamage; }
    else if (triggerName == "FrontKick") { targetPos = frontKickPos; targetRange = frontKickRange; targetDamage = frontKickDamage; }
    else if (triggerName == "BackKick") { targetPos = backKickPos; targetRange = backKickRange; targetDamage = backKickDamage; }
    else if (triggerName == "DownKick") { targetPos = downKickPos; targetRange = downKickRange; targetDamage = downKickDamage; }
    else if (triggerName == "SwordSwing") { targetPos = punchPos; targetRange = punchRange; targetDamage = frontKickDamage; }
    else if (triggerName == "UpSwordSwing") { targetPos = upPunchPos; targetRange = upPunchRange; targetDamage = frontKickDamage; }
    else if (triggerName == "StaffSwing") { targetPos = punchPos; targetRange = frontKickRange; targetDamage = frontKickDamage; }
    else if (triggerName == "StaffUpDownSwing") { targetPos = upPunchPos; targetRange = backKickRange; targetDamage = backKickDamage; }
    else if (triggerName == "Finisher") { targetPos = frontKickPos; targetRange = backKickRange; targetDamage = 2 * backKickDamage; }
    else if (triggerName == "Combo") { targetPos = frontKickPos; targetRange = punchRange; targetDamage = 2 * punchDamage; }
    else if (triggerName == "TaijutsuCombo") { targetPos = backKickPos; targetRange = backKickRange; targetDamage = 3 * punchDamage; }

    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(targetPos.position, targetRange, whatIsEnemies);
    for (int i = 0; i < enemiesToDamage.Length; i++)
    {
      EnemyController enemy = enemiesToDamage[i].GetComponent<EnemyController>();
      if (enemy != null) enemy.TakeDamage(targetDamage);
    }
  }

  void HandleWeaponSwitching()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1)) { currentWeapon = WeaponType.Fist; animator.SetInteger("WeaponID", 0); }
    else if (Input.GetKeyDown(KeyCode.Alpha2)) { currentWeapon = WeaponType.Sword; animator.SetInteger("WeaponID", 1); }
    else if (Input.GetKeyDown(KeyCode.Alpha3)) { currentWeapon = WeaponType.Staff; animator.SetInteger("WeaponID", 2); }
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
