using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AI2CardToHand : MonoBehaviour
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
    public GameObject EnemyZone;

    public int z = 0;
    public GameObject It;

    public bool isTarget;
    public GameObject Graveyard;

    public bool thisCardCanBeDestroyed;

    public GameObject AiZone;

    public bool canAttack;
    public bool summoningSickness;

    public bool isSummoned;

    public GameObject battleZone;

    public int healXpower;
    //new 
    public bool spell;
    public int damageDealtBySpell;

    public bool dealDamage;

    public bool stopDealDamage;
    public int increaseXdame;
    public int actualDame;
    public int dameIncrease;
    public bool deathcrys;
    
    public bool attackedTarget = false;
    public GameObject attackBorder;
    // Start is called before the first frame update
    void Start()
    {
        
        CardBackScript = GetComponent<CardBack>();
        thisCard = CardDataBase.cardList[thisId];
        Hand = GameObject.Find("Hand");

        z = 0;
        hurted = 0;

        Graveyard = GameObject.Find("EGraveyard");
        StartCoroutine(AfterVoidStart());
        summoningSickness = true;
        
        AiZone = GameObject.Find("Zone");
        battleZone = GameObject.Find("Zone");
        EnemyZone = GameObject.Find("EnemyZone");
    }

    // Update is called once per frame
    public void Update()
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
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        if (this.transform.parent == AiZone.transform)
        {
            cardBack = false;
            if (!isTarget)
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }

        if (isTarget)
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
            
        id = thisCard.id;
        if (thisCard.id == 4)
        {
            summoningSickness = false;
        }
        cardName = thisCard.cardName;
        dame = thisCard.dame;
        blood = thisCard.blood;
        mana = thisCard.mana;
        cardDescription = thisCard.cardDescription;
        thisSprite = thisCard.thisImage;

        drawXcards = thisCard.drawXcards;
        addXmaxMana = thisCard.addXmaxMana;

        increaseXdame = thisCard.increaseXdame;
        deathcrys = thisCard.deathcrys;
        
        spell = thisCard.spell;
        damageDealtBySpell = thisCard.damageDealtBySpell;

        returnXcards = thisCard.returnXcards;
        actualblood = blood - hurted;
        actualDame = dame + dameIncrease;

        nameText.text = "" + cardName;
        dameText.text = "" + actualDame;
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
        if (!TurnSystem1.isYourTurn && transform.parent == AiZone.transform)
        {
            summoningSickness = false;
            attackedTarget = false;
        }
        if (TurnSystem1.isYourTurn && summoningSickness == false)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }

        if (canAttack && transform.parent == battleZone.transform)
        {
            attackBorder.SetActive(true);
        }
        else 
            attackBorder.SetActive(false);

        if (actualblood <= 0 && thisCardCanBeDestroyed && spell == false && id != 4)
        {
            if (deathcrys && !isSummoned)
            {
                if (healXpower > 0)
                {
                    Heal();
                }
                if (increaseXdame > 0)
                {
                    increaseDame();
                }
                if (damageDealtBySpell > 0)
                {
                    dealXDamage();
                }
                isSummoned = true;
            }
            this.transform.SetParent(Graveyard.transform);
            this.transform.position = new Vector3(transform.position.x + 4000, transform.position.y,
                transform.position.z);
            hurted = 0;
        }

        if (transform.parent == battleZone.transform && isSummoned == false && !deathcrys)
        {
            if (drawXcards > 0)
            {
                DrawX = drawXcards;
            }
            if (healXpower > 0)
            {
                Heal();
            }

            if (increaseXdame > 0)
            {
                increaseDame();
            }
            if (damageDealtBySpell > 0)
            {
                dealXDamage();
            }
            isSummoned = true;
        }
        if (spell && isSummoned)
        {
            StartCoroutine(WaitSpell());
        }
        if (id == 4 && actualblood <= 0 && isSummoned)
            StartCoroutine(WaitSpell());
    }
    public void BeingTarget()
    {
        
        if (id == 1 || id == 13 || id == 19)
            isTarget = true;
        else
        {
            foreach (Transform child in battleZone.transform)
            {
                if (child.GetComponent<AI2CardToHand>().id == 1 || child.GetComponent<AI2CardToHand>().id == 13 || child.GetComponent<AI2CardToHand>().id == 19)
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
    
    IEnumerator WaitSpell()
    {
        yield return new WaitForSeconds(1f);
        this.transform.SetParent(Graveyard.transform);
        this.transform.position = new Vector3(transform.position.x + 4000, transform.position.y,
            transform.position.z);
        hurted = 0;
    }
    public void Heal()
    {
        if (healXpower > 0)
        {
            if (id == 2 || id == 3)
                HealOne();
            else if (id == 20 || id == 24 || id == 21 || id == 11 || id == 16 || id == 12 || id == 23)
                HealAll();
            else if (id == 10)
            {
                HealAll();
                HealHero();
            }
        }
    }

    public void HealOne()
    {
        // int x;
        // if (spell)
        //     x = Random.Range(0, CardsInZone.eHowMany);
        // else
        // {
        //     x = Random.Range(0, CardsInZone.eHowMany-1);
        // } 
        // int i = 0;
        // if ((CardsInZone.eHowMany <= 1 && !spell) || CardsInZone.eHowMany == 0)
        //     return;
        // foreach (Transform child in battleZone.transform)
        // {
        //     if (i == x)
        //     {
        //         if (child != transform && child.GetComponent<AI2CardToHand>() != null && child.GetComponent<AI2CardToHand>().actualblood > 0)
        //         {
        //             child.GetComponent<AI2CardToHand>().hurted -= healXpower;
        //             break;
        //         }
        //         continue;
        //     }
        //     i++;
        // }
        foreach (Transform child in battleZone.transform)
        {
            if (child != transform && child.GetComponent<AI2CardToHand>() != null && child.GetComponent<AI2CardToHand>().actualblood > 0)
            {
                child.GetComponent<AI2CardToHand>().hurted -= healXpower;
                break;
            }
        }
    }

    public void HealAll()
    {
        if ((CardsInZone.howMany1 <= 1 && !spell) || CardsInZone.howMany1 == 0)
            return;
        foreach (Transform child in battleZone.transform)
        {
            if (child != transform && child.GetComponent<AI2CardToHand>() != null && child.GetComponent<AI2CardToHand>().actualblood > 0)
                child.GetComponent<AI2CardToHand>().hurted -= healXpower;
        }
    }

    public void HealHero()
    {
        MinimaxHp.staticHp += healXpower;
        if (id == 10)
            MinimaxHp.staticHp -= 1;
        if (MinimaxHp.staticHp > MinimaxHp.maxHp)
            MinimaxHp.staticHp = MinimaxHp.maxHp;
    }
    public void dealXDamage()
    {
        if (id == 5 || id == 14)
            dealHero();
        else if (id == 7)
            dealOne();
        else
            dealAll();
    }

    public void dealHero()
    {
        KbHp.staticHp -= damageDealtBySpell;
        if (id == 5 && Field1.checkChargePlayer())
            KbHp.staticHp -= damageDealtBySpell;
    }

    public void dealAll()
    {
        foreach (Transform child in EnemyZone.transform)
        {   
            if (child.GetComponent<AI1CardToHand>() != null)
                child.GetComponent<AI1CardToHand>().isTarget = true;
            if (child.GetComponent<AI1CardToHand>() != null && child.GetComponent<AI1CardToHand>().isTarget)
            {
                child.GetComponent<AI1CardToHand>().hurted += damageDealtBySpell;
                if (id == 18 && Field1.checkNTN())
                    child.GetComponent<AI1CardToHand>().hurted += 1;
                child.GetComponent<AI1CardToHand>().isTarget = false;
            }
        }
    }

    public void dealOne()
    {
        // int x = Random.Range(0, CardsInZone.howMany);
        // int i = 0;
        // foreach (Transform child in EnemyZone.transform)
        // {
        //     if (i==x && child.GetComponent<ThisCard>().actualblood > 0)
        //     {
        //         child.GetComponent<ThisCard>().isTarget = true;
        //         if (child.GetComponent<ThisCard>().isTarget)
        //         {
        //             child.GetComponent<ThisCard>().hurted += damageDealtBySpell;
        //             child.GetComponent<ThisCard>().isTarget = false;
        //             break;
        //         }
        //     }
        //     i++;
        // }
        foreach (Transform child in EnemyZone.transform)
        {
            if (child.GetComponent<AI1CardToHand>().actualblood > 0)
            {
                child.GetComponent<AI1CardToHand>().isTarget = true;
                if (child.GetComponent<AI1CardToHand>().isTarget)
                {
                    child.GetComponent<AI1CardToHand>().hurted += damageDealtBySpell;
                    child.GetComponent<AI1CardToHand>().isTarget = false;
                    break;
                }
            }
        }
    }

    public void increaseDame()
    {
        if ((CardsInZone.howMany1 <= 1 && !spell) || CardsInZone.howMany1 == 0)
            return;
        foreach (Transform child in battleZone.transform)
        {
            if (child != transform && child.GetComponent<AI2CardToHand>() != null)
                child.GetComponent<AI2CardToHand>().dameIncrease += increaseXdame;
        }
    }
    public ThisCard1 toThisCard1()
    {
        ThisCard1 newcard = new ThisCard1();
        newcard.thisId = id;
        newcard.thisCard = CardDataBase.cardList[id];
        newcard.id = id;
        newcard.cardName = cardName;
        newcard.dame = dame;
        
        newcard.blood = blood;
        newcard.mana = mana;
        newcard.cardDescription = cardDescription;
        newcard.thisSprite = thisCard.thisImage;

        newcard.drawXcards = drawXcards;
        newcard.addXmaxMana = addXmaxMana;

        newcard.returnXcards = returnXcards;
        newcard.healXpower = healXpower;
        
        newcard.increaseXdame = increaseXdame;
        newcard.deathcrys = deathcrys;

        newcard.spell = spell;
        newcard.damageDealtBySpell = damageDealtBySpell;
        newcard.hurted = hurted;

        newcard.actualblood = newcard.blood - newcard.hurted;
        newcard.actualDame = newcard.dame + newcard.dameIncrease;
        newcard.summoned = isSummoned;
        newcard.canAttack = canAttack;
        return newcard;
    }
}

