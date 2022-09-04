using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    [SerializeField] GameObject healthMeter;
    private float playerHealth;
    private float prevHealth;
    private float startPlayerHealth;
    private float healthMeterOriginalXPos;

    private float healthMeterLength;
    private float startLength;
    void Start()
    {
        healthMeterLength = healthMeter.transform.localScale.x;
        startPlayerHealth = playerHealth = player.GetComponent<PlayerManager>().health;
        healthMeterOriginalXPos  = healthMeter.transform.position.x;

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
        float delta = ((healthMeterLength / 100) * ((1 - playerHealth / startPlayerHealth) * 100));
        delta /= 2;
        pos.x = healthMeterOriginalXPos - delta;
        healthMeter.transform.position = pos;
    }
}
