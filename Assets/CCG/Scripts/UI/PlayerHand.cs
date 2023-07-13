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
    private bool start = true;
    public int cardCount = 0; // Amount of cards in hand

    void Update()
    {
        player = Player.localPlayer;
        if (player && player.hasEnemy) enemyInfo = player.enemyInfo;

        if (playerType == PlayerType.PLAYER && Input.GetKeyDown(KeyCode.C))
        {
            if (start)
            {
                player.deck.DrawCard(3);
                start = false;
            }
        }

        if (IsEnemyHand())
        {
            // instantiate/destroy enough slots
                if (enemyInfo.cardCount + enemyInfo.handCount -enemyInfo.deckCount > 0)
                    UIUtils.BalancePrefabs(cardPrefab.gameObject, enemyInfo.cardCount + enemyInfo.handCount - enemyInfo.deckCount, handContent);
                Debug.Log("Card Count: " + enemyInfo.cardCount);
                // refresh all members
                for (int i = 0; i < enemyInfo.cardCount + enemyInfo.handCount - enemyInfo.deckCount; ++i)
                {
                    HandCard slot = handContent.GetChild(i).GetComponent<HandCard>();

                    slot.AddCardBack();

                    cardCount = enemyInfo.cardCount + enemyInfo.handCount - enemyInfo.deckCount;
                }
            
        }
    }

    public void AddCard(int index)
    {
        GameObject cardObj = Instantiate(cardPrefab.gameObject);
        cardObj.transform.SetParent(handContent, false);
        
        CardInfo card = player.deck.hand[index];
        HandCard slot = cardObj.GetComponent<HandCard>();

        slot.AddCard(card, index, playerType);
    }

    public void RemoveCard(int index)
    {
        for (int i = index; i < handContent.childCount; ++i)
        {
            HandCard slot = handContent.GetChild(i).GetComponent<HandCard>();
            int count = i;
            if (count == index) slot.RemoveCard();
            else if (slot.handIndex > index) slot.handIndex--;
        }
    }

    bool IsEnemyHand() => player && player.hasEnemy && playerType == PlayerType.ENEMY; //&& enemyInfo.handCount != cardCount
    bool IsPlayerHand() => player && player.deck.spawnInitialCards && playerType == PlayerType.PLAYER;
}