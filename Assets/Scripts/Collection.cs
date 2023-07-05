using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    public GameObject CardOne;

    public GameObject CardTwo;

    public GameObject CardThree;

    public GameObject CardFour;

    public static int x;
    public bool notBeCollection;

    public int[] HowManyCards;

    public TMPro.TextMeshProUGUI CardOneText;

    public TMPro.TextMeshProUGUI CardTwoText;

    public TMPro.TextMeshProUGUI CardThreeText;

    public TMPro.TextMeshProUGUI CardFourText;

    public GameObject CardFive;

    public bool openPack;

    public int[] o;

    public int oo;

    public int rand;

    public string card;

    public int cardsInCollection;

    public int numberOfCardsOnPage;
     // Start is called before the first frame update
    void Start()
    {
        x = 1;
        for (int i = 1; i <= 24; i++)
        {
            HowManyCards[i] = PlayerPrefs.GetInt("x" + i, 0);
        }
        
        if (openPack == true)
        {
            for (int i = 0; i <= 4; i++)
            {
                getRandomCard();
            }
        }

        cardsInCollection = 24;
        numberOfCardsOnPage = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (openPack == false)
        {
            CardOne.GetComponent<CardsInCollection>().thisId = x;
            CardTwo.GetComponent<CardsInCollection>().thisId = x+1;
            CardThree.GetComponent<CardsInCollection>().thisId = x+2;
            CardFour.GetComponent<CardsInCollection>().thisId = x+3;

            CardOneText.text = "x" + HowManyCards[x];
            CardTwoText.text = "x" + HowManyCards[x + 1];
            CardThreeText.text = "x" + HowManyCards[x + 2];
            CardFourText.text = "x" + HowManyCards[x + 3];

            if (CardOneText.text == "x0")
            {
                CardOne.GetComponent<CardsInCollection>().beGrey = true;
            }
            else
            {
                CardOne.GetComponent<CardsInCollection>().beGrey = false;
            }
            if (CardTwoText.text == "x0")
            {
                CardTwo.GetComponent<CardsInCollection>().beGrey = true;
            }
            else
            {
                CardTwo.GetComponent<CardsInCollection>().beGrey = false;
            }
            if (CardThreeText.text == "x0")
            {
                CardThree.GetComponent<CardsInCollection>().beGrey = true;
            }
            else
            {
                CardThree.GetComponent<CardsInCollection>().beGrey = false;
            }
            if (CardFourText.text == "x0")
            {
                CardFour.GetComponent<CardsInCollection>().beGrey = true;
            }
            else
            {
                CardFour.GetComponent<CardsInCollection>().beGrey = false;
            }
        }
        
        for (int i = 1; i <= 24; i++)
        {
            if (notBeCollection)
                PlayerPrefs.SetInt("x"+i , HowManyCards[i]);
        }

        if (openPack == true)
        {
            CardOne.GetComponent<CardsInCollection>().thisId = o[0];
            CardTwo.GetComponent<CardsInCollection>().thisId = o[1];
            CardThree.GetComponent<CardsInCollection>().thisId = o[2];
            CardFour.GetComponent<CardsInCollection>().thisId = o[3];
            CardFive.GetComponent<CardsInCollection>().thisId = o[4];
            
        }
    }

    public void Left()
    {
        if (x != 1)
        {
            x -= numberOfCardsOnPage;
        }
    }

    public void Right()
    {
        if (x != (cardsInCollection - numberOfCardsOnPage)+1)
        {
            x += numberOfCardsOnPage;
        }
    }

    public void Card1Minus()
    {
        HowManyCards[x] -= 1;
    }

    public void Card1Plus()
    {
        HowManyCards[x] += 1;
    }
    public void Card2Minus()
    {
        HowManyCards[x+1] -= 1;
    }

    public void Card2Plus()
    {
        HowManyCards[x+1] += 1;
    }
    public void Card3Minus()
    {
        HowManyCards[x+2] -= 1;
    }

    public void Card3Plus()
    {
        HowManyCards[x+2] += 1;
    }
    public void Card4Minus()
    {
        HowManyCards[x+3] -= 1;
    }

    public void Card4Plus()
    {
        HowManyCards[x+3] += 1;
    }

    public void getRandomCard()
    {
        rand = Random.Range(1, 25);
        PlayerPrefs.SetInt("x" + rand, (int)HowManyCards[rand]++);
        card = CardDataBase.cardList[rand].cardName;
        for (int i = 0; i <= 24; i++)
        {
            PlayerPrefs.SetInt("x"+i,(int)HowManyCards[i]);
        }

        o[oo] = rand;
        oo++;
    }
}
