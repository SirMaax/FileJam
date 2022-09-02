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

    [Header("CharacterStatus")] 
    public bool jumping;
    public bool grounded;

    [Header("Jumping")] [SerializeField] private float gravityAmplifier;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDuration;
    private double timeJumped;                //When the player started the last jump
    private float colliderHeight;
    private float colliderWidth;

    [Header("Dashing")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashForce;
    private bool dashing = false;
    private float timeDashed;               //When the player started the last dash
    
    [Header("GroundCheck")] [SerializeField]
    private LayerMask groundLayer;
    [SerializeField] private float rayCastDistance;

    [Header("Refs")] 
    private bool canMove = false;
    private SpriteRenderer spriteRenderer;                  

    [Header("OtherStuff")] public int direction = 1; //IN which direciton the player is facing 1 is to the right -1 is to the left
    public static Vector3 Position;

    [SerializeField] Animator animator;
    Vector3 lastPos;
    
    // Start is called before the first frame update
    void Start()
    {
        //Wait 1 second before moving
        StartCoroutine(WaitOneSec());
        desiredMove = Vector3.zero;
        colliderHeight = GetComponent<BoxCollider2D>().size.y/2;
        colliderWidth = GetComponent<BoxCollider2D>().size.x/2;
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        Position = transform.position; 
        if (!canMove) return;
        GroundCheck();
        WallCheck();
        JumpDurationCheck();
        DashDurationCheck();
        if (!grounded)
        {
            Gravity();
        }

        Move();

        // Sprite Handling
        spriteRenderer.flipX = direction == 1;
        lastPos = transform.position;
        animator.SetFloat("Speed", (Position-lastPos).magnitude);

    }

    //Get horizontal iNput
    public void Horizontal(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>();
        if (horizontal.x < 0) direction = -1;
        else if (horizontal.x > 0) direction = 1;
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
        if (ray.collider != null)TouchGround(ray);

        else grounded = false;
    }

    //Gets called when touching the ground
    private void TouchGround(RaycastHit2D ray)
    {
        if (grounded) return;

       Vector3 newPos = Physics2D.ClosestPoint(transform.position, ray.collider);
       newPos.y += colliderHeight;
       transform.position = newPos;

        // //Set the height correct
        // float newYPos = ray.transform.position.y + colliderHeight;
        // Vector3 temp = transform.position;
        // temp.y = newYPos;
        
        grounded = true;
        desiredMove.y = 0;
    }

    private void Move()
    {
        
        if (dashing)
        {
            desiredMove.y = 0;
            desiredMove.x = direction * dashForce;
        }
        else desiredMove.x = horizontal.x;
        transform.Translate(desiredMove * force * Time.deltaTime );
    }

    private void Gravity()
    {
        if (jumping || dashing) return;
        desiredMove.y = gravityAmplifier * Physics2D.gravity.y;
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
        jumping = false;
    }

    private IEnumerator WaitOneSec()
    {
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }
    
    //Prevents the player from going through walls
    private void WallCheck()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.left, rayCastDistance, groundLayer);
        if (ray.collider != null)
        {
            Vector3 newPos = Physics2D.ClosestPoint(transform.position, ray.collider);
            newPos.x += colliderWidth;
            transform.position = newPos;
            EndOfDash();
        }
         ray = Physics2D.Raycast(transform.position, Vector2.right, rayCastDistance, groundLayer);
        if (ray.collider != null)
        {
            Vector3 newPos = Physics2D.ClosestPoint(transform.position, ray.collider);
            newPos.x -= colliderWidth;
            transform.position = newPos;
            EndOfDash();
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!CanDash()) return; //Coyote Time
            
            
            dashing = true;
            timeDashed = Time.time;
        }

    }
    //Checks if the player is allowed to jump. Including coyote time
    private bool CanDash()
    {
        if (!dashing) return true;
        return false;
    }

    private void DashDurationCheck()
    {
        if (dashing && timeDashed + dashDuration - Time.time <= 0)
        {
            EndOfDash();
        }
    }

    private void EndOfDash()
    {
        dashing = false;
    }
}