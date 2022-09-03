using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    [SerializeField] private float aimRadius;
    public static Vector3 ActualMousePos;
    public static Vector3 WereMouseIsPointed;
    private InputManager _inputManager;
    public GameObject inputmanager;
    // Start is called before the first frame update
    void Start()
    {
        _inputManager = inputmanager.GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {

        var mouseWorldPos = Camera.main.ScreenToWorldPoint(InputManager.aimingMouse);
        mouseWorldPos.z = 0;
        WereMouseIsPointed = mouseWorldPos;
        Vector2 distance = (mouseWorldPos - Movement2.Position);
        var temp = Mathf.Clamp(distance.magnitude, 0, aimRadius);
        distance = distance.normalized;
        
       ActualMousePos = Movement2.Position + (Vector3)distance * temp;
        transform.position = ActualMousePos;
    }
}
