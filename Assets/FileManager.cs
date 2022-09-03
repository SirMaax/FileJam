using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] colliders;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckIfColliderIsTouching(GameObject gameObject)
    {
        foreach (var ele in colliders)
        {
            //
            

        }
        //Return collider to original pos

        gameObject.GetComponent<Word>().ResetPos();

    }
}
