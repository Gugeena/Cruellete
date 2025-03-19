using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonteScript : MonoBehaviour
{
    public GameObject[] WhatToSpawn;
    public Transform[] SpawnLocation;
    public GameObject[] WhatToSpawnClone;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //StartCoroutine(ExplosiveCardAttack());
        //StartCoroutine(DiamondAttack());
        StartCoroutine(DashAttack());
    }

    void Update()
    {
        
    }

    private IEnumerator ExplosiveCardAttack()
    {
        int[] spawnIndices = { 0, 1, 2, 3};

        foreach (int i in spawnIndices)
        {
            Instantiate(WhatToSpawn[0], SpawnLocation[i].transform.position, Quaternion.identity);
        }
        yield return null;
    }

    private IEnumerator DiamondAttack()
    {
        int[] spawnIndices = { 0, 1, 2, 3, 4, 5};

        for (int i = 1; i <= spawnIndices.Length; i++)
        {
            Instantiate(WhatToSpawn[1], SpawnLocation[i + 3].transform.position, SpawnLocation[i + 3].transform.rotation);
            print(WhatToSpawn + " spawned at " + SpawnLocation + SpawnLocation[i]);
        }
        yield return null;
    }
    private IEnumerator DashAttack()
    {
        int DashStartUpLocation = Random.RandomRange(0, 2);
        if(DashStartUpLocation == 0)
        {
            print(0);
            this.transform.position = SpawnLocation[10].transform.position;
            this.transform.rotation = SpawnLocation[10].transform.rotation;
            yield return new WaitForSeconds(0.4f);
            //rb.AddForce(1100 * transform.right * -1);

        }
        else
        {
            print(1);
            this.transform.position = SpawnLocation[11].transform.position;
            this.transform.rotation = SpawnLocation[11].transform.rotation;
            yield return new WaitForSeconds(0.4f);
            rb.AddForce(1100 * transform.right);
        }
        yield return null;
    }
}
