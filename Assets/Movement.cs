using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Moving")] 
    [SerializeField] private float force;
    private Vector3 horizontal;
    private Vector2 vertical;
    [Header("CharacterStatus")] public bool grounded;
    
    [Header("GroundCheck")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayCastDistance;

    [Header("Refs")] 
    private Vector2 position;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Remove later!
        Debug.DrawRay(transform.position,Vector2.down,Color.green);
        //
        
        if(!grounded)GroundCheck();
        Move();
    }

    //Get horizontal iNput
    public void Horizontal(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>();
    }
    //Get vertical Input
    public void Vertical(InputAction.CallbackContext context)
    {
        vertical = context.ReadValue<Vector2>();
    }


    
    //Checks if player is grounded
    private void GroundCheck()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, rayCastDistance, groundLayer);
        if (ray.collider != null) grounded = true;
        else grounded = false;
    }

    private void Move()
    {
        Vector3 move = horizontal;
        
        transform.position += move * force;
    }
}