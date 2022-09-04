using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawningPoints;
    public float[] changeOfSpawning;

    [Header("Wave")] public int currentWave;
    [SerializeField] private float timeBetweenRounds;
    [Header("Enemy")] public GameObject[] enemy;
    private bool once = false;
    private bool spawning = false;
    /// <summary>
    /// ////////////////
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        SpawnNormal();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawning && !PlayerManager.fileOpen)
        {
            
            SpawnNormal();
            
        }
        CheckHowManyEnemies();
    }

    private void SpawnNormal()
    {
        spawning = true;
        foreach (var pos in spawningPoints)
        {
            if (Random.value < changeOfSpawning[currentWave])
            {
                int enemyType = Random.Range(0, 4);
                GameObject go = Instantiate(enemy[enemyType], pos.position, Quaternion.identity);
                go.GetComponent<EnemyAi>().typeOfEnemy = enemyType;
                
            }
        }
    }

    private void CheckHowManyEnemies()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0 && !once)
        {
            once = true;
            GameObject.FindWithTag("FileManager").GetComponent<FileManager>().Show(true);
            spawning = false;
            GameObject.FindWithTag("Player").GetComponent<PlayerManager>().health =
                GameObject.FindWithTag("Player").GetComponent<PlayerManager>().startHealth;
        }

    }

    private IEnumerator NextRound()
    {
        
        yield return new WaitForSeconds(timeBetweenRounds);
        currentWave++;
        SpawnNormal();
        once = false;
    }
}
