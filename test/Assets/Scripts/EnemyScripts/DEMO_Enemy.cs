using UnityEngine;

public class DEMO_Enemy : MonoBehaviour
{
    private int lives = 3;
    public float speed = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        //check if dead
        if (lives < 1)
        {
            GameManager.instance.awardPoints(10, "player destroyed demo enemy");
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Bullet")) {
            lives--;
            print("Demo enemy got shot");
        }
        else if (c.gameObject.CompareTag("Player")) {
            GameManager.instance.awardPoints(7, "player destroyed demo enemy via ramming");
            GameManager.instance.LoseLife();
            Destroy(this.gameObject);

        }
        else if (c.gameObject.CompareTag("Enemy"))
        {
            lives--;
            print("Enemy collided with enemy");
            GameManager.instance.awardPoints(1, "enemy collided with other enemy"); // fully aware this is complete rng and no skill lol
            //ADD MISSILE AND ASTEROID STUFF
        }
        else if (c.gameObject.CompareTag("Asteroid")) {
            lives--;
            print("Enemy collided with asteroid");
            GameManager.instance.awardPoints(1, "enemy collided with asteroid"); // same for this one lol
        }
    }

}
