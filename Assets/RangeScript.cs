using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeScript : MonoBehaviour
{   

    private CircleCollider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col2){
        if(col2.gameObject.tag == "Player"){
            transform.parent.gameObject.GetComponent<EnemyAi>().setInRange(true);
        }
    }

    void OnTriggerExit2D(Collider2D col2){
        if(col2.gameObject.tag == "Player"){
            transform.parent.gameObject.GetComponent<EnemyAi>().setInRange(false);
        }
    }

}
