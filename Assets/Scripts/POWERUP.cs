using UnityEngine;

public class POWERUP : MonoBehaviour
{

    public float speed = 6f;
    private float maxSpawnY = 4.5f;
    private float minSpawnY = -4.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomY = Random.Range(minSpawnY, maxSpawnY);
        gameObject.transform.position = new Vector3(10.8f, randomY, 0f);

    }
    private void Awake()
    {
        //check object tag to determine what powerup it is
        //set timer times depending on what it is
        //call to methods in game to do stuff
    }

    // Update is called once per frame
    void Update()
    {
        //go left
        gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);

        
        // all logic for actual deciding whats done is handled in PLAYER, UPON DETECTING HIT.


    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MapBorder"))
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            //maybe some effect here
            Destroy(this.gameObject);
            print("Destroyed POWERUP");
        }
    }
}
