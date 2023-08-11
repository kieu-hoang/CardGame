using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndGame : MonoBehaviour
{
    public GameObject fireworkWin;
    public GameObject fireworkLose;
    public GameObject imageWin;
    public GameObject imageLose;
    public GameObject money;

    public bool gotMoney;
    public string menu;
    public bool protect;

    public AudioSource audioSource;

    public AudioClip win, lose;

    public bool playAudio;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (PlayerHp.staticHp <= 0)
        {
            fireworkLose.SetActive(true);
            imageLose.SetActive(true);
            if (!playAudio)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(lose, 1f);
                playAudio = true;
            }
            if (protect == false)
            {
                StartCoroutine(ReturnToMenu());
                protect = true;
            }
        }
        if (EnemyHp.staticHp <= 0)
        {
            fireworkWin.SetActive(true);
            imageWin.SetActive(true);
            if (!playAudio)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(win, 1f);
                playAudio = true;
            }
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
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(menu);
    }
}
