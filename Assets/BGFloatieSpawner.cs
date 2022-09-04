using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGFloatieSpawner : MonoBehaviour
{

    [SerializeField] Sprite[] floatieSprites;

    [SerializeField] float spawnSpeedMin;
    [SerializeField] float spawnSpeedMax;
    [SerializeField] float minPos;
    [SerializeField] float maxPos;
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;

    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] GameObject floatie;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnFloaties());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawnFloaties(){
        while(true){
            
            Vector2 floatiePos = new Vector2(transform.position.x, RandomGaussian(minPos, maxPos));
            GameObject newFloatie = Instantiate(floatie, floatiePos, Quaternion.identity);

            int rdmSprite = (int)Random.Range(0,floatieSprites.Length);
            newFloatie.GetComponent<SpriteRenderer>().sprite = floatieSprites[rdmSprite];

            newFloatie.transform.localScale *= RandomGaussian(minScale, maxScale);

            newFloatie.GetComponent<Floatie>().speed = RandomGaussian(minSpeed, maxSpeed);

            yield return new WaitForSeconds(RandomGaussian(spawnSpeedMin, spawnSpeedMax));

        }
    }



    public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
    {
         float u, v, S;
     
         do
         {
             u = 2.0f * UnityEngine.Random.value - 1.0f;
             v = 2.0f * UnityEngine.Random.value - 1.0f;
             S = u * u + v * v;
         }
         while (S >= 1.0f);
     
         // Standard Normal Distribution
         float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
     
         // Normal Distribution centered between the min and max value
         // and clamped following the "three-sigma rule"
         float mean = (minValue + maxValue) / 2.0f;
         float sigma = (maxValue - mean) / 3.0f;
         return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
    }
}
