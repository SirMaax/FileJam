using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float force;
    [SerializeField] private float maxX;
    [SerializeField] private float maxY;
    public Vector3 target;
    public int damage;

    // 0 Player Bullet; 1 Enemy Bullet
    public int typeOfBullet;
    void Start()
    {
        Vector3 direction = target - transform.position;
        direction = direction.normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * force);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > maxX || transform.position.x < -maxX
                                        || transform.position.y < -maxY || transform.position.y > maxY)
        {
            Destroy(gameObject);
        }
    }

}
