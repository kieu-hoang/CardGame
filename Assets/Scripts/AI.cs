using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public Card container = new Card();
    public static List<Card> staticEnemyDeck = new List<Card>();
    public int[] startingDeck;
    public List<AICardToHand> cardsInHand = new List<AICardToHand>();

    public List<AICardToHand> cardsInZone = new List<AICardToHand>();
    
    public static GameObject Hand;
    public static GameObject Zone;
    public static GameObject PlayerZone;
    public static GameObject PlayerHand;
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
        PlayerZone = GameObject.Find("Zone");
        PlayerHand = GameObject.Find("Hand");
        Graveyard = GameObject.Find("EGraveyard");
        currentGame = new GameState();

        x = 0;
        draw = true;
        if (whichEnemy == 1)
        {
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
        }
        if (whichEnemy == 2)
        {
            for (int i = 0; i < DECKSIZE; i++)
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
        staticEnemyDeck = deck;
        
    }

    // Update is called once per frame
    void Update()
    {
        staticEnemyDeck = deck;
        checkClone();
        // if (started)
        // {
        //     getGameState();
        //     Log.SaveData(currentGame.toString());
        //     started = false;
        // }
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

        if (AICardToHand.DrawX == 1)
        {
            if (CardsInHand.eHowMany <= 6 && deckSize > 0)
                StartCoroutine(Draw(1));
            AICardToHand.DrawX = 0;
        }
        else if (AICardToHand.DrawX == 2)
        {
            if (CardsInHand.eHowMany <= 5 && deckSize > 0)
                StartCoroutine(Draw(2));
            else if (CardsInHand.eHowMany <= 6 && deckSize > 0)
                StartCoroutine(Draw(1));
            AICardToHand.DrawX = 0;
        }
        
        if (TurnSystem.isYourTurn)
        {
            drawPhase = false;
            summonPhase = false;
            attackPhase = false;
            endPhase = false;
        }
        
        // if (TurnSystem.startTurn == false && draw == false)
        // {
        //     if (CardsInHand.eHowMany < 7 && deckSize > 0)
        //         StartCoroutine(Draw(1));
        //     else if (deckSize <= 0)
        //         EnemyHp.staticHp -= 1;
        //     draw = true;
        // }
        
        if (draw == false && !TurnSystem.isYourTurn)
        {
            if (CardsInHand.eHowMany < 7 && deckSize > 0)
                StartCoroutine(Draw(1));
            else if (deckSize <= 0)
                EnemyHp.staticHp -= 1;
            if (currentGame != null)
            {
                getGameState();
                Log.SaveData(currentGame.toString());
            }
            draw = true;
        }
        
        currentMana = TurnSystem.currentEnemyMana;
        //Update Cards in Hand
        updateCIH();
        if (TurnSystem.isYourTurn == false)
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
            child.GetComponent<AICardToHand>().thisCard = staticEnemyDeck[deckSize - 1];
            deckSize -= 1;
            child.GetComponent<AICardToHand>().cardBack = false;
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
            cardsInHand[j] = child.GetComponent<AICardToHand>();
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
            
            cardsInZone[j] = child.GetComponent<AICardToHand>();
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
                canAttack[k] = child.GetComponent<AICardToHand>().canAttack;
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
        foreach (Transform child in PlayerZone.transform)
        {
            if (child.GetComponent<ThisCard>() != null && child.GetComponent<ThisCard>().blood - child.GetComponent<ThisCard>().hurted > 0)
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
            foreach (Transform child in PlayerZone.transform)
            {
                if (child.GetComponent<ThisCard>().blood - child.GetComponent<ThisCard>().hurted > 0)
                    howManyCards++;
            }
            noOfCardInPlayerZone = howManyCards;
            if (canAttack[i])
            {
                if (noOfCardInPlayerZone == 0)
                {
                    if (!cardsInZone[i].attackedTarget)
                    {
                        Log.SaveData("0 " + cardsInZone[i].id + " 0" + " 1" + " 0" + "\n");
                        PlayerHp.staticHp -= cardsInZone[i].actualDame;
                        cardsInZone[i].attackedTarget = true;
                        canAttack[i] = false;
                    }
                }
                else
                {
                    if (checkTaunt())
                    {
                        foreach (Transform child in PlayerZone.transform)
                        {
                            if (canAttack[i] && child.GetComponent<ThisCard>().blood - child.GetComponent<ThisCard>().hurted > 0)
                            {
                                if (child.GetComponent<ThisCard>().id == 1 || child.GetComponent<ThisCard>().id == 13 ||
                                    child.GetComponent<ThisCard>().id == 19)
                                {
                                    child.GetComponent<ThisCard>().isTarget = true;
                                }
                                if (!child.GetComponent<ThisCard>().isTarget) continue;
                                child.GetComponent<ThisCard>().hurted += cardsInZone[i].actualDame;
                                cardsInZone[i].hurted += child.GetComponent<ThisCard>().actualDame;
                                child.GetComponent<ThisCard>().isTarget = false;
                                canAttack[i] = false;
                                Log.SaveData("1 " + cardsInZone[i].id + " 0" + " 1 " + child.GetComponent<ThisCard>().id + "\n");
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (Transform child in PlayerZone.transform)
                        {
                            if (canAttack[i] && child.GetComponent<ThisCard>().blood - child.GetComponent<ThisCard>().hurted > 0)
                            {
                                child.GetComponent<ThisCard>().isTarget = true;
                                if (!child.GetComponent<ThisCard>().isTarget) continue;
                                if (child.GetComponent<ThisCard>().id == 17)
                                    child.GetComponent<ThisCard>().hurted += 1;
                                else 
                                    child.GetComponent<ThisCard>().hurted += cardsInZone[i].actualDame;
                                if (cardsInZone[i].id == 17)
                                    cardsInZone[i].hurted += 1;
                                else
                                    cardsInZone[i].hurted += child.GetComponent<ThisCard>().actualDame;
                                if (isMutualBirth(cardsInZone[i].GetComponent<AICardToHand>(), child.GetComponent<ThisCard>()))
                                {
                                    child.GetComponent<ThisCard>().hurted -= 2;
                                }
                                if (isOpposition(cardsInZone[i].GetComponent<AICardToHand>(), child.GetComponent<ThisCard>()))
                                {
                                    child.GetComponent<ThisCard>().hurted += 2;
                                }
                                child.GetComponent<ThisCard>().isTarget = false;
                                Log.SaveData("1 " + cardsInZone[i].id + " 0" + " 1 " + child.GetComponent<ThisCard>().id + "\n");
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
            if (TurnSystem.isYourTurn == false)
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
                if (child.GetComponent<AICardToHand>().id == summonID &&
                    CardDataBase.cardList[summonID].mana <= currentMana)
                {
                    Log.SaveData("0 " + summonID + " 1" + " 0" + " 0" +"\n");
                    child.transform.SetParent(Zone.transform);
                    TurnSystem.currentEnemyMana -= CardDataBase.cardList[summonID].mana;
                    currentMana = TurnSystem.currentEnemyMana;
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
            if (CardsInHand.eHowMany < 7 && deckSize > 0)
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
        foreach (Transform child in PlayerZone.transform)
        {
            if (child.GetComponent<ThisCard>().id == 1 || child.GetComponent<ThisCard>().id == 13 ||
                child.GetComponent<ThisCard>().id == 19)
            {
                if (child.GetComponent<ThisCard>().blood - child.GetComponent<ThisCard>().hurted > 0)
                    return true;
            }
        }
        return false;
    }
    public bool isMutualBirth(AICardToHand player, ThisCard enemy)
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
    public bool isOpposition(AICardToHand player, ThisCard enemy)
    {
        if (player.thisCard.element == Card.Element.NoElement  || enemy.thisCard.element == Card.Element.NoElement)
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
    public static void getGameState()
    {
        currentGame.playerTurn = TurnSystem.isYourTurn;
        currentGame.playerHp = PlayerHp.staticHp;
        currentGame.aiHp = EnemyHp.staticHp;
        currentGame.playerMana = TurnSystem.currentMana;
        currentGame.aiMana = TurnSystem.currentEnemyMana;
        currentGame.playerManaTurn = TurnSystem.maxMana;
        currentGame.aiManaTurn = TurnSystem.maxEnemyMana;
        currentGame.cardsInHand = new List<ThisCard1>();
        currentGame.cardsInZone = new List<ThisCard1>();
        currentGame.cardsInHandAI = new List<AICardToHand1>();
        currentGame.cardsInZoneAI = new List<AICardToHand1>();
        
        foreach (Transform child in PlayerHand.transform)
        {
            if (child.GetComponent<ThisCard>() != null)
                currentGame.cardsInHand.Add(child.GetComponent<ThisCard>().toThisCard1());
        }
        foreach (Transform child in PlayerZone.transform)
        {
            if (child.GetComponent<ThisCard>() != null)
            {
                currentGame.cardsInZone.Add(child.GetComponent<ThisCard>().toThisCard1());
                if (child.GetComponent<ThisCard>().canAttack)
                {
                    int x = currentGame.cardsInZone.Count - 1;
                    currentGame.cardsInZone[x].canAttack = true;
                }
            }
        }
        
        foreach (Transform child in Hand.transform)
        {
            if (child.GetComponent<AICardToHand>() != null)
                currentGame.cardsInHandAI.Add(child.GetComponent<AICardToHand>().ToAICardToHand1());
        }
        foreach (Transform child in Zone.transform)
        {
            if (child.GetComponent<AICardToHand>() != null)
            {
                currentGame.cardsInZoneAI.Add(child.GetComponent<AICardToHand>().ToAICardToHand1());
                if (child.GetComponent<AICardToHand>().canAttack)
                {
                    int x = currentGame.cardsInZoneAI.Count - 1;
                    currentGame.cardsInZoneAI[x].canAttack = true;
                }
            }
        }

        for (int i = 0; i < deckSize; i++)
        {
            currentGame.AIdeck.Add(staticEnemyDeck[i].ToAICardToHand1());
        }
        for (int i = 0; i < PlayerDeck.deckSize; i++)
        {
            currentGame.deck.Add(PlayerDeck.staticDeck[i].toThisCard1());
        }
    }
}
