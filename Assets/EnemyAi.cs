using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    // Start is called before the first frame update
    
    [Header("Type of Enemy")] 
    [Range(0,3)]public int typeOfEnemy;
    public float[] shootCooldown;
    public GameObject[] bullets;
    public float[] moveSpeed;
    public float[] moveTime;
    public float direction;
    [Header("movin")] public float minDistance;
    public float maxDistance;
    public float nextTarget;
    public float distanceToNext;                //Distance to next target unitl a new one is picked
    [Header("Status")] public bool canShoot;
    void Start()
    {
        MoveRandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (typeOfEnemy == 0) Enemy1();

    }

    private void Enemy1()
    {
        //Shooti
        // Shoot();
        //Walki
    }

    private void Shoot()
    {
        if (!canShoot) return;
        canShoot = false;
        StartCoroutine(ShootCooldown());

        GameObject go = Instantiate(bullets[typeOfEnemy]);
        BulletScript bs = go.GetComponent<BulletScript>();
        bs.target = Movement.Position;
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown[typeOfEnemy]);
        canShoot = true;
    }

    private void MoveRandomDirection()
    {
        direction = Random.Range(-1, 1);


        StartCoroutine(MOve());
        //Pick new target between  x 0 and x max
    }


    private IEnumerator MOve()
    {
        while (true)
        {
            var temp = transform.position;
            temp.x += moveSpeed[typeOfEnemy] * direction;
            Debug.Log(moveSpeed[typeOfEnemy]);
            transform.position = temp;
        }
        yield return new WaitForSeconds(moveTime[typeOfEnemy]);
        StartCoroutine(WaitABit());
    }

    private IEnumerator WaitABit()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 2));
        MoveRandomDirection();
    }
}
