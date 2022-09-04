using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fighting : MonoBehaviour
{
    [Header("Current Weapon")] public int currentWeapon;
    public bool canShot;


    [Header("Weapon1")] public bool charging = false;
    public GameObject weaponGameObject;
    public float rotationCurrent;
    public double chargeBegin;
    private Quaternion startRot;
    [SerializeField] private float deltaRotation;
    [SerializeField] private float chargeTime1;
    [SerializeField] private float chargeTime2;
    [SerializeField] public Vector3 targetRot;
    private Vector3 thisRotation;
    public Vector3 extraRot;
    
    [Header("WeaponInfo")] public float[] dmgList;
    public float[] Cooldown;
    public GameObject[] BulletPreFabs;
    public Sprite[] sprites;
    [Header("Refs")] private PolygonCollider2D collider;
    private PlayerManager _playerManager;
    [Header("ExtraInforo")] public float minusCoolDown;
    public float perCentageCooldown;
    public float extraDmg;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<PolygonCollider2D>();
        _playerManager = transform.parent.parent.GetComponent<PlayerManager>();
        UpdateSprite(currentWeapon);
        startRot = weaponGameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        UpdateFileValues();
        if (charging) ChargingWeapon();
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
        yield return new WaitForSeconds(Cooldown[currentWeapon] - perCentageCooldown);
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
                ResetRotation();
                return;
            }
            else if (timeCharged >= chargeTime1)
            {
                //LightBlow
                Debug.Log("lightBlow");
                ResetRotation();

                //Reset Rotation
                return;
            }
            else
            {
                //No Blow
                charging = false;
                Debug.Log("no blow");
                ResetRotation();
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
            go.GetComponent<BulletScript>().target = Cursor.ActualMousePos;
            go.GetComponent<BulletScript>().damage = dmgList[currentWeapon] + extraDmg;
            go.GetComponent<BulletScript>().typeOfBullet = 0;
        }

        canShot = false;
        StartCoroutine(WeaponCooldown());
    }

    private void Rotate()
    {
        // transform.right = (Vector3)InputManager.aimingMouse - transform.position;
       thisRotation = Cursor.ActualMousePos - transform.position;
       Vector3 test = transform.rotation * extraRot;
       test.z = 0;
       // test.x = Mathf.Abs(test.x);
       // test.y = Mathf.Abs(test.y);
       Vector3 finalVec = Cursor.ActualMousePos - (test);
        transform.right = finalVec - transform.position;
        // transform.rotation *= extraRot;
    }

    private void UpdateFileValues()
    {
        
        minusCoolDown = _playerManager.GetCooldownIncrease();
        float percentage = (Cooldown[currentWeapon] / 100) * minusCoolDown;
        perCentageCooldown = percentage;
        float temp = _playerManager.GetStrengthIncrease();
        percentage = (dmgList[currentWeapon] / 100f) * temp;
        extraDmg = percentage;
    }

    private void UpdateSprite(int index)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[currentWeapon];
        
    }

    private void ChargingWeapon()
    {
        //change rotation back till maximum
        //Start rot is 90
       extraRot = Vector3.MoveTowards(extraRot,targetRot,deltaRotation);
        // extraRot =  newRot ;
    }

    private void ResetRotation()
    {
        extraRot = Vector3.zero;
        // extraRot = Quaternion.identity;
        // weaponGameObject.transform.rotation = startRot;
        charging = false;
    }
}