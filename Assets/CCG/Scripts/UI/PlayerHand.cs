using System.Collections;
using UnityEngine;
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
    
    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;

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

        if (player && player.hasEnemy && IsPlayerHand())
        {
            if (player.actualDeckSize < 20)
            {
                cardInDeck1.SetActive (false);
            }
            if (player.actualDeckSize < 10)
            {
                cardInDeck2.SetActive(false);
            }
            if (player.actualDeckSize < 2)
            {
                cardInDeck3.SetActive(false);
            }
            if (player.actualDeckSize < 1)
            {
                cardInDeck4.SetActive(false);
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
            
            if (enemyInfo.deckCount < 20)
            {
                cardInDeck1.SetActive (false);
            }
            if (enemyInfo.deckCount < 10)
            {
                cardInDeck2.SetActive(false);
            }
            if (enemyInfo.deckCount < 2)
            {
                cardInDeck3.SetActive(false);
            }
            if (enemyInfo.deckCount < 1)
            {
                cardInDeck4.SetActive(false);
            }
            handCount = enemyInfo.handCardCount;
        }
    }

    public void AddCard()
    {
        StartCoroutine(Wait());
    }
    
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
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
    bool IsPlayerHand() => player && playerType == PlayerType.PLAYER; //player.deck.spawnInitialCards &&
}