    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

//using static UnityEngine.RuleTile.TilingRuleOutput; //this was causing error

public class Player : MonoBehaviour {
      // set in inspector
      public float speed = 6f;
      public float LRspeed = 4.5f;


      public GameObject bulletPrefab;
      public GameObject missilePrefab;
      public Transform bulletSpawnPoint;
      public Transform missileSpawnPoint;

      private SpaceShooterInputActions input;

      private int missileAmount = 4;

      public Image missiles1; public Image missiles2; public Image missiles3; public Image missiles4;

      private const float Y_LIMIT = 4.6f;
      private const float X_LIMIT = 9.92f;

      private void Start() {
        input = new SpaceShooterInputActions();
        input.Enable();
      }

        private void Update()
        {
            // FIRE BULLET
            if (input.Standard.Fire.WasPressedThisFrame())
            {
                if (GameManager.instance.CanFire() == true)
                {
                    GameManager.instance.RemoveEnergy();
                    GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                }
                
            }
             
            // FIRE MISSILE
            if (input.Standard.FireMissile.WasPressedThisFrame())
            {
                if (missileAmount > 0) {
                    GameObject MissileObj = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
                    missileAmount = missileAmount - 1;
                } else {
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





    }

    //fixes player after they finish colliding with object - THIS IS INTENDED DO NOT REMOVE!
    private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("EnemyPhysicalHit"))
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }


        }


    }
