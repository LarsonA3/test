using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;

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

    private SpaceShooterInputActions input;

    private int missileAmount = 4;

    public Image missiles1; public Image missiles2; public Image missiles3; public Image missiles4;

    private const float Y_LIMIT = 4.6f;
    private const float X_LIMIT = 9.92f;

    private bool isMultishot = false;


    public GameObject GameOver;
    public TextMeshProUGUI FinalScoreText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI poweruptext;
    public TextMeshProUGUI poweruptextchange;

    private void Start()
    {
        input = new SpaceShooterInputActions();
        input.Enable();
        GameOver.SetActive(false);
        this.gameObject.SetActive(true);
        poweruptext.gameObject.SetActive(false);
        poweruptextchange.gameObject.SetActive(false);
        isMultishot = false;
    }
      
    private void Update()
    {
        // FIRE BULLET
        if (input.Standard.Fire.WasPressedThisFrame())
        {
            if (GameManager.instance.CanFire() == true)
            {
                if (isMultishot == true)
                {
                    // THREE BULLET POWER UP
                    GameManager.instance.RemoveEnergy();

                    GameObject bulletObj1 = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                    Vector3 topPos = new Vector3(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y + 1f, bulletSpawnPoint.position.z);
                    GameObject bulletObj2 = Instantiate(bulletPrefab, topPos, Quaternion.identity);
                    Vector3 botPos = new Vector3(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y - 1f, bulletSpawnPoint.position.z);
                    GameObject bulletObj3 = Instantiate(bulletPrefab, botPos, Quaternion.identity);

                }
                else
                {
                    GameManager.instance.RemoveEnergy();
                    GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                }
            }
        }

        // FIRE MISSILE
        if (input.Standard.FireMissile.WasPressedThisFrame())
        {
            if (missileAmount > 0)
            {
                GameObject MissileObj = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
                missileAmount = missileAmount - 1;
            }
            else
            {
                //CANT FIRE - maybe put sfx here idk
            }
        }

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
            print("GAME OVER!");

            //put game over screen on
            GameOver.SetActive(true);
            FinalScoreText.text = ScoreText.text;

            input.Standard.Disable();
            //destroy character
            this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            //reload scene
            StartCoroutine(ReloadEverything());
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
            int random = Random.Range(0,3); //will not give 3
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

    }
}