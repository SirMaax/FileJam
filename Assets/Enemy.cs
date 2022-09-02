using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] Rigidbody2D body; 
    [SerializeField] float speed;
    float timePassed;
    // Start is called before the first frame update
    void Start()
    {
        timePassed = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {
        body.MovePosition(body.position + Vector2.right * speed);
        if(Time.fixedTime - timePassed > 2){
             timePassed = Time.fixedTime;
             speed *= -1;
        }
    }


    public void Hit(int dmg)
    {
        Debug.Log("took dmg");
    }

}
