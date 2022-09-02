using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Vector2 aimingMouse;
    // Update is called once per frame
    void Update()
    {
        
    }
    
     public void AimingMouse(InputAction.CallbackContext context)
        {
            if (this == null) return;
            if (aimingMouse == context.ReadValue<Vector2>())
            {
                
            }
            aimingMouse = context.ReadValue<Vector2>();
            
        }
        
}
