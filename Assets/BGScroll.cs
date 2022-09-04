using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{

    [SerializeField] float scrollSpeed;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] bool waveEffect; 
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
        if(waveEffect)
            transform.position += new Vector3(0, Mathf.Sin(transform.position.x) * 0.5f * Time.deltaTime);
        if(transform.position.x > startPos.x + (sprite.bounds.size.x / 2))
            transform.position = startPos;
    }
    
}
