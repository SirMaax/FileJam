using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("PlayerINfo")] [SerializeField]
    public float health;

    [SerializeField] private float fileSize;
    private float startFileSize;

    [Header("FileAttributesStuff")] public float[] strengthOfAttributes;
    public int[] typeOfAttribut;
    public static bool fileOpen = false;
    [Header("Refs")] public GameObject ps;
    public GameObject fileSizeMeter;
    public float fileSizeMeterLength;
    private float fileSizeMeterOriginalXPos;
    private FileManager _fileManager;

    [Header("Tes")] public bool test1;
    public bool test2;
    public float testValue;

    public float startHealth;
    
    
    void Start()
    {
        startHealth = health;
        startFileSize = fileSize;
        fileSizeMeterLength = fileSizeMeter.transform.localScale.x;
        fileSizeMeterOriginalXPos = fileSizeMeter.transform.position.x;
        _fileManager = GameObject.FindWithTag("FileManager").GetComponent<FileManager>();
        CheckAttributeHowStrong();
        fileOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttributeHowStrong();
        if (test1)
        {
            test1 = false;
            DecreaseFileSize(testValue);
            CheckAttributeHowStrong();
        }

        if (test2)
        {
            test2 = false;
            IncreaseFileSize(testValue);
            CheckAttributeHowStrong();
        }
    }

    //Is triggered when Particle is pickedu p
    public void PickUp()
    {
        GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(12);

        ps.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        ps.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        IncreaseFileSize(10f);
        TakeHealth();
    }

    //Decreases fileSize by x percent
    public void DecreaseFileSize(float percent)
    {
        float delta = (startFileSize / 100) * percent;
        fileSize -= delta;
        fileSize = Mathf.Clamp(fileSize, 0, startFileSize);
        UpdateFileSizeUI();
    }

    //Increases fileSize by x percent
    public void IncreaseFileSize(float percent)
    {
        float delta = (startFileSize / 100) * percent;
        fileSize += delta;
        fileSize = Mathf.Clamp(fileSize, 0, startFileSize);
        UpdateFileSizeUI();
    }

    //Sets the length of the bar correct
    public void UpdateFileSizeUI()
    {
        Vector3 newSize = fileSizeMeter.transform.localScale;
        newSize.x = fileSize * fileSizeMeterLength / startFileSize;
        fileSizeMeter.transform.localScale = newSize;

        //Move to the left
        Vector3 pos = fileSizeMeter.transform.position;
        float delta = ((fileSizeMeterLength / 100) * ((1 - fileSize / startFileSize) * 100));
        delta /= 2;
        pos.x = fileSizeMeterOriginalXPos - delta;
        fileSizeMeter.transform.position = pos;
    }

    //How strong are the attributes calculate the percentages
    private void CheckAttributeHowStrong()
    {
        float percent = fileSize / startFileSize;
        if (_fileManager == null) return;
        for (int i = 0; i < _fileManager.filePostions.Length; i++)
        {
            if (_fileManager.filePostions[i] == null)
            {
                strengthOfAttributes[i] = 0;
                continue;
                
            };
            float n1 = (0.33333333f * (i + 1)) - percent;
            if (n1 < 0)
            {
                strengthOfAttributes[i] = _fileManager.filePostions[i].strengthOfAttribute * 1;
            }
            else
            {
                float number = (0.33333333f - n1) / 0.33333333f;
                number = Mathf.Clamp(number, 0, 1);
                strengthOfAttributes[i] = _fileManager.filePostions[i].strengthOfAttribute * number;
                
            }
            typeOfAttribut[i] = _fileManager.filePostions[i].typeOfAttribute;

        }
    }

    /// <summary>
    /// ///////////IMPORTANT
    /// 0 IS SPEED
    /// 1 IS STRNEGTH
    /// 2 IS COOLDOWN
    /// </summary>
    /// <returns></returns>
    public float GetSpeedIncrease()
    {
        for (int i = 0; i < typeOfAttribut.Length; i++)
        {
            if (typeOfAttribut[i] == 0)
            {
                return strengthOfAttributes[i];
            }
        }

        return -1;
    }

    public float GetStrengthIncrease()
    {
        for (int i = 0; i < typeOfAttribut.Length; i++)
        {
            if (typeOfAttribut[i] == 1)
            {
                return strengthOfAttributes[i];
            }
        }

        return -1;
    }

    public float GetCooldownIncrease()
    {
        for (int i = 0; i < typeOfAttribut.Length; i++)
        {
            if (typeOfAttribut[i] == 2)
            {
                return strengthOfAttributes[i];
            }
        }

        return -1;
    }


    public void TakeHealth()
    {
        if(health < startHealth)
            health += 1;
    }
    public void TakeDmg()
    {
        GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(2);
        //Took Dmg
        health -= 1;
        if (health <= 0) Die();
    }

    public void Die()
    {
        GameObject.FindWithTag("Sound").GetComponent<SoundManager>().Play(5);
        GameOver();
    }

    public void OpenFile(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            fileOpen = !fileOpen;
            GameObject.FindWithTag("FileManager").GetComponent<FileManager>().Show(fileOpen);
            //Open file
            Debug.Log(fileOpen);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Bullet"))
        {
            if (col.gameObject.GetComponent<BulletScript>().typeOfBullet == 1)
            {
                TakeDmg();
                Destroy(col.gameObject);
            }
        }
    }

    private void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}