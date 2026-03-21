using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//using static UnityEngine.RuleTile.TilingRuleOutput; //this was causing error



public class Player : MonoBehaviour
{
    // set in inspector
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

    private AudioSource audiosrc;
    public AudioClip firesfx;
    public AudioClip missilesfx;
    public AudioClip powerupsfx;
    public AudioClip gameoversfx;

    public GameObject GameOver;
    public TextMeshProUGUI FinalScoreText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI poweruptext;
    public TextMeshProUGUI poweruptextchange;

    public ParticleSystem toggleparticle;
    private ParticleSystem.EmissionModule particleJet;
    public ParticleSystem baseJet;

    private bool isExploded = false;

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
    }
      
    private void Update()
    {
        // FIRE BULLET
        if (input.Standard.Fire.WasPressedThisFrame())
        {
            if (GameManager.instance.CanFire() == true || isInfEnergy)
            {
                audiosrc.clip = firesfx;
                audiosrc.Play();

                if (isMultishot)
                {
                    // THREE BULLET POWER UP
                    if (isInfEnergy == false) GameManager.instance.RemoveEnergy();

                    GameObject bulletObj1 = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                    Vector3 topPos = new Vector3(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y + 1f, bulletSpawnPoint.position.z);
                    GameObject bulletObj2 = Instantiate(bulletPrefab, topPos, Quaternion.identity);
                    Vector3 botPos = new Vector3(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y - 1f, bulletSpawnPoint.position.z);
                    GameObject bulletObj3 = Instantiate(bulletPrefab, botPos, Quaternion.identity);

                }
                else if (isDoubleShot)
                {
                    //two bullets going straight
                    if (isInfEnergy == false) GameManager.instance.RemoveEnergy();
                    GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                    GameObject bulletObj2 = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                }
                else
                {
                    if (isInfEnergy == false) GameManager.instance.RemoveEnergy();
                    GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
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

                GameObject MissileObj = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
                if (isInfMissile == false) missileAmount = missileAmount - 1;
            }
            else
            {
                //CANT FIRE - maybe put sfx here idk
            }
        }



        //makes sure effects dont show after game over
        if (isExploded == false)
        {
            //up down
            //NEW STUFF
            float vertMove = input.Standard.MoveVertically.ReadValue<float>(); // GIVES you -1 or +1 based on input
            this.transform.Translate(Vector3.up * vertMove * speed * Time.deltaTime);


            //OLD (but works for rn) BUT REPLACE EVENTUALLY WITH ABOVE METHOD UNDER NEW STUFF
            //left right
            if (input.Standard.MoveLeft.IsPressed())
            {
                this.transform.Translate(Vector3.left * LRspeed * Time.deltaTime);
            }
            if (input.Standard.MoveRight.IsPressed())
            {
                this.transform.Translate(Vector3.right * LRspeed * Time.deltaTime);
            }

            // JET EFFECT
            if (vertMove != 0 || input.Standard.MoveRight.IsPressed())
            {
                particleJet.rateOverTime = 1000;
            }
            else
            {
                particleJet.rateOverTime = 0;
            }


            //y max limits - prevents plr from going out of bounds on y axis
            if (this.transform.position.y > Y_LIMIT)
            {
                this.transform.position = new Vector3(transform.position.x, Y_LIMIT);
            }
            else if (this.transform.position.y < -Y_LIMIT)
            {
                this.transform.position = new Vector3(transform.position.x, -Y_LIMIT);
            }

            //x max limits - prevents plr from going out of bounds on x axis
            if (this.transform.position.x > X_LIMIT)
            {
                this.transform.position = new Vector3(X_LIMIT, transform.position.y, 0);
            }
            else if (this.transform.position.x < -X_LIMIT)
            {
                this.transform.position = new Vector3(-X_LIMIT, transform.position.y, 0);
            }

        }
        


        //CHANGE UI TO REPRESENT CORRECT MISSILES
        if (missileAmount == 0)
        {
            missiles1.gameObject.SetActive(false);
            missiles2.gameObject.SetActive(false);
            missiles3.gameObject.SetActive(false);
            missiles4.gameObject.SetActive(false);
        }
        else if (missileAmount == 1)
        {
            missiles1.gameObject.SetActive(true);
            missiles2.gameObject.SetActive(false);
            missiles3.gameObject.SetActive(false);
            missiles4.gameObject.SetActive(false);
        }
        else if (missileAmount == 2)
        {
            missiles1.gameObject.SetActive(true);
            missiles2.gameObject.SetActive(true);
            missiles3.gameObject.SetActive(false);
            missiles4.gameObject.SetActive(false);
        }
        else if (missileAmount == 3)
        {
            missiles1.gameObject.SetActive(true);
            missiles2.gameObject.SetActive(true);
            missiles3.gameObject.SetActive(true);
            missiles4.gameObject.SetActive(false);
        }
        else if (missileAmount == 4)
        {
            missiles1.gameObject.SetActive(true);
            missiles2.gameObject.SetActive(true);
            missiles3.gameObject.SetActive(true);
            missiles4.gameObject.SetActive(true);
        }
        else
        {
            print("ERROR: NOT ENOUGH UI SPRITES TO REPRESENT HEALTH, OR OUT OF RANGE");
            missiles1.gameObject.SetActive(false);
            missiles2.gameObject.SetActive(false);
            missiles3.gameObject.SetActive(false);
            missiles4.gameObject.SetActive(false);
        }


        if (GameManager.instance.Lives < 1)
        {
            // GAME OVER LOGIC 
         
            //EXPLOSION EFFECT
            if (isExploded == false)
            {
                //put game over screen on
                GameOver.SetActive(true);
                FinalScoreText.text = ScoreText.text;
                print("GAME OVER!");
                var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
                Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);
                //hide
                transform.Find("Triangle").gameObject.SetActive(false);

                isExploded = true;

                audiosrc.clip = gameoversfx;
                audiosrc.Play();


                input.Standard.Disable();
                //reload scene
                StartCoroutine(ReloadEverything());
            }
            
        }


    }

    //fixes player after they finish colliding with object - THIS IS INTENDED DO NOT REMOVE!
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyPhysicalHit"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private IEnumerator ReloadEverything()
    {
        yield return new WaitForSeconds(5f);
        int sceneindex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneindex);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("POWERUP"))
        {
            print("player picked up POWERUP");
            //POWER UP LOGIC
            int random = Random.Range(0,8); //will not give max val
            if (random == 0) // FULL HEALTH REFILL
            {
                print("player got full hp refill");
                //change ui text
                poweruptextchange.text = "HP REFILL";
                //do effect
                GameManager.instance.RestoreLives();

            }
            else if (random == 1) // FULL MISSILE REFILL 
            {
                print("player got full missile refill");
                //change ui text
                poweruptextchange.text = "MISSILE REFILL";
                //do effect
                missileAmount = 4;
                missiles1.gameObject.SetActive(true);
                missiles2.gameObject.SetActive(true);
                missiles3.gameObject.SetActive(true);
                missiles4.gameObject.SetActive(true);

            }
            else if (random == 2) // Multishot
            {
                print("player got multishot");
                //change ui text
                poweruptextchange.text = "MULTISHOT";
                //do effect
                isMultishot = true;
                StartCoroutine(WaitandEND(PowerupTime)); // power up length in seconds time
            }
            else if (random == 3) // infinite missiles
            {
                print("player got inf missiles");
                //change ui text
                poweruptextchange.text = "INF MISSILES";
                //do effect
                isInfMissile = true;
                StartCoroutine(WaitandEND(PowerupTime));
            }
            else if (random == 4) // infinite energy
            {
                print("player got inf energy");
                //change ui text
                poweruptextchange.text = "INF ENERGY";
                //do effect
                isInfEnergy = true;
                StartCoroutine(WaitandEND(PowerupTime));
            }
            else if (random == 5) // infinite health (make sure player cant lose points)
            {
                print("player got inf health");
                //change ui text
                poweruptextchange.text = "INVULNERABILITY";
                //do effect
                GameManager.instance.setInvulnerable(true);
                StartCoroutine(WaitandEND(PowerupTime));
            }
            else if (random == 6) // nuke, blows up all enemies on screen
            {
                print("player got nuke");
                //change ui text
                poweruptextchange.text = "NUKE";
                //do effect
                GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("EnemyPhysicalHit"); // find all enemies in scene
                foreach (GameObject enemy in allEnemies)
                {
                    Instantiate(expoPrefab, enemy.transform.position, Quaternion.identity, enemy.transform);
                    GameManager.instance.awardPoints(10, "player nuked something");
                    foreach (SpriteRenderer sprite in enemy.GetComponentsInChildren<SpriteRenderer>()) { sprite.enabled = false; }
                    Destroy(enemy, expoPrefab.GetComponent<ParticleSystem>().main.duration);
                }
            }
            else if (random == 7) // double shot
            {
                print("player got double shot");
                //change ui text
                poweruptextchange.text = "DOUBLE SHOT";
                //do effect
                isDoubleShot = true;
                StartCoroutine(WaitandEND(PowerupTime));
            }


            audiosrc.clip = powerupsfx;
            audiosrc.Play();


            //after loop, show text on screen
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
    private IEnumerator WaitandEND(float thing)
    {
        yield return new WaitForSeconds(thing);
        //reset all bools to stop powerups
        isMultishot = false;
        isInfMissile = false;
        isInfEnergy = false;
        GameManager.instance.setInvulnerable(false);
        isDoubleShot = false;
    }
}