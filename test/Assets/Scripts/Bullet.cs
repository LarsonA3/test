using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float ttl = 8000;
    public float speed = 95f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.right * speed * Time.deltaTime);



        // destroy after certain amount of time. TTL is set high # to account for any frame rate
        ttl = ttl - 1; 
        if (ttl == 0) {
            Destroy(this.gameObject);
        }
        
    }
}
