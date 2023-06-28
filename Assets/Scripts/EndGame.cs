using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndGame : MonoBehaviour
{
    public TMPro.TextMeshProUGUI victoryText;
    public GameObject textObject;

    public GameObject money;

    public bool gotMoney;

    public string menu;

    public bool protect;
    // Start is called before the first frame update
    void Start()
    {
        textObject.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHp.staticHp <= 0)
        {
            textObject.SetActive(true);
            victoryText.text = "!!!BẠN THUA RỒI!!!";
            if (protect == false)
            {
                StartCoroutine(ReturnToMene());
                protect = true;
            }
        }
        if (EnemyHp.staticHp <= 0)
        {
            textObject.SetActive(true);
            victoryText.text = "***CHIẾN THẮNG***";
            if (gotMoney == false)
            {
                money.GetComponent<Shop>().gold += 50;
                gotMoney = true;
            }
            if (protect == false)
            {
                StartCoroutine(ReturnToMene());
                protect = true;
            }    
        }
    }

    IEnumerator ReturnToMene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(menu);
    }
}
