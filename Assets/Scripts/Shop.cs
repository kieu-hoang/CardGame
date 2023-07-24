using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public TMPro.TextMeshProUGUI goldText;

    public int gold;

    public int shouldOpen;

    public int showedNumber;

    public int increaser;

    public TMPro.TextMeshProUGUI shouldOpenText;

    public GameObject morePacksWindow;

    public bool playDuel;
    // Start is called before the first frame update
    void Start()
    {
        gold = 750;
        gold = PlayerPrefs.GetInt("gold", 750);
        shouldOpen = PlayerPrefs.GetInt("shouldOpen", 0);
        if (shouldOpen > 0)
        {
            shouldOpen--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playDuel == false)
        {
            goldText.text = "Tài khoản của bạn: " + gold + " Thông bảo";
            if (shouldOpen > 0)
            {
                StartCoroutine(Wait());
            }

            showedNumber = shouldOpen + increaser;
            shouldOpenText.text = "" + showedNumber;
            PlayerPrefs.SetInt("shouldOpen",shouldOpen);
        }
        else
        {
            PlayerPrefs.SetInt("gold", gold);
        }
    }

    public void BuyPack()
    {
        if (gold >= 100)
        {
            gold -= 100;
            PlayerPrefs.SetInt("gold", gold);
            SceneManager.LoadScene("OpenPack");
        }
    }

    public void OpenMorePacks()
    {
        morePacksWindow.SetActive(true);
    }

    public void CloseMorePacks()
    {
        morePacksWindow.SetActive(false);
    }

    public void Plus()
    {
        increaser++;
    }

    public void Minus()
    {
        increaser--;
    }

    public void Confirm()
    {
        if (increaser * 100 <= gold)
        {
            shouldOpen = shouldOpen + increaser;
            increaser = 0;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        gold -= 100;
        PlayerPrefs.SetInt("gold", gold);
        SceneManager.LoadScene("OpenPack");
    }
}
