using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    // Start is called before the first frame update
    
    [Header("Type of Enemy")] 
    [Range(0,3)]public int typeOfEnemy;

    public int[] healthOfEnemyType;
    public float[] shootCooldown;
    public GameObject[] bullets;

    [Header("Bullet status")] 
    public GameObject bulletPosition;
//Distance to next target unitl a new one is picked
    [Header("Status")] public bool canShoot;
    public bool inRange;

    private int health;
    void Start()
    {
        health = healthOfEnemyType[typeOfEnemy];
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
        if (!canShoot || !inRange) return;
        canShoot = false;
        StartCoroutine(ShootCooldown());

        GameObject go = Instantiate(bullets[typeOfEnemy],bulletPosition.transform.position,Quaternion.identity);
        BulletScript bs = go.GetComponent<BulletScript>();
        bs.target = Movement2.Position;
        bs.typeOfBullet = 1;
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown[typeOfEnemy]);
        canShoot = true;
    }

    public void setInRange(bool inRange){
        this.inRange = inRange;
    }

    public void takeDamage(int damage){
        health -= damage;
        StartCoroutine(showDamageFeedback());
        if(health < 0)
            die();
    }

    private void die(){
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col2){
        Debug.Log("Collided");
        if(col2.gameObject.tag == "Bullet"){
            if(col2.GetComponent<BulletScript>().typeOfBullet == 0){
                Destroy(col2.gameObject);
                // Change depending on weapon shoooting da bullet
                takeDamage(col2.GetComponent<BulletScript>().damage);
            }

        }
    }

    private IEnumerator showDamageFeedback(){
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }



    
}
