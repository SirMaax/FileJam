using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public GameObject[] guns;
    [Header("ExtraInforo")] public float minusCoolDown;
    public float perCentageCooldown;
    public float extraDmg;
    private int blow;
    private int direction = -1;

    public int test1;

    public int test2;

    public Sprite gun1;
    public Sprite gun2;
    public GameObject gun;

    public bool[] bools;
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
        

        Flip();
        Rotate();
        UpdateFileValues();
        if (charging) ChargingWeapon();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject go = col.gameObject;
        if (go.tag.Equals("Enemy"))
        {
            if(blow == 0)            go.GetComponent<EnemyAi>().takeDamage(dmgList[currentWeapon]);
            if(blow == 1)            go.GetComponent<EnemyAi>().takeDamage(dmgList[currentWeapon]/2);

            blow = -1;
        }
        
        
    }

    IEnumerator WeaponCooldown()
    {
        gun.GetComponent<SpriteRenderer>().sprite = gun2;
        yield return new WaitForSeconds(Cooldown[currentWeapon] - perCentageCooldown);
        gun.GetComponent<SpriteRenderer>().sprite = gun1;
        canShot = true;
    }

    public void Shot(InputAction.CallbackContext context)
    {
        if (PlayerManager.fileOpen) return;

        if (!canShot) return;
        if (context.canceled && canShot && charging)
        {
            double timeCharged = Time.time - chargeBegin;
            if (timeCharged >= chargeTime2)
            {
                //HeavyBlow
                Debug.Log("heavyblow");
                blow = 0;
                collider.enabled = true;
                StartCoroutine(DisableCollider());
                ResetRotation();
                GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(1);

                return;
            }
            else if (timeCharged >= chargeTime1)
            {
                //LightBlow
                blow = 1;
                GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(1);

                Debug.Log("lightBlow");
                ResetRotation();
                StartCoroutine(DisableCollider());
                collider.enabled = true;
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
            GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(0);
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
        
        if (PlayerManager.fileOpen) return;

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

    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (PlayerManager.fileOpen) return;

        if (context.performed)
        {
            currentWeapon = currentWeapon == 0 ? 1 : 0;
            UpdateSprite(currentWeapon);
            if (currentWeapon == 1)
            {
                guns[0].SetActive(false);
                guns[1].SetActive(true);
            }
            else
            {
                guns[1].SetActive(false);
                guns[0].SetActive(true);
            }
        }
    }

    private void Flip()
    {
        // int tempdir= Movement2.Position.x - InputManager.MousePosition.x  < 0? -1 : 1;
        // if (tempdir == direction) return;
        // Debug.Log("flipped");
        // direction = tempdir;
        // guns[0].GetComponent<SpriteRenderer>().flipX = direction == -1;
        // guns[0].GetComponent<SpriteRenderer>().flipY = direction == -1;
        // // guns[1].GetComponent<SpriteRenderer>().flipX = direction == test1;
        // // guns[1].GetComponent<SpriteRenderer>().flipY = direction == test2;
        // if (direction < -1)
        // {
        //     guns[1].GetComponent<SpriteRenderer>().flipX = bools[0];
        //     guns[1].GetComponent<SpriteRenderer>().flipY = bools[1];
        //
        // }
        // else
        // {
        //     guns[1].GetComponent<SpriteRenderer>().flipX = bools[2];
        //     guns[1].GetComponent<SpriteRenderer>().flipY = bools[3];
        // }
    }

    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(0.1f);
        collider.enabled = false;
    }
}