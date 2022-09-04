using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] Rigidbody2D body; 
    [SerializeField] float speed;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator anim;
    [SerializeField] GameObject bulletPosition;
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
        anim.SetFloat("speed", Math.Abs(speed));
        if(Time.fixedTime - timePassed > 2 && Math.Abs(speed) > 0){
             turn();
        }
    }


    public void Hit(float dmg)
    {
        Debug.Log("took dmg from hammer");
        GetComponent<EnemyAi>().takeDamage(dmg);

    }

    public void turn(){
        timePassed = Time.fixedTime;
        speed *= -1;
        StartCoroutine(turnWait());
    }

    private IEnumerator turnWait(){
        float tempSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(1);
        speed = tempSpeed;
        sprite.flipX = !sprite.flipX;
        bulletPosition.transform.localPosition *= new Vector2(-1, 1);

    }

    
}
