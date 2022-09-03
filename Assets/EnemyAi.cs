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
//Distance to next target unitl a new one is picked
    [Header("Status")] public bool canShoot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (typeOfEnemy == 0) Enemy1();
        
    }

    private void Enemy1()
    {
        //Shooti
        Shoot();
    }

    private void Shoot()
    {
        if (!canShoot) return;
        canShoot = false;
        StartCoroutine(ShootCooldown());

        GameObject go = Instantiate(bullets[typeOfEnemy],transform.position,Quaternion.identity);
        BulletScript bs = go.GetComponent<BulletScript>();
        bs.target = Movement2.Position;
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown[typeOfEnemy]);
        canShoot = true;
    }

    
}
