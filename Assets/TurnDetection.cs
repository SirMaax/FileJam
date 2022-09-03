using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col){

        if(col.gameObject.tag == "Wall"){
            if(transform.parent.gameObject.TryGetComponent(out Enemy script)){
                transform.parent.gameObject.GetComponent<Enemy>().turn();
            }
            else{
                transform.parent.gameObject.GetComponent<WarCookie>().turn();
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.tag == "Stage"){
            if(transform.parent.gameObject.TryGetComponent(out Enemy script)){
                transform.parent.gameObject.GetComponent<Enemy>().turn();
            }
            else{
                transform.parent.gameObject.GetComponent<WarCookie>().turn();
            }
        }
    }

}
