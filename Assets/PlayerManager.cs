using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("PlayerINfo")] 
    [SerializeField] private float health;
    [SerializeField] private float fileSize;
    private float startFileSize;
    
    
    [Header("Refs")] public GameObject ps;
    public GameObject fileSizeMeter;
    private float fileSizeMeterLength;
    private float fileSizeMeterOriginalXPos;

    [Header("Tes")] public bool test1;
    public bool test2;
    public float testValue;
    void Start()
    {
        startFileSize = fileSize;
        fileSizeMeterLength = fileSizeMeter.transform.localScale.x;
        fileSizeMeterOriginalXPos = fileSizeMeter.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (test1)
        {
            test1 = false;
            DecreaseFileSize(testValue);
        }

        if (test2)
        {
            test2 = false;
            IncreaseFileSize(testValue);
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
        newSize.x =  fileSize * fileSizeMeterLength/startFileSize;
        fileSizeMeter.transform.localScale = newSize;
        
        //Move to the left
        Vector3 pos = fileSizeMeter.transform.position;
        // float temp1 = 1 - (fileSize / startFileSize);
        // float temp2 = temp1 * 100;
        // float temp3 = fileSizeMeterLength / 100;
        // float temp4 = temp3 * temp2;
        float delta = ((fileSizeMeterLength/100) * ((1 - fileSize/ startFileSize) * 100));
        delta /= 2;
        pos.x = fileSizeMeterOriginalXPos - delta;
        fileSizeMeter.transform.position = pos;
    }
}
