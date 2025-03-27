using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetCardScript : MonoBehaviour
{
    public int BetValue;
    public bool hasPlacedBet = false;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("playerSlash") && !hasPlacedBet)
        {
            BetValue = Random.Range(0, 101);
            hasPlacedBet = true;
            Bet();
        }
        if (collision.gameObject.CompareTag("HeavyAttack") && !hasPlacedBet)
        {
            BetValue = Random.Range(0, 101);
            hasPlacedBet = true;
            Bet();
        }
        if (collision.gameObject.CompareTag("LHAttack") && !hasPlacedBet)
        {
            BetValue = Random.Range(0, 101);
            hasPlacedBet = true;
            Bet();
        }
    }

    public void Bet()
    {
        if (BetValue >= 0 && BetValue < 50)
        {
            print(1);
        }
        else print(2);
    }
}
