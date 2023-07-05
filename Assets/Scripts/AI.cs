using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public Card container = new Card();
    public static List<Card> staticEnemyDeck = new List<Card>();

    public List<Card> cardsInHand = new List<Card>();

    public List<Card> cardsInZone = new List<Card>();
    
    public GameObject Hand;
    public GameObject Zone;
    public GameObject Graveyard;

    public int x;
    public static int deckSize;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;

    public GameObject CardToHand;

    public GameObject[] Clones;

    public static bool draw;

    public GameObject CardBack;

    public int currentMana;

    public bool[] AiCanSummon;

    public bool drawPhase;
    public bool summonPhase;
    public bool attackPhase;
    public bool endPhase;

    public int[] cardsID;
    
    public AICardToHand aiCardToHand;

    public int summonID;

    public int howManyCards;

    public bool[] canAttack;
    public static bool AiEndPhase;
    public static int whichEnemy;

    // Start is called before the first frame update
    private void Awake()
    {
        //Shuffle();
    }
    void Start()
    {
        //StartCoroutine(WaitOneSeconds());

        //StartCoroutine(StartGame());
        Hand = GameObject.Find("EnemyHand");
        Zone = GameObject.Find("EnemyZone");
        Graveyard = GameObject.Find("EGraveyard");

        x = 0;
        deckSize = 30;
        draw = true;
        // for (int i = 0;i < deckSize; i++)
        // {
        //     x = Random.Range(1, 10);
        //     deck.Add(CardDataBase.cardList[x]);
        // }
        if (whichEnemy == 1)
        {
            for (int i = 0;i < deckSize; i++)
            {
                x = Random.Range(1, 26);
                deck.Add(CardDataBase.cardList[x]);
            }
            // for (int i = 0; i < deckSize; i++)
            // {
            //     if (i <= 19)
            //     {
            //         deck.Add(CardDataBase.cardList[2]);
            //     }
            //     else
            //     {
            //         deck.Add(CardDataBase.cardList[3]);
            //     }
            // }
        }
        if (whichEnemy == 2)
        {
            for (int i = 0; i < deckSize; i++)
            {
                if (i <= 19)
                {
                    deck.Add(CardDataBase.cardList[1]);
                }
                else
                {
                    deck.Add(CardDataBase.cardList[4]);
                }
            }
        }
        Shuffle();
        StartCoroutine(StartGame());

    }

    // Update is called once per frame
    void Update()
    {
        staticEnemyDeck = deck;
        if (deckSize < 30)
        {
            cardInDeck1.SetActive(false);
        }
        if (deckSize < 20)
        {
            cardInDeck2.SetActive(false);
        }
        if (deckSize < 2)
        {
            cardInDeck3.SetActive(false);
        }
        if (deckSize < 1)
        {
            cardInDeck4.SetActive(false);
        }

        if (AICardToHand.DrawX > 0)
        {
            StartCoroutine(Draw(AICardToHand.DrawX));
            AICardToHand.DrawX = 0;
        }
        if (TurnSystem.startTurn == false && draw == false)
        {
            StartCoroutine(Draw(1));
            draw = true;
        }
        
        currentMana = TurnSystem.currentEnemyMana;
        if (0 == 0)
        {
            int j = 0;
            howManyCards = 0;
            foreach (Transform child in Hand.transform)
            {
                howManyCards++;
            }
            foreach (Transform child in Hand.transform)
            {
                cardsInHand[j] = child.GetComponent<AICardToHand>().thisCard;
                j++;
            }
            for (int i = 0; i < deckSize; i++)
            {
                if (i >= howManyCards)
                {
                    cardsInHand[i] = CardDataBase.cardList[0];
                }
            }
            j = 0;
        }
        if (TurnSystem.isYourTurn == false)
        {
            for (int i=0; i<deckSize; i++)
            {
                if (cardsInHand[i].id != 0)
                {
                    if (currentMana >= cardsInHand[i].mana)
                    {
                        AiCanSummon[i] = true;
                    }
                }
            }
        }
        else
        {
            for (int i=0; i < deckSize; i++)
            {
                AiCanSummon[i] = false;
            }
        }

        if (TurnSystem.isYourTurn == false)
        {
            drawPhase = true;
        }
        if (drawPhase == true && summonPhase == false && attackPhase == false)
        {
            StartCoroutine(WaitForSummonPhase());
        }
        if (TurnSystem.isYourTurn == true)
        {
            drawPhase = false;
            summonPhase = false;
            attackPhase = false;
            endPhase = false;
        }
        if (summonPhase == true)
        {
            summonID = 0;

            int index = 0;
            for (int i=0; i < deckSize; i++)
            {
                if (AiCanSummon[i] == true)
                {
                    cardsID[index] = cardsInHand[i].id;
                    index++;
                }
            }
            for (int i = 0; i < deckSize; i++)
            {
                if (cardsID[i] != 0)
                {
                    summonID = cardsID[i];
                    foreach(Transform child in Hand.transform)
                    {
                
                        if (child.GetComponent<AICardToHand>().id == summonID && CardDataBase.cardList[summonID].mana <= currentMana)
                        {
                            child.transform.SetParent(Zone.transform);
                            TurnSystem.currentEnemyMana -= CardDataBase.cardList[summonID].mana;
                            currentMana = TurnSystem.currentEnemyMana;
                        }
                
                    }
                }
            }

            summonPhase = false;
            attackPhase = true;
        }
        if (0 == 0)
        {
            int k = 0;
            int howManyCards2 = 0;
            foreach (Transform child in Zone.transform)
            {
                howManyCards2++;
            }
            foreach (Transform child in Zone.transform)
            {
                canAttack[k] = child.GetComponent<AICardToHand>().canAttack;
                k++;
            }
            for (int i = 0; i < deckSize; i++)
            {
                if (i >= howManyCards2)
                {
                    canAttack[i] = false;
                }
            }
            k = 0;
        }
        if (0 == 0)
        {
            int l = 0;
            int howManyCards3 = 0;
            foreach (Transform child in Zone.transform)
            {
                howManyCards3++;
            }
            foreach (Transform child in Zone.transform)
            {
                cardsInZone[l] = child.GetComponent<AICardToHand>().thisCard;
                l++;
            }
            for (int i = 0; i < deckSize; i++)
            {
                if (i >= howManyCards3)
                {
                    cardsInZone[i] = CardDataBase.cardList[0];
                }
            }
            l = 0;
        }
        if (attackPhase == true && endPhase == false)
        {
            for (int i = 0; i<deckSize; i++)
            {
                if (canAttack[i] == true) 
                {
                    PlayerHp.staticHp -= cardsInZone[i].dame;
                }
            }
            endPhase = true;
        }
        if (endPhase == true)
        {
            AiEndPhase = true;
        }

    }
    public void Shuffle()
    {
        for (int i=0;i<deckSize;i++)
        {
            container = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container;
        }
        
    }
    IEnumerator StartGame()
    {
        for (int i=0;i<= 2; i++)
        {
            yield return new WaitForSeconds(1);
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }
    IEnumerator ShuffleNow()
    {
        yield return new WaitForSeconds(1);
        Clones = GameObject.FindGameObjectsWithTag("Clone");
        foreach (GameObject clone in Clones)
        {
            Destroy(clone);
        }
    }
    IEnumerator Draw(int x)
    {
        for (int i =0; i<x;i++) 
        { 
            yield return new WaitForSeconds(1);
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }
    IEnumerator WaitOneSeconds()
    {
        yield return new WaitForSeconds(1);
    }
    IEnumerator WaitForSummonPhase()
    {
        yield return new WaitForSeconds(3);
        summonPhase = true;
    }
}
