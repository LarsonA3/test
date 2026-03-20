using UnityEngine;
public class Game : MonoBehaviour
{
    // set in inspector
    public float enemySpawnDelay;
    public float powerupSpawnDelay;
    public float asteroidSpawnDelay;

    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject AsteroidPrefab;

    //private
    private float enemySpawnTimer;
    private float powerupSpawnTimer;
    private float asteroidSpawnTimer;

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

    private void spawnAsteroid()
    {
        Vector3 enemySpawnPt = new Vector3(Random.Range(enemySpawnRange.bounds.min.x, enemySpawnRange.bounds.max.x),
            Random.Range(enemySpawnRange.bounds.min.y, enemySpawnRange.bounds.max.y),
            0);
        Instantiate(AsteroidPrefab, enemySpawnPt, Quaternion.identity);
    }

    private void spawnPowerup()
    {
        Vector3 enemySpawnPt = new Vector3(Random.Range(enemySpawnRange.bounds.min.x, enemySpawnRange.bounds.max.x),
            Random.Range(enemySpawnRange.bounds.min.y, enemySpawnRange.bounds.max.y),
            0);
        Instantiate(powerupPrefab, enemySpawnPt, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnDelay)
        {
            spawnEnemy();
            enemySpawnTimer = 0;
        }

        powerupSpawnTimer += Time.deltaTime;
        if (powerupSpawnTimer >= powerupSpawnDelay)
        {
            spawnPowerup();
            powerupSpawnTimer = 0;
        }

        asteroidSpawnTimer += Time.deltaTime;
        if (asteroidSpawnTimer >= asteroidSpawnDelay)
        {
            spawnAsteroid();
            asteroidSpawnTimer = 0;
        }
    }
}