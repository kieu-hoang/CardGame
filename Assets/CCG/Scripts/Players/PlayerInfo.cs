﻿using System;
using UnityEngine;
using Mirror;

[Serializable]
public partial struct PlayerInfo
{
    public GameObject player;
    public PlayerInfo(GameObject player)
    {
        this.player = player;
    }

    public Player data
    {
        get
        {
            // Return ScriptableItem from our cached list, based on the card's uniqueID.
            return player.GetComponentInChildren<Player>();
        }
    }

    // Player's username
    public string username => data.username;
    public Sprite portrait => data.portrait;

    // Player health and mana
    public int health => data.health;
    public int mana => data.mana;

    // Cardback image
    public Sprite cardback => data.cardback;

    // Card count for UI
    public int handCount => data.deck.hand.Count;
    public int handCardCount => data.cardCount;
    public int deckCount => data.actualDeckSize;
    public int graveCount => data.deck.graveyard.Count;
    public bool firstPlayer => data.firstPlayer;
    public bool isTargetable
    {
        get => data.isTargetable;
        set => data.isTargetable = value;
    }
}

// Card List
public class SyncListPlayerInfo : SyncList<PlayerInfo> { }
