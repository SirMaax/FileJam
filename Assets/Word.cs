using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 startPosition;
    
    private Vector2 offset;
    private bool dragging = false;
    private FileManager _fileManager;
    void Start()
    {
        startPosition = transform.position;
        _fileManager = GameObject.FindWithTag("FileManager").GetComponent<FileManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDrag()
    {
        dragging = true;
        if (offset == Vector2.zero)
        {        
            Vector2 temp = transform.position;
            offset = InputManager.MousePosition - temp;

        }
        transform.position = InputManager.MousePosition - offset ;
    }
    
    //Checks if the words is dragged to a space in the filer where it can sit if it is there
    private void CheckIfCollidesWithDestinationBoxes()
    {
        //Check if mouse hover over one of the colliders
        // GetComponent<Co>()
    }

    public void ResetPos()
    {
        transform.position = startPosition;
    }

    private void OnMouseUp()
    {
        offset = Vector2.zero;
        _fileManager.Released(this);
        //Check where if it is in there distance ez
    }
}


