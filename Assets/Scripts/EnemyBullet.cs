using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 8f;
    private Vector2 direction = Vector2.left; // default straight left

    // Call this from BossEnemy to set aimed direction
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        // Rotate bullet sprite to face direction, offset by -90 to account for default sprite orientation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("EnemyBullet triggered by: " + collision.gameObject.tag + " | root: " + collision.transform.root.gameObject.tag);

        if (collision.gameObject.CompareTag("MapBorder"))
        {
            Destroy(this.gameObject);
        }
        // Check both the object and its root for Player tag
        if (collision.gameObject.CompareTag("Player") || collision.transform.root.gameObject.CompareTag("Player"))
        {
            Debug.Log("EnemyBullet hit player!");
            GameManager.instance.LoseLife();
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("EnemyBullet hit player via collision");
            GameManager.instance.LoseLife();
            Destroy(this.gameObject);
        }
    }
}