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
    
    public GameObject EnemyZone;
    public AICardToHand aiCardToHand;
    //new
    public bool spell;
    public int damageDealtBySpell;

    public bool dealDamage;

    public bool stopDealDamage;


    void Start()
    {
        CardBackScript = GetComponent<CardBack>();
        thisCard = CardDataBase.cardList[thisId];

        numberOfCardsInDeck = PlayerDeck.deckSize;
        canBeSummon = false;
        summoned = false;

        drawX = 0;
        hurted = 0;
        
        canAttack = false;
        summoningSickness = true;

        Enemy = GameObject.Find("EnemyHp");

        targeting = false;
        targetingEnemy = false;

        canHeal = true;

        EnemyZone = GameObject.Find("EnemyZone");
        Graveyard = GameObject.Find("Graveyard");
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

        spell = thisCard.spell;
        damageDealtBySpell = thisCard.damageDealtBySpell;

        actualblood = blood - hurted;

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
            frame.GetComponent<Image>().color = new Color32(255, 255, 0, 255);
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

        dameText.text = "" + dame;
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

        if (TurnSystem.currentMana >= mana && summoned == false && beInGraveyard == false && TurnSystem.isYourTurn == true && TurnSystem.protectStart == false)
        {
            canBeSummon = true;
        }
        else canBeSummon = false;

        if (canBeSummon == true)
        {
            gameObject.GetComponent<Draggable>().enabled = true;
        }
        else gameObject.GetComponent<Draggable>().enabled = false;

        battleZone = GameObject.Find("Zone");

        if (summoned == false && this.transform.parent == battleZone.transform)
        {
            Summon();
        }
        
        if (canAttack == true && beInGraveyard == false)
        {
            attackBorder.SetActive(true);
        }
        else
        {
            attackBorder.SetActive (false);
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
        targeting = staticTargeting;
        targetingEnemy = staticTargetingEnemy;

        if (targetingEnemy == true)
        {
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
        if (damageDealtBySpell > 0)
        {
            dealDamage = true;
        }
        if (dealDamage == true && this.transform.parent == battleZone.transform)
        {
            dealXDamage(damageDealtBySpell);
        }
        if (stopDealDamage == true)
        {
            attackBorder.SetActive(false);
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
                    EnemyHp.staticHp -= dame;
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
                        child.GetComponent<AICardToHand>().hurted += dame;
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
            // this.transform.SetParent(Graveyard.transform);
            // canBeDestroyed = false;
            // summoned = false;
            // beInGraveyard = true;
            // hurted = 0;
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
    // public void ReturnCard()
    // {
    //     UcanReturn = true;
    // }
    // public void ReturnThis()
    // {
    //     if (beInGraveyard == true && UcanReturn == true)
    //     {
    //         this.transform.SetParent(Hand.transform);
    //         UcanReturn = false;
    //         beInGraveyard = false;
    //         summoningSickness = true;
    //     }
    // }
    public void Heal()
    {
        PlayerHp.staticHp += healXpower;
        if (PlayerHp.staticHp > PlayerHp.maxHp)
            PlayerHp.staticHp = PlayerHp.maxHp;
    }
    public void dealXDamage(int x)
    {
        if (Target != null)
        {
            if (Target == Enemy && stopDealDamage == false && Input.GetMouseButton(0)){
                EnemyHp.staticHp -= damageDealtBySpell;
                stopDealDamage = true;
            }
        }
        else
        {
            foreach (Transform child in EnemyZone.transform)
            {
                //if (child.GetComponent<AICardToHand>().isTarget == true && Input.GetMouseButton(0))
                //{
                //child.GetComponent<AICardToHand>().hurted += damageDealtBySpell;
                //stopDealDamage = true;
                //}
                child.GetComponent<AICardToHand>().isTarget = true;
                if (child.GetComponent<AICardToHand>().isTarget == true)
                {
                    child.GetComponent<AICardToHand>().hurted += damageDealtBySpell;
                    child.GetComponent<AICardToHand>().isTarget = false;
                }
            }
            stopDealDamage = true;
        }
    }
}
