using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsInHand : MonoBehaviour
{
    public GameObject Hand;
    public GameObject EnemyHand;

    public static int howMany;
    public static int eHowMany;
 
    public int howManyCards;
    public int eHowManyCards;
    public static int howMany1;
    public static int eHowMany1;
 
    public int howManyCards1;
    public int eHowManyCards1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int x = 0;
        foreach (Transform child in Hand.transform)
        {
            if (child.GetComponent<ThisCard>() != null)
                x++;
        }

        if (x != howManyCards)
        {
            howManyCards = x;
        }
        howMany = howManyCards;
        
        int y = 0;
        foreach (Transform child in EnemyHand.transform)
        {
            if (child.GetComponent<AICardToHand>() != null)
                y++;
        }

        if (y != eHowManyCards)
        {
            eHowManyCards = y;
        }
        eHowMany = eHowManyCards;
        //Minimax vs Kb
        int a = 0;
        foreach (Transform child in Hand.transform)
        {
            if (child.GetComponent<AI2CardToHand>() != null)
                a++;
        }

        if (a != howManyCards1)
        {
            howManyCards1 = a;
        }
        howMany1 = howManyCards1;
        
        int b = 0;
        foreach (Transform child in EnemyHand.transform)
        {
            if (child.GetComponent<AI1CardToHand>() != null)
                b++;
        }

        if (b != eHowManyCards1)
        {
            eHowManyCards1 = b;
        }
        eHowMany1 = eHowManyCards1;
    }
}
