using UnityEngine;

public class Game : MonoBehaviour
{
    // set in inspector
    public float enemySpawnDelay;
    public GameObject enemyPrefab;
    //private
    private float enemySpawnTimer;

    public BoxCollider2D enemySpawnRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void spawnEnemy()
    {
        Vector3 enemySpawnPt = new Vector3(Random.Range(enemySpawnRange.bounds.min.x, enemySpawnRange.bounds.max.x),
            Random.Range(enemySpawnRange.bounds.min.y, enemySpawnRange.bounds.max.y),
            0);
        Instantiate(enemyPrefab, enemySpawnPt, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnDelay)
        {
            //spawn enemy
            spawnEnemy();
            enemySpawnTimer = 0;
        }
     }
}
