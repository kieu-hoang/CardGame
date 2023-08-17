using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Unity.VisualScripting;

public class EndGameP : MonoBehaviour
{
    public GameObject winImage;
    public GameObject loseImage;

    public GameObject money;
    public int playerHp;
    public int enemyHp;

    public bool gotMoney;

    public string menu;

    public bool protect;

    public GameObject winFire;
    public GameObject loseFire;

    public NetworkManagerHUDCCG manager;

    public GameManager gameManager;

    public bool end = false;
    // Start is called before the first frame update
    void Start()
    {
        winImage.SetActive(false);
        loseImage.SetActive(false);
        manager = GameObject.Find("Network Manager").GetComponent<NetworkManagerHUDCCG>();
    }

    // Update is called once per frame
    void Update()
    {
        playerHp = GameObject.Find("PlayerPortrait").GetComponent<UIPortrait>().health.text.ToInt();
        enemyHp = GameObject.Find("EnemyPortrait").GetComponent<UIPortrait>().health.text.ToInt();
        if (playerHp <= 0)
        {
            loseImage.SetActive(true);
            loseFire.SetActive(true);
            if (!end)
            {
                gameManager.audioSource.Stop();
                gameManager.audioSource.clip = gameManager.lose;
                gameManager.audioSource.Play();
                end = true;
            }
            if (protect == false)
            {
                manager.StopButtons();
                StartCoroutine(ReturnToMenu());
                protect = true;
            }
        }
        if (enemyHp <= 0)
        {
            winImage.SetActive(true);
            winFire.SetActive(true);
            if (!end)
            {
                gameManager.audioSource.Stop();
                gameManager.audioSource.clip = gameManager.win;
                gameManager.audioSource.Play();
                end = true;
            }
            if (gotMoney == false)
            {
                money.GetComponent<Shop>().gold += 50;
                gotMoney = true;
            }
            if (protect == false)
            {
                manager.StopButtons();
                StartCoroutine(ReturnToMenu());
                protect = true;
            }    
        }
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(menu);
    }

    public void Return()
    {
        StartCoroutine(ReturnToMenu());
    }
}