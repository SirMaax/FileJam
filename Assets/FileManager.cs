using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] colliders; //Here is position of the 3 target boxes
    public Word[] filePostions; //Important for later which box is used first


    public void Released(Word word)
    {
        //Check if mouse collides with one of the colliders
        foreach (var ele in colliders)
        {
            GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(11);

            BoxCollider2D col = ele.GetComponent<BoxCollider2D>();
            if(InputManager.MousePosition.x < col.bounds.center.x + col.bounds.size.x 
               && InputManager.MousePosition.x > col.bounds.center.x - col.bounds.size.x
               && InputManager.MousePosition.y < col.bounds.center.y + col.bounds.size.y
               && InputManager.MousePosition.y > col.bounds.center.y - col.bounds.size.y)
                
            // if (ele.GetComponent<BoxCollider2D>().bounds.Contains(InputManager.MousePosition)) ;
            {
                for (int i = 0; i < filePostions.Length; i++)
                {
                    if (filePostions[i] == null) continue;
                    if (filePostions[i].Equals(word))
                    {
                        filePostions[i] = null;
                    }
                }
                if (filePostions[ele.GetComponent<TargetWord2>().position] != null)
                {
                    filePostions[ele.GetComponent<TargetWord2>().position].GetComponent<Word>().ResetPos();
                }
                filePostions[ele.GetComponent<TargetWord2>().position] = word;
                var temp = ele.transform.position;
                temp.z -= 1;
                word.gameObject.transform.position = temp;
                return;
            }
            

            
        }

        for (int i = 0; i < filePostions.Length; i++)
        {
            if (filePostions[i] == null) continue;
            if (filePostions[i].Equals(word))
            {
                filePostions[i] = null;
            }
        }
       
        word.ResetPos();
    }

    public void Show(bool yesorno)
    {
        transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = yesorno;
        GetComponent<SpriteRenderer>().enabled = yesorno;
        transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().enabled = yesorno;
        transform.GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().enabled = yesorno;
        transform.GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().enabled = yesorno;

    }
}