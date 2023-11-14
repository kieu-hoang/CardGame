using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIa : MonoBehaviour //Kich ban vs Minimax
{
    public List<Card> deck = new List<Card>();
    public Card container = new Card();
    public static List<Card> staticEnemyDeck = new List<Card>();
    public int[] startingDeck;
    public List<AI1CardToHand> cardsInHand = new List<AI1CardToHand>();

    public List<AI1CardToHand> cardsInZone = new List<AI1CardToHand>();
    
    public static GameObject Hand;
    public static GameObject Zone;
    public static GameObject MinimaxZone;
    public static GameObject MinimaxHand;
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
    
    public AI1CardToHand aiCardToHand;

    public int summonID;

    public int howManyCards;

    public bool[] canAttack;
    public static bool AiEndPhase;
    public static int whichEnemy;
    private const int DECKSIZE = 30;
    
    public AudioSource audioSource;
    public AudioClip drawAudio;
    private int noOfCardCanSummon;
    private int noOfCardInHand;

    private int noOfCardInZone;

    private int noOfCardInPlayerZone;
    public static GameState currentGame;

    public static bool started;
    // Start is called before the first frame update
    private void Awake()
    {
        deckSize = 30;
        started = false;
    }
    void Start()
    {
        //StartCoroutine(WaitOneSeconds());

        //StartCoroutine(StartGame());
        Hand = GameObject.Find("EnemyHand");
        Zone = GameObject.Find("EnemyZone");
        MinimaxZone = GameObject.Find("Zone");
        MinimaxHand = GameObject.Find("Hand");
        Graveyard = GameObject.Find("EGraveyard");
        currentGame = new GameState();

        x = 0;
        draw = true;
        for (int i = 0; i <= 25; i++)
        {
            startingDeck[i] = i == 0 ? PlayerPrefs.GetInt("deck" + i, 0) : PlayerPrefs.GetInt("deck" + i, i<5 ? 2 : 1);
        }
        for (int i = 0; i <= 25; i++)
        {
            if (startingDeck[i] > 0)
            {
                for (int j = 1; j <= startingDeck[i]; j++)
                {
                    deck[x] = CardDataBase.cardList[i];
                    x++;
                }
            }
        }
        Shuffle();
        StartCoroutine(StartGame());
        staticEnemyDeck = deck;
        
    }

    // Update is called once per frame
    void Update()
    {
        staticEnemyDeck = deck;
        checkClone();
        if (deckSize < 20)
        {
            cardInDeck1.SetActive(false);
        }
        if (deckSize < 10)
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

        if (AI1CardToHand.DrawX == 1)
        {
            if (CardsInHand.eHowMany1 <= 6 && deckSize > 0)
                StartCoroutine(Draw(1));
            AI1CardToHand.DrawX = 0;
        }
        else if (AI1CardToHand.DrawX == 2)
        {
            if (CardsInHand.eHowMany1 <= 5 && deckSize > 1)
                StartCoroutine(Draw(2));
            else if (CardsInHand.eHowMany1 <= 6 && deckSize > 0)
                StartCoroutine(Draw(1));
            AI1CardToHand.DrawX = 0;
        }
        
        if (TurnSystem1.isYourTurn)
        {
            drawPhase = false;
            summonPhase = false;
            attackPhase = false;
            endPhase = false;
        }
        
        // if (TurnSystem1.startTurn == false && draw == false)
        // {
        //     if (CardsInHand.eHowMany < 7 && deckSize > 0)
        //         StartCoroutine(Draw(1));
        //     else if (deckSize <= 0)
        //         EnemyHp.staticHp -= 1;
        //     draw = true;
        // }
        
        if (draw == false && !TurnSystem1.isYourTurn)
        {
            if (CardsInHand.eHowMany1 < 7 && deckSize > 0)
                StartCoroutine(Draw(1));
            else if (deckSize <= 0)
                KbHp.staticHp -= 1;
            draw = true;
        }
        
        currentMana = TurnSystem1.currentEnemyMana;
        //Update Cards in Hand
        updateCIH();
        if (TurnSystem1.isYourTurn == false)
        {
            drawPhase = true;
        }
        if (drawPhase && summonPhase == false && attackPhase == false)
        {
            StartCoroutine(WaitForSummonPhase());
        }
        
        if (summonPhase)
        {
            StartCoroutine(Summon());
        }
        updateCIZ();
        if (attackPhase && endPhase == false)
        {
            //StartCoroutine(EndPhase());
            DoEndPhase();
        }
        if (endPhase)
        {
            AiEndPhase = true;
        }
        
    }
    public void checkClone()
    {
        foreach (Transform child in Hand.transform)
        {
            if (child.tag != "Clone") continue;
            child.GetComponent<AI1CardToHand>().thisCard = staticEnemyDeck[deckSize - 1];
            deckSize -= 1;
            child.GetComponent<AI1CardToHand>().cardBack = false;
            child.tag = "Untagged";
        }
    }
    
    IEnumerator StartAttackPhase()
    {
        yield return new WaitForSeconds(1f);
        attackPhase = true;
    }
    IEnumerator EndPhase()
    {
        yield return new WaitForSeconds(1.5f);
        DoEndPhase();
    }
    public void updateCIH()
    {
        int j = 0;
        howManyCards = 0;
        foreach (Transform child in Hand.transform)
        {
            howManyCards++;
        }
        foreach (Transform child in Hand.transform)
        {
            cardsInHand[j] = child.GetComponent<AI1CardToHand>();
            j++;
        }
        for (int i = 0; i < DECKSIZE; i++)
        {
            if (i >= howManyCards)
            {
                cardsInHand[i] = aiCardToHand;
            }
        }

        noOfCardInHand = howManyCards;
        j = 0;
        int howManyCards2 = 0;
        foreach (Transform child in Zone.transform)
        {
            howManyCards2++;
        }

        noOfCardInZone = howManyCards2;
    }
    
    public void updateCIZ()
    {
        int j = 0;
        howManyCards = 0;
        foreach (Transform child in Zone.transform)
        {
            howManyCards++;
        }
        foreach (Transform child in Zone.transform)
        {
            cardsInZone[j] = child.GetComponent<AI1CardToHand>();
            j++;
        }
        for (int i = 0; i < DECKSIZE; i++)
        {
            if (i >= howManyCards)
            {
                cardsInZone[i] = aiCardToHand;
            }
        }

        noOfCardInZone = howManyCards;
        j = 0;
        if (attackPhase)
        {
            int k = 0;
            foreach (Transform child in Zone.transform)
            {
                canAttack[k] = child.GetComponent<AI1CardToHand>().canAttack;
                k++;
            }
            for (int i = 0; i < DECKSIZE; i++)
            {
                if (i >= howManyCards)
                {
                    canAttack[i] = false;
                }
            }
        }
        int howManyCards2 = 0;
        foreach (Transform child in MinimaxZone.transform)
        {
            if (child.GetComponent<AI2CardToHand>() != null && child.GetComponent<AI2CardToHand>().blood - child.GetComponent<AI2CardToHand>().hurted > 0)
                howManyCards2++;
        }

        noOfCardInPlayerZone = howManyCards2;
    }

    public void DoEndPhase()
    {
        //yield return new WaitForSecondsRealtime(0f);
        //updateCIZ();
        for (int i = 0; i< noOfCardInZone ; i++)
        {
            howManyCards = 0;
            foreach (Transform child in MinimaxZone.transform)
            {
                if (child.GetComponent<AI2CardToHand>().blood - child.GetComponent<AI2CardToHand>().hurted > 0)
                    howManyCards++;
            }
            noOfCardInPlayerZone = howManyCards;
            if (canAttack[i])
            {
                if (noOfCardInPlayerZone == 0)
                {
                    if (!cardsInZone[i].attackedTarget)
                    {
                        MinimaxHp.staticHp -= cardsInZone[i].actualDame;
                        cardsInZone[i].attackedTarget = true;
                        canAttack[i] = false;
                    }
                }
                else
                {
                    if (checkTaunt())
                    {
                        foreach (Transform child in MinimaxZone.transform)
                        {
                            if (canAttack[i] && child.GetComponent<AI2CardToHand>().blood - child.GetComponent<AI2CardToHand>().hurted > 0)
                            {
                                if (child.GetComponent<AI2CardToHand>().id == 1 || child.GetComponent<AI2CardToHand>().id == 13 ||
                                    child.GetComponent<AI2CardToHand>().id == 19)
                                {
                                    child.GetComponent<AI2CardToHand>().isTarget = true;
                                }
                                if (!child.GetComponent<AI2CardToHand>().isTarget) continue;
                                child.GetComponent<AI2CardToHand>().hurted += cardsInZone[i].actualDame;
                                cardsInZone[i].hurted += child.GetComponent<AI2CardToHand>().actualDame;
                                child.GetComponent<AI2CardToHand>().isTarget = false;
                                canAttack[i] = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (Transform child in MinimaxZone.transform)
                        {
                            if (canAttack[i] && child.GetComponent<AI2CardToHand>().blood - child.GetComponent<AI2CardToHand>().hurted > 0)
                            {
                                child.GetComponent<AI2CardToHand>().isTarget = true;
                                if (!child.GetComponent<AI2CardToHand>().isTarget) continue;
                                if (child.GetComponent<AI2CardToHand>().id == 17)
                                    child.GetComponent<AI2CardToHand>().hurted += 1;
                                else 
                                    child.GetComponent<AI2CardToHand>().hurted += cardsInZone[i].actualDame;
                                if (cardsInZone[i].id == 17)
                                    cardsInZone[i].hurted += 1;
                                else
                                    cardsInZone[i].hurted += child.GetComponent<AI2CardToHand>().actualDame;
                                if (isMutualBirth(cardsInZone[i].GetComponent<AI1CardToHand>(), child.GetComponent<AI2CardToHand>()))
                                {
                                    child.GetComponent<AI2CardToHand>().hurted -= 2;
                                }
                                if (isOpposition(cardsInZone[i].GetComponent<AI1CardToHand>(), child.GetComponent<AI2CardToHand>()))
                                {
                                    child.GetComponent<AI2CardToHand>().hurted += 2;
                                }
                                child.GetComponent<AI2CardToHand>().isTarget = false;
                                canAttack[i] = false;
                                break;
                            }
                        }
                    }
                }

                // float time = 100f;
                // while (time > 0)
                // {
                //     time -= Time.deltaTime;
                // }
                DoEndPhase();
            }
        }
        endPhase = true;
    }
    public void Shuffle()
    {
        for (int i=0;i<deckSize;i++)
        {
            container = deck[i];
            int randomIndex = UnityEngine.Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container;
        }
        
    }
    IEnumerator StartGame()
    {
        for (int i=0;i<= 2; i++)
        {
            yield return new WaitForSeconds(1f);
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
        checkClone();
        started = true;
    }

    IEnumerator Summon()
    {
        yield return new WaitForSecondsRealtime(1f);
        updateCIH();
        if (noOfCardInHand <= 0)
        {
            summonPhase = false;
            StartCoroutine(StartAttackPhase());
        }
        else
        {
            if (TurnSystem1.isYourTurn == false)
            {
                for (int i=0; i< noOfCardInHand; i++)
                {
                    if (currentMana >= cardsInHand[i].mana && noOfCardInZone < 5)
                    {
                        AiCanSummon[i] = true;
                    }
                    else
                    {
                        AiCanSummon[i] = false;
                    }
                }
                for (int i = noOfCardInHand; i < DECKSIZE; i++)
                {
                    AiCanSummon[i] = false;
                }
            }
            else
            {
                for (int i=0; i < DECKSIZE; i++)
                {
                    AiCanSummon[i] = false;
                }
            }
            int index = 0;
            noOfCardCanSummon = 0;
            for (int i = 0; i < noOfCardInHand; i++)
            {
                if (AiCanSummon[i])
                {
                    cardsID[index] = cardsInHand[i].id;
                    index++;
                    noOfCardCanSummon = index;
                }
            }
            // Random for checking
            foreach (Transform child in Hand.transform)
            {
                summonID = cardsID[0];
                if (child.GetComponent<AI1CardToHand>().id == summonID &&
                    CardDataBase.cardList[summonID].mana <= currentMana)
                {
                    child.transform.SetParent(Zone.transform);
                    TurnSystem1.currentEnemyMana -= CardDataBase.cardList[summonID].mana;
                    currentMana = TurnSystem1.currentEnemyMana;
                    break;
                }
            }
            if (noOfCardCanSummon == 1 || noOfCardCanSummon == 0 || currentMana == 0)
            {
                summonPhase = false;
                StartCoroutine(StartAttackPhase()); 
            }
            else
            {
                StartCoroutine(Summon());
            }
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
            if (CardsInHand.eHowMany1 < 7 && deckSize > 0)
            {
                yield return new WaitForSeconds(1f);
                audioSource.PlayOneShot(drawAudio,1f);
                Instantiate(CardToHand, transform.position, transform.rotation);
            }
        }
    }
    
    IEnumerator WaitForSummonPhase()
    {
        yield return new WaitForSeconds(3);
        summonPhase = true;
    }

    public bool checkTaunt()
    {
        foreach (Transform child in MinimaxZone.transform)
        {
            if (child.GetComponent<AI2CardToHand>().id == 1 || child.GetComponent<AI2CardToHand>().id == 13 ||
                child.GetComponent<AI2CardToHand>().id == 19)
            {
                if (child.GetComponent<AI2CardToHand>().blood - child.GetComponent<AI2CardToHand>().hurted > 0)
                    return true;
            }
        }
        return false;
    }
    public bool isMutualBirth(AI1CardToHand player, AI2CardToHand enemy)
    {
        if (player.thisCard.element == Card.Element.NoElement  || enemy.thisCard.element == Card.Element.NoElement)
            return false;
        if (player.thisCard.element == Card.Element.Earth && enemy.thisCard.element == Card.Element.Metal)
            return true;
        if (player.thisCard.element == Card.Element.Metal && enemy.thisCard.element == Card.Element.Water)
            return true;
        if (player.thisCard.element == Card.Element.Water && enemy.thisCard.element == Card.Element.Wood)
            return true;
        if (player.thisCard.element == Card.Element.Wood && enemy.thisCard.element == Card.Element.Fire)
            return true;
        if (player.thisCard.element == Card.Element.Fire && enemy.thisCard.element == Card.Element.Earth)
            return true;
        return false;
    }

    public bool isOpposition(AI1CardToHand player, AI2CardToHand enemy)
    {
        if (player.thisCard.element == Card.Element.NoElement || enemy.thisCard.element == Card.Element.NoElement)
            return false;
        if (player.thisCard.element == Card.Element.Earth && enemy.thisCard.element == Card.Element.Water)
            return true;
        if (player.thisCard.element == Card.Element.Metal && enemy.thisCard.element == Card.Element.Wood)
            return true;
        if (player.thisCard.element == Card.Element.Water && enemy.thisCard.element == Card.Element.Fire)
            return true;
        if (player.thisCard.element == Card.Element.Wood && enemy.thisCard.element == Card.Element.Earth)
            return true;
        if (player.thisCard.element == Card.Element.Fire && enemy.thisCard.element == Card.Element.Metal)
            return true;
        return false;
    }
}
