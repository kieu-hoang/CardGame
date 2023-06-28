using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public static float maxHp;
    public static float staticHp;
    public float hp;
    public Image Health;
    public TMPro.TextMeshProUGUI hpText;

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
        Health.fillAmount = hp/maxHp;

        if (hp > maxHp)
        {
            hp = maxHp;
        }
        hpText.text = hp + "HP";
    }
}
