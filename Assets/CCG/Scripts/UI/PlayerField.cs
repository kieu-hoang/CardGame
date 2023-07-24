using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerField : MonoBehaviour, IDropHandler
{
    public Transform content;
    public int fieldCount;
    public PlayerType playerType;
    public bool aeNhaNguyenEffect = false;

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
                content.GetChild(i).GetComponent<FieldCard>().health = 0;
        }
    }

    public void toGraveyard()
    {
        Player player = Player.localPlayer;
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i).GetComponent<FieldCard>().IsDead())
            {
                Player.gameManager.isSpawning = true;
                player.deck.CmdDestroyCard(content.GetChild(i));
            }
        }
    }

    public bool checkPresent(string name)
    {
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i).GetComponent<FieldCard>().cardName.text == name)
            {
                return true;
            }
        }
        return false;
    }
    public bool checkAENhaNguyen()
    {
        if (checkPresent("Nguyễn Huệ") && checkPresent("Nguyễn Lữ") && checkPresent("Nguyễn Nhạc"))
            return true;
        return false;
    }
    
    private void Update()
    {
        fieldCount = content.childCount;
        if (Player.localPlayer && Player.localPlayer.hasEnemy)
        {
            Player player = Player.localPlayer;
            if (playerType == PlayerType.ENEMY)
            {
                if (checkTaunt() && fieldCount > 0)
                {
                    player.enemyInfo.isTargetable = false;
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
                    player.enemyInfo.isTargetable = true;
                    for (int i = 0; i < content.childCount; i++)
                    {
                        content.GetChild(i).GetComponent<FieldCard>().isTargetable = true;
                    }
                }
                else if (fieldCount == 0)
                {
                    player.enemyInfo.isTargetable = true;
                }
            }
            if (player.IsOurTurn() && playerType == PlayerType.ENEMY)
                checkCharge();
            if (!player.IsOurTurn() && playerType == PlayerType.PLAYER)
                checkCharge();
            if (player.IsOurTurn())
                toGraveyard();
            if (checkAENhaNguyen() && aeNhaNguyenEffect == false)
            {
                Player.gameManager.isSpawning = true;
                player.deck.CmdAENhaNguyen();
                aeNhaNguyenEffect = true;
            }
        }
    }
}
