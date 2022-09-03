using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] colliders;      //Here is position of the 3 target boxes
    public Word[] filePostions;         //Important for later which box is used first






    public void Released(Word word)
    {
        //Check if mouse collides with one of the colliders
        foreach (var ele in colliders)
        {
            if (ele.GetComponent<BoxCollider2D>().bounds.Contains(InputManager.MousePosition))
            {
                filePostions[ele.GetComponent<TargetWord>().position] = word;
                var temp = ele.transform.position;
                temp.z -= 1;
                word.gameObject.transform.position = temp;
                return;
            }
            
        }
        word.ResetPos();
    }

}
