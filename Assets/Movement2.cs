using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement2 : MonoBehaviour
{
    [Header("Moving")] [SerializeField] private float force;
    public Vector2 horizontal;
    public Vector3 desiredMove;
    private Vector2 vertical;
    [SerializeField] private float slowDown;
    [SerializeField] private float maxXSpeed;
    [SerializeField] private float maxYSpeed;
    private float startmaxXSpeed;
    [Header("CharacterStatus")] 
    public bool jumping;
    public bool grounded;
    


    [Header("OtherStuff")]
    public int direction = 1; //IN which direciton the player is facing 1 is to the right -1 is to the left
    public int lastDirection;
    public static Vector3 Position;
    private bool canMove = false;
    Vector3 lastPos;


    
    [Header("reFS")] 
    [SerializeField] Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitOneSec());
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalGravity = rb.gravityScale;
        _playerManager = GetComponent<PlayerManager>();
        startmaxXSpeed = maxXSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFileValues();
        if (!canMove) return;
        GroundCheck();
        Move();
        JumpDurationCheck();
        Position = transform.position;

        // Sprite Handling
        spriteRenderer.flipX = direction == 1;
        lastPos = transform.position;
        animator.SetFloat("Speed", (Position - lastPos).magnitude);
    }

    private IEnumerator WaitOneSec()
    {
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }

    //Get horizontal iNput
    public void Horizontal(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>();
        lastDirection = direction;
        if (horizontal.x < 0) direction = -1;
        else if (horizontal.x > 0) direction = 1;
        
    }

    private void Move()
    {
        if (dashing) return;

        if (horizontal.x == 0)
        {
            var temp = rb.velocity;
            
            temp.x = Mathf.MoveTowards(temp.x, 0, slowDown);
            rb.velocity = temp;
            // GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Stop(1);

        }
        else
        {
            // GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(1);

            if(lastDirection != direction)
            {
                var vel = rb.velocity;
                vel.x = 0;
                rb.velocity = vel;
            }
            
            rb.AddForce(Vector2.right * horizontal.x * Time.deltaTime * force);
        }
        
        //Clamp max Speed
        var temp2 = rb.velocity;
        temp2.x = Mathf.Clamp(temp2.x, -maxXSpeed, maxXSpeed);
        temp2.y = Mathf.Clamp(temp2.y, -maxYSpeed, maxYSpeed);
        rb.velocity = temp2;

    }

    #region GroundCheck
    [Header("GroundCheck")] [SerializeField]
    private LayerMask groundLayer;
    [SerializeField] private float rayCastDistance;
    [SerializeField] private float width;
    private void GroundCheck()
    {
        //Remove later!
        Debug.DrawRay(transform.position, Vector2.down * rayCastDistance, Color.green);
        Debug.DrawRay(transform.position + new Vector3(width, 0, 0), Vector2.down * rayCastDistance, Color.green);
        Debug.DrawRay(transform.position + new Vector3(-width, 0, 0), Vector2.down * rayCastDistance, Color.green);

        //
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, rayCastDistance, groundLayer);
        RaycastHit2D ray2 = Physics2D.Raycast(transform.position + new Vector3(width, 0, 0), Vector2.down, rayCastDistance, groundLayer);
        RaycastHit2D ray3 = Physics2D.Raycast(transform.position + new Vector3(-width, 0, 0), Vector2.down, rayCastDistance, groundLayer);

        if (ray.collider != null)TouchGround(ray);
        else if(ray2.collider != null) TouchGround(ray2);
        else if(ray3.collider != null) TouchGround(ray3);

        else grounded = false;
    }
    
    private void TouchGround(RaycastHit2D ray)
    {
        if (grounded) return;

        // Vector3 newPos = Physics2D.ClosestPoint(transform.position, ray.collider);
        // newPos.y += colliderHeight;
        // transform.position = newPos;

        // //Set the height correct
        // float newYPos = ray.transform.position.y + colliderHeight;
        // Vector3 temp = transform.position;
        // temp.y = newYPos;
        EndOfJump();
        grounded = true;
        desiredMove.y = 0;
    }
    

    #endregion
  
    
    

    #region Jumping
    [Header("Jumping")] [SerializeField] private float gravityAmplifier;
    private float originalGravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDuration;
    private double timeJumped;    
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(6);

            if (!CanJump()) return; //Coyote Time
            //InputBuffer
            
            jumping = true;
            timeJumped = Time.time;
            rb.AddForce(Vector2.up * jumpForce);
        }
        if(context.canceled && jumping)
        {
            EndOfJump();
        }
    }
    //When the player started the last jump
    private bool CanJump()
    {
        if (grounded) return true;
        return false;
    }
    
    //What happens after the jump
    private void EndOfJump()
    {
        jumping = false;
        //Set vertical speed to zero
        var temp = rb.velocity;
        temp.y = 0;
        rb.velocity = temp;
    }
    
    private void JumpDurationCheck()
    {
        
        if (jumping && timeJumped + jumpDuration - Time.time <= 0)
        {
            jumping = false;
        }
    }

    #endregion

    #region dash

    [Header("Dashing")] public bool canDash = true;
    [SerializeField] private float dashForce;
    private bool dashing = false;
    [SerializeField] private float dashCoolDown;
    [SerializeField] private float timeAfterDashWithoutXInput;
    
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!CanDash()) return; //Coyote Time
            
            dashing = true;
            canDash = false;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(direction * dashForce,0));
            StartCoroutine(DashCoolDown());
        }

    }
    private bool CanDash()
    {
        if (!dashing && canDash) return true;
        return false;
    }

    private IEnumerator DashCoolDown()
    {
        rb.gravityScale = 0;

        yield return new WaitForSeconds(timeAfterDashWithoutXInput);
        dashing = false;
        rb.gravityScale = originalGravity;

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;

    }
    #endregion"

    [Header("ExtraFile")] public float extraSpeed;
    private PlayerManager _playerManager;
    private void UpdateFileValues()
    {
        extraSpeed = _playerManager.GetSpeedIncrease();
        maxXSpeed = startmaxXSpeed + extraSpeed;
    }
}