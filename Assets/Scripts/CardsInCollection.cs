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
    public TMPro.TextMeshProUGUI descriptionText;
    public GameObject[] stars;
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
        descriptionText.text = " " + cardDescription;
        
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < mana)
            {
                stars[i].SetActive(true);

                RectTransform starTransform = stars[i].GetComponent<RectTransform>();
                starTransform.localPosition = new Vector3(15 * (i - (mana - 1) / 2.0f), starTransform.localPosition.y, starTransform.localPosition.z);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }

        thatImage.sprite = thisSprite;
        // if (beGrey == true)
        // {
        //     frame.GetComponent<Image>().color = new Color32(155, 155, 155, 255);
        // }
        // else
        {
            if (thisCard.element == Card.Element.Fire)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardFire");
            }
            if (thisCard.element == Card.Element.Water)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWater");
            }
            if (thisCard.element == Card.Element.Metal)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardMetal");
            }
            if (thisCard.element == Card.Element.Wood)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardWood");
            }
            if (thisCard.element == Card.Element.Earth)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardEarth");
            }
            if (thisCard.element == Card.Element.NoElement)
            {
                frame.GetComponent<Image>().sprite = Resources.Load<Sprite>("CardNoElement");
            }
        }
    }
}
