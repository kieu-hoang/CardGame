using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour
{
    public static int maxHp;
    public static int staticHp;
    public int hp;
    public Text hpText;
    public Text deckSize;
    public Text deadNo;
    public Text handNo;
    public GameObject edeck;
    private int x;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = 30;
        staticHp = 30;
        if (edeck.GetComponent<AI>() != null)
            x = 1;
        else if (edeck.GetComponent<AI1>() != null)
            x = 2;
    }

    // Update is called once per frame
    void Update()
    {
        hp = staticHp;

        if (hp > maxHp)
        {
            hp = maxHp;
        }
        hpText.text = "" + hp;
        if (x == 1)
        {
            deckSize.text = AI.deckSize + "";
            deadNo.text = 30 - AI.deckSize - CardsInHand.eHowMany - CardsInZone.eHowMany + "";
        }
        else if (x == 2)
        {
            deckSize.text = AI1.deckSize + "";
            deadNo.text = 30 - AI1.deckSize - CardsInHand.eHowMany - CardsInZone.eHowMany + "";
        }
        handNo.text = CardsInHand.eHowMany + "";
    }
}
