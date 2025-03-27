using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class MonteScript : MonoBehaviour
{
    public GameObject[] WhatToSpawn;
    public Transform[] SpawnLocation;
    public GameObject[] WhatToSpawnClone;
    Rigidbody2D rb;
    public MovementScript movementScript;
    public bool canAttack = true;
    public GameObject Blade;
    public SpriteRenderer spriteRenderer;
    public float dashForce;
    public int hp;
    public bool BetQueued, LifeStealQueued;
    public bool BetUnderWay;

    public GameObject Attractor;
    public GameObject TraversalParticles;
    int nextAttack;
    public bool isFollowing;

    void Start()
    {
        LifeStealQueued = false;
        hp = 120;
        spriteRenderer = GetComponent<SpriteRenderer>();
        canAttack = true;
        rb = GetComponent<Rigidbody2D>();
        //StartCoroutine(ExplosiveCardAttack());
        //StartCoroutine(DiamondAttack());
        //StartCoroutine(DashAttack());
        //StartCoroutine(movementScript.LifeSteal());
        //StartCoroutine(cardAttack());   
        //StartCoroutine(SlamDownAttack());
        StartCoroutine(AttackLoop());
        //StartCoroutine(AOEAttack());
    }

    void Update()
    {
        print(hp);

        if ((hp <= 100 && movementScript.LifeStealCount == 0) || (hp <= 80 && movementScript.LifeStealCount == 1) || (hp <= 60 && movementScript.LifeStealCount == 2) || (hp <= 40 && movementScript.LifeStealCount == 3) || (hp <= 20 && movementScript.LifeStealCount == 4))
        {
            LifeStealQueued = true;
        }
        if (BetQueued && !BetUnderWay)
        {
            StartCoroutine(cardAttack());
        }
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            if (canAttack)
            {
                ChooseAttack();
            }
            yield return null;
        }
    }

    void ChooseAttack()
    {
        if (canAttack == true && !LifeStealQueued)
        {
            /*
            int nextAttack = Random.RandomRange(0, 6);
            switch (nextAttack)
            {
                case 0:
                    StartCoroutine(ExplosiveCardAttack());
                    //print("Explosion is art");
                    canAttack = false;
                    return;
                case 1:
                    gameObject.layer = LayerMask.NameToLayer("MonteHeart");
                    StartCoroutine(DiamondAttack());
                    //print("Neptune");
                    canAttack = false;
                    return;
                case 2:
                    StartCoroutine(DashAttack());
                    //print("Gotta go fast!");
                    canAttack = false;
                    return;
                case 3:
                    StartCoroutine(SlamDownAttack());
                    //print("Ready for landing");
                    canAttack = false;
                    return;
                    if (movementScript.hp >= 2)
                    {
                        spriteRenderer.enabled = false;
                        gameObject.layer = LayerMask.NameToLayer("Monte");
                        StartCoroutine(movementScript.LifeSteal());
                    }
                    else StartCoroutine(PiercingAttack());
                    //print("Shen jibeshi racaa visia?");
                    canAttack = false;
                    return;
                case 5:
                    StartCoroutine(SlamDownAttack());
                    //print("Ready for landing");
                    canAttack = false;
                    return;

                    //StartCoroutine(AmbushAttack());
                    //print("Gotcha");
                    //StartCoroutine(cardAttack());
                    //canAttack = false;
                    //return;
                    /*
                case 5:
                    StartCoroutine(SlamDownAttack());
                    print("Ready for landing");
                    canAttack = false;
                    return;
            }
                    */
            //gameObject.layer = LayerMask.NameToLayer("MonteHeart");
            nextAttack = Random.Range(0, 5);
            StartCoroutine(Traversal(0));
        }
        else if (LifeStealQueued)
        {
            canAttack = false;
            spriteRenderer.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Monte");
            StartCoroutine(movementScript.LifeSteal());
        }
    }

    private IEnumerator ExplosiveCardAttack()
    {
        int[] spawnIndices = { 0, 1, 2, 3 };

        //StartCoroutine(Traversal(15));

        foreach (int i in spawnIndices)
        {
            Instantiate(WhatToSpawn[0], SpawnLocation[i].transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(2f);
        canAttack = true;
    }

    private IEnumerator DiamondAttack()
    {
        int[] spawnIndices = { 0, 1, 2, 3, 4, 5 };

        //StartCoroutine(Traversal(16));

        for (int i = 1; i <= spawnIndices.Length; i++)
        {
            Instantiate(WhatToSpawn[1], SpawnLocation[i + 3].transform.position, SpawnLocation[i + 3].transform.rotation);
        }
        yield return new WaitForSeconds(2f);
        // gameObject.layer = LayerMask.NameToLayer("Default");
        canAttack = true;
    }

    private IEnumerator DashAttack(int x)
    {
        yield return new WaitForSeconds(0.4f);
        //gameObject.layer = LayerMask.NameToLayer("Default");
        rb.AddForce(dashForce * 1000 * transform.right * x);

        yield return new WaitForSeconds(2f);
        //gameObject.layer = LayerMask.NameToLayer("Default");
        rb.velocity = Vector2.zero;
        canAttack = true;
    }

    private IEnumerator cardAttack()
    {
        BetUnderWay = true;

        //StartCoroutine(Traversal(16));

        for (int i = 4; i < 6; i++)
        {
            Instantiate(WhatToSpawn[i - 2], SpawnLocation[i + 8].transform.position, SpawnLocation[i + 8].transform.rotation);
        }
        yield return null;
    }

    private IEnumerator SlamDownAttack()
    {
        //StartCoroutine(Traversal(14));
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.4f);
        rb.gravityScale = 40;
        yield return new WaitForSeconds(1f);

        this.transform.position = SpawnLocation[16].transform.position;
        spriteRenderer.enabled = true;
        //gameObject.layer = LayerMask.NameToLayer("Default");

        TraversalParticles.transform.position = this.transform.position;
        gameObject.layer = LayerMask.NameToLayer("Boss");
        canAttack = true;
    }

    /*
    private IEnumerator AmbushAttack()
    {
        this.transform.position = SpawnLocation[17].position;
        yield return new WaitForSeconds(1f);
        rb.AddForce(0.2f * this.transform.right);
        Blade.transform.rotation = this.transform.rotation;
        Blade.SetActive(true);
        yield return new WaitForSeconds(0.12f);
        Blade.SetActive(false);
        canAttack = true;
    }
    */

    /*
    private IEnumerator PiercingAttack()
    {
        this.transform.position = SpawnLocation[11].transform.position;
        yield return new WaitForSeconds(0.4f);
        Instantiate(WhatToSpawn[1], SpawnLocation[6].transform.position, SpawnLocation[6].transform.rotation);
        canAttack = true;
    }
    */

    private IEnumerator AOEAttack()
    {
        print("six");
        for (int i = 20; i <= 25; i++)
        {
            Instantiate(WhatToSpawn[4], SpawnLocation[i - 3].transform.position, SpawnLocation[i - 3].transform.rotation);
            print("six");
        }
        yield return new WaitForSeconds(2f);
        canAttack = true;
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("playerSlash"))
        {
            hp--;
        }
        else if (collision.gameObject.CompareTag("HeavyAttack"))
        {
            hp -= 20;
        }
    }

    IEnumerator Traversal(int x)
    {
        if (!isFollowing)
        {
            canAttack = false;
            spriteRenderer.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Monte");

            if (nextAttack == 0) x = 12;
            else if (nextAttack == 1) x = 13;
            else if (nextAttack == 2) x = 12 + nextAttack;
            else if (nextAttack == 3) x = 16;
            else if (nextAttack == 4) x = 13;

            int direction = Random.Range(0, 2);
            if (nextAttack == 2) x += direction;

            Transform targetSpawnLocation = SpawnLocation[x];

            isFollowing = true; // Enable following
            Attractor.SetActive(true);
            TraversalParticles.SetActive(true);

            float duration = 2f;
            float elapsedTime = 0f;

            while (elapsedTime < duration && isFollowing)
            {
                Attractor.transform.position = Vector2.Lerp
                (
                    Attractor.transform.position,
                    targetSpawnLocation.position,
                    elapsedTime / duration
                );

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            isFollowing = false;
            Attractor.SetActive(false);
            
            TraversalParticles.SetActive(false);

            transform.position = Attractor.transform.position; // Stay at last position
            spriteRenderer.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Boss");

            switch (nextAttack)
            {
                case 0:
                    StartCoroutine(ExplosiveCardAttack());
                    canAttack = false;
                    yield break;
                case 1:
                    gameObject.layer = LayerMask.NameToLayer("MonteHeart");
                    StartCoroutine(DiamondAttack());
                    canAttack = false;
                    yield break;
                case 2:
                    if (direction == 0) direction = -1;
                    StartCoroutine(DashAttack(direction));
                    canAttack = false;
                    yield break;
                case 3:
                    StartCoroutine(SlamDownAttack());
                    canAttack = false;
                    yield break;
                case 4:
                    StartCoroutine(AOEAttack());
                    canAttack = false;
                    yield break;
            }
        }
    }
}
