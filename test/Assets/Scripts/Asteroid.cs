using System.Runtime.CompilerServices;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    CircleCollider2D selfcollider;
    public float speed = 6f;

    public float maxSpawnY = 4.5f;
    public float minSpawnY = -4.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CircleCollider2D selfcollider = GetComponent<CircleCollider2D>();
        // spawn logic (transform)
        float randomY = Random.Range(minSpawnY, maxSpawnY);
        gameObject.transform.position = new Vector3(10.8f, randomY, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // move left ()
        gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);

    }

    // player hit logic
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // send to GameManager to remove life
            //////////////////////////////////ADDD

            // destroy own hitbox
            Destroy(selfcollider);
        }
    }
}
