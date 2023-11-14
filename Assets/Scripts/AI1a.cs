using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading;

public class AI1a : MonoBehaviour //Minimax vs Kịch bản
{
    public List<Card> deck = new List<Card>();
    public Card container = new Card();
    public static List<Card> staticEnemyDeck = new List<Card>();
    public int[] startingDeck;

    public GameObject Hand;
    public GameObject Zone;
    public GameObject KbZone;
    public GameObject KbHand;
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

    public bool drawPhase;
    public bool summonPhase;
    public bool attackPhase;
    public bool endPhase;
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
    
    // Start is called before the first frame update
    private void Awake()
    {
        deckSize = 30;
    }
    void Start()
    {
        //StartCoroutine(WaitOneSeconds());
        Thread mainThread = Thread.CurrentThread;

        //StartCoroutine(StartGame());
        KbHand = GameObject.Find("EnemyHand");
        KbZone = GameObject.Find("EnemyZone");
        Zone = GameObject.Find("Zone");
        Hand = GameObject.Find("Hand");
        Graveyard = GameObject.Find("Graveyard");
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

        if (AI2CardToHand.DrawX == 1)
        {
            if (CardsInHand.howMany1 <= 6 && deckSize > 0)
                StartCoroutine(Draw(1));
            AI2CardToHand.DrawX = 0;
        }
        else if (AI2CardToHand.DrawX == 2)
        {
            if (CardsInHand.howMany1 <= 5 && deckSize > 0)
                StartCoroutine(Draw(2));
            else if (CardsInHand.howMany1 <= 6 && deckSize > 0)
                StartCoroutine(Draw(1));
            AI2CardToHand.DrawX = 0;
        }
        
        if (!TurnSystem1.isYourTurn)
        {
            drawPhase = false;
            summonPhase = false;
            attackPhase = false;
            endPhase = false;
            draw = false;
        }

        if (draw == false && TurnSystem1.isYourTurn)
        {
            if (CardsInHand.howMany1 < 7 && deckSize > 0)
                StartCoroutine(Draw(1));
            else if (deckSize <= 0)
                MinimaxHp.staticHp -= 1;
            draw = true;
        }
        
        currentMana = TurnSystem1.currentMana;
        if (TurnSystem1.isYourTurn)
        {
            drawPhase = true;
        }
        if (drawPhase && summonPhase == false && attackPhase == false)
        {
            StartCoroutine(WaitForSummonPhase());
        }
        
        if (summonPhase)
        {
            getGameState();
            List<Move> nextmove = minimax(currentGame, false, 0, -1000, 1000).Item2;
            for (int i = 0; i < nextmove.Count; i++)
            {
                Debug.Log("Move " + i + ": " + nextmove[i].id + " " + nextmove[i].summon + " " + nextmove[i].attack + " " + nextmove[i].idAtk);
            }
            make_move(nextmove);
            endPhase = true;
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
            child.GetComponent<AI2CardToHand>().thisCard = staticEnemyDeck[deckSize - 1];
            deckSize -= 1;
            child.GetComponent<AI2CardToHand>().cardBack = false;
            child.tag = "Untagged";
        }
    }
    
    IEnumerator StartAttackPhase()
    {
        yield return new WaitForSeconds(1f);
        attackPhase = true;
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
            if (CardsInHand.howMany1 < 7 && deckSize > 0)
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
        foreach (Transform child in KbZone.transform)
        {
            if (child.GetComponent<AI1CardToHand>().id == 1 || child.GetComponent<AI1CardToHand>().id == 13 ||
                child.GetComponent<AI1CardToHand>().id == 19)
            {
                if (child.GetComponent<AI1CardToHand>().blood - child.GetComponent<AI1CardToHand>().hurted > 0)
                    return true;
            }
        }
        return false;
    }
    public bool isMutualBirth(AI2CardToHand player, AI1CardToHand enemy)
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
    public bool isOpposition(AI2CardToHand player, AI1CardToHand enemy)
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
        currentGame.playerTurn = TurnSystem1.isYourTurn;
        currentGame.playerHp = MinimaxHp.staticHp;
        currentGame.aiHp = KbHp.staticHp;
        currentGame.playerMana = TurnSystem1.currentMana;
        currentGame.aiMana = TurnSystem1.currentEnemyMana;
        currentGame.playerManaTurn = TurnSystem1.maxMana;
        currentGame.aiManaTurn = TurnSystem1.maxEnemyMana;
        currentGame.cardsInHand = new List<ThisCard1>();
        currentGame.cardsInZone = new List<ThisCard1>();
        currentGame.cardsInHandAI = new List<AICardToHand1>();
        currentGame.cardsInZoneAI = new List<AICardToHand1>();
        
