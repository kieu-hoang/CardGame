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

    public string color;
    
    public int returnXcards;
    
    public int healXpower;
    //new
    public bool spell;
    public int damageDealtBySpell;

    public Card()
    {
        
    }

    public Card(int Id, string CardName, int Dame, int Blood, int Mana, string CardDescription, Sprite ThisImage, string Color, int DrawXcards, int AddXmaxMana, int ReturnXcards, int HealXpower, bool Spell, int DamageBySpell)
    {
        id = Id;
        cardName = CardName;
        dame = Dame;
        blood = Blood;
        mana = Mana;
        cardDescription = CardDescription;

        thisImage = ThisImage;
        color = Color;

        drawXcards = DrawXcards;
        addXmaxMana = AddXmaxMana;
        returnXcards = ReturnXcards;

        healXpower = HealXpower;
        spell = Spell;
        damageDealtBySpell = DamageBySpell;
    }
}
