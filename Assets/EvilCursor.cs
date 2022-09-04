using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EvilCursor : MonoBehaviour
{   
    [SerializeField] float spinValue;
    [SerializeField] Rigidbody2D body;
    [SerializeField] float force;
    bool charging;
    Vector3 playerPos;

    Transform[] children;
    // Start is called before the first frame update
    void Start()
    { 
        children = new Transform[] {transform.GetChild(0), transform.GetChild(1)};
        StartCoroutine(changePhases());

    }

    // Update is called once per frame
    void Update()
    {
        if(charging){
            children[0].parent = null;
            transform.rotation *= Quaternion.AngleAxis(spinValue , new Vector3(0, 0, 1));
            children[0].parent = transform;
        }
     

    }

    private IEnumerator changePhases(){
        while(true)
        {
            // var rot = transform.rotation;
            // Vector3 rot2 = rot.eulerAngles;
            // rot2.x = 0;
            // rot2.y = 0;
            // transform.rotation = quaternion.Euler(rot2);
            // transform.GetChild(0).transform.rotation = Quaternion.identity;
            
            charging = true;
            body.velocity = Vector2.zero;
            yield return new WaitForSeconds(2);
            children[1].GetComponent<SpriteRenderer>().color = Color.cyan;
            yield return new WaitForSeconds(0.5f);
            children[1].GetComponent<SpriteRenderer>().color = Color.white;
            playerPos = Movement2.Position;
            charging = false;
            children[0].parent = null;
            transform.right = Movement2.Position - transform.position;
            children[0].parent = transform;
            body.AddForce( (playerPos - transform.position) * (force + Random.Range(-8, 16))) ;
            GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(14);

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator showDamageFeedback(){
        children[1].GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        children[1].GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Bullet"){
            if(col.gameObject.GetComponent<BulletScript>().typeOfBullet == 0)
                StartCoroutine(showDamageFeedback());
        }
    }
}
