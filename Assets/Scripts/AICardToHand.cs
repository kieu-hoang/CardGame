using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AICardToHand : MonoBehaviour
{
    public Card thisCard;

    public int thisId;
    public int id;
    public string cardName;
    public int dame;
    public int blood;
    public int mana;
    public string cardDescription;

    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI dameText;
    public TMPro.TextMeshProUGUI bloodText;
    public TMPro.TextMeshProUGUI descriptionText;

    public GameObject[] stars;
    public Sprite thisSprite;
    public Image thatImage;

    public Image frame;

    public bool cardBack;
    CardBack CardBackScript;

    public static int DrawX;
    public int drawXcards;
    public int addXmaxMana;

    public int hurted;
    public int actualblood;
    public int returnXcards;

    public GameObject Hand;

    public int z = 0;
    public GameObject It;

    public int numberOfCardsInDeck;

    public bool isTarget;
    public GameObject Graveyard;

    public bool thisCardCanBeDestroyed;

    public GameObject AiZone;

    public bool canAttack;
    public bool summoningSickness;

    public bool isSummoned;

    public GameObject battleZone;

    public int healXpower;
    // Start is called before the first frame update
    void Start()
    {
        
        CardBackScript = GetComponent<CardBack>();
        thisCard = CardDataBase.cardList[thisId];
        Hand = GameObject.Find("EnemyHand");

        z = 0;
        hurted = 0;
        numberOfCardsInDeck = AI.deckSize;

        Graveyard = GameObject.Find("EGraveyard");
        StartCoroutine(AfterVoidStart());

        AiZone = GameObject.Find("EnemyZone");

        summoningSickness = true;
        battleZone = GameObject.Find("EnemyZone");
    }

    // Update is called once per frame
    void Update()
    {
        if (z == 0)
        {
            It.transform.SetParent(Hand.transform);
            It.transform.localScale = Vector3.one;
            It.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
            It.transform.eulerAngles = new Vector3(25, 0, 0);
            z = 1;
        }
        if (this.transform.parent == Hand.transform)
        {
            cardBack = true;
        }
        if (this.transform.parent == AiZone.transform)
        {
            cardBack = false;
        }
        id = thisCard.id;
        cardName = thisCard.cardName;
        dame = thisCard.dame;
        blood = thisCard.blood;
        mana = thisCard.mana;
        cardDescription = thisCard.cardDescription;
        thisSprite = thisCard.thisImage;

        drawXcards = thisCard.drawXcards;
        addXmaxMana = thisCard.addXmaxMana;

        returnXcards = thisCard.returnXcards;

        actualblood = blood - hurted;

        nameText.text = "" + cardName;
        dameText.text = "" + dame;
        bloodText.text = "" + actualblood;
        descriptionText.text = "• " + cardDescription;
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < mana)
            {
                stars[i].SetActive(true);

                RectTransform starTransform = stars[i].GetComponent<RectTransform>();
                starTransform.localPosition = new Vector3(15 * (i - (mana - 1) / 2.0f), starTransform.localPosition.y, starTransform.localPosition.z);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }

        thatImage.sprite = thisSprite;
        healXpower = thisCard.healXpower;

        if (thisCard.element == Card.Element.Fire)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardFire");
        }
        if (thisCard.element == Card.Element.Water)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWater");
        }
        if (thisCard.element == Card.Element.Metal)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardMetal");
        }
        if (thisCard.element == Card.Element.Wood)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWood");
        }
        if (thisCard.element == Card.Element.Earth)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardEarth");
        }
        if (thisCard.element == Card.Element.NoElement)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardNoElement");
        }

        CardBackScript.UpdateCard(cardBack);
        if (this.tag == "Clone")
        {
            
            thisCard = AI.staticEnemyDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
            AI.deckSize -= 1;
            cardBack = false;
            this.tag = "Untagged";
        }
        if (actualblood<= 0 && thisCardCanBeDestroyed == true)
        {
            this.transform.SetParent(Graveyard.transform);
            this.transform.position = new Vector3(transform.position.x + 4000, transform.position.y,
                transform.position.z);
            hurted = 0;
        }

        if (TurnSystem.isYourTurn == false && summoningSickness == false)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
        if (TurnSystem.isYourTurn == true && this.transform.parent == AiZone.transform)
        {
            summoningSickness = false;
        }

        if (this.transform.parent == battleZone.transform && isSummoned == false)
        {
            if (drawXcards > 0)
            {
                DrawX = drawXcards;
                isSummoned = true;
            }

            if (id == 6)
            {
                TurnSystem.maxEnemyMana += 1;
                isSummoned = true;
            }

            if (healXpower > 0)
            {
                EnemyHp.staticHp += healXpower;
                if (EnemyHp.staticHp > EnemyHp.maxHp)
                    EnemyHp.staticHp = EnemyHp.maxHp;
                isSummoned = true;
            }

            isSummoned = true;
        }
    }
    public void BeingTarget()
    {
        
        if (id == 1 || id == 13 || id == 19)
            isTarget = true;
        else
        {
            foreach (Transform child in battleZone.transform)
            {
                if (child.GetComponent<AICardToHand>().id == 1 || child.GetComponent<AICardToHand>().id == 13 || child.GetComponent<AICardToHand>().id == 19)
                {
                    isTarget = false;
                    return;
                }
            }
            isTarget = true;
        }
    }
    public void DontBeingTarget()
    {
        isTarget = false;
    }
    IEnumerator AfterVoidStart()
    {
        yield return new WaitForSeconds(1);
        thisCardCanBeDestroyed = true;
    }
}
