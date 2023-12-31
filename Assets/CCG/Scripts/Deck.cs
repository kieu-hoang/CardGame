﻿using System;
using System.Collections;
using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;

public class Deck : NetworkBehaviour
{
    [Header("Player")]
    public Player player;
    [HideInInspector] public int deckSize = 30;
    [HideInInspector] public int handSize = 7;
    
    [Header("Decks")]
    public SyncListCard deckList = new SyncListCard(); // DeckList used during the match. Contains all cards in the deck. This is where we'll be drawing card froms.
    public SyncListCard graveyard = new SyncListCard(); // Cards in player graveyard.
    public SyncListCard hand = new SyncListCard(); // Cards in player's hand during the match.

    [Header("Battlefield")]
    public SyncListCard playerField = new SyncListCard(); // Field where we summon creatures.

    [Header("Starting Deck")]
    public CardAndAmount[] startingDeck;

    [HideInInspector] public bool spawnInitialCards = true;

    public void OnDeckListChange(SyncListCard.Operation op, int index, CardInfo oldCard, CardInfo newCard)
    {
        UpdateDeck(index, 1, newCard);
    }

    public void OnHandChange(SyncListCard.Operation op, int index, CardInfo oldCard, CardInfo newCard)
    {
        UpdateDeck(index, 2, newCard);
    }

    public void OnGraveyardChange(SyncListCard.Operation op, int index, CardInfo oldCard, CardInfo newCard)
    {
        UpdateDeck(index, 3, newCard);
    }

    public void UpdateDeck(int index, int type, CardInfo newCard)
    {
        // Deck List
        if (type == 1) deckList[index] = newCard;

        // Hand
        if (type == 2) hand[index] = newCard;

        // Graveyard
        if (type == 3) graveyard[index] = newCard;
        
    }

    ///////////////
    public bool CanPlayCard(int manaCost)
    {
        return player.mana >= manaCost && player.health > 0;
    }

    public void DrawCard(int amount)
    {
        PlayerHand playerHand = Player.gameManager.playerHand;
        for (int i = 0; i < amount; ++i)
        {
            if (playerHand.hIndex < deckSize && Player.localPlayer.cardCount < 7)
            {
                player.CmdAddCardCount(1);
                playerHand.AddCard();
                Player.gameManager.CmdPlayDraw();
            }
        }
        spawnInitialCards = false;
    }

    [Command]
    public void CmdPlayCard(CardInfo card, int index)
    {
        CreatureCard creature = (CreatureCard)card.data;
        GameObject boardCard = Instantiate(creature.cardPrefab.gameObject);
        FieldCard newCard = boardCard.GetComponent<FieldCard>();
        newCard.card = new CardInfo(card.data); // Save Card Info so we can re-access it later if we need to.
        newCard.cardName.text = card.name;
        newCard.health = creature.health;
        newCard.strength = creature.strength;
        newCard.costText.text = card.cost;
        newCard.descriptionText.text = card.description;
        newCard.image.sprite = card.image;
        newCard.image.color = Color.white;
        newCard.element = creature.element;
        

        // If creature has charge, reduce waitTurn to 0 so they can attack right away.
        if (creature.hasCharge) newCard.waitTurn = 0;

        // Update the Card Info that appears when hovering
        newCard.cardHover.UpdateFieldCardInfo(card);

        // Spawn it
        NetworkServer.Spawn(boardCard);
        if (isServer) RpcPlayCard(boardCard, index);
    }

    [Command]
    public void CmdStartNewTurn()
    {
        player.currentMax++;
        if (player.currentMax >= player.maxMana) 
            player.currentMax = player.maxMana;
        player.mana = player.currentMax;
        
    }

