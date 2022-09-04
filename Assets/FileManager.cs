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

            if (ele.GetComponent<BoxCollider2D>().bounds.Contains(InputManager.MousePosition))
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
}