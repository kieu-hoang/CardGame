using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCreator : MonoBehaviour
{
    public int[] cardsWithThisId;

    public bool mouseOverDeck;

    public int dragged;

    public GameObject coll;

    public int numberOfCardsInDatabase;

    public int sum;

    public int numberOfDifferentCards;

    public int[] savedDeck;

    public GameObject prefab;

    public bool[] alreadyCreated;

    public static int lastAdded;

    public int[] quantity;
    public GameObject notice;
    public TextMeshProUGUI total;
    public TextMeshProUGUI noticeText;
    public GameObject panel;
    
    // Start is called before the first frame update
    void Start()
    {
        sum = 0;
        numberOfCardsInDatabase = 25;
    }

    // Update is called once per frame
    void Update()
    {
        total.text = sum + "/30";
    }

    public void CreateDeck()
    {
        notice.SetActive(true);
        if (sum < 30)
        {
            noticeText.text = "Chưa đủ 30 lá bài!!!";
        }
        else if (sum == 30)
        {
            for (int i = 0; i <= numberOfCardsInDatabase; i++)
            {
                PlayerPrefs.SetInt("deck"+i,cardsWithThisId[i]);
            }
            DestroyPanel();
            sum = 0;
            numberOfDifferentCards = 0;
            for (int i = 0; i <= numberOfCardsInDatabase; i++)
            {
                savedDeck[i] = PlayerPrefs.GetInt("deck" + i, 0);
                cardsWithThisId[i] = 0;
                alreadyCreated[i] = false;
            }
            noticeText.text = "Đã tạo bộ bài";
        }
    }

    public void EnterDeck()
    {
        mouseOverDeck = true;
    }

    public void ExitDeck()
    {
        mouseOverDeck = false;
    }

    public void Card1()
    {
        dragged = Collection.x;
    }
    public void Card2()
    {
        dragged = Collection.x+1;
    }
    public void Card3()
    {
        dragged = Collection.x+2;
    }
    public void Card4()
    {
        dragged = Collection.x+3;
    }

    public void Drop()
    {
        notice.SetActive(false);
        if (mouseOverDeck == true && coll.GetComponent<Collection>().HowManyCards[dragged] > 0 && sum < 30)
        {
            if ((dragged > 15 && cardsWithThisId[dragged] >= 1) || (dragged is > 10 and <= 15 && cardsWithThisId[dragged] >= 2) || cardsWithThisId[dragged] >= 3)
            {
                return;
            }
            cardsWithThisId[dragged]++;
            if (cardsWithThisId[dragged] < 0)
            {
                cardsWithThisId[dragged] = 0;
            }

            sum += 1;
            coll.GetComponent<Collection>().HowManyCards[dragged]--;
            CalculateDrop();
        }
    }

    public void CalculateDrop()
    {
        lastAdded = 0;
        int i = dragged;
        if (cardsWithThisId[i] > 0 && alreadyCreated[i] == false)
        {
            lastAdded = i;
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            alreadyCreated[i] = true;

            quantity[i] = 1;
        }
        else if (cardsWithThisId[i] > 0 && alreadyCreated[i] == true)
        {
            quantity[i]++;
        }
    }

    public void DestroyPanel()
    {
        foreach (Transform card in panel.transform)
        {
            Destroy(card.gameObject);
        }
    }
}
