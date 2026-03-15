using UnityEngine;
//using static UnityEngine.RuleTile.TilingRuleOutput; //this was causing error

public class Player : MonoBehaviour {
  // set in inspector
  public float speed = 6f;
  public float LRspeed = 4.5f;
  public GameObject bulletPrefab;
  public Transform bulletSpawnPoint;

  private SpaceShooterInputActions inputActions;
  


  private const float Y_LIMIT = 4.6f;
  private const float X_LIMIT = 9.92f;

  private void Start() {
    inputActions = new();
    inputActions.Enable();
    inputActions.Standard.Enable();
  }

    private void Update()
    {
        if (inputActions.Standard.Fire.WasPressedThisFrame())
        {
            GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
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
