using System.Collections;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [Header("Stats")]
    private int lives = 9;
    private bool isDead = false;

    [Header("Movement")]
    public float entrySpeed = 3f;
    public float targetX = 5f;
    public float oscillateSpeed = 1.5f;
    public float oscillateRange = 3.5f;

    [Header("Shooting")]
    public GameObject enemyBulletPrefab;
    public float fireRate = 1.8f;
    private float fireTimer = 0f;
    private bool alternateShot = false;

    [Header("Effects")]
    public GameObject expoPrefab;
    public ParticleSystem hitfx;

    [Header("Win Screen")]
    public GameObject WinScreen;
    public TMPro.TextMeshProUGUI WinScoreText;

    private bool inPosition = false;
    private float startY;
    private float timeElapsed = 0f;
    private Transform playerTransform;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Lock rigidbody completely so physics never moves the boss
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        startY = transform.position.y;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
        else Debug.LogWarning("BOSS: Player not found!");
    }

    void Update()
    {
        if (isDead) return;

        if (!inPosition)
        {
            // Move left by directly setting position - ignores physics
            float newX = transform.position.x - entrySpeed * Time.deltaTime;
            transform.position = new Vector3(newX, transform.position.y, 0);

            if (transform.position.x <= targetX)
            {
                transform.position = new Vector3(targetX, transform.position.y, 0);
                startY = transform.position.y;
                inPosition = true;
            }
        }
        else
        {
            // Oscillate by directly setting position - ignores physics entirely
            timeElapsed += Time.deltaTime;
            float newY = startY + Mathf.Sin(timeElapsed * oscillateSpeed) * oscillateRange;
            transform.position = new Vector3(targetX, newY, 0);

            // Shooting
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
    }

    private void Shoot()
    {
        if (enemyBulletPrefab == null) return;

        alternateShot = !alternateShot;

        GameObject bulletObj = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
        EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();
        if (bullet == null) return;

        if (alternateShot)
        {
            bullet.SetDirection(Vector2.left);
        }
        else
        {
            if (playerTransform != null)
            {
                Vector2 dir = (playerTransform.position - transform.position).normalized;
                bullet.SetDirection(dir);
            }
            else
            {
                bullet.SetDirection(Vector2.left);
            }
        }
    }

    private void TakeDamage(int amount)
    {
        if (isDead) return;
        lives -= amount;

        if (hitfx != null)
        {
            var thing = Instantiate(hitfx, transform.position, Quaternion.identity, this.transform);
            Destroy(thing, thing.main.duration);
        }

        Debug.Log("Boss took " + amount + " damage, lives remaining: " + lives);
        if (lives < 1) Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        GameManager.instance.awardPoints(50, "boss defeated");
        GameManager.instance.RegisterKill();

        if (expoPrefab != null)
        {
            var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
            Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);
        }

        if (WinScreen != null)
        {
            WinScreen.SetActive(true);
            if (WinScoreText != null)
                WinScoreText.text = "SCORE: " + GameManager.instance.Points;
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("BOSS hit by: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("Explosion"))
        {
            TakeDamage(2);
        }
        else if (collision.gameObject.CompareTag("Player") || collision.transform.root.CompareTag("Player"))
        {
            GameManager.instance.LoseLife();
        }
        else if (collision.gameObject.CompareTag("MapBorder"))
        {
            Destroy(this.gameObject);
        }
    }
}