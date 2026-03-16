using System.Runtime.CompilerServices;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    CircleCollider2D selfcollider;
    public float speed = 6f;

    public float maxSpawnY = 4.5f;
    public float minSpawnY = -4.5f;

    private int health = 4; //hits required to kill by bullets

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

        //if health gone destroy
        if (health < 1 )
        {
            Destroy(this.gameObject);
        }

    }


    private bool hitAlready = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //missiles destroy logic is in missile script..
        //here is bullet destroy logic 
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health--;
            //LOSE HALF HEALTH
            Debug.Log("asteroid lost 1hp");
        }

        //if hit plr damage
        if (collision.gameObject.CompareTag("Player"))
        {
            if (hitAlready == true) return;
            hitAlready = true;
            // send to GameManager to remove life
            GameManager.instance.LoseLife();

            // destroy own hitbox
            Destroy(selfcollider);
        }
    }



}
