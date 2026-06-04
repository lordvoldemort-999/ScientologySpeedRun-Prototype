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

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    currentSpeed = walkSpeed;
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey(KeyCode.LeftShift))
    {
      currentSpeed = sprintSpeed;
      print("You are sprinting");
    }
    else
    {
      currentSpeed = walkSpeed;
    }

    horizontalMove = Input.GetAxisRaw("Horizontal") * currentSpeed;
    animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

    if (Input.GetButtonDown("Jump"))
    {
      jump = true;
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

    // Check if player goes out of bounds and reset position
  }

  public void OnLanding()
  {
    animator.SetBool("IsJumping", false);
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
}

