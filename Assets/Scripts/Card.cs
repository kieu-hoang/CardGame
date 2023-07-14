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

    public Card()
    {
        
    }

//  public Card(int Id, string CardName, int Dame, int Blood, int Mana, string CardDescription, Sprite ThisImage, string Color, int DrawXcards, int AddXmaxMana, int ReturnXcards, int HealXpower, bool Spell, int DamageBySpell, int IncreaseXdame)
    public Card(int Id, string CardName, int Dame, int Blood, int Mana, string CardDescription, Sprite ThisImage, Element element, int DrawXcards, int AddXmaxMana, int ReturnXcards, int HealXpower, bool Spell, int DamageBySpell, int IncreaseXdame)
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
    }
}
