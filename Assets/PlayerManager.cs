using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Refs")] public GameObject ps;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp()
    {
        ps.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        ps.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        Debug.Log("do the particle stuff");
    }
}
