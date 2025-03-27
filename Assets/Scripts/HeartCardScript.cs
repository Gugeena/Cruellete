using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCardScript : MonoBehaviour
{
    public Vector2 minBounds = new Vector2(-10.8f, -4.2f);
    public Vector2 maxBounds = new Vector2(16.09f, 1.67f);
    [SerializeField]
    private GameObject SpawnParticles;
    [SerializeField]
    private GameObject DespawnParticles;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private BoxCollider2D collider;
    private Rigidbody2D rb;
    public float torqMultiplier;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        float torque = Random.Range(-8.5f, 8.5f);
        if (torque >= 0) torque = Mathf.Clamp(torque, 7f, 8.5f);
        else torque = Mathf.Clamp(torque, -8.5f, -7f);
        rb.AddTorque(torque * (torqMultiplier + Random.Range(-0.4f, 0.4f)));
        spriteRenderer.enabled = false;
        collider.enabled = false;
        StartCoroutine(Spawn());
        StartCoroutine(Death());
    }

    private IEnumerator Spawn()
    {
        float x = Random.RandomRange(minBounds.x, maxBounds.x);
        float y = Random.RandomRange(minBounds.y, maxBounds.y);
        transform.position = new Vector2(x, y);
        GameObject Particles = Instantiate(SpawnParticles, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.enabled = true;
        collider.enabled = true;
        ApplyRandomPush();
        yield return new WaitForSeconds(0.8f);
        Destroy(Particles);
    }

    void ApplyRandomPush()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        float pushForce = 2f; 
        rb.AddForce(randomDirection * pushForce, ForceMode2D.Impulse);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Ground" || collision.gameObject.name == "Walls")
        {
            StartCoroutine(Deactivation());
        }

        if(collision.gameObject.tag == "Player")
        {
           StartCoroutine(Deactivation());
        }
    }

    private IEnumerator Deactivation()
    {
        spriteRenderer.enabled = false;
        collider.enabled = false;
        GameObject Particles = Instantiate(DespawnParticles, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.8f);
        Destroy(Particles);
        Destroy(this.gameObject);
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(Deactivation());
    }
}
