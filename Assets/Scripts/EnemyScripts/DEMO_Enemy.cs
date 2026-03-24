using UnityEngine;

public class DEMO_Enemy : MonoBehaviour
{
    private int lives = 3;
    public float speed = 1f;
    public GameObject expoPrefab;
    public ParticleSystem hitfx;

    private bool isDead = false;

    void Start() { }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        GameManager.instance.awardPoints(10, "player destroyed demo enemy");
        GameManager.instance.RegisterKill(); // triggers level check + energy refill

        var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
        Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Bullet"))
        {
            lives--;
            print("Demo enemy got shot");
            var thing = Instantiate(hitfx, gameObject.transform.position, Quaternion.identity, this.transform);
            Destroy(thing, thing.main.duration);
            if (lives < 1) Die();
        }
        else if (c.gameObject.CompareTag("Player"))
        {
            GameManager.instance.LoseLife();
            Die(); // counts as a kill - player rammed it
        }
        else if (c.gameObject.CompareTag("Enemy"))
        {
            lives--;
            print("Enemy collided with enemy");
            GameManager.instance.awardPoints(1, "enemy collided with other enemy");
            var thing = Instantiate(hitfx, gameObject.transform.position, Quaternion.identity, this.transform);
            Destroy(thing, thing.main.duration);
            if (lives < 1) Die();
        }
        else if (c.gameObject.CompareTag("EnemyPhysicalHit"))
        {
            lives--;
            print("Enemy collided with asteroid");
            GameManager.instance.awardPoints(1, "enemy collided with asteroid");
            var thing = Instantiate(hitfx, gameObject.transform.position, Quaternion.identity, this.transform);
            Destroy(thing, thing.main.duration);
            if (lives < 1) Die();
        }
        else if (c.gameObject.CompareTag("Explosion"))
        {
            lives--;
            print("Demo enemy caught in explosion");
            GameManager.instance.awardPoints(4, "enemy hit with explosion");
            var thing = Instantiate(hitfx, gameObject.transform.position, Quaternion.identity, this.transform);
            Destroy(thing, thing.main.duration);
            if (lives < 1) Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MapBorder"))
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            lives--;
            print("Demo enemy got shot (trigger)");
            var thing = Instantiate(hitfx, gameObject.transform.position, Quaternion.identity, this.transform);
            Destroy(thing, thing.main.duration);
            if (lives < 1) Die();
        }
    }
}