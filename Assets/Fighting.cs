using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
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

    [SerializeField] private float maxCharge;
    [SerializeField] private float maxCharge2;

    [SerializeField] private float maxChargeAfterHit;
    [SerializeField] private float maxChargeAfterHit2;


        [SerializeField] private float changeRate;
        [SerializeField] private float angleAfterhit;
            [SerializeField] private float angleAfterhit2;

        [SerializeField] private float getToBaseCharge;
        public bool hitting = false;

        public bool getToBase = false;

        public GameObject chageParticle;

        public bool onceChage = false;
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
        CheckMouseCursorStuff();
        
        double timeCharged2 = Time.time - chargeBegin;

        if (timeCharged2 >= chargeTime2 && !onceChage && chargeBegin != 0 && charging)
        {
            onceChage = true;
            chageParticle.GetComponent<ParticleSystem>().Play();
        }
        if (charging)
        {
            if (flipOnce)
            {
                angle = Mathf.MoveTowards(angle, maxCharge2,  changeRate);

            }
            else
            {
                angle = Mathf.MoveTowards(angle, maxCharge, changeRate);

            }
        }

        if (hitting)
        {
            if (flipOnce)
            {
                angle = Mathf.MoveTowards(angle, angleAfterhit2, maxChargeAfterHit2);
                if(angle >= angleAfterhit2)
                {
                
                    hitting = false;
                    getToBase = true;
                    // angle = 0;
                }
            }
            else
            {
                angle = Mathf.MoveTowards(angle,  angleAfterhit , maxChargeAfterHit);
                if(angle <= angleAfterhit)
                {
                
                    hitting = false;
                    getToBase = true;
                    // angle = 0;
                }
            }
            // if (Mathf.Abs(angle - angleAfterhit) <= maxChargeAfterHit * 1.5f)
            
        }

        if (getToBase)
        {
            angle = Mathf.MoveTowards(angle, 0, getToBaseCharge);
            if (flipOnce && angle <= 0)
            {
                getToBase = false;

            }
            else if (angle >= 0 && !flipOnce)
            {
                getToBase = false;
            }
        }
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
        if (hitting || getToBase) return;
        if (!canShot) return;


        
        if (context.canceled && canShot && charging)
        {
            double timeCharged = Time.time - chargeBegin;
            if (timeCharged >= chargeTime2)
            {
                chageParticle.GetComponent<ParticleSystem>().Stop();
                onceChage = false;

                //HeavyBlow
                Debug.Log("heavyblow");
                blow = 0;
                collider.enabled = true;
                StartCoroutine(DisableCollider());
                ResetRotation();
                GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(1);
                GameObject.FindWithTag("Player").GetComponent<PlayerManager>().DecreaseFileSize(5);
                hitting = true;
                charging = false;
                return;
            }
            else if (timeCharged >= chargeTime1)
            {
                //LightBlow
                blow = 1;
                GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(1);
                chageParticle.GetComponent<ParticleSystem>().Stop();
                onceChage = false;
                Debug.Log("lightBlow");
                ResetRotation();
                StartCoroutine(DisableCollider());
                collider.enabled = true;
                //Reset Rotation
                GameObject.FindWithTag("Player").GetComponent<PlayerManager>().DecreaseFileSize(5);
                hitting = true;
                charging = false;

                return;
            }
            else
            {
                //No Blow
                charging = false;
                Debug.Log("no blow");
                ResetRotation();
                angle = 0;
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
            GameObject.FindWithTag("Player").GetComponent<PlayerManager>().DecreaseFileSize(5);
        }

        canShot = false;
        StartCoroutine(WeaponCooldown());
    }


    [Header("Test")] [SerializeField] private Vector3 test;
    [SerializeField] private Vector3 axis;
    [SerializeField] private float angle;
    [SerializeField] private Vector3 axis2;
    [SerializeField] private float angle2;

    private void Rotate()
    {

            test = (Vector3) InputManager.MousePosition -  transform.position;
            test = Quaternion.AngleAxis(angle, axis) * test; 
        
        Debug.DrawLine(transform.position,InputManager.MousePosition,Color.green);

        // Debug.DrawLine(transform.position,InputManager.aimingMouse,Color.red);

        transform.right =  test;


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
        
        
        // extraRot = Vector3.zero;
        // // extraRot = Quaternion.identity;
        // // weaponGameObject.transform.rotation = startRot;
        // charging = false;
    }

    private void AfterHit()
    {
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

    }

    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(0.1f);
        collider.enabled = false;
    }

    public bool flipOnce = false;
    public Vector3 customVec;
    private void CheckMouseCursorStuff()
    
    { if (Cursor.ActualMousePos.x < transform.position.x && !flipOnce && !getToBase)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
            transform.GetChild(0).transform.Translate(customVec);
                flipOnce = true;
            }

        if (Cursor.ActualMousePos.x > transform.position.x && flipOnce &&  !getToBase)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            transform.GetChild(0).transform.Translate(-customVec);


            flipOnce = false;
        }
    }
}