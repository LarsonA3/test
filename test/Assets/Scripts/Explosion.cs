using UnityEngine;
using static UnityEngine.UI.Image;
using System.Collections; //for explosion effect

public class Explosion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ScaleAndDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyPhysicalHit"))
        {
            Destroy(collision.gameObject);
        }
    }

    IEnumerator ScaleAndDestroy()
    {
        Vector3 original = transform.localScale;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            transform.localScale = Vector3.Lerp(original, original * 1.25f, t);
            yield return null;
        }
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            transform.localScale = Vector3.Lerp(original * 1.25f, Vector3.zero, t);
            yield return null;
        }
        Destroy(gameObject);
    }
}
