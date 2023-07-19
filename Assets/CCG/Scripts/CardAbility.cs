// Learn more : https://mirror-networking.com/docs/Guides/DataTypes.html#scriptable-objects
using System;
using System.Collections.Generic;

public enum AbilityType : byte { DAMAGE, HEAL, DRAW, DISCARD, BUFF, DEBUFF }

[Serializable]
public struct CardAbility
{
    public AbilityType abilityType; // Doesn't actually do anything. This is just to help visualize what each ScriptableAbility is doing.
    public List<Target> targets;
    public int damage;
    public int heal;
    
    public int buff;
    public int debuff;
    
    // public int draw;
    // public int discard;
    
    public bool untilEndOfTurn;
}