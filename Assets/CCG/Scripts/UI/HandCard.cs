using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class HandCard : MonoBehaviour
{
    [Header("Sprite")]
    public Image image;

    [Header("Front & Back")]
    public Image cardfront;
    public Image cardback;

    [Header("Properties")]
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI strength; //dame
    public TextMeshProUGUI health;
    public TextMeshProUGUI description;
    public Text creatureType;
    
    public GameObject[] stars;
    public Image frame;

    [Header("Card Drag & Hover")]
    public HandCardDragHover cardDragHover;

    [Header("Outline")]
    public Image cardOutline;
    public Color readyColor;
    public int handIndex;
    [HideInInspector] public PlayerType playerType;

    // Called from PlayerHand to instantiate the cards in the player's hand
    public void AddCard(CardInfo newCard, int index, PlayerType playerT)
    {
        handIndex = index;
        playerType = playerT;

        // Enable hover on player cards. We disable it for enemy cards.
        cardDragHover.canHover = true;
        cardOutline.gameObject.SetActive(true);
        
        if (newCard.element == Element.Fire)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardFire");
        }
        if (newCard.element == Element.Water)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWater");
        }
        if (newCard.element == Element.Metal)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardMetal");
        }
        if (newCard.element == Element.Wood)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWood");
        }
        if (newCard.element == Element.Earth)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardEarth");
        }
        if (newCard.element == Element.NoElement)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardNoElement");
        }

        // Reveal card FRONT, hide card BACK
        cardfront.color = Color.white;
        cardback.color = Color.clear;

        // Set card image
        image.sprite = newCard.image;
        
        // Assign description, name and remaining stats
        description.text = newCard.description; // Description
        cost.text = newCard.cost; // Cost
        cardName.text = newCard.name;
        
        //set stars in mana
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < cost.text.ToInt())
            {
                stars[i].SetActive(true);

                RectTransform starTransform = stars[i].GetComponent<RectTransform>();
                starTransform.localPosition = new Vector3(15 * (i - (cost.text.ToInt() - 1) / 2.0f), starTransform.localPosition.y, starTransform.localPosition.z);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }

        // Only set Health & Strength if CreatureCard
        if (newCard.data is CreatureCard)
        {
            health.text = ((CreatureCard)newCard.data).health.ToString();
            strength.text = ((CreatureCard)newCard.data).strength.ToString();
        }
    }

    public void AddCardBack()
    {
        cardfront.color = Color.clear;
        cardback.color = Color.white;
    }

    // Clears the card. Called when we Play/remove a card.
    public void RemoveCard()
    {
        Destroy(gameObject);
    }

    public void UpdateFieldCardInfo(CardInfo card)
    {
        // Reveal card FRONT, hide card BACK
        cardfront.color = Color.white;
        cardback.color = Color.clear;

        // Set card image
        image.sprite = card.image;

        // Assign description, name and remaining stats
        description.text = card.description; // Description
        cost.text = card.cost; // Cost
        cardName.text = card.name;

        // Stats
        health.text = ((CreatureCard)card.data).health.ToString();
        strength.text = ((CreatureCard)card.data).strength.ToString();
        
        //set stars in mana
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < cost.text.ToInt())
            {
                stars[i].SetActive(true);

                RectTransform starTransform = stars[i].GetComponent<RectTransform>();
                starTransform.localPosition = new Vector3(15 * (i - (cost.text.ToInt() - 1) / 2.0f), starTransform.localPosition.y, starTransform.localPosition.z);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }
        if (card.element == Element.Fire)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardFire");
        }
        if (card.element == Element.Water)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWater");
        }
        if (card.element == Element.Metal)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardMetal");
        }
        if (card.element == Element.Wood)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWood");
        }
        if (card.element == Element.Earth)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardEarth");
        }
        if (card.element == Element.NoElement)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardNoElement");
        }
    }

    private void Update()
    {
        if (playerType == PlayerType.PLAYER && cardDragHover != null)
        {
            // Only drag during our turn, if our player has enough mana.
            Player player = Player.localPlayer;
            int manaCost = cost.text.ToInt();
            if (Player.gameManager.isOurTurn)
            {
                cardDragHover.canDrag = player.deck.CanPlayCard(manaCost);
                cardOutline.GetComponent<Image>().color = cardDragHover.canDrag ? readyColor : Color.clear;
            }
        }
    }
}