using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    CircleCollider2D selfcollider;
    public float speed = 6f;

    public float maxSpawnY = 4.5f;
    public float minSpawnY = -4.5f;

    public GameObject expoPrefab;
    public ParticleSystem hitfx;

    private int health = 4;
    private bool isDead = false;

    void Start()
    {
        selfcollider = GetComponent<CircleCollider2D>();
        float randomY = Random.Range(minSpawnY, maxSpawnY);
        gameObject.transform.position = new Vector3(10.8f, randomY, 0f);
    }

    void Update()
    {
        gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        GameManager.instance.awardPoints(5, "asteroid destroyed");
        GameManager.instance.RegisterKill(); // triggers level check + energy refill

        var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
        Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);
        Destroy(this.gameObject);
    }

    private bool hitAlready = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health--;
            Debug.Log("asteroid lost 1hp");
            var thing = Instantiate(hitfx, gameObject.transform.position, Quaternion.identity, this.transform);
            Destroy(thing, thing.main.duration);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (hitAlready) return;
            hitAlready = true;
            GameManager.instance.LoseLife();
            var thing = Instantiate(hitfx, gameObject.transform.position, Quaternion.identity, this.transform);
            var thing2 = Instantiate(hitfx, collision.gameObject.transform.position, Quaternion.identity, this.transform);
            Destroy(thing, thing.main.duration);
            Destroy(thing2, thing.main.duration);
            Destroy(selfcollider);
        }

        if (health < 1)
        {
            Die();
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
            health--;
            Debug.Log("asteroid lost 1hp (trigger)");
            var thing = Instantiate(hitfx, gameObject.transform.position, Quaternion.identity, this.transform);
            Destroy(thing, thing.main.duration);
            if (health < 1) Die();
        }
    }
}