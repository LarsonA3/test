using UnityEngine;

public class DEMO_Enemy : MonoBehaviour
{
    private int lives = 3;
    public float speed = 1f;
    public GameObject expoPrefab;

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
            //EXPLOSION EFFECT
            var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
            Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);

            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Bullet"))
        {
            lives--;
            print("Demo enemy got shot");
        }
        else if (c.gameObject.CompareTag("Player"))
        {
            GameManager.instance.awardPoints(7, "player destroyed demo enemy via ramming");
            GameManager.instance.LoseLife();
            //EXPLOSION EFFECT
            var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
            Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);

            Destroy(this.gameObject);
            

        }
        else if (c.gameObject.CompareTag("Enemy"))
        {
            lives--;
            print("Enemy collided with enemy");
            GameManager.instance.awardPoints(1, "enemy collided with other enemy"); // fully aware this is complete rng and no skill lol
            
        }
        else if (c.gameObject.CompareTag("EnemyPhysicalHit"))
        {
            lives--;
            print("Enemy collided with asteroid");
            GameManager.instance.awardPoints(1, "enemy collided with asteroid"); // same for this one lol
        }
        else if (c.gameObject.CompareTag("Explosion"))
        {
            lives--;
            print("Demo enemy got caught in explosion");
            GameManager.instance.awardPoints(4, "enemy hit with explosion");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MapBorder"))
        {
            Destroy(this.gameObject);
        }
    }
}
