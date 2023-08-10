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
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        int x = 0;
        foreach (Transform child in Zone.transform)
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
        
        foreach (Transform child in EnemyZone.transform)
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
