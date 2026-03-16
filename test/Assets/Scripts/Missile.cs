using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float accAmount = 0.1f;
    private float accelerate = 0f;

    public GameObject ExplosionPrefab;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.right * speed * Time.deltaTime * accelerate);
        accelerate = accelerate + 1f;


        // destroy after certain amount of time. TTL is set high # to account for any frame rate
        if (Mathf.Abs(this.transform.position.x - 10.63f) < 0.3f) {
            Destroy(this.gameObject);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyPhysicalHit")) //put other enemies and hit things or'ed here
        {
            GameObject ExplosionObject = Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
