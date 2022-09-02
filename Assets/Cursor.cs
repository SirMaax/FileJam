using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    private Vector3 mouse;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mouse);
        mouseWorldPos.z = 0f; // zero z
        Debug.Log(mouseWorldPos);
        transform.position = mouseWorldPos;
    }

    public void MousePosition(InputAction.CallbackContext context)
    {
        mouse = context.ReadValue<Vector2>();
        mouse.z = Camera.main.gameObject.transform.position.z * -1;
        Debug.Log(mouse);
    }
}
