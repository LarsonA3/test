using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
        if (Mathf.Abs(this.transform.position.x - 10.63f) < 0.1f) {
            Destroy(this.gameObject);
        }
        
    }
}
