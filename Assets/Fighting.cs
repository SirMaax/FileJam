using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fighting : MonoBehaviour
{
    [Header("Current Weapon")] 
    public int currentWeapon;
    public bool canShot;
    

    [Header("Weapon1")]
    public bool charging = false;
    public double chargeBegin;
    [SerializeField] private float chargeTime1;
    [SerializeField] private float chargeTime2;
    [Header("WeaponInfo")] 
    public int[] dmgList;
    public float[] Cooldown;
    public GameObject[] BulletPreFabs;
    
    [Header("Refs")] private PolygonCollider2D collider;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject go = col.gameObject;
        if (go.tag.Equals("Enemy"))
        {
            go.GetComponent<Enemy>().Hit(dmgList[currentWeapon]);
        }
    }

    IEnumerator WeaponCooldown()
    {
        yield return new WaitForSeconds(Cooldown[currentWeapon]);
        canShot = true;
    }

    public void Shot(InputAction.CallbackContext context)
    {
        if (!canShot) return;
        if (context.canceled && canShot && charging)
        {
            double timeCharged = Time.time - chargeBegin;
            if (timeCharged >= chargeTime2)
            {
                //HeavyBlow
                Debug.Log("heavyblow");
            }else if (timeCharged >= chargeTime1)
            {
                //LightBlow
                Debug.Log("lightBlow");
            }
            else
            {
                //No Blow
                charging = false;
                Debug.Log("no blow");
                return;
                
            }
            
        }
        else if (context.performed || context.canceled) return;
        
        if (currentWeapon == 0)
        {
            charging = true;
            chargeBegin = Time.time;
        }
        else
        {
            //Do the pew pew
            GameObject go = Instantiate(BulletPreFabs[currentWeapon], transform.position, Quaternion.identity); 
        }
        
        canShot = true;
        StartCoroutine(WeaponCooldown());
        Debug.Log("shots");
    }

    

}
