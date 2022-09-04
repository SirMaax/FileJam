using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] float test;

    // Start is called before the first frame update
    [SerializeField] GameObject player;
    [SerializeField] GameObject healthMeter;
    private float playerHealth;
    private float prevHealth;
    private float startPlayerHealth;
    private float healthMeterOriginalXPos;

    private float healthMeterLength;
    public float healthMeterLength2;

    private float startLength;
    void Start()
    {
        healthMeterLength = healthMeter.transform.localScale.x;
        startPlayerHealth = playerHealth = player.GetComponent<PlayerManager>().health;
        healthMeterOriginalXPos  = healthMeter.transform.position.x;
        healthMeterLength2 = healthMeter.GetComponent<BoxCollider2D>().bounds.extents.x * 2;
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = player.GetComponent<PlayerManager>().health;
        if(playerHealth != prevHealth)
            UpdateHP();
        prevHealth = playerHealth;
    }

    void UpdateHP(){
        Vector3 newSize = healthMeter.transform.localScale;
        newSize.x = playerHealth * healthMeterLength / startPlayerHealth;
        healthMeter.transform.localScale = newSize;

        //Move to the left
        Vector3 pos = healthMeter.transform.position;
        float delta = ((healthMeterLength2 / 100) * ((1 - playerHealth / startPlayerHealth) * test));
        delta /= 2;
        pos.x = healthMeterOriginalXPos - delta;
        healthMeter.transform.position = pos;
    }
}
