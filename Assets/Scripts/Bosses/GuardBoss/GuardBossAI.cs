using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBossAI : MonoBehaviour
{
    [Header("HandAttack1")]

    [SerializeField]
    private GameObject handAttack1;

    [SerializeField]
    private Transform leftPosition, rightPosition;

    [SerializeField]
    private float attackForce, hand1IdleTime;


    [Header("CircleShootAttack")]
    [SerializeField]
    private GameObject bat, shooter;
    [SerializeField]
    private Animator popOut;
    [SerializeField]
    private float batIdleTime, attackTime;

    [Header("Misc")]
    [SerializeField]
    private GameObject shakeCam;
    private GameObject player;

    private void Start()
    {
        //StartCoroutine(handFallAttack(Random.Range(0, 2) == 0));
        player = GameObject.Find("Player");
        StartCoroutine(circleShootAttack());
    }


    private IEnumerator handFallAttack(bool rightToLeft)
    {
        ParticleSystem grindParticles;
        Rigidbody2D rb;
        ShakeSelfScript shakescript;
        GameObject hand;
        if (rightToLeft)
        {
            hand = Instantiate(handAttack1, rightPosition.position, Quaternion.identity);
            rb = hand.GetComponent<Rigidbody2D>();
            grindParticles = hand.transform.GetChild(1).GetComponent<ParticleSystem>();
            shakescript = hand.transform.GetChild(0).GetComponent<ShakeSelfScript>();
            yield return new WaitForSeconds(hand1IdleTime - 1f);
            shakescript.Begin();
            yield return new WaitForSeconds(1f);
            rb.AddForce(attackForce * -100 * transform.right);

        }
        else
        {
            hand = Instantiate(handAttack1, leftPosition.position, Quaternion.identity);
            rb = hand.GetComponent<Rigidbody2D>();
            grindParticles = hand.transform.GetChild(1).GetComponent<ParticleSystem>();
            shakescript = hand.transform.GetChild(0).GetComponent<ShakeSelfScript>();
            yield return new WaitForSeconds(hand1IdleTime - 1f);
            shakescript.Begin();
            yield return new WaitForSeconds(1f);
            rb.AddForce(attackForce * 100 * transform.right);
        }
        shakeCam.GetComponent<CinemachineVirtualCamera>().enabled = true;
        
        grindParticles.Play();
        yield return new WaitForSeconds(1f);
        grindParticles.Stop();
        shakeCam.GetComponent<CinemachineVirtualCamera>().enabled = false;
        yield return new WaitForSeconds(1f);
        shakescript.stopShake();
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(rb.velocity.x, 65);
        yield return new WaitForSeconds(1f);
        Destroy(hand);

    }

    private IEnumerator circleShootAttack()
    {
        shooter.SetActive(true);
        shooter.GetComponent<Animator>().Play("ShooterComeDown");
        yield return new WaitForSeconds(2.5f);
        shooter.GetComponent<ShooterScript>().enabled = true;
        yield return new WaitForSeconds(2f);
        float attackTime = Time.time + this.attackTime;
        while (Time.time < attackTime) {
            Vector2 pos = new Vector2(player.transform.position.x, popOut.transform.position.y);
            GameObject batObj = Instantiate(bat, pos, Quaternion.identity, popOut.gameObject.transform);
            Destroy(batObj, 4);
            popOut.Play("batPopOut");  
            yield return new WaitForSeconds(batIdleTime);
            
        }

    }
    private IEnumerator screenShake()
    {
        shakeCam.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        shakeCam.SetActive(false);
    }
}
