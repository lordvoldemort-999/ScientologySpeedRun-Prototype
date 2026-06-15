using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public CharacterController2D controller;
  float horizontalMove = 0f;
  public float walkSpeed = 40f;
  public float sprintSpeed = 70f;
  float currentSpeed;
  bool jump = false;
  bool crouch = false;
  public Animator animator;

  [Header("Footstep Audio Collections")]
  [Tooltip("Drop your collection of walking footstep sounds here.")]
  public AudioClip[] walkSounds;
  [Tooltip("Drop your collection of sprinting footstep sounds here.")]
  public AudioClip[] sprintSounds;
  [Range(0f, 1f)] public float footstepVolume = 0.4f;

  [Header("Footstep Rhythm Timing")]
  [Tooltip("Time between steps when walking (higher = slower pacing).")]
  public float walkStepInterval = 0.45f;
  [Tooltip("Time between steps when sprinting (lower = faster pacing).")]
  public float sprintStepInterval = 0.28f;

  private float footstepTimer;
  private bool isSprinting = false;
  private bool isGrounded = true;

  void Start()
  {
    currentSpeed = walkSpeed;
  }

  void Update()
  {
    if (Input.GetKey(KeyCode.LeftShift))
    {
      currentSpeed = sprintSpeed;
      isSprinting = true;
      print("You are sprinting");
    }
    else
    {
      currentSpeed = walkSpeed;
      isSprinting = false;
    }

    horizontalMove = Input.GetAxisRaw("Horizontal") * currentSpeed;
    animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

    if (Input.GetButtonDown("Jump"))
    {
      jump = true;
      isGrounded = false;
      animator.SetBool("IsJumping", true);
    }

    if (Input.GetButtonDown("Crouch"))
    {
      crouch = true;
    }
    else if (Input.GetButtonUp("Crouch"))
    {
      crouch = false;
    }

    animator.SetBool("IsCrouching", crouch);

    if (Mathf.Abs(horizontalMove) > 0.1f && isGrounded && !crouch)
    {
      footstepTimer -= Time.deltaTime;

      if (footstepTimer <= 0f)
      {
        PlayFootstepSound();
        footstepTimer = isSprinting ? sprintStepInterval : walkStepInterval;
      }
    }
    else
    {
      footstepTimer = 0f;
    }
  }

  public void OnLanding()
  {
    animator.SetBool("IsJumping", false);
    isGrounded = true;
  }

  public void OnCrouching(bool isCrouching)
  {
    animator.SetBool("IsCrouching", isCrouching);
  }

  void FixedUpdate()
  {
    controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
    jump = false;
  }

  void PlayFootstepSound()
  {
    if (AudioManager.Instance == null) return;

    AudioClip chosenClip = null;

    if (isSprinting)
    {
      if (sprintSounds == null || sprintSounds.Length == 0) return;
      int randomIndex = Random.Range(0, sprintSounds.Length);
      chosenClip = sprintSounds[randomIndex];
    }
    else
    {
      if (walkSounds == null || walkSounds.Length == 0) return;
      int randomIndex = Random.Range(0, walkSounds.Length);
      chosenClip = walkSounds[randomIndex];
    }

    if (chosenClip != null)
    {
      AudioManager.Instance.PlaySFX(chosenClip, footstepVolume);
    }
  }
}