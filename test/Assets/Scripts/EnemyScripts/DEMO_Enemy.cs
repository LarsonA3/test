using UnityEngine;

public class DEMO_Enemy : MonoBehaviour
{
    public float speed = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.CompareTag("Bullet")) {
            Destroy(gameObject);
        }
        else if (c.gameObject.CompareTag("Player")) {
            Destroy(gameObject); // 
        }

        //ADD MISSILE AND ASTEROID STUFF
    }
}
