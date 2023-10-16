using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public Card container;
    public static List<Card> staticDeck = new List<Card>();
    public int[] startingDeck;

    public int x;
    public static int deckSize;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;

    public GameObject CardBack;
    public GameObject CardToHand;
    public GameObject Deck;

    public GameObject[] Clones;
    public GameObject Hand;

    public GameObject concedeWindow;
    public string menu = "Menu";
    public AudioSource audioSource;
    public AudioClip shuffle, draw, theme;
    private void Awake()
    {
        Shuffle();
    }

    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        deckSize = 30;
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
        // Random instead for checking
        // for (int i = 0;i < deckSize; i++)
        // {
        //     x = Random.Range(1, 26);
        //     deck[i] = CardDataBase.cardList[x];
        // }
        Shuffle();
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        staticDeck = deck;
        if (deckSize < 20)
        {
            cardInDeck1.SetActive (false);
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

        if (ThisCard.drawX > 0)
        {
            if (CardsInHand.howMany == 6 && deckSize > 0)
                StartCoroutine(Draw(1));
            else
            {
                for (int i = 0; i < ThisCard.drawX; i++)
                {
                    if (CardsInHand.howMany < 7 && deckSize > 0)
                        StartCoroutine(Draw(1));
                }
            }
            ThisCard.drawX = 0;
        }

        if (TurnSystem.startTurn == true)
        {
            if (CardsInHand.howMany < 7 && deckSize > 0)
            {
                StartCoroutine(Draw(1));
            }
            else if (deckSize <= 0)
            {
                PlayerHp.staticHp -= 1;
            }
            checkClone();
            if (AI.currentGame != null)
            {
                AI.getGameState();
                Log.SaveData(AI.currentGame.toString());
            }
            
            TurnSystem.startTurn = false;
        }
        checkClone();
    }

    public void checkClone()
    {
        foreach (Transform child in Hand.transform)
        {
            if (child.tag != "Hand Clone") continue;
            child.GetComponent<ThisCard>().thisCard = staticDeck[deckSize - 1];
            deckSize -= 1;
            child.GetComponent<ThisCard>().cardBack = false;
            child.tag = "Untagged";
        }
    }

    IEnumerator Example()
    {
    GameObject prefb = Instantiate(CardBack, transform.position, transform.rotation);
        yield return new WaitForSeconds(1.5f);
        Destroy(prefb);
    }

    IEnumerator StartGame()
    {
        for (int i = 0; i <= 2; i++)
        {
            yield return new WaitForSeconds(1);
            audioSource.PlayOneShot(draw,1f);
            Instantiate(CardToHand, transform.position, transform.rotation); ;
        }
        audioSource.PlayOneShot(theme,1f);
    }

    public void Shuffle()
    {
        for (int i = 0; i < deckSize; i++)
        {
            container = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container;
        }
        audioSource.PlayOneShot(shuffle,1f);
        // Instantiate(CardBack, transform.position, transform.rotation);
        StartCoroutine(Example());
    }

    IEnumerator Draw(int x)
    {
        for (int i=0; i<x; i++)
        {
            yield return new WaitForSeconds(1);
            audioSource.PlayOneShot(draw,1f);
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }
    
    public void OpenWindow()
    {
        concedeWindow.SetActive(true);
    }
    
    public void CloseWindow()
    {
        concedeWindow.SetActive(false);
    }

    public void ConcedeDefeat()
    {
        StartCoroutine(LoseGame());
    }

    IEnumerator LoseGame()
    {
        PlayerHp.staticHp -= 30;
        concedeWindow.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(menu);
    }
}
