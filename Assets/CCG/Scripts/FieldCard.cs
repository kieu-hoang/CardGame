using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class FieldCard : Entity
{
    [SyncVar, HideInInspector] public CardInfo card; // Get card info

    [Header("Card Properties")]
    public Image image; // card image on field
    public TextMeshProUGUI cardName; // Text of the card name
    public TextMeshProUGUI costText; // Text of the card mana or cost
    public TextMeshProUGUI healthText; // Text of the health
    public TextMeshProUGUI strengthText; // Text of the strength
    public TextMeshProUGUI descriptionText; //Text of the description

    public GameObject[] stars;
    public Image frame;

    [Header("Shine")]
    public Image shine;
    public Color hoverColor;
    public Color readyColor; // Shine color when ready to attack
    public Color targetColor; // Shine color when ready to attack

    [Header("Card Hover")]
    public HandCard cardHover;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        // If we have a card but no sprite, make sure the sprite is up to date since we can't SyncVar the sprite.
        // Useful to avoid bugs when a player was offline when the card spawned, or if they reconnected.
        if (image.sprite == null && (card.name != null || cardName.text == ""))
        {
            // Update Stats
            image.color = Color.white;
            image.sprite = card.image;
            cardName.text = card.name;
            costText.text = card.cost;
            descriptionText.text = card.description;
            element = card.element;
            
            for (int i = 0; i < stars.Length; i++)
            {
                if (i < costText.text.ToInt())
                {
                    stars[i].SetActive(true);

                    RectTransform starTransform = stars[i].GetComponent<RectTransform>();
                    starTransform.localPosition = new Vector3(15 * (i - (costText.text.ToInt() - 1) / 2.0f), starTransform.localPosition.y, starTransform.localPosition.z);
                }
                else
                {
                    stars[i].SetActive(false);
                }
            }
            if (element == Element.Fire)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardFire");
            }
            if (element == Element.Water)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWater");
            }
            if (element == Element.Metal)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardMetal");
            }
            if (element == Element.Wood)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWood");
            }
            if (element == Element.Earth)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardEarth");
            }
            if (element == Element.NoElement)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardNoElement");
            }

            // Update card hover info
            cardHover.UpdateFieldCardInfo(card);
        }

        healthText.text = health.ToString();
        strengthText.text = strength.ToString();
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < costText.text.ToInt())
            {
                stars[i].SetActive(true);

                RectTransform starTransform = stars[i].GetComponent<RectTransform>();
                starTransform.localPosition = new Vector3(15 * (i - (costText.text.ToInt() - 1) / 2.0f), starTransform.localPosition.y, starTransform.localPosition.z);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }
        if (element == Element.Fire)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardFire");
        }
        if (element == Element.Water)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWater");
        }
        if (element == Element.Metal)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardMetal");
        }
        if (element == Element.Wood)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWood");
        }
        if (element == Element.Earth)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardEarth");
        }
        if (element == Element.NoElement)
        {
            frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardNoElement");
        }

        if (CanAttack())
        {
            shine.color = targetColor;
        }
        else if (CantAttack())
        {
            shine.color = Color.clear;
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdUpdateWaitTurn()
    {
        if (waitTurn > 0) waitTurn--;
    }
}