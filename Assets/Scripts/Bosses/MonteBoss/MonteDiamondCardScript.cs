using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonteDiamondCardScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    private GameObject deathParticles;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(1100 * transform.right);
        Destroy(gameObject, 7);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        Instantiate(deathParticles, transform.position, transform.rotation);
        yield return null;
        Destroy(gameObject);
    }
}
