using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{   


    [SerializeField] private AudioSource[] effectSource;


    public void Play(int index){
        // effectSource[index].Play();
    }
    public void Stop(int index){
        effectSource[index].Stop();
    }
    

}
