using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsInZone : MonoBehaviour
{
    public GameObject Zone;
    public GameObject EnemyZone;

    public static int howMany;
    public static int eHowMany;
    
    public int howManyCards;
    public int eHowManyCards;
    public static int howMany1;
    public static int eHowMany1;
    
    public int howManyCards1;
    public int eHowManyCards1;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        int x = 0;
        foreach (Transform child in Zone.transform)
        {
            if (child.GetComponent<ThisCard>() != null && child.GetComponent<ThisCard>().actualblood > 0)
                x++;
        }

        if (x != howManyCards)
        {
            howManyCards = x;
        }
        howMany = howManyCards;
        
        int y = 0;
        
        foreach (Transform child in EnemyZone.transform)
        {
            if (child.GetComponent<AICardToHand>() != null && child.GetComponent<AICardToHand>().actualblood > 0)
                y++;
        }

        if (y != eHowManyCards)
        {
            eHowManyCards = y;
        }
        eHowMany = eHowManyCards;
        //minimax vs kb
        int a = 0;
        foreach (Transform child in Zone.transform)
        {
            if (child.GetComponent<AI2CardToHand>() != null && child.GetComponent<AI2CardToHand>().actualblood > 0)
                a++;
        }

        if (a != howManyCards1)
        {
            howManyCards1 = a;
        }
        howMany1 = howManyCards1;
        
        int b = 0;
        
        foreach (Transform child in EnemyZone.transform)
        {
            if (child.GetComponent<AI1CardToHand>() != null && child.GetComponent<AI1CardToHand>().actualblood > 0)
                b++;
        }

        if (b != eHowManyCards1)
        {
            eHowManyCards1 = b;
        }
        eHowMany1 = eHowManyCards1;
    }
}
