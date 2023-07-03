using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsInZone : MonoBehaviour
{
    public GameObject Zone;

    public static int howMany;
 
    public int howManyCards;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int x = 0;
        foreach (Transform child in Zone.transform)
        {
            x++;
        }

        if (x != howManyCards)
        {
            howManyCards = x;
        }

        howMany = howManyCards;

    }
}
