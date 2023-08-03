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

    public TMPro.TextMeshProUGUI LoseText;
    public GameObject LoseTextGameObject;

    public GameObject concedeWindow;
    public string menu = "Menu";
    public AudioSource audioSource;
    public AudioClip shuffle, draw;
    private void Awake()
    {
        Shuffle();
    }

    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        deckSize = 30;

        // for (int i = 1; i <= 25; i++)
        // {
        //     if (PlayerPrefs.GetInt("deck" + i, 0) > 0)
        //     {
        //         for (int j = 1; j <= PlayerPrefs.GetInt("deck" + i, 0); j++)
        //         {
        //             deck[x] = CardDataBase.cardList[i];
        //             x++;
        //         }
        //     }
        // }
        // Random instead for checking
        for (int i = 0;i < deckSize; i++)
        {
            x = Random.Range(1, 26);
            deck[i] = CardDataBase.cardList[x];
        }
        Shuffle();
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (deckSize <= 0)
        {
            LoseTextGameObject.SetActive(true);
            LoseText.text = "BẠN THUA RỒI!";
        }
        staticDeck = deck;
        if (deckSize < 30)
        {
            cardInDeck1.SetActive (false);
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

        if (ThisCard.drawX > 0)
        {
            StartCoroutine(Draw(ThisCard.drawX));
            ThisCard.drawX = 0;
        }

        if (TurnSystem.startTurn == true)
        {
            if (CardsInHand.howMany < 7)
            {
                StartCoroutine(Draw(1));
            }
            
            TurnSystem.startTurn = false;
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
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        LoseTextGameObject.SetActive(true);
        LoseText.text = "BẠN THUA RỒI";
        concedeWindow.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(menu);
    }
}
