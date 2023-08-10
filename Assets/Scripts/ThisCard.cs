using System.Collections;
using System.Security.Principal;
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
    public TMPro.TextMeshProUGUI descriptionText;

    public GameObject[] stars;

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
    
    public bool isTarget;
    public bool attackedTarget = false;
    public bool deathcrys;

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

        Enemy = GameObject.Find("EnemyPortrait");

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
        if (this.transform.parent == Hand.transform)
        {
            cardBack = false;
            if (!isTarget)
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        if (isTarget)
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        id = thisCard.id;
        // if (thisCard.id == 4)
        // {
        //     summoningSickness = false;
        //     cantAttack = false;
        // }
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
        deathcrys = thisCard.deathcrys;

        spell = thisCard.spell;
        damageDealtBySpell = thisCard.damageDealtBySpell;

        actualblood = blood - hurted;
        actualDame = dame + dameIncrease;

        nameText.text = "" + cardName;
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

        dameText.text = "" + actualDame;
        bloodText.text = "" + actualblood;
        
        CardBackScript.UpdateCard(cardBack);

        if (this.tag == "Hand Clone")
        {
            thisCard = PlayerDeck.staticDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
            PlayerDeck.deckSize -= 1;
            cardBack = false;
            this.tag = "Untagged";
        }

        if (TurnSystem.currentMana >= mana && summoned == false && beInGraveyard == false && TurnSystem.isYourTurn == true 
            && TurnSystem.protectStart == false && CardsInZone.howMany < 5)
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
            attackedTarget = false;
        }
        if (TurnSystem.isYourTurn && summoningSickness == false && cantAttack == false && summoned)
        {
            canAttack = true;
        }
        else if (TurnSystem.isYourTurn && id == 4 && summoned && attackedTarget == false)
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
            attackBorder.SetActive(false);
        }
        targeting = staticTargeting;
        targetingEnemy = staticTargetingEnemy;

        if (targetingEnemy)
        {
            bool flag = !(CardsInZone.eHowMany > 0);
            
            // foreach (Transform child in EnemyZone.transform)
            // {
            //     if (child.GetComponent<AICardToHand>().id == 1 || child.GetComponent<AICardToHand>().id == 13 ||
            //         child.GetComponent<AICardToHand>().id == 19)
            //     {
            //         Target = null;
            //         flag = false;
            //         break;
            //     }
            // }

            Target = flag ? Enemy : null;
        }
        else
        {
            Target = null;
        }
        if (targeting && onlyThisCardAttack)
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
        if (canHeal == true && summoned == true && !deathcrys)
        {
            Heal();
            canHeal = false;
        }

        if (canIncreaseDame == true && summoned == true && !deathcrys)
        {
            increaseDame();
            canIncreaseDame = false;
        }
        if (damageDealtBySpell > 0)
        {
            dealDamage = true;
        }
        if (dealDamage == true && transform.parent == battleZone.transform && stopDealDamage == false && !deathcrys)
        {
            dealXDamage();
        }
        if (stopDealDamage == true)
        {
            dealDamage = false;
        }
        if (this.transform.parent == battleZone.transform && spell && dealDamage == false)
        {
            StartCoroutine(DestroySpell());
        }
    }

    IEnumerator DestroySpell()
    {
        yield return new WaitForSeconds(1f);
        this.transform.SetParent(Graveyard.transform);
        this.transform.position = new Vector3(transform.position.x + 4000, transform.position.y,
            transform.position.z);
        hurted = 0;
        // canBeDestroyed = true;
        // if (canBeDestroyed == true)
        // {
        //     for (int i = 0; i < 32; i++)
        //     {
        //         if (Graveyard.GetComponent<GraveyardScript>().graveyard[i].id == 0)
        //         {
        //             Graveyard.GetComponent<GraveyardScript>().graveyard[i] = CardDataBase.cardList[id];
        //             Graveyard.GetComponent<GraveyardScript>().objectsInGraveyard[i] = this.gameObject;
        //             canBeDestroyed = false;
        //             summoned = false;
        //             beInGraveyard = true;
        //             hurted = 0;
        //             
        //             transform.SetParent(Graveyard.transform);
        //             transform.position = new Vector3(transform.position.x + 4000, transform.position.y,
        //                 transform.position.z);
        //             break;
        //         }
        //     }
        // }
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
        if (canAttack && summoned)
        {
            if (Target != null)
            {
                if (Target == Enemy && attackedTarget == false)
                {
                    EnemyHp.staticHp -= actualDame;
                    targeting = false;
                    attackedTarget = true;
                    cantAttack = true;
                    Arrow._Hide = true;
                }
            }
            else
            {
                foreach(Transform child in EnemyZone.transform)
                {
                    if (child.GetComponent<AICardToHand>().isTarget)
                    {
                        if (child.GetComponent<AICardToHand>().id == 17)
                            child.GetComponent<AICardToHand>().hurted += 1;
                        else
                            child.GetComponent<AICardToHand>().hurted += actualDame;
                        if (id == 17)
                            hurted += 1;
                        else
                            hurted += child.GetComponent<AICardToHand>().actualDame;
                        if (isMutualBirth(transform.GetComponent<ThisCard>(), child.GetComponent<AICardToHand>()))
                        {
                            child.GetComponent<AICardToHand>().hurted -= 2;
                        }
                        if (isOpposition(transform.GetComponent<ThisCard>(), child.GetComponent<AICardToHand>()))
                        {
                            child.GetComponent<AICardToHand>().hurted += 2;
                        }
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
        Arrow._Hide = true;
    }
    public void OneCardAttack()
    {
        onlyThisCardAttack = true;
    }
    public void OneCardAttackStop()
    {
        onlyThisCardAttack = false;
        Arrow._Hide = true;
    }
    public void Destroy()
    {
        if (deathcrys)
        {
            if (canHeal)
            {
                Heal();
                canHeal = false;
            }

            if (dealDamage)
            {
                dealXDamage();
                dealDamage = false;
            }
        }
        transform.SetParent(Graveyard.transform);
        transform.position = new Vector3(transform.position.x + 4000, transform.position.y,
            transform.position.z);
        hurted = 0;
        // canBeDestroyed = true;
        // if (canBeDestroyed)
        // {
        //     for (int i = 0; i < 32; i++)
        //     {
        //         if (Graveyard.GetComponent<GraveyardScript>().graveyard[i].id == 0)
        //         {
        //             Graveyard.GetComponent<GraveyardScript>().graveyard[i] = CardDataBase.cardList[id];
        //             Graveyard.GetComponent<GraveyardScript>().objectsInGraveyard[i] = this.gameObject;
        //             canBeDestroyed = false;
        //             summoned = false;
        //             beInGraveyard = true;
        //             hurted = 0;
        //             
        //             transform.SetParent(Graveyard.transform);
        //             transform.position = new Vector3(transform.position.x + 4000, transform.position.y,
        //                 transform.position.z);
        //             
        //             break;
        //         }
        //     }
        // }
    }
    public void Return(int x)
    {
        Graveyard.GetComponent<GraveyardScript>().returnCard = x;
    }
    
    public void Heal()
    {
        if (healXpower > 0)
        {
            if (id == 2 || id == 3)
                HealOne();
            else if (id == 20 || id == 24 || id == 21 || id == 11 || id == 16 || id == 12)
                HealAll();
            else if (id == 10)
            {
                HealOne();
                HealHero();
            }
        }
    }

    public void HealOne()
    {
        int x = Random.Range(0, CardsInZone.howMany-1);
        int i = 0;
        if (CardsInZone.howMany <= 1)
            return;
        foreach (Transform child in battleZone.transform)
        {
            if (i == x)
            {
                if (child != transform && child.GetComponent<ThisCard>() != null)
                {
                    child.GetComponent<ThisCard>().hurted -= healXpower;
                    break;
                }
                continue;
            }
            i++;
        }
    }

    public void HealAll()
    {
        if (CardsInZone.howMany <= 1)
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
        if (id == 10)
            PlayerHp.staticHp -= 1;
        if (PlayerHp.staticHp > PlayerHp.maxHp)
            PlayerHp.staticHp = PlayerHp.maxHp;
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
        EnemyHp.staticHp -= damageDealtBySpell;
        if (id == 5 && Field.checkChargePlayer())
            EnemyHp.staticHp -= damageDealtBySpell;
        stopDealDamage = true;
    }

    public void dealAll()
    {
        foreach (Transform child in EnemyZone.transform)
        {   
            if (child.GetComponent<AICardToHand>() != null)
                child.GetComponent<AICardToHand>().isTarget = true;
            if (child.GetComponent<AICardToHand>() != null && child.GetComponent<AICardToHand>().isTarget)
            {
                child.GetComponent<AICardToHand>().hurted += damageDealtBySpell;
                if (id == 18 && Field.checkNTN())
                    child.GetComponent<AICardToHand>().hurted += 1;
                child.GetComponent<AICardToHand>().isTarget = false;
            }
        }
        stopDealDamage = true;
    }

    public void dealOne()
    {
        int x = Random.Range(0, CardsInZone.eHowMany);
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
        if (CardsInZone.howMany <= 1)
            return;
        foreach (Transform child in battleZone.transform)
        {
            if (child != transform && child.GetComponent<ThisCard>() != null)
                child.GetComponent<ThisCard>().dameIncrease += increaseXdame;
        }
    }

    public bool isMutualBirth(ThisCard player, AICardToHand enemy)
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
    public bool isOpposition(ThisCard player, AICardToHand enemy)
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
}
