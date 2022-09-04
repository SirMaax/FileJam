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
    bool isTurning;


    // Start is called before the first frame update
    void Start()
    {
        timePassed = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        if(speed > 0.1)
            sprite.flipX = true; 
        else if(speed < -0.1)
            sprite.flipX = false;

        if(!inRange){
            body.MovePosition(body.position + Vector2.right * speed);
            //anim.SetFloat("speed", Mathf.Abs(speed));
            if(Time.fixedTime - timePassed > 2)
                turn();
        }
        else{
            float delta = Mathf.MoveTowards(body.position.x, player.transform.position.x, Mathf.Abs(speed));
            body.MovePosition(new Vector2(delta, body.position.y));
            if(player.transform.position.y > body.position.y +0.1 && grounded){
                body.AddForce(Vector2.up * 1000);
                
            }
            if(player.transform.position.x > body.position.x){
                speed = Mathf.Abs(speed);
            }
            else{
                speed = -Mathf.Abs(speed);
            }

        }
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
        if(!isTurning){
            isTurning = true;
            float tempSpeed = speed;
            speed = 0;
            yield return new WaitForSeconds(1);
            speed = -tempSpeed;
        }
        isTurning = false;

    }


    public void setInRange(bool inRange){
        this.inRange = inRange;
    }



    [Header("GroundCheck")] [SerializeField]
    private LayerMask groundLayer;
    [SerializeField] private float rayCastDistance;
    [SerializeField] private float width;
    private void GroundCheck()
    {



        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, rayCastDistance, groundLayer);
        RaycastHit2D ray2 = Physics2D.Raycast(transform.position + new Vector3(width, 0, 0), Vector2.down, rayCastDistance, groundLayer);
        RaycastHit2D ray3 = Physics2D.Raycast(transform.position + new Vector3(-width, 0, 0), Vector2.down, rayCastDistance, groundLayer);

        if (ray.collider != null)grounded = true;
        else if(ray2.collider != null) grounded = true;
        else if(ray3.collider != null) grounded = true;

        else grounded = false;
    }
}
