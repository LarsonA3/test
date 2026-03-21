using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Game : MonoBehaviour
{
    // set in inspector
    public float enemySpawnDelay;
    public float powerupSpawnDelay;
    public float asteroidSpawnDelay;

    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject AsteroidPrefab;

    public Volume globalVolume;
    private LensDistortion lensDist;
    private ChromaticAberration chromAb;

    //private
    private float enemySpawnTimer;
    private float powerupSpawnTimer;
    private float asteroidSpawnTimer;

    public TextMeshProUGUI levelText;
    public int levelNumber = 1;
    public TextMeshProUGUI gameOverlevelText;
    public TextMeshProUGUI nextlvltext;

    public BoxCollider2D enemySpawnRange;

    public GameObject warpstars;




    public float intensity = 1f; // difficulty - adjust THIS for difficulty change - affects spawn timers


    private bool normalSpawning = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelNumber = 1;
        levelText.text = "LEVEL: " + levelNumber;
        gameOverlevelText.text = levelNumber.ToString();
        enemySpawnDelay = 2.0f / (1f + (levelNumber * intensity));
        asteroidSpawnDelay = 3.0f / (1f + (levelNumber * (intensity * 0.5f)));
        nextlvltext.gameObject.SetActive(false);

        Volume vol = globalVolume.GetComponent<Volume>();
        vol.profile.TryGet(out lensDist);
        vol.profile.TryGet(out chromAb);
        lensDist.intensity.overrideState = true;
        chromAb.intensity.overrideState = true;
        lensDist.intensity.value = 0f;
        chromAb.intensity.value = 0f;

        StartCoroutine(LevelHandler());
        
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
        if (normalSpawning == true) // if spawning spawn (shocker ik)
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



    IEnumerator LevelHandler()
    {
        while (true)
        {

            print("LEVEL: Playing current level for x seconds");
            yield return new WaitForSeconds(60f);

            normalSpawning = false;
            if (GameManager.instance.Lives < 1) yield return null;

            ///////////////BOSS LOGIC WOULD BE IN HERE

            print("LEVEL: turning off spawning and adjusting values");
            // set new timers based on the next level
            enemySpawnDelay = 2.0f / (1f + ((levelNumber + 1) * intensity));
            asteroidSpawnDelay = 3.0f / (1f + ((levelNumber + 1) * (intensity * 0.5f)));

            yield return new WaitForSeconds(5f); // wait for enemies to get off screen
            print("LEVEL: enemies assumed off screen, incrementing level");
            levelNumber++;
            levelText.text = "LEVEL: " + levelNumber;
            gameOverlevelText.text = levelNumber.ToString();

            print("LEVEL: running warp effect");
            ////////////// WARPEFFECT HERE//////////////
            StartCoroutine(WarpEffect());

            print("LEVEL:transition period");
            //show text
            nextlvltext.text = "LEVEL " + levelNumber.ToString();
            nextlvltext.gameObject.SetActive(true);
            nextlvltext.GetComponent<AudioSource>().Play();


            yield return new WaitForSeconds(5f); // Transition period
            nextlvltext.gameObject.SetActive(false);

            print("LEVEL: resetting timers, starting next level...");
            // RESET TIMERS
            enemySpawnTimer = 0;
            asteroidSpawnTimer = 0;
            powerupSpawnTimer = 0;

            normalSpawning = true;
        }
    }


    IEnumerator WarpEffect()
    {
        float rampDur = 1.0f;
        float elapsed = 0f;
        warpstars.gameObject.SetActive(true);

        // go to peak
        while (elapsed < rampDur)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rampDur;
            float curve = Mathf.Sin(t * Mathf.PI * 0.5f);
            lensDist.intensity.value = curve * -0.5f;
            chromAb.intensity.value = curve * 1.0f;
            yield return null;
        }

        // hold at peak
        lensDist.intensity.value = -0.5f;
        chromAb.intensity.value = 1.0f;
        globalVolume.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(5f);

        warpstars.gameObject.SetActive(false);

        // ramp down after
        elapsed = 0f;
        while (elapsed < rampDur)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rampDur;
            float curve = 1f - Mathf.Sin(t * Mathf.PI * 0.5f);
            lensDist.intensity.value = curve * -0.5f;
            chromAb.intensity.value = curve * 1.0f;
            yield return null;
        }

        lensDist.intensity.value = 0f;
        chromAb.intensity.value = 0f;
    }
}