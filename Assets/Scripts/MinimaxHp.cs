using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimaxHp : MonoBehaviour
{
    public static int maxHp;
    public static int staticHp;
    public int hp;
    public Text hpText;
    public Text deckSize;
    public Text deadNo;
    public Text handNo;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = 30;
        staticHp = 30;
    }

    // Update is called once per frame
    void Update()
    {
        hp = staticHp;

        if (hp > maxHp)
        {
            hp = maxHp;
        }
        hpText.text = hp + "";
        deckSize.text = AI1a.deckSize + "";
        deadNo.text = 30 - AI1a.deckSize - CardsInHand.howMany1 - CardsInZone.howMany1 + "";
        handNo.text = CardsInHand.howMany1 + "";
    }
}
