using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AI1 : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public Card container = new Card();
    public static List<Card> staticEnemyDeck = new List<Card>();
    public int[] startingDeck;

    //public List<AICardToHand> cardsInHand = new List<AICardToHand>();

    //public List<AICardToHand> cardsInZone = new List<AICardToHand>();
    
    public GameObject Hand;
    public GameObject Zone;
    public GameObject PlayerZone;
    public GameObject PlayerHand;
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

    //public bool[] AiCanSummon;

    public bool drawPhase;
    public bool summonPhase;
    public bool attackPhase;
    public bool endPhase;

    //public int[] cardsID;
    
    //public AICardToHand aiCardToHand;

    //public int summonID;

    //public int howManyCards;

    //public bool[] canAttack;
    public static bool AiEndPhase;
    public static int whichEnemy;
    private const int DECKSIZE = 30;
    
    public AudioSource audioSource;
    public AudioClip drawAudio;
    private int noOfCardCanSummon;
    private int noOfCardInHand;

    private int noOfCardInZone;

    private int noOfCardInPlayerZone;

    private GameState currentGame;
    // Start is called before the first frame update
    private void Awake()
    {
        deckSize = 30;
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

        if (AICardToHand.DrawX > 0)
        {
            if (CardsInHand.eHowMany == 6 && deckSize > 0)
                StartCoroutine(Draw(1));
            else
            {
                for (int i = 0; i < AICardToHand.DrawX; i++)
                {
                    if (CardsInHand.eHowMany < 7 && deckSize > 0)
                        StartCoroutine(Draw(1));
                }
            }
            AICardToHand.DrawX = 0;
        }
        
        if (TurnSystem.isYourTurn)
        {
            drawPhase = false;
            summonPhase = false;
            attackPhase = false;
            endPhase = false;
            draw = false;
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
            draw = true;
        }
        
        currentMana = TurnSystem.currentEnemyMana;
        //Update Cards in Hand
        // updateCIH();
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
            // StartCoroutine(Summon());
            getGameState();
            GameState gs = currentGame;
            List<Move> nextmove = minimax(gs, true, 0, -1000, 1000).Item2;
            make_move(nextmove);
            endPhase = true;
        }
        // updateCIZ();
        // if (attackPhase && endPhase == false)
        // {
        //     //StartCoroutine(EndPhase());
        //     DoEndPhase();
        // }
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
        //DoEndPhase();
    }
    // public void updateCIH()
    // {
    //     int j = 0;
    //     howManyCards = 0;
    //     foreach (Transform child in Hand.transform)
    //     {
    //         howManyCards++;
    //     }
    //     foreach (Transform child in Hand.transform)
    //     {
    //         cardsInHand[j] = child.GetComponent<AICardToHand>();
    //         j++;
    //     }
    //     for (int i = 0; i < DECKSIZE; i++)
    //     {
    //         if (i >= howManyCards)
    //         {
    //             cardsInHand[i] = aiCardToHand;
    //         }
    //     }
    //
    //     noOfCardInHand = howManyCards;
    //     j = 0;
    //     int howManyCards2 = 0;
    //     foreach (Transform child in Zone.transform)
    //     {
    //         howManyCards2++;
    //     }
    //
    //     noOfCardInZone = howManyCards2;
    // }
    
    // public void updateCIZ()
    // {
    //     int j = 0;
    //     howManyCards = 0;
    //     foreach (Transform child in Zone.transform)
    //     {
    //         howManyCards++;
    //     }
    //     foreach (Transform child in Zone.transform)
    //     {
    //         
    //         cardsInZone[j] = child.GetComponent<AICardToHand>();
    //         j++;
    //     }
    //     for (int i = 0; i < DECKSIZE; i++)
    //     {
    //         if (i >= howManyCards)
    //         {
    //             cardsInZone[i] = aiCardToHand;
    //         }
    //     }
    //
    //     noOfCardInZone = howManyCards;
    //     j = 0;
    //     if (attackPhase)
    //     {
    //         int k = 0;
    //         foreach (Transform child in Zone.transform)
    //         {
    //             canAttack[k] = child.GetComponent<AICardToHand>().canAttack;
    //             k++;
    //         }
    //         for (int i = 0; i < DECKSIZE; i++)
    //         {
    //             if (i >= howManyCards)
    //             {
    //                 canAttack[i] = false;
    //             }
    //         }
    //     }
    //     int howManyCards2 = 0;
    //     foreach (Transform child in PlayerZone.transform)
    //     {
    //         if (child.GetComponent<ThisCard>() != null && child.GetComponent<ThisCard>().blood - child.GetComponent<ThisCard>().hurted > 0)
    //             howManyCards2++;
    //     }
    //
    //     noOfCardInPlayerZone = howManyCards2;
    // }

    // public void DoEndPhase()
    // {
    //     //yield return new WaitForSecondsRealtime(0f);
    //     //updateCIZ();
    //     for (int i = 0; i< deckSize ; i++)
    //     {
    //         howManyCards = 0;
    //         foreach (Transform child in PlayerZone.transform)
    //         {
    //             if (child.GetComponent<ThisCard>().blood - child.GetComponent<ThisCard>().hurted > 0)
    //                 howManyCards++;
    //         }
    //         noOfCardInPlayerZone = howManyCards;
    //         if (canAttack[i])
    //         {
    //             if (noOfCardInPlayerZone == 0)
    //             {
    //                 if (!cardsInZone[i].attackedTarget)
    //                 {
    //                     PlayerHp.staticHp -= cardsInZone[i].actualDame;
    //                     cardsInZone[i].attackedTarget = true;
    //                     canAttack[i] = false;
    //                 }
    //             }
    //             else
    //             {
    //                 if (checkTaunt())
    //                 {
    //                     foreach (Transform child in PlayerZone.transform)
    //                     {
    //                         if (canAttack[i] && child.GetComponent<ThisCard>().blood - child.GetComponent<ThisCard>().hurted > 0)
    //                         {
    //                             if (child.GetComponent<ThisCard>().id == 1 || child.GetComponent<ThisCard>().id == 13 ||
    //                                 child.GetComponent<ThisCard>().id == 19)
    //                             {
    //                                 child.GetComponent<ThisCard>().isTarget = true;
    //                             }
    //                             if (!child.GetComponent<ThisCard>().isTarget) continue;
    //                             child.GetComponent<ThisCard>().hurted += cardsInZone[i].actualDame;
    //                             cardsInZone[i].hurted += child.GetComponent<ThisCard>().actualDame;
    //                             child.GetComponent<ThisCard>().isTarget = false;
    //                             canAttack[i] = false;
    //                             break;
    //                         }
    //                     }
    //                 }
    //                 else
    //                 {
    //                     foreach (Transform child in PlayerZone.transform)
    //                     {
    //                         if (canAttack[i] && child.GetComponent<ThisCard>().blood - child.GetComponent<ThisCard>().hurted > 0)
    //                         {
    //                             child.GetComponent<ThisCard>().isTarget = true;
    //                             if (!child.GetComponent<ThisCard>().isTarget) continue;
    //                             if (child.GetComponent<ThisCard>().id == 17)
    //                                 child.GetComponent<ThisCard>().hurted += 1;
    //                             else 
    //                                 child.GetComponent<ThisCard>().hurted += cardsInZone[i].actualDame;
    //                             if (cardsInZone[i].id == 17)
    //                                 cardsInZone[i].hurted += 1;
    //                             else
    //                                 cardsInZone[i].hurted += child.GetComponent<ThisCard>().actualDame;
    //                             if (isMutualBirth(cardsInZone[i].GetComponent<AICardToHand>(), child.GetComponent<ThisCard>()))
    //                             {
    //                                 child.GetComponent<ThisCard>().hurted -= 2;
    //                             }
    //                             if (isOpposition(cardsInZone[i].GetComponent<AICardToHand>(), child.GetComponent<ThisCard>()))
    //                             {
    //                                 child.GetComponent<ThisCard>().hurted += 2;
    //                             }
    //                             child.GetComponent<ThisCard>().isTarget = false;
    //                             canAttack[i] = false;
    //                             break;
    //                         }
    //                     }
    //                 }
    //             }
    //             DoEndPhase();
    //         }
    //     }
    //     endPhase = true;
    // }
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
            yield return new WaitForSeconds(1);
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }

    // IEnumerator Summon()
    // {
    //     yield return new WaitForSecondsRealtime(1f);
    //     updateCIH();
    //     if (TurnSystem.isYourTurn == false)
    //     {
    //         for (int i=0; i< noOfCardInHand; i++)
    //         {
    //             if (currentMana >= cardsInHand[i].mana && noOfCardInZone < 5)
    //             {
    //                 AiCanSummon[i] = true;
    //             }
    //             else
    //             {
    //                 AiCanSummon[i] = false;
    //             }
    //         }
    //         for (int i = noOfCardInHand; i < DECKSIZE; i++)
    //         {
    //             AiCanSummon[i] = false;
    //         }
    //     }
    //     else
    //     {
    //         for (int i=0; i < DECKSIZE; i++)
    //         {
    //             AiCanSummon[i] = false;
    //         }
    //     }
    //     int index = 0;
    //     noOfCardCanSummon = 0;
    //     for (int i = 0; i < noOfCardInHand; i++)
    //     {
    //         if (AiCanSummon[i])
    //         {
    //             cardsID[index] = cardsInHand[i].id;
    //             index++;
    //             noOfCardCanSummon = index;
    //         }
    //     }
    //     // Random for checking
    //     foreach (Transform child in Hand.transform)
    //     {
    //         summonID = cardsID[0];
    //         if (child.GetComponent<AICardToHand>().id == summonID &&
    //             CardDataBase.cardList[summonID].mana <= currentMana)
    //         {
    //             child.transform.SetParent(Zone.transform);
    //             TurnSystem.currentEnemyMana -= CardDataBase.cardList[summonID].mana;
    //             currentMana = TurnSystem.currentEnemyMana;
    //             break;
    //         }
    //     }

        // // Optimize Card Summoning
        // int[,] L = new int[index + 1, currentMana + 1];
        //
        // for (int i = 0; i <= index; i++)
        // {
        //     L[i, 0] = 0;
        // }
        //
        // for (int j = 0; j <= currentMana; j++)
        // {
        //     L[0, j] = 0;
        // }
        //
        // for (int i = 1; i <= index; i++)
        // {
        //     for (int j = 1; j <= currentMana; j++)
        //     {
        //         if (CardDataBase.cardList[cardsID[i - 1]].mana > j)
        //             L[i, j] = L[i - 1, j];
        //         else
        //             L[i, j] = Math.Max(L[i - 1, j],
        //                 L[i - 1, j - CardDataBase.cardList[cardsID[i - 1]].mana] 
        //                 + CardDataBase.cardList[cardsID[i - 1]].dame 
        //                 + CardDataBase.cardList[cardsID[i - 1]].blood);
        //     }
        // }
        //
        // // Summoning Cards
        // while (index > 0)
        // {
        //     if (L[index, currentMana] == L[index - 1, currentMana])
        //     {
        //         index--;
        //     }
        //     else
        //     {
        //         summonID = cardsID[index - 1];
        //         foreach (Transform child in Hand.transform)
        //         {
        //             if (child.GetComponent<AICardToHand>().id == summonID && CardDataBase.cardList[summonID].mana <= currentMana)
        //             {
        //                 child.transform.SetParent(Zone.transform);
        //                 TurnSystem.currentEnemyMana -= CardDataBase.cardList[summonID].mana;
        //                 currentMana = TurnSystem.currentEnemyMana;
        //             }
        //         }
        //         index--;
        //     }
        // }
    //     if (noOfCardCanSummon == 1 || noOfCardCanSummon == 0 || currentMana == 0)
    //     {
    //         summonPhase = false;
    //         StartCoroutine(StartAttackPhase()); 
    //     }
    //     else
    //     {
    //         StartCoroutine(Summon());
    //     }
    // }
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

    public void getGameState()
    {
        currentGame.playerTurn = TurnSystem.isYourTurn;
        currentGame.playerHp = PlayerHp.staticHp;
        currentGame.aiHp = EnemyHp.staticHp;
        currentGame.playerMana = TurnSystem.currentMana;
        currentGame.aiMana = TurnSystem.currentEnemyMana;
        foreach (Transform child in PlayerHand.transform)
        {
            currentGame.cardsInHand.Add(child.GetComponent<ThisCard>().toThisCard1());
        }
        foreach (Transform child in PlayerZone.transform)
        {
            currentGame.cardsInZone.Add(child.GetComponent<ThisCard>().toThisCard1());
        }
        foreach (Transform child in Hand.transform)
        {
            currentGame.cardsInHandAI.Add(child.GetComponent<AICardToHand>().ToAICardToHand1());
        }
        foreach (Transform child in Zone.transform)
        {
            currentGame.cardsInZoneAI.Add(child.GetComponent<AICardToHand>().ToAICardToHand1());
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

    public void make_move(List<Move> moves)
    {
        foreach (Move mo in moves)
        {
            if (mo.summon)
            {
                Summon(mo.id);
            }

            if (mo.attack)
            {
                if (mo.idAtk == 0)
                {
                    foreach (Transform child in Zone.transform)
                    {
                        if (child.GetComponent<AICardToHand>().id == mo.id)
                        {
                            PlayerHp.staticHp -= child.GetComponent<AICardToHand>().actualDame;
                            break;
                        }
                    }
                }
                else
                {
                    Attack(mo.id, mo.idAtk);
                }
            }
        }
    }

    public void Summon(int id)
    {
        foreach (Transform child in Hand.transform)
        {
            if (child.GetComponent<AICardToHand>().id == id)
            {
                child.transform.SetParent(Zone.transform);
                TurnSystem.currentEnemyMana -= CardDataBase.cardList[id].mana;
                currentMana = TurnSystem.currentEnemyMana;
                break;
            }
        }
    }

    public void Attack(int id1, int id2)
    {
        foreach (Transform child in Zone.transform)
        {
            if (child.GetComponent<AICardToHand>().id == id1)
            {
                foreach (Transform child1 in PlayerZone.transform)
                {
                    if (child1.GetComponent<ThisCard>().id == id2)
                    {
                        if (id1 == 17)
                        {
                            child.GetComponent<AICardToHand>().hurted += 1;
                        }
                        else
                            child.GetComponent<AICardToHand>().hurted += child1.GetComponent<ThisCard>().actualDame;

                        if (id2 == 17)
                        {
                            child1.GetComponent<ThisCard>().hurted += 1;
                        }
                        else
                        {
                            child1.GetComponent<ThisCard>().hurted += child.GetComponent<AICardToHand>().actualDame;
                        }

                        if (isMutualBirth(child.GetComponent<AICardToHand>(), child1.GetComponent<ThisCard>()))
                        {
                            child1.GetComponent<ThisCard>().hurted -= 2;
                        }

                        if (isOpposition(child.GetComponent<AICardToHand>(), child1.GetComponent<ThisCard>()))
                        {
                            child1.GetComponent<ThisCard>().hurted += 2;
                        }
                        break;
                    }
                }
                break;
            }
        }
    }

    public Tuple<int, List<Move>> minimax(GameState gs, bool maximizing, int depth, int alpha, int beta)
    {
        List<Move> moves = new List<Move>();
        if (gs.checkGameOver())
        {
            if (gs.playerHp <= 0)
                return new Tuple<int, List<Move>>(1000, moves);
            else if (gs.aiHp <= 0)
            {
                return new Tuple<int, List<Move>>(-1000, moves);
            }
        }
        else if (depth >= 5)
        {
            return new Tuple<int, List<Move>>(gs.evaluate(), moves);
        }

        if (maximizing)
        {
            int max_eval = -1000;
            List<Move> best_move = new List<Move>();
            List<List<Move>> all_valid_moves = gs.getValidMoves();
            foreach (List<Move> valid_move in all_valid_moves)
            {
                GameState tempGs = new GameState();
                tempGs.copy(gs);
                tempGs.make_move(valid_move);
                tempGs.nextTurn();
                int eval = minimax(tempGs, false, depth + 1, alpha, beta).Item1;
                Debug.Log("Evaluate: " + eval);
                if (eval > max_eval)
                {
                    max_eval = eval;
                    best_move = valid_move;
                }
                alpha = Math.Max(alpha, max_eval);
                if (beta <= alpha)
                    break;
            }
            Debug.Log("Evaluate: " + max_eval);
            return new Tuple<int, List<Move>>(max_eval, best_move);
        }
        else
        {
            int min_eval = 1000;
            List<Move> best_move = new List<Move>();
            List<List<Move>> all_valid_moves = gs.getValidMoves();
            foreach (List<Move> valid_move in all_valid_moves)
            {
                GameState tempGs = new GameState();
                tempGs.copy(gs);
                tempGs.make_move(valid_move);
                tempGs.nextTurn();
                int eval = minimax(tempGs, true, depth + 1, alpha, beta).Item1;
                Debug.Log("Evaluate: " + eval);
                if (eval < min_eval)
                {
                    min_eval = eval;
                    best_move = valid_move;
                }
                beta = Math.Min(beta, min_eval);
                if (beta <= alpha)
                    break;
            }
            return new Tuple<int, List<Move>>(min_eval, best_move);
        }
    }
}