        foreach (Transform child in Hand.transform)
        {
            if (child.transform)
                currentGame.cardsInHand.Add(child.GetComponent<AI2CardToHand>().toThisCard1());
        }
        foreach (Transform child in Zone.transform)
        {
            if (child.transform)
                currentGame.cardsInZone.Add(child.GetComponent<AI2CardToHand>().toThisCard1());
            if (child.GetComponent<AI2CardToHand>().canAttack)
            {
                int x = currentGame.cardsInZone.Count - 1;
                currentGame.cardsInZone[x].canAttack = true;
            }
        }
        
        foreach (Transform child in KbHand.transform)
        {
            if (child.transform)
                currentGame.cardsInHandAI.Add(child.GetComponent<AI1CardToHand>().toAICardToHand1());
        }
        foreach (Transform child in KbZone.transform)
        {
            if (child.transform)
                currentGame.cardsInZoneAI.Add(child.GetComponent<AI1CardToHand>().toAICardToHand1());
            if (child.GetComponent<AI1CardToHand>().canAttack)
            {
                int x = currentGame.cardsInZoneAI.Count - 1;
                currentGame.cardsInZoneAI[x].canAttack = true;
            }
        }

        for (int i = 0; i < deckSize; i++)
        {
            currentGame.AIdeck.Add(AI1a.staticEnemyDeck[i].ToAICardToHand1());
        }
        for (int i = 0; i < AIa.deckSize; i++)
        {
            currentGame.deck.Add(AIa.staticEnemyDeck[i].toThisCard1());
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
                        if (child.GetComponent<AI2CardToHand>().id == mo.id)
                        {
                            KbHp.staticHp -= child.GetComponent<AI2CardToHand>().actualDame;
                            // Debug.Log("Attack PlayerHp");
                            break;
                        }
                    }
                }
                else
                {
                    Attack(mo.id, mo.idAtk);
                    // Debug.Log(mo.id + " Attack " + mo.idAtk);
                }
            }
        }
    }

    public void Summon(int id)
    {
        foreach (Transform child in Hand.transform)
        {
            if (child.GetComponent<AI2CardToHand>().id == id)
            {
                child.transform.SetParent(Zone.transform);
                TurnSystem1.currentMana -= CardDataBase.cardList[id].mana;
                currentMana = TurnSystem1.currentMana;
                break;
            }
        }
    }

    public void Attack(int id1, int id2)
    {
        foreach (Transform child in Zone.transform)
        {
            if (child.GetComponent<AI2CardToHand>().id == id1)
            {
                foreach (Transform child1 in KbZone.transform)
                {
                    if (child1.GetComponent<AI1CardToHand>().id == id2)
                    {
                        if (id1 == 17)
                        {
                            child.GetComponent<AI2CardToHand>().hurted += 1;
                        }
                        else
                            child.GetComponent<AI2CardToHand>().hurted += child1.GetComponent<AI1CardToHand>().actualDame;

                        if (id2 == 17)
                        {
                            child1.GetComponent<AI1CardToHand>().hurted += 1;
                        }
                        else
                        {
                            child1.GetComponent<AI1CardToHand>().hurted += child.GetComponent<AI2CardToHand>().actualDame;
                        }

                        if (isMutualBirth(child.GetComponent<AI2CardToHand>(), child1.GetComponent<AI1CardToHand>()))
                        {
                            child1.GetComponent<AI1CardToHand>().hurted -= 2;
                        }

                        if (isOpposition(child.GetComponent<AI2CardToHand>(), child1.GetComponent<AI1CardToHand>()))
                        {
                            child1.GetComponent<AI1CardToHand>().hurted += 2;
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
        else if (depth >= 3)
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
                // Debug.Log("Evaluate: " + eval);
                if (eval > max_eval)
                {
                    max_eval = eval;
                    best_move = valid_move;
                }
                alpha = Math.Max(alpha, max_eval);
                if (beta <= alpha)
                    break;
            }
            // Debug.Log("Evaluate max: " + max_eval);
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
                // Debug.Log("Evaluate: " + eval);
                if (eval < min_eval)
                {
                    min_eval = eval;
                    best_move = valid_move;
                }
                beta = Math.Min(beta, min_eval);
                if (beta <= alpha)
                    break;
            }
            // Debug.Log("Evaluate min: " + min_eval);
            return new Tuple<int, List<Move>>(min_eval, best_move);
        }
    }
}

