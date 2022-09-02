using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Moving")] [SerializeField] private float force;
    private Vector2 horizontal;
    public Vector3 desiredMove;
    private Vector2 vertical;

    [Header("CharacterStatus")] public bool jumping;
    public bool grounded;

    [Header("Jumping")] [SerializeField] private float gravityForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDuration;
    private double timeJumped;

    [Header("GroundCheck")] [SerializeField]
    private LayerMask groundLayer;

    [SerializeField] private float rayCastDistance;

    [Header("Refs")] private Vector2 position;
    public int test = 0;

    // Start is called before the first frame update
    void Start()
    {
        desiredMove = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        JumpDurationCheck();
        if (!grounded)
        {
            Gravity();
        }

        Move();
    }

    //Get horizontal iNput
    public void Horizontal(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>();
        Debug.Log(horizontal);
    }

    //Get vertical Input
    public void Vertical(InputAction.CallbackContext context)
    {
        vertical = context.ReadValue<Vector2>();
    }


    //Checks if player is grounded
    private void GroundCheck()
    {
        //Remove later!
        Debug.DrawRay(transform.position, Vector2.down * rayCastDistance, Color.green);
        //
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, rayCastDistance, groundLayer);
        if (ray.collider != null) TouchGround();
        else grounded = false;
    }

    //Gets called when touching the ground
    private void TouchGround()
    {
        if (grounded) return;
        grounded = true;
        desiredMove.y = 0;
    }

    private void Move()
    {
        desiredMove.x = horizontal.x * Time.deltaTime;


        transform.position += desiredMove * force;
    }

    private void Gravity()
    {
        if (jumping) return;
        desiredMove.y = gravityForce;
    }

    //What Happens when the player triggers the jump
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!CanJump()) return; //Coyote Time
            //InputBuffer
            jumping = true;
            timeJumped = Time.time;
            desiredMove.y = jumpForce;
        }
        if(context.canceled)
        {
            test = 0;
            EndOfJump();
        }
    }

    private void JumpDurationCheck()
    {
        if (jumping && timeJumped + jumpDuration - Time.time <= 0)
        {
            EndOfJump();
        }
    }

    //Checks if the player is allowed to jump. Including coyote time
    private bool CanJump()
    {
        if (grounded || jumping) return true;
        return false;
    }


    //What happens after the jump
    private void EndOfJump()
    {
        Debug.Log(transform.position.y);
        jumping = false;
    }
}