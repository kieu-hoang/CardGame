using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerField : MonoBehaviour, IDropHandler
{
    public Transform content;
    public int fieldCount;
    public PlayerType playerType;

    public void OnDrop(PointerEventData eventData)
    {
        HandCard card = eventData.pointerDrag.transform.GetComponent<HandCard>();
        Player player = Player.localPlayer;
        int manaCost = card.cost.text.ToInt();

        //
        if (player.IsOurTurn() && player.deck.CanPlayCard(manaCost) && fieldCount < 5)
        {
            int index = card.handIndex;
            CardInfo cardInfo = player.deck.hand[index];
            //Debug.LogError(index + " / " + cardInfo.name);
            //
            Player.gameManager.isSpawning = true;
            Player.gameManager.isHovering = false;
            //Player.gameManager.CmdOnCardHover(0, index); // 13/7 fixing...
            player.deck.CmdPlayCard(cardInfo, index); // Summon card onto the board
            player.combat.CmdChangeMana(-manaCost); // Reduce player's mana
        }
    }

    public void UpdateFieldCards()
    {
        int cardCount = content.childCount;
        for (int i = 0; i < cardCount; ++i)
        {
            FieldCard card = content.GetChild(i).GetComponent<FieldCard>();
            card.CmdUpdateWaitTurn();
        }
    }

    public bool checkTaunt()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            CardInfo card = content.GetChild(i).GetComponent<FieldCard>().card;
            CreatureCard creature = (CreatureCard)card.data;
            if (creature.hasTaunt)
                return true;
        }
        return false;
    }
    
    public void checkCharge()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            CardInfo card = content.GetChild(i).GetComponent<FieldCard>().card;
            CreatureCard creature = (CreatureCard)card.data;
            if (creature.hasCharge)
                Destroy(content.GetChild(i).gameObject);
        }
    }
    
    private void Update()
    {
        fieldCount = content.childCount;
        if (Player.localPlayer && playerType == PlayerType.ENEMY && Player.localPlayer.hasEnemy)
        {
            if (checkTaunt() && fieldCount > 0)
            {
                Player.localPlayer.enemyInfo.isTargetable = false;
                for (int i = 0; i < content.childCount; i++)
                {
                    CardInfo card = content.GetChild(i).GetComponent<FieldCard>().card;
                    CreatureCard creature = (CreatureCard)card.data;
                    if (creature.hasTaunt == false)
                    {
                        content.GetChild(i).GetComponent<FieldCard>().isTargetable = false;
                    }
                    else
                    {
                        content.GetChild(i).GetComponent<FieldCard>().isTargetable = true;
                    }
                }
            }
            else if (checkTaunt() == false && fieldCount > 0)
            {
                Player.localPlayer.enemyInfo.isTargetable = true;
                for (int i = 0; i < content.childCount; i++)
                {
                    content.GetChild(i).GetComponent<FieldCard>().isTargetable = true;
                }
            }
            else if (fieldCount == 0)
            {
                Player.localPlayer.enemyInfo.isTargetable = true;
            }
        }
        if (Player.localPlayer && Player.localPlayer.IsOurTurn() && playerType == PlayerType.ENEMY && Player.localPlayer.hasEnemy)
            checkCharge();
        if (Player.localPlayer && !Player.localPlayer.IsOurTurn() && playerType == PlayerType.PLAYER && Player.localPlayer.hasEnemy)
            checkCharge();
    }
}
