using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using static WallClimbing;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float speed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    bool readyToJump;
    public float airMultiplier;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsBuilding;
    bool grounded;
    bool groundedBuilding;

    [Header("Shoot")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Camera mainCamera;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;
    public MovementState state;
    public Animator animator;

    // attack
    public float cooldown = 2f;
    private float lastAttackTime;

    public enum MovementState
    {
        //Creating different states of movement so it is easier to know what the player can and cannot do
        idle, // idle onground
        walking, // walk on fround
        sprinting, // spring on ground
        air, // on air load the animation Airborne
        wallrunning, // crawling fast on wall = groundedBuilding == sprinting but on wall
        wallstick, // idle on wall = groundedBuilding
        landFromBuilding,
        jumping, // jump onground
        swing,
    }

    
    public bool wallrunning;
    public bool wallstick;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }


    private void Update()
    {
        //Checking if the player is on the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        //Checking is the player is ontop of a building
        groundedBuilding = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsBuilding);

        MyInput();
        SpeedControl();
        StateHandler();
        Punch();
        Kich();
        //Drag
        if (grounded || groundedBuilding)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        //Vector3 cameraForward = mainCamera.transform.forward;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Quaternion rotation = mainCamera.transform.rotation;
        Vector3 eulerAngles = rotation.eulerAngles;
        // Tạo viên đạn tại vị trí bắn
        Instantiate(bulletPrefab, firePoint.position , firePoint.rotation);
    }

    private void FixedUpdate()
    {
        MovePlayer();
        // check state
        //Debug.Log("state hien tai: " + state);
    }

    private void MyInput()
    {
        //Getting inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Horizontal", horizontalInput);
        verticalInput = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Vertical", verticalInput);
        if(grounded == true)
        {
            animator.SetBool("Grounded", true);
        }
        else
        {
            animator.SetBool("Grounded", false);
        }
        if (groundedBuilding == true)
        {
            animator.SetBool("groundedBuilding", true);
        }
        else
        {
            animator.SetBool("groundedBuilding", false);
        }

        //Jump mechanics
        if (Input.GetKey(jumpKey) && readyToJump && (grounded || groundedBuilding))
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // jump back from wall
        /*if (Input.GetKey(KeyCode.S) && groundedBuilding && verticalInput < 0)
        {
            animator.SetTrigger("jumpBack");
            Debug.Log("goi duoc jumpback");
        }*/
    }

    private void StateHandler()
    {
        bool isOnWall = animator.GetBool("isOnWall");
        //Sprinting
        if ((grounded || groundedBuilding) && Input.GetKey(sprintKey) && isOnWall == false)
        {
            state = MovementState.sprinting;
            animator.SetBool("isSprinting", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isCrawling", false);
            animator.SetBool("isWallRunning", false);

            speed = sprintSpeed;
        }
        else if (Input.GetMouseButtonDown(0) && (!grounded || !groundedBuilding) && isOnWall == false)
        {
            state = MovementState.swing;
            animator.SetBool("isSwing", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isCrawling", false);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isSwing", false);
        }
        else if (wallrunning == true)
        {
            state = MovementState.wallrunning;
            animator.SetBool("isWallRunning", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isCrawling", false);
        }
        /*else if (wallrunning == false)
        {
            animator.SetBool("isWallRunning", false);
        }*/
        else if(isOnWall == true)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isWallRunning", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isCrawling", false);
        }
        // idle
        else if ((grounded || groundedBuilding) && (verticalInput == 0 && horizontalInput == 0) && isOnWall == false)
        {
            state = MovementState.idle;
            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isCrawling", false);
            animator.SetBool("isWallRunning", false);
            speed = walkSpeed;
        }

        //Walking
        else if ((grounded || groundedBuilding) && (Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.S))
            && isOnWall == false)
        {
            state = MovementState.walking;
            animator.SetBool("isWalking", true);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isCrawling", false);
            animator.SetBool("isWallRunning", false);
            speed = walkSpeed;
        }
        else if (Input.GetKey(KeyCode.Space) && groundedBuilding)
        {
            state = MovementState.landFromBuilding;
            animator.SetBool("isCrawling", false);
            animator.SetBool("isJumpBack", false);
            animator.SetBool("isWallRunning", false);
            animator.SetTrigger("landFromBuilding");
        }
        else if( Input.GetKey(KeyCode.Space) && isOnWall == false)
        {
            state = MovementState.jumping;
            animator.SetBool("isJumping", true);
            animator.SetBool("isCrawling", false);
            animator.SetBool("isWallRunning", false);
        }
        else
        {
            state = MovementState.air;
            animator.SetBool("isWallRunning", false);
        }
    }


        
    private void MovePlayer()
    {

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Grounded
        if (grounded || groundedBuilding)
        {
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }

        //Airborne
        else if (!(grounded || groundedBuilding))
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);

        /*if (Input.GetKeyDown(KeyCode.V) && Input.GetKey(KeyCode.S) && (grounded || groundedBuilding))
        {
            rb.AddForce(-orientation.forward * 50f, ForceMode.Impulse);
            animator.SetTrigger("backDash");
        }*/
        
    }

    private void Punch()
    {
        if (Input.GetMouseButtonDown(1) && (Time.time > lastAttackTime + cooldown))
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Punch");
        }


    }

    private void Kich()
    {
        if(Input.GetKeyDown(KeyCode.CapsLock) && (Time.time > lastAttackTime + cooldown))
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Kich");
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Speed limit
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

}