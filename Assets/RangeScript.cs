using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeScript : MonoBehaviour
{   

    [SerializeField] Collider2D col;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col2){
        if(col2.gameObject.tag == "Player"){
            if(transform.parent.gameObject.TryGetComponent(out EnemyAi script)){
                transform.parent.gameObject.GetComponent<EnemyAi>().setInRange(true);
            }
            else{
                transform.parent.gameObject.GetComponent<WarCookie>().setInRange(true);
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D col2){
        if(col2.gameObject.tag == "Player"){
            if(transform.parent.gameObject.TryGetComponent(out EnemyAi script)){
                transform.parent.gameObject.GetComponent<EnemyAi>().setInRange(false);
            }
            else{
                transform.parent.gameObject.GetComponent<WarCookie>().setInRange(false);
            }
        }
    }

}
