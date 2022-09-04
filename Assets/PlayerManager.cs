using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("PlayerINfo")] [SerializeField]
    private float health;

    [SerializeField] private float fileSize;
    private float startFileSize;

    [Header("FileAttributesStuff")] public float[] strengthOfAttributes;
    public int[] typeOfAttribut;

    [Header("Refs")] public GameObject ps;
    public GameObject fileSizeMeter;
    private float fileSizeMeterLength;
    private float fileSizeMeterOriginalXPos;
    private FileManager _fileManager;

    [Header("Tes")] public bool test1;
    public bool test2;
    public float testValue;

    void Start()
    {
        startFileSize = fileSize;
        fileSizeMeterLength = fileSizeMeter.transform.localScale.x;
        fileSizeMeterOriginalXPos = fileSizeMeter.transform.position.x;
        _fileManager = GameObject.FindWithTag("FileManager").GetComponent<FileManager>();
        CheckAttributeHowStrong();

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
        ps.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        ps.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        IncreaseFileSize(10f);
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
        for (int i = 0; i < _fileManager.filePostions.Length; i++)
        {
            if (_fileManager.filePostions[i] == null) continue;
            float n1 = (0.33333333f * (i + 1)) - percent;
            if (n1 < 0)
            {
                strengthOfAttributes[i] = _fileManager.filePostions[i].strengthOfAttribute * 1;
                typeOfAttribut[i] = _fileManager.filePostions[i].typeOfAttribute;
            }
            else
            {
                float number = (0.33333333f - n1) / 0.33333333f;
                number = Mathf.Clamp(number, 0, 1);
                strengthOfAttributes[i] = _fileManager.filePostions[i].strengthOfAttribute * number;
                
            }
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
}