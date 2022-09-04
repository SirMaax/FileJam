using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floatie : MonoBehaviour
{
    public float speed;
    public float maxPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if(transform.position.x > maxPos)
            Destroy(gameObject);
    }


}
