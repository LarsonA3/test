using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Set in inspector
    public float speed = 6f;
    public float LRspeed = 4.5f;
    public int PowerupTime = 15;

    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public Transform bulletSpawnPoint;
    public Transform missileSpawnPoint;
    public GameObject expoPrefab;

    private SpaceShooterInputActions input;
    private int missileAmount = 4;

    public Image missiles1; public Image missiles2; public Image missiles3; public Image missiles4;

    private const float Y_LIMIT = 4.6f;
    private const float X_LIMIT = 9.92f;

    private bool isMultishot = false;
    private bool isInfMissile = false;
    private bool isInfEnergy = false;
    private bool isDoubleShot = false;

    // Banking/tilt animation
    private const float maxTiltAngle = 25f;
    private const float tiltSpeed = 8f;
    private Transform shipVisual; // the Triangle child

    public Slider sliderPower;

    private AudioSource audiosrc;
    public AudioClip firesfx;
    public AudioClip missilesfx;
    public AudioClip powerupsfx;
    public AudioClip gameoversfx;

    // Game Over UI
    public GameObject GameOver;
    public TextMeshProUGUI FinalScoreText;
    public TextMeshProUGUI FinalLevelText;   // assign in inspector - shows "LEVEL REACHED: X"
    public Button ReplayButton;              // assign in inspector
    public Button MainMenuButton;            // assign in inspector

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI poweruptext;
    public TextMeshProUGUI poweruptextchange;

    public ParticleSystem toggleparticle;
    private ParticleSystem.EmissionModule particleJet;
    public ParticleSystem baseJet;

    private bool isExploded = false;
    public ParticleSystem hitfx;

    private void Start()
    {
        input = new SpaceShooterInputActions();
        input.Enable();
        GameOver.SetActive(false);
        this.gameObject.SetActive(true);
        poweruptext.gameObject.SetActive(false);
        poweruptextchange.gameObject.SetActive(false);
        isMultishot = false;
        isInfMissile = false;
        isInfEnergy = false;
        isDoubleShot = false;

        particleJet = toggleparticle.gameObject.GetComponent<ParticleSystem>().emission;
        audiosrc = GetComponent<AudioSource>();
        sliderPower.value = 0;

        // Cache the ship visual child for tilt
        shipVisual = transform.Find("Triangle");

        // Wire up game over buttons
        ReplayButton.onClick.AddListener(OnReplay);
        MainMenuButton.onClick.AddListener(OnMainMenu);
    }

    private void OnReplay()
    {
        SceneManager.LoadScene("ShooterGame");
    }

    private void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        // FIRE BULLET
        if (input.Standard.Fire.WasPressedThisFrame())
        {
            if (GameManager.instance.CanFire() || isInfEnergy)
            {
                audiosrc.clip = firesfx;
                audiosrc.Play();

                if (isMultishot)
                {
                    if (!isInfEnergy) GameManager.instance.RemoveEnergy();
                    Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                    Instantiate(bulletPrefab, new Vector3(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y + 1f, bulletSpawnPoint.position.z), Quaternion.identity);
                    Instantiate(bulletPrefab, new Vector3(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y - 1f, bulletSpawnPoint.position.z), Quaternion.identity);
                }
                else if (isDoubleShot)
                {
                    if (!isInfEnergy) GameManager.instance.RemoveEnergy();
                    Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                    Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                }
                else
                {
                    if (!isInfEnergy) GameManager.instance.RemoveEnergy();
                    Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                }
            }
        }

        // FIRE MISSILE
        if (input.Standard.FireMissile.WasPressedThisFrame())
        {
            if (missileAmount > 0 || isInfMissile)
            {
                audiosrc.clip = missilesfx;
                audiosrc.Play();
                Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
                if (!isInfMissile) missileAmount--;
            }
        }

        if (!isExploded)
        {
            float vertMove = input.Standard.MoveVertically.ReadValue<float>();
            this.transform.Translate(Vector3.up * vertMove * speed * Time.deltaTime, Space.World);

            if (input.Standard.MoveLeft.IsPressed())
                this.transform.Translate(Vector3.left * LRspeed * Time.deltaTime, Space.World);
            if (input.Standard.MoveRight.IsPressed())
                this.transform.Translate(Vector3.right * LRspeed * Time.deltaTime, Space.World);

            particleJet.rateOverTime = (vertMove != 0 || input.Standard.MoveRight.IsPressed()) ? 1000 : 0;

            // Clamp position
            float clampedY = Mathf.Clamp(transform.position.y, -Y_LIMIT, Y_LIMIT);
            float clampedX = Mathf.Clamp(transform.position.x, -X_LIMIT, X_LIMIT);
            transform.position = new Vector3(clampedX, clampedY, 0);

            // Banking tilt - keeps -90 base rotation (sprite faces right) and adds tilt on top
            float targetTilt = vertMove * maxTiltAngle;
            float currentTilt = shipVisual.localRotation.eulerAngles.z;
            if (currentTilt > 180f) currentTilt -= 360f;
            float smoothedTilt = Mathf.Lerp(currentTilt, -90f + targetTilt, Time.deltaTime * tiltSpeed);
            shipVisual.localRotation = Quaternion.Euler(0, 0, smoothedTilt);
        }

        // Missile UI
        missiles1.gameObject.SetActive(missileAmount >= 1);
        missiles2.gameObject.SetActive(missileAmount >= 2);
        missiles3.gameObject.SetActive(missileAmount >= 3);
        missiles4.gameObject.SetActive(missileAmount >= 4);

        // GAME OVER
        if (GameManager.instance.Lives < 1 && !isExploded)
        {
            isExploded = true;

            // Reset tilt before exploding
            shipVisual.localRotation = Quaternion.Euler(0, 0, -90f);

            // Show game over screen with score and level
            GameOver.SetActive(true);
            FinalScoreText.text = "SCORE: " + GameManager.instance.Points;
            FinalLevelText.text = "LEVEL REACHED: " + GameManager.instance.CurrentLevel;

            print("GAME OVER!");

            var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
            Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);

            transform.Find("Triangle").gameObject.SetActive(false);

            audiosrc.clip = gameoversfx;
            audiosrc.Play();

            input.Standard.Disable();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyPhysicalHit"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            var thing = Instantiate(hitfx, gameObject.transform.position, Quaternion.identity, this.transform);
            Destroy(thing, thing.main.duration);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("POWERUP"))
        {
            print("player picked up POWERUP");
            int random = Random.Range(0, 8);

            if (random == 0)
            {
                poweruptextchange.text = "HP REFILL";
                GameManager.instance.RestoreLives();
            }
            else if (random == 1)
            {
                poweruptextchange.text = "MISSILE REFILL";
                missileAmount = 4;
            }
            else if (random == 2)
            {
                poweruptextchange.text = "MULTISHOT";
                isMultishot = true;
                StartCoroutine(WaitandEND(PowerupTime));
            }
            else if (random == 3)
            {
                poweruptextchange.text = "INF MISSILES";
                isInfMissile = true;
                StartCoroutine(WaitandEND(PowerupTime));
            }
            else if (random == 4)
            {
                poweruptextchange.text = "INF ENERGY";
                isInfEnergy = true;
                StartCoroutine(WaitandEND(PowerupTime));
            }
            else if (random == 5)
            {
                poweruptextchange.text = "INVULNERABILITY";
                GameManager.instance.setInvulnerable(true);
                StartCoroutine(WaitandEND(PowerupTime));
            }
            else if (random == 6)
            {
                poweruptextchange.text = "NUKE";
                GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("EnemyPhysicalHit");
                foreach (GameObject enemy in allEnemies)
                {
                    Instantiate(expoPrefab, enemy.transform.position, Quaternion.identity, enemy.transform);
                    GameManager.instance.awardPoints(10, "player nuked something");
                    GameManager.instance.RegisterKill();
                    foreach (SpriteRenderer sprite in enemy.GetComponentsInChildren<SpriteRenderer>())
                        sprite.enabled = false;
                    Destroy(enemy, expoPrefab.GetComponent<ParticleSystem>().main.duration);
                }
            }
            else if (random == 7)
            {
                poweruptextchange.text = "DOUBLE SHOT";
                isDoubleShot = true;
                StartCoroutine(WaitandEND(PowerupTime));
            }

            audiosrc.clip = powerupsfx;
            audiosrc.Play();
            StartCoroutine(PowerbarCountdown());

            poweruptext.gameObject.SetActive(true);
            poweruptextchange.gameObject.SetActive(true);
            StartCoroutine(HidePowerupText());
        }
    }

    private IEnumerator HidePowerupText()
    {
        yield return new WaitForSeconds(2);
        poweruptext.gameObject.SetActive(false);
        poweruptextchange.gameObject.SetActive(false);
    }

    private IEnumerator WaitandEND(float duration)
    {
        yield return new WaitForSeconds(duration);
        isMultishot = false;
        isInfMissile = false;
        isInfEnergy = false;
        GameManager.instance.setInvulnerable(false);
        isDoubleShot = false;
    }

    private IEnumerator PowerbarCountdown()
    {
        sliderPower.maxValue = PowerupTime;
        int i = PowerupTime;
        while (i > 0)
        {
            sliderPower.value = i;
            yield return new WaitForSeconds(1);
            i--;
        }
        sliderPower.value = 0;
        sliderPower.GetComponent<AudioSource>().Play();
    }
}