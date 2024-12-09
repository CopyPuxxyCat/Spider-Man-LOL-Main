using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public LayerMask whatIsWall;
    public FirstPerson cam;

    [Header("Climbing")]
    public float climbSpeed;
    public bool climbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    [Header("Animation")]
    public Animator animator;
    public ClimbingState climbingState;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private void Update()
    {
        WallCheck();
        StateMachine();

        if (climbing)
        {
            Climbing();
            StateHandler();
        }
        //if (animator.GetBool("isOnWall") == true)
           /// animator.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            
    }

    public enum ClimbingState
    {
        idle,
        crawl,        // Climbing on the wall
        jumpBack     // Jumping back from the wall
    }

    private void StateHandler()
    {
        if (Input.GetKey(KeyCode.W) && climbing)
        {
            climbingState = ClimbingState.crawl;
            animator.SetBool("isCrawling", true);
            animator.SetBool("isJumpBack", false);
            animator.SetBool("isLanding", false);
            animator.SetBool("isWallRunning", false);
            //animator.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        }
        
        else if (Input.GetKey(KeyCode.Space) && climbing)
        {
            climbingState = ClimbingState.jumpBack;
            animator.SetBool("isCrawling", false);
            animator.SetTrigger("jumpBack");
            animator.SetBool("isJumpBack", true);
            animator.SetBool("isLanding", false);
            animator.SetBool("isWallRunning", false);
        }
        else
        {
            climbingState = ClimbingState.idle;
            animator.SetBool("isCrawling", false);
            animator.SetBool("isJumpBack", false);
            animator.SetBool("isLanding", false);
            animator.SetBool("isWallRunning", false);
            //animator.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        }
    }

    private void StateMachine()
    {
        //Climbing
        if (wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)
        {
            if (!climbing) StartClimbing();
        }

        else
        {
            if (climbing) StopClimbing();
        }
    }

    private void WallCheck()
    {
        //Checking if there is a wall in front of the player
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        if (wallFront)
        {
            animator.SetBool("isOnWall", true);
            //Debug.Log("goi duoc wallFront");
        }
        else
            animator.SetBool("isOnWall", false);

        //Angle between the player's vision and the wall normal
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
    }


    private void StartClimbing()
    {
        climbing = true;
        cam.DoFov(90f);
    }

    private void Climbing()
    {
        //Changing the player velocity so that the player can move up and climb
        rb.velocity = new Vector3(rb.velocity.z, climbSpeed, rb.velocity.z);
        if(Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.back * 20f, ForceMode.Impulse);
            climbing = false;
        }
    }

    private void StopClimbing()
    {
        climbing = false;
        cam.DoFov(80f);
    }
    
}