    [ClientRpc]
    public void RpcPlayCard(GameObject boardCard, int index)
    {
        if (Player.gameManager.isSpawning)
        {
            // Set our FieldCard as a FRIENDLY creature for our local player, and ENEMY for our opponent.
            boardCard.GetComponent<FieldCard>().casterType = Target.FRIENDLIES;
            Player.gameManager.playerHand.RemoveCard(index); // Update player's hand
            CardInfo card = boardCard.GetComponent<FieldCard>().card;
            CreatureCard creature = (CreatureCard)card.data;
            if (card.name == "Phòng tuyến Tam Điệp" && Player.gameManager.playerField.checkPresent("Ngô Thì Nhậm"))
            {
                foreach (Transform child in Player.gameManager.enemyField.content)
                {
                    child.GetComponent<FieldCard>().combat.CmdChangeHealth(-1);
                }
            }
            if (card.name == "Phi tiêu 0" && Player.gameManager.playerField.checkPresent("Lính cảm tử"))
            {
                player.enemyInfo.player.GetComponent<Player>().combat.CmdChangeHealth(-2);
            }
            if (creature.hasDiplomacy)
                boardCard.GetComponent<FieldCard>().diplomacy = true;
            if (creature.strengthChange)
            {
                foreach (CardAbility cardAbility in creature.intiatives)
                {
                    foreach (Target tar in cardAbility.targets)
                    {
                        if (tar == Target.ENEMIES)
                        {
                            foreach (Transform child in Player.gameManager.enemyField.content)
                            {
                                child.GetComponent<FieldCard>().combat.CmdChangeStrength(cardAbility.debuff);
                            }
                        }
                        else if (tar == Target.FRIENDLIES)
                        {
                            foreach (Transform child in Player.gameManager.playerField.content)
                            {
                                if (child != boardCard.transform)
                                    child.GetComponent<FieldCard>().combat.CmdChangeStrength(cardAbility.buff);
                            }
                        }
                        else if (tar == Target.RANDOM)
                        {
                            if (cardAbility.buff > 0)
                            {
                                int x = Random.Range(0, Player.gameManager.playerField.content.childCount);
                                int i = 0;
                                foreach (Transform child in Player.gameManager.playerField.content)
                                {
                                    if (i == x)
                                    {
                                        child.GetComponent<FieldCard>().combat.CmdChangeStrength(cardAbility.buff);
                                        break;
                                    }
                                    i++;
                                }
                            }
                            else if (cardAbility.debuff < 0)
                            {
                                int x = Random.Range(0, Player.gameManager.enemyField.content.childCount);
                                int i = 0;
                                foreach (Transform child in Player.gameManager.enemyField.content)
                                {
                                    if (i == x)
                                    {
                                        child.GetComponent<FieldCard>().combat.CmdChangeStrength(cardAbility.debuff);
                                        break;
                                    }
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
            if (creature.cardDraw > 0)
            {
                if (player.cardCount == 6)
                    DrawCard(1);
                if (player.cardCount < 6)
                    DrawCard(creature.cardDraw);
            }
            if (creature.healthChange)
            {
                foreach (CardAbility cardAbility in creature.intiatives)
                {
                    foreach (Target tar in cardAbility.targets)
                    {
                        if (tar == Target.OWNER)
                        {
                            player.combat.CmdChangeHealth(cardAbility.heal);
                        }
                        else if (tar == Target.OPPONENT)
                        {
                            player.enemyInfo.player.GetComponent<Player>().combat.CmdChangeHealth(cardAbility.damage);
                        }
                        else if (tar == Target.ENEMIES)
                        {
                            foreach (Transform child in Player.gameManager.enemyField.content)
                            {
                                child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.damage);
                            }
                        }
                        else if (tar == Target.FRIENDLIES)
                        {
                            foreach (Transform child in Player.gameManager.playerField.content)
                            {
                                if (child != boardCard.transform)
                                    child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.heal);
                            }
                        }
                        else if (tar == Target.RANDOM)
                        {
                            if (cardAbility.heal > 0)
                            {
                                int x = Random.Range(0, Player.gameManager.playerField.content.childCount);
                                int i = 0;
                                foreach (Transform child in Player.gameManager.playerField.content)
                                {
                                    if (i == x)
                                    {
                                        child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.heal);
                                        break;
                                    }
                                    i++;
                                }
                            }
                            else if (cardAbility.damage < 0)
                            {
                                int x = Random.Range(0, Player.gameManager.enemyField.content.childCount);
                                int i = 0;
                                foreach (Transform child in Player.gameManager.enemyField.content)
                                {
                                    if (i == x)
                                    {
                                        child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.damage);
                                        break;
                                    }
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
            boardCard.transform.SetParent(Player.gameManager.playerField.content, false);
            Player.gameManager.audioSource.PlayOneShot(Player.gameManager.play, 1.5f);
            if (creature.creatureType == CreatureType.SPELL)
            {
                StartCoroutine(Wait(boardCard));
                
            }
            Player.gameManager.isSpawning = false;
        }
        else if (player.hasEnemy)
        {
            boardCard.GetComponent<FieldCard>().casterType = Target.ENEMIES;
            CardInfo card = boardCard.GetComponent<FieldCard>().card;
            CreatureCard creature = (CreatureCard)card.data;
            boardCard.transform.SetParent(Player.gameManager.enemyField.content, false);
            Player.gameManager.audioSource.PlayOneShot(Player.gameManager.play, 1.5f);
            Player.gameManager.enemyHand.RemoveCard(index);
            if (creature.hasDiplomacy)
                boardCard.GetComponent<FieldCard>().diplomacy = true;
            if (creature.creatureType == CreatureType.SPELL)
            {
                StartCoroutine(Wait(boardCard));
            }
        }
    }
    IEnumerator Wait(GameObject boardCard)
    {
        yield return new WaitForSeconds(1f);
        CmdDestroyCard(boardCard.transform);
    }

    [Command(ignoreAuthority = true)]
    public void CmdDestroyCard(Transform card)
    {
        if (isServer) RpcDestroyCard(card);
    }

    [ClientRpc]
    public void RpcDestroyCard(Transform boardCard)
    {
        if (Player.gameManager.isOurTurn)
        {
            CardInfo card = boardCard.GetComponent<FieldCard>().card;
            CreatureCard creature = (CreatureCard)card.data;
            if (creature.hasDeathCry)
            {
                if (boardCard.transform.parent == Player.gameManager.playerField.content)
                {
                    foreach (CardAbility cardAbility in creature.deathcrys)
                    {
                        foreach (Target tar in cardAbility.targets)
                        {
                            if (tar == Target.ENEMIES)
                            {
                                foreach (Transform child in Player.gameManager.enemyField.content)
                                {
                                    child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.damage);
                                }
                            }
                            else if (tar == Target.FRIENDLIES)
                            {
                                foreach (Transform child in Player.gameManager.playerField.content)
                                {
                                    if (child != boardCard.transform)
                                        child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.heal);
                                }
                            }
                            else if (tar == Target.RANDOM)
                            {
                                if (cardAbility.heal > 0)
                                {
                                    int x = Random.Range(0, Player.gameManager.playerField.content.childCount);
                                    int i = 0;
                                    foreach (Transform child in Player.gameManager.playerField.content)
                                    {
                                        if (i == x)
                                        {
                                            child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.heal);
                                            break;
                                        }
                                        i++;
                                    }
                                }
                                else if (cardAbility.damage < 0)
                                {
                                    int x = Random.Range(0, Player.gameManager.enemyField.content.childCount);
                                    int i = 0;
                                    foreach (Transform child in Player.gameManager.enemyField.content)
                                    {
                                        if (i == x)
                                        {
                                            child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.damage);
                                            break;
                                        }
                                        i++;
                                    }
                                }
                            }
                        } 
                    }
                    boardCard.transform.SetParent(Player.gameManager.graveyard, false);
                    boardCard.transform.position = new Vector3(boardCard.transform.position.x + 4000, boardCard.transform.position.y,
                        boardCard.transform.position.z);
                }
                else if (boardCard.transform.parent == Player.gameManager.enemyField.content)
                {
                    foreach (CardAbility cardAbility in creature.deathcrys)
                    {
                        foreach (Target tar in cardAbility.targets)
                        {
                            if (tar == Target.ENEMIES)
                            {
                                foreach (Transform child in Player.gameManager.playerField.content)
                                {
                                    child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.damage);
                                }
                            }
                            else if (tar == Target.FRIENDLIES)
                            {
                                foreach (Transform child in Player.gameManager.enemyField.content)
                                {
                                    if (child != boardCard.transform)
                                        child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.heal);
                                }
                            }
                            else if (tar == Target.RANDOM)
                            {
                                if (cardAbility.heal > 0)
                                {
                                    int x = Random.Range(0, Player.gameManager.enemyField.content.childCount);
                                    int i = 0;
                                    foreach (Transform child in Player.gameManager.enemyField.content)
                                    {
                                        if (i == x)
                                        {
                                            child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.heal);
                                            break;
                                        }
                                        i++;
                                    }
                                }
                                else if (cardAbility.damage < 0)
                                {
                                    int x = Random.Range(0, Player.gameManager.playerField.content.childCount);
                                    int i = 0;
                                    foreach (Transform child in Player.gameManager.playerField.content)
                                    {
                                        if (i == x)
                                        {
                                            child.GetComponent<FieldCard>().combat.CmdChangeHealth(cardAbility.damage);
                                            break;
                                        }
                                        i++;
                                    }
                                }
                            }
                        } 
                    }
                    boardCard.transform.SetParent(Player.gameManager.eGraveyard, false);
                    boardCard.transform.position = new Vector3(boardCard.transform.position.x + 4000, boardCard.transform.position.y,
                        boardCard.transform.position.z);
                }
            }
            else
            {
                if (boardCard.transform.parent == Player.gameManager.playerField.content)
                {
                    boardCard.transform.SetParent(Player.gameManager.graveyard, false);
                    boardCard.transform.position = new Vector3(boardCard.transform.position.x + 4000, boardCard.transform.position.y,
                        boardCard.transform.position.z);
                }
                else if (boardCard.transform.parent == Player.gameManager.enemyField.content)
                {
                    boardCard.transform.SetParent(Player.gameManager.eGraveyard, false);
                    boardCard.transform.position = new Vector3(boardCard.transform.position.x + 4000, boardCard.transform.position.y,
                        boardCard.transform.position.z);
                }
            }
        }
        else if (player.hasEnemy)
        {
            if (boardCard.transform.parent == Player.gameManager.playerField.content)
            {
                boardCard.transform.SetParent(Player.gameManager.graveyard, false);
                boardCard.transform.position = new Vector3(boardCard.transform.position.x + 4000, boardCard.transform.position.y,
                    boardCard.transform.position.z);
            }
            else if (boardCard.transform.parent == Player.gameManager.enemyField.content)
            {
                boardCard.transform.SetParent(Player.gameManager.eGraveyard, false);
                boardCard.transform.position = new Vector3(boardCard.transform.position.x + 4000, boardCard.transform.position.y,
                    boardCard.transform.position.z);
            }
        }
    }
    [Command]
    public void CmdAENhaNguyen()
    {
        if (isServer) RpcAENhaNguyen();
    }

    [ClientRpc]
    public void RpcAENhaNguyen()
    {
        if (Player.gameManager.isOurTurn && Player.gameManager.isSpawning)
        {
            foreach (Transform child in Player.gameManager.enemyField.content)
            {
                child.GetComponent<FieldCard>().combat.CmdChangeHealth(-3);
            }
        }
        Player.gameManager.isSpawning = false;
    }
    
}
