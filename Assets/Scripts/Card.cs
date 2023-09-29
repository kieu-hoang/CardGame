using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Card
{
    public int id;
    public string cardName;
    public int dame;
    public int blood;
    public int mana;
    public string cardDescription;

    public int drawXcards;
    public int addXmaxMana;

    public Sprite thisImage;

    // public string color;
    public enum Element
    {
        NoElement,
        Metal,
        Wood,
        Water,
        Fire,
        Earth
    }
    
    public Element element;

    public int returnXcards;
    
    public int healXpower;
    
    public bool spell;
    public int damageDealtBySpell;
    public int increaseXdame;
    public bool deathcrys;

    public Card()
    {
        
    }

//  public Card(int Id, string CardName, int Dame, int Blood, int Mana, string CardDescription, Sprite ThisImage, string Color, int DrawXcards, int AddXmaxMana, int ReturnXcards, int HealXpower, bool Spell, int DamageBySpell, int IncreaseXdame)
    public Card(int Id, string CardName, int Dame, int Blood, int Mana, string CardDescription, Sprite ThisImage, Element element, int DrawXcards, int AddXmaxMana, int ReturnXcards, int HealXpower, bool Spell, int DamageBySpell, int IncreaseXdame, bool Deathcrys)
    {
        id = Id;
        cardName = CardName;
        dame = Dame;
        blood = Blood;
        mana = Mana;
        cardDescription = CardDescription;

        thisImage = ThisImage;
        // color = Color;
        this.element = element;

        drawXcards = DrawXcards;
        addXmaxMana = AddXmaxMana;
        returnXcards = ReturnXcards;

        healXpower = HealXpower;
        spell = Spell;
        damageDealtBySpell = DamageBySpell;
        increaseXdame = IncreaseXdame;
        deathcrys = Deathcrys;
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
        newcard.thisSprite = thisImage;

        newcard.drawXcards = drawXcards;
        newcard.addXmaxMana = addXmaxMana;

        newcard.returnXcards = returnXcards;
        newcard.healXpower = healXpower;
        
        newcard.increaseXdame = increaseXdame;
        newcard.deathcrys = deathcrys;

        newcard.spell = spell;
        newcard.damageDealtBySpell = damageDealtBySpell;

        newcard.actualblood = newcard.blood - newcard.hurted;
        newcard.actualDame = newcard.dame + newcard.dameIncrease;
        return newcard;
    }
    public AICardToHand1 ToAICardToHand1()
    {
        AICardToHand1 newcard = new AICardToHand1();
        newcard.thisId = id;
        newcard.thisCard = CardDataBase.cardList[id];
        newcard.id = id;
        newcard.cardName = cardName;
        newcard.dame = dame;
        newcard.blood = blood;
        newcard.mana = mana;
        newcard.cardDescription = cardDescription;
        newcard.thisSprite = thisImage;

        newcard.drawXcards = drawXcards;
        newcard.addXmaxMana = addXmaxMana;

        newcard.increaseXdame = increaseXdame;
        newcard.deathcrys = deathcrys;
        
        newcard.spell = spell;
        newcard.damageDealtBySpell = damageDealtBySpell;

        newcard.returnXcards = returnXcards;
        newcard.actualblood = newcard.blood - newcard.hurted;
        newcard.actualDame = newcard.dame + newcard.dameIncrease;
        newcard.healXpower = healXpower;
        return newcard;
    }
}
