using UnityEngine;
using UnityEngine.UI;

public class ThisCard : MonoBehaviour
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
    public TMPro.TextMeshProUGUI manaText;
    public TMPro.TextMeshProUGUI descriptionText;

    public Sprite thisSprite;
    public Image thatImage;

    public Image frame;

    public bool cardBack;
    CardBack CardBackScript;

    public GameObject Hand;
    public int numberOfCardsInDeck;

    public bool canBeSummon;
    public bool summoned;
    public GameObject battleZone;

    public static int drawX;
    public int drawXcards;
    public int addXmaxMana;
    
    public GameObject attackBorder;

    public GameObject Target;
    public GameObject Enemy;

    public bool summoningSickness;
    public bool canAttack;
    public bool cantAttack;

    public static bool staticTargeting;
    public static bool staticTargetingEnemy;

    public bool targeting;
    public bool targetingEnemy;

    public bool onlyThisCardAttack;
    
    public GameObject summonBorder;
    
    public bool canBeDestroyed;
    public GameObject Graveyard;
    public bool beInGraveyard;
    
    public int hurted;
    public int actualblood;
    public int returnXcards;
    public bool useReturn;

    public static bool UcanReturn;
   
    public int healXpower;
    public bool canHeal;
    public bool canIncreaseDame;
    
    public GameObject EnemyZone;
    public AICardToHand aiCardToHand;
    //new
    public bool spell;
    public int damageDealtBySpell;

    public bool dealDamage;

    public bool stopDealDamage;
    public int increaseXdame;
    public int actualDame;
    public int dameIncrease;

    void Start()
    {
        CardBackScript = GetComponent<CardBack>();
        thisCard = CardDataBase.cardList[thisId];

        numberOfCardsInDeck = PlayerDeck.deckSize;
        canBeSummon = false;
        summoned = false;

        drawX = 0;
        hurted = 0;
        dameIncrease = 0;
        
        canAttack = false;
        summoningSickness = true;

        Enemy = GameObject.Find("EnemyHp");

        targeting = false;
        targetingEnemy = false;

        canHeal = true;
        canIncreaseDame = true;

        EnemyZone = GameObject.Find("EnemyZone");
        Graveyard = GameObject.Find("Graveyard");
        battleZone = GameObject.Find("Zone");
    }

    void Update()
    {
        Hand = GameObject.Find("Hand");
        if (this.transform.parent == Hand.transform.parent)
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
        healXpower = thisCard.healXpower;
        
        increaseXdame = thisCard.increaseXdame;

        spell = thisCard.spell;
        damageDealtBySpell = thisCard.damageDealtBySpell;

        actualblood = blood - hurted;
        actualDame = dame + dameIncrease;

        nameText.text = "" + cardName;
        descriptionText.text = " " + cardDescription;

        thatImage.sprite = thisSprite;

        if (thisCard.color == "Red")
        {
            frame.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }
        if (thisCard.color == "Blue")
        {
            frame.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
        }
        if (thisCard.color == "Yellow")
        {
            frame.GetComponent<Image>().color = new Color32(164, 152, 28, 255);
        }
        if (thisCard.color == "Green")
        {
            frame.GetComponent<Image>().color = new Color32(60, 159, 81, 255);
        }
        if (thisCard.color == "Brown")
        {
            frame.GetComponent<Image>().color = new Color32(109, 45, 45, 255);
        }
        if (thisCard.color == "Black")
        {
            frame.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }

        dameText.text = "" + actualDame;
        bloodText.text = "" + actualblood;
        manaText.text = "" + mana;
        
        CardBackScript.UpdateCard(cardBack);

        if (this.tag == "Hand Clone")
        {
            thisCard = PlayerDeck.staticDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
            PlayerDeck.deckSize -= 1;
            cardBack = false;
            this.tag = "Untagged";
        }

        if (TurnSystem.currentMana >= mana && summoned == false && beInGraveyard == false && TurnSystem.isYourTurn == true && TurnSystem.protectStart == false
            )
        {
            canBeSummon = true;
        }
        else canBeSummon = false;

        if (canBeSummon == true)
        {
            gameObject.GetComponent<Draggable>().enabled = true;
        }
        else gameObject.GetComponent<Draggable>().enabled = false;

        if (summoned == false && this.transform.parent == battleZone.transform)
        {
            Summon();
        } 
        if (TurnSystem.isYourTurn == false && summoned == true)
        {
            summoningSickness = false;
            cantAttack = false;
        }
        if (TurnSystem.isYourTurn == true && summoningSickness == false && cantAttack == false)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
        if (canAttack == true && beInGraveyard == false)
        {
            attackBorder.SetActive(true);
        }
        else
        {
            attackBorder.SetActive (false);
        }
        targeting = staticTargeting;
        targetingEnemy = staticTargetingEnemy;

        if (targetingEnemy == true)
        {
            bool flag = true;
            foreach (Transform child in EnemyZone.transform)
            {
                if (child.GetComponent<AICardToHand>().id == 1 || child.GetComponent<AICardToHand>().id == 13 ||
                    child.GetComponent<AICardToHand>().id == 19)
                {
                    Target = null;
                    flag = false;
                    break;
                }
            }
            if (flag == true)
                Target = Enemy;
        }
        else
        {
            Target = null;
        }
        if (targeting == true && onlyThisCardAttack == true)
        {
            Attack();
        }
         
        if (canBeSummon == true || (UcanReturn == true && beInGraveyard == true))
        {
            summonBorder.SetActive(true);
        }
        else
        {
            summonBorder.SetActive(false);
        }
        if (actualblood <= 0 && spell == false)
        {
            Destroy();
        }
        if (returnXcards > 0 && summoned == true && useReturn == false && TurnSystem.isYourTurn == true)
        {
            Return(returnXcards);
            useReturn = true;
        }
        if (TurnSystem.isYourTurn == false)
        {
            UcanReturn = false;
        }
        if (canHeal == true && summoned == true)
        {
            Heal();
            canHeal = false;
        }

        if (canIncreaseDame == true && summoned == true)
        {
            increaseDame();
            canIncreaseDame = false;
        }
        if (damageDealtBySpell > 0)
        {
            dealDamage = true;
        }
        if (dealDamage == true && this.transform.parent == battleZone.transform && stopDealDamage == false)
        {
            dealXDamage(damageDealtBySpell);
        }
        if (stopDealDamage == true)
        {
            dealDamage = false;
        }
        if (this.transform.parent == battleZone.transform && spell == true && dealDamage == false)
        {
            Destroy();
        }
    }
    public void Summon()
    {
        TurnSystem.currentMana -= mana;
        summoned = true;

        MaxMana(addXmaxMana);
        drawX = drawXcards;

    }

    public void MaxMana(int x)
    {
        TurnSystem.maxMana += x;
        if (TurnSystem.maxMana > 7)
        {
            TurnSystem.maxMana = 7;
        }
    }
    public void Attack()
    {
        if (canAttack == true && summoned == true)
        {
            if (Target != null)
            {
                if (Target == Enemy)
                {
                    EnemyHp.staticHp -= (dame + dameIncrease);
                    targeting = false;
                    cantAttack = true;
                    Arrow._Hide = true;
                }
            }
            else
            {
                foreach(Transform child in EnemyZone.transform)
                {
                    if (child.GetComponent<AICardToHand>().isTarget == true)
                    {
                        child.GetComponent<AICardToHand>().hurted += (dame + dameIncrease);
                        hurted += child.GetComponent<AICardToHand>().dame;
                        cantAttack = true;
                        Arrow._Hide = true;
                        child.GetComponent<AICardToHand>().isTarget = false;
                    }
                }
            }
        }
    }
    public void UntargetEnemy()
    {
        staticTargetingEnemy = false;
    }
    public void TargetEnemy()
    {
        staticTargetingEnemy = true;
    }
    public void StartAttack()
    {
        staticTargeting = true;
        if (canAttack == true)
        {
            Arrow._Show = true;
            Arrow.startPoint = transform.position;
        }
    }
    public void StopAttack()
    {
        staticTargeting = false;
    }
    public void OneCardAttack()
    {
        onlyThisCardAttack = true;
    }
    public void OneCardAttackStop()
    {
        onlyThisCardAttack = false;
    }
    public void Destroy()
    {
        Graveyard = GameObject.Find("Graveyard");
        canBeDestroyed = true;
        if (canBeDestroyed == true)
        {
            for (int i = 0; i < 40; i++)
            {
                if (Graveyard.GetComponent<GraveyardScript>().graveyard[i].id == 0)
                {
                    Graveyard.GetComponent<GraveyardScript>().graveyard[i] = CardDataBase.cardList[id];
                    Graveyard.GetComponent<GraveyardScript>().objectsInGraveyard[i] = this.gameObject;
                    canBeDestroyed = false;
                    summoned = false;
                    beInGraveyard = true;
                    hurted = 0;
                    
                    transform.SetParent(Graveyard.transform);
                    transform.position = new Vector3(transform.position.x + 4000, transform.position.y,
                        transform.position.z);
                    break;
                }
            }
        }
    }
    public void Return(int x)
    {
        Graveyard.GetComponent<GraveyardScript>().returnCard = x;
    }
    
    public void Heal()
    {
        if (healXpower > 0)
        {
            if (transform.GetComponent<ThisCard>().id == 2 || transform.GetComponent<ThisCard>().id == 3)
                HealOne();
            else if (transform.GetComponent<ThisCard>().id == 20 || transform.GetComponent<ThisCard>().id == 24)
                HealAll();
            else if (transform.GetComponent<ThisCard>().id == 10)
            {
                HealAll();
                HealHero();
            }
        }
    }

    public void HealOne()
    {
        if (CardsInZone.howMany <= 2)
            return;
        foreach (Transform child in battleZone.transform)
        {
            if (child != transform && child.GetComponent<ThisCard>() != null)
            {
                child.GetComponent<ThisCard>().hurted -= healXpower;
                break;
            }
        }
    }

    public void HealAll()
    {
        if (CardsInZone.howMany <= 2)
            return;
        foreach (Transform child in battleZone.transform)
        {
            if (child != transform && child.GetComponent<ThisCard>() != null)
                child.GetComponent<ThisCard>().hurted -= healXpower;
        }
    }

    public void HealHero()
    {
        PlayerHp.staticHp += healXpower;
        if (PlayerHp.staticHp > PlayerHp.maxHp)
            PlayerHp.staticHp = PlayerHp.maxHp;
    }
    public void dealXDamage(int x)
    {
        if (transform.GetComponent<ThisCard>().id == 5 || transform.GetComponent<ThisCard>().id == 14)
            dealHero();
        else if (transform.GetComponent<ThisCard>().id == 7)
            dealOne();
        else
            dealAll();
    }

    public void dealHero()
    {
        EnemyHp.staticHp -= damageDealtBySpell;
        stopDealDamage = true;
    }

    public void dealAll()
    {
        foreach (Transform child in EnemyZone.transform)
        {
            child.GetComponent<AICardToHand>().isTarget = true;
            if (child.GetComponent<AICardToHand>().isTarget == true)
            {
                child.GetComponent<AICardToHand>().hurted += damageDealtBySpell;
                child.GetComponent<AICardToHand>().isTarget = false;
            }
        }
        stopDealDamage = true;
    }

    public void dealOne()
    {
        int x = Random.Range(0, EnemyZone.GetComponent<CardsInZone>().howManyCards);
        int i = 0;
        foreach (Transform child in EnemyZone.transform)
        {
            if (i==x)
            {child.GetComponent<AICardToHand>().isTarget = true;
                if (child.GetComponent<AICardToHand>().isTarget == true)
                {
                    child.GetComponent<AICardToHand>().hurted += damageDealtBySpell;
                    child.GetComponent<AICardToHand>().isTarget = false;
                    break;
                }
                
            }
            i++;
        }
        stopDealDamage = true;
    }

    public void increaseDame()
    {
        if (CardsInZone.howMany <= 2)
            return;
        foreach (Transform child in battleZone.transform)
        {
            if (child != transform && child.GetComponent<ThisCard>() != null)
                child.GetComponent<ThisCard>().dameIncrease += increaseXdame;
        }
    }
}
