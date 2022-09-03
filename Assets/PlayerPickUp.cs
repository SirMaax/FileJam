using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Player picks Up
        if (other.gameObject.tag.Equals("ParticlePickUp"))
        {
            other.gameObject.GetComponent<PlayerManager>().PickUp();
            //Trigger particles expo in Player
            Destroy(gameObject);
        }
    }
}
