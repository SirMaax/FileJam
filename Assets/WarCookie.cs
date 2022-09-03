using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarCookie : MonoBehaviour
{

    [SerializeField] Rigidbody2D body; 
    [SerializeField] float speed;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator anim;
    [SerializeField] GameObject player;
    float timePassed;
    bool inRange;
    bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = Time.fixedTime;
        grounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(speed > 0.1)
            sprite.flipX = true; 
        else if(speed < -0.1)
            sprite.flipX = false;

        if(!inRange){
            body.MovePosition(body.position + Vector2.right * speed);
            //anim.SetFloat("speed", Mathf.Abs(speed));
            if(Time.fixedTime - timePassed > 2 && Mathf.Abs(speed) > 0)
                turn();
        }
        else{
            float delta = Mathf.MoveTowards(body.position.x, player.transform.position.x, Mathf.Abs(speed));
            body.MovePosition(new Vector2(delta, body.position.y));
            if(player.transform.position.y > body.position.y && grounded){
                body.AddForce(Vector2.up * 0, ForceMode2D.Impulse);
                
            }
            if(player.transform.position.x > body.position.x){
                speed = Mathf.Abs(speed);
            }
            else{
                speed = -Mathf.Abs(speed);
            }

        }
        Debug.Log(grounded);
        anim.SetFloat("speed", Mathf.Abs(speed));

    }


    public void Hit(int dmg)
    {
        Debug.Log("took dmg");
    }

    public void turn(){
        if(inRange) return;
        timePassed = Time.fixedTime;
        StartCoroutine(turnWait());
    }

    private IEnumerator turnWait(){
        float tempSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(1);
        speed = -tempSpeed;

    }

    private void OnTriggerEnter2D(Collider2D col2){
        if(col2.gameObject.tag == "Stage")
            grounded = true;
    }

    private void OnTriggerExit2D(Collider2D col2){
        if(col2.gameObject.tag == "Stage"){
            turn();
            grounded = false;
        }


    }

    public void setInRange(bool inRange){
        this.inRange = inRange;
    }
}
