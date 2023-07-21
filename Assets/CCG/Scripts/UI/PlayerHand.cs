﻿using UnityEngine;
using Mirror;

public class PlayerHand : MonoBehaviour
{
    public GameObject panel;
    public HandCard cardPrefab;
    public Transform handContent;
    public PlayerType playerType;
    private Player player;
    private PlayerInfo enemyInfo;
    private bool start = false;
    public int handCount = 0; // Amount of cards in hand
    public int hIndex = 0;

    void Update()
    {
        player = Player.localPlayer;
        if (player && player.hasEnemy) enemyInfo = player.enemyInfo;

        if (player && player.hasEnemy && playerType == PlayerType.PLAYER)
        {
            if (!start)
            {
                player.deck.DrawCard(3);
                start = true;
            }
        }

        if (IsEnemyHand())
        {
            // instantiate/destroy enough slots
                UIUtils.BalancePrefabs(cardPrefab.gameObject, enemyInfo.handCardCount, handContent);
                // refresh all members
                for (int i = 0; i < enemyInfo.handCardCount; ++i)
                {
                    HandCard slot = handContent.GetChild(i).GetComponent<HandCard>();

                    slot.AddCardBack();
                    slot.cost.text = "10";
                }
                handCount = enemyInfo.handCardCount;
        }
    }

    public void AddCard()
    {
        GameObject cardObj = Instantiate(cardPrefab.gameObject);
        cardObj.transform.SetParent(handContent, false);
        
        CardInfo card = player.deck.hand[hIndex];
        HandCard slot = cardObj.GetComponent<HandCard>();

        slot.AddCard(card, hIndex, playerType);
        hIndex++;
    }

    public void RemoveCard(int index)
    {
        for (int i = 0; i < handContent.childCount; ++i)
        {
            HandCard slot = handContent.GetChild(i).GetComponent<HandCard>();
            if (slot.handIndex == index) slot.RemoveCard();
        }
    }

    bool IsEnemyHand() =>
        player && player.hasEnemy && playerType == PlayerType.ENEMY && enemyInfo.handCount != handCount;
    bool IsPlayerHand() => player && player.deck.spawnInitialCards && playerType == PlayerType.PLAYER;
}