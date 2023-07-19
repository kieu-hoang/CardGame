// Put all our items in the Resources folder. We use Resources.LoadAll down
// below to load our items into a cache so we can easily reference them.
using UnityEngine;
using System.Collections.Generic;

public enum CreatureType : byte { SOLDIER, SPELL }
public enum Element
{
    NoElement,
    Metal,
    Wood,
    Water,
    Fire,
    Earth
}
// Struct for cards in your deck. Card + amount (ex : Sinister Strike x3). Used for Deck Building. Probably won't use it, just add amount to Card struct instead.
[CreateAssetMenu(menuName = "Card/Creature Card", order = 111)]
public partial class CreatureCard : ScriptableCard
{
    [Header("Stats")]
    public int strength;
    public int health;

    [Header("Targets")]
    public List<Target> acceptableTargets = new List<Target>();

    [Header("Type")]
    public List<CreatureType> creatureType;

    public Element element;

    [Header("Specialities")]
    public bool hasCharge = false;
    public bool hasTaunt = false;
    
    [Header("Death Abilities")]
    public List<CardAbility> deathcrys = new List<CardAbility>();
    [HideInInspector] public bool hasDeathCry = false; // If our card has a DEATHCRY ability

    [Header("Board Prefab")]
    public FieldCard cardPrefab;
    
    [Header("Propeties")]
    public bool targeted = false; // Targeted or random
    public bool healthChange = false; // If it affects a creature's stats (+X for positive changes like healing, -X for negative changes like damage)
    public bool strengthChange = false; // 
    public int cardDraw = 0; // Same as health. +X for positive (drawing cards), -X for negative (discarding)
    public bool untilEndOfTurn = false; // If the changes only purposes until end of turn.

    public virtual void Attack(Entity attacker, Entity target)
    {
        // Reduce the target's health by damage dealt.
        target.combat.CmdChangeHealth(-attacker.strength);
        attacker.combat.CmdChangeHealth(-target.strength);
        attacker.DestroyTargetingArrow();
        attacker.combat.CmdIncreaseWaitTurn();
    }

    private void OnValidate()
    {
        if (deathcrys.Count > 0) hasDeathCry = true;

        // By default, all creatures can only attack enemy creatures and our opponent. We set it here so every card get it's automatically.
        if (acceptableTargets.Count == 0)
        {
            acceptableTargets.Add(Target.ENEMIES);
            acceptableTargets.Add(Target.OPPONENT);
        }
    }
}