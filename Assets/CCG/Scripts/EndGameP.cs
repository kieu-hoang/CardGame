using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGameP : MonoBehaviour
{
    public TMPro.TextMeshProUGUI victoryText;
    public GameObject textObject;

    public GameObject money;
    public int playerHp;
    public int enemyHp;

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
        playerHp = GameObject.Find("PlayerPortrait").GetComponent<UIPortrait>().health.text.ToInt();
        enemyHp = GameObject.Find("EnemyPortrait").GetComponent<UIPortrait>().health.text.ToInt();
        if (playerHp <= 0)
        {
            textObject.SetActive(true);
            victoryText.text = "!!!BẠN THUA RỒI!!!";
            if (protect == false)
            {
                StartCoroutine(ReturnToMenu());
                protect = true;
            }
        }
        if (enemyHp <= 0)
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
                StartCoroutine(ReturnToMenu());
                protect = true;
            }    
        }
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(menu);
    }
}