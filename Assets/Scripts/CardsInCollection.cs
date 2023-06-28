using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardsInCollection : MonoBehaviour
{
    public Card thisCard;
    public int thisId;

    public int id;
    public string cardName;
    public int dame;
    public int blood;
    public int mana;
    public string cardDescription;

    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI dameText;
    public TMPro.TextMeshProUGUI bloodText;
    public TMPro.TextMeshProUGUI manaText;
    public TMPro.TextMeshProUGUI descriptionText;

    public Sprite thisSprite;
    public Image thatImage;

    public bool beGrey;

    public GameObject frame;
    // Start is called before the first frame update
    void Start()
    {
        thisCard = CardDataBase.cardList[thisId];
    }

    // Update is called once per frame
    void Update()
    {
        thisCard = CardDataBase.cardList[thisId];
        
        id = thisCard.id;
        cardName = thisCard.cardName;
        dame = thisCard.dame;
        blood = thisCard.blood;
        mana = thisCard.mana;
        cardDescription = thisCard.cardDescription;
        thisSprite = thisCard.thisImage;
        
        nameText.text = "" + cardName;
        dameText.text = "" + dame;
        bloodText.text = "" + blood;
        manaText.text = "" + mana;
        descriptionText.text = " " + cardDescription;

        thatImage.sprite = thisSprite;
        if (beGrey == true)
        {
            frame.GetComponent<Image>().color = new Color32(155, 155, 155, 255);
        }
        else
        {
            if (thisCard.color == "Red")
            {
                frame.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            }
            if (thisCard.color == "Blue")
            {
                frame.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
            }
            if (thisCard.color == "Yellow")
            {
                frame.GetComponent<Image>().color = new Color32(255, 255, 0, 255);
            }
            if (thisCard.color == "Green")
            {
                frame.GetComponent<Image>().color = new Color32(60, 159, 81, 255);
            }
            if (thisCard.color == "Brown")
            {
                frame.GetComponent<Image>().color = new Color32(109, 45, 45, 255);
            }
            if (thisCard.color == "Black")
            {
                frame.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
            }
        }
    }
}
