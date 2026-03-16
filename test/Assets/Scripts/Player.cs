    using UnityEngine;
    //using static UnityEngine.RuleTile.TilingRuleOutput; //this was causing error

    public class Player : MonoBehaviour {
      // set in inspector
      public float speed = 6f;
      public float LRspeed = 4.5f;


      public GameObject bulletPrefab;
      public GameObject missilePrefab;
      public Transform bulletSpawnPoint;
      public Transform missileSpawnPoint;

      private SpaceShooterInputActions inputActions;

      private int missileAmount = 5;
  


      private const float Y_LIMIT = 4.6f;
      private const float X_LIMIT = 9.92f;

      private void Start() {
        inputActions = new();
        inputActions.Enable();
        inputActions.Standard.Enable();
      }

        private void Update()
        {
            // FIRE BULLET
            if (inputActions.Standard.Fire.WasPressedThisFrame())
            {
                if (GameManager.instance.CanFire() == true)
                {
                    GameManager.instance.RemoveEnergy();
                    GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                }
                
            }
             
            // FIRE MISSILE
            if (inputActions.Standard.FireMissile.WasPressedThisFrame())
            {
                if (missileAmount > 0) {
                    GameObject MissileObj = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
                    missileAmount = missileAmount - 1;
                } else {
                    //CANT FIRE - maybe put sfx here idk
                }
                
            }


            if (inputActions.Standard.MoveUp.IsPressed())
            {
                this.transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            if (inputActions.Standard.MoveDown.IsPressed())
            {
                this.transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
            if (inputActions.Standard.MoveLeft.IsPressed())
            {
                this.transform.Translate(Vector3.left * LRspeed * Time.deltaTime);
            }
            if (inputActions.Standard.MoveRight.IsPressed())
            {
                this.transform.Translate(Vector3.right * LRspeed * Time.deltaTime);
            }



            if (this.transform.position.y > Y_LIMIT)
            {
                this.transform.position = new Vector3(transform.position.x, Y_LIMIT);
            }
            else if (this.transform.position.y < -Y_LIMIT)
            {
                this.transform.position = new Vector3(transform.position.x, -Y_LIMIT);
            }


            if (this.transform.position.x > X_LIMIT)
            {
                this.transform.position = new Vector3(X_LIMIT, transform.position.y, 0);
            }
            else if (this.transform.position.x < -X_LIMIT)
            {
                this.transform.position = new Vector3(-X_LIMIT, transform.position.y, 0);
            }



        }


        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("EnemyPhysicalHit"))
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
  
        }


    }
