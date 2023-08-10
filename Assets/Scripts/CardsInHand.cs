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
    }
}
