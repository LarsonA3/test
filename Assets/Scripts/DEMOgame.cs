using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Game : MonoBehaviour
{
    // Set in inspector
    public float enemySpawnDelay;
    public float powerupSpawnDelay;
    public float asteroidSpawnDelay;

    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject AsteroidPrefab;
    public GameObject bossPrefab;         // assign in inspector

    public Volume globalVolume;
    private LensDistortion lensDist;
    private ChromaticAberration chromAb;

    private float enemySpawnTimer;
    private float powerupSpawnTimer;
    private float asteroidSpawnTimer;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI gameOverlevelText;
    public TextMeshProUGUI nextlvltext;
    public TextMeshProUGUI bossText;      // assign in inspector - for "BOSS INCOMING" message
    public GameObject WinScreen;          // assign in inspector
    public TMPro.TextMeshProUGUI WinScoreText; // assign in inspector - score text on win screen

    public BoxCollider2D enemySpawnRange;

    public GameObject warpstars;

    public float intensity = 1f;

    private bool normalSpawning = true;

    void Start()
    {
        int levelNumber = GameManager.instance.CurrentLevel;
        levelText.text = "LEVEL: " + levelNumber;
        gameOverlevelText.text = levelNumber.ToString();

        enemySpawnDelay = 2.0f / (1f + (levelNumber * intensity));
        asteroidSpawnDelay = 3.0f / (1f + (levelNumber * (intensity * 0.5f)));

        if (nextlvltext != null) nextlvltext.gameObject.SetActive(false);
        if (bossText != null) bossText.gameObject.SetActive(false);
        if (WinScreen != null) WinScreen.SetActive(false);

        Volume vol = globalVolume.GetComponent<Volume>();
        vol.profile.TryGet(out lensDist);
        vol.profile.TryGet(out chromAb);
        lensDist.intensity.overrideState = true;
        chromAb.intensity.overrideState = true;
        lensDist.intensity.value = 0f;
        chromAb.intensity.value = 0f;

        // Subscribe to GameManager events
        if (GameManager.instance != null)
        {
            GameManager.instance.OnLevelUp += HandleLevelUp;
            GameManager.instance.OnBossLevel += HandleBossLevel;
            Debug.Log("DEMOgame: Successfully subscribed to GameManager events");
        }
        else
        {
            Debug.LogError("DEMOgame: GameManager.instance is NULL in Start! Check script execution order.");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid errors
        if (GameManager.instance != null)
        {
            GameManager.instance.OnLevelUp -= HandleLevelUp;
            GameManager.instance.OnBossLevel -= HandleBossLevel;
        }
    }

    private void HandleLevelUp(int newLevel)
    {
        StartCoroutine(LevelUpSequence(newLevel));
    }

    private void HandleBossLevel()
    {
        StartCoroutine(BossLevelSequence());
    }

    private void spawnEnemy()
    {
        Vector3 spawnPt = RandomSpawnPoint();
        Instantiate(enemyPrefab, spawnPt, Quaternion.identity);
    }

    private void spawnAsteroid()
    {
        Vector3 spawnPt = RandomSpawnPoint();
        Instantiate(AsteroidPrefab, spawnPt, Quaternion.identity);
    }

    private void spawnPowerup()
    {
        Vector3 spawnPt = RandomSpawnPoint();
        Instantiate(powerupPrefab, spawnPt, Quaternion.identity);
    }

    private Vector3 RandomSpawnPoint()
    {
        return new Vector3(
            Random.Range(enemySpawnRange.bounds.min.x, enemySpawnRange.bounds.max.x),
            Random.Range(enemySpawnRange.bounds.min.y, enemySpawnRange.bounds.max.y),
            0);
    }

    void Update()
    {
        if (!normalSpawning) return;

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

    IEnumerator LevelUpSequence(int newLevel)
    {
        normalSpawning = false;

        // Update spawn delays for new level
        enemySpawnDelay = 2.0f / (1f + (newLevel * intensity));
        asteroidSpawnDelay = 3.0f / (1f + (newLevel * (intensity * 0.5f)));

        // Update UI
        levelText.text = "LEVEL: " + newLevel;
        gameOverlevelText.text = newLevel.ToString();

        // Warp effect
        StartCoroutine(WarpEffect());

        // Show level text
        if (nextlvltext != null)
        {
            nextlvltext.text = "LEVEL " + newLevel;
            nextlvltext.gameObject.SetActive(true);
            nextlvltext.GetComponent<AudioSource>()?.Play();
        }

        yield return new WaitForSeconds(5f);

        if (nextlvltext != null) nextlvltext.gameObject.SetActive(false);

        // Reset timers and resume spawning
        enemySpawnTimer = 0;
        asteroidSpawnTimer = 0;
        powerupSpawnTimer = 0;
        normalSpawning = true;
    }

    IEnumerator BossLevelSequence()
    {
        normalSpawning = false;

        levelText.text = "LEVEL: 4";
        gameOverlevelText.text = "4";

        // Warp effect
        StartCoroutine(WarpEffect());

        // Show boss incoming message
        if (bossText != null)
        {
            bossText.text = "BOSS INCOMING";
            bossText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("bossText is not assigned in DEMOgame inspector!");
        }

        yield return new WaitForSeconds(5f);

        if (bossText != null) bossText.gameObject.SetActive(false);

        // Spawn boss
        if (bossPrefab != null)
        {
            float spawnY = Random.Range(-2f, 2f);
            GameObject bossObj = Instantiate(bossPrefab, new Vector3(12f, spawnY, 0), Quaternion.identity);

            // Pass win screen reference to boss
            BossEnemy bossScript = bossObj.GetComponent<BossEnemy>();
            if (bossScript != null)
            {
                bossScript.WinScreen = WinScreen;
                bossScript.WinScoreText = WinScoreText;
            }
        }
        else
        {
            print("BOSS PREFAB NOT ASSIGNED IN INSPECTOR");
        }
    }

    IEnumerator WarpEffect()
    {
        float rampDur = 1.0f;
        float elapsed = 0f;
        warpstars.gameObject.SetActive(true);

        while (elapsed < rampDur)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rampDur;
            float curve = Mathf.Sin(t * Mathf.PI * 0.5f);
            lensDist.intensity.value = curve * -0.5f;
            chromAb.intensity.value = curve * 1.0f;
            yield return null;
        }

        lensDist.intensity.value = -0.5f;
        chromAb.intensity.value = 1.0f;
        globalVolume.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(5f);

        warpstars.gameObject.SetActive(false);

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