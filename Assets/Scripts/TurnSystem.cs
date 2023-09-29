using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public static bool isYourTurn;
    public int yourTurn;
    public int yourOpponentTurn;

    public static int maxMana;
    public static int currentMana;
    public Text manaText;
    public GameObject button;
    public static bool startTurn;

    public int random;

    public bool turnEnd;
    public TMPro.TextMeshProUGUI timerText;
    public int seconds;
    public bool timerStart;

    public static int maxEnemyMana;
    public static int currentEnemyMana;
    public Text enemyManaText;

    public static bool protectStart;
    
    // Start is called before the first frame update
    void Start()
    {
        seconds = 30;
        timerStart = true;

        protectStart = true;
        StartGame();
        StartCoroutine(Protection());
    }

    // Update is called once per frame
    void Update()
    {
        if (isYourTurn)
        {
            button.SetActive(true);
        }
        else button.SetActive(false);

        manaText.text = currentMana + "";

        if (isYourTurn == true && seconds > 0 && timerStart == true)
        {
            StartCoroutine(Timer());
            timerStart = false;
        }
        if (seconds == 0 && isYourTurn == true)
        {
            EndYourTurn();
            
        }

        timerText.text = "Thời gian: " + seconds + "s";

        if (isYourTurn == false && seconds > 0 && timerStart == true)
        {
            StartCoroutine(EnemyTimer());
            timerStart = false;
        }
        if (seconds == 0 && isYourTurn == false)
        {
            EndYourOpponentTurn();
            
        }

        enemyManaText.text = currentEnemyMana + "";

        if (AI.AiEndPhase)
        {
            EndYourOpponentTurn();
            AI.AiEndPhase = false;
        }
        if (AI1.AiEndPhase)
        {
            EndYourOpponentTurn();
            AI1.AiEndPhase = false;
        }
    }
    public void EndYourTurn()
    {
        isYourTurn = false;
        yourOpponentTurn += 1;
        maxEnemyMana += 1;
        if (maxEnemyMana >= 7)
        {
            maxEnemyMana = 7;
        }
        currentEnemyMana = maxEnemyMana;
        AI.draw = false;
        timerStart = true;
        seconds = 30;
    }
    public void EndYourOpponentTurn()
    {
        isYourTurn = true;
        yourTurn += 1;
        maxMana += 1;
        if (maxMana >= 7)
        {
            maxMana = 7;
        }
        currentMana = maxMana;
        timerStart = true;
        seconds = 30;

        startTurn = true;
    }
    public void StartGame()
    {
        random = Random.Range(0, 2);
        if (random == 0)
        {
            isYourTurn = true;
            yourTurn = 1;
            yourOpponentTurn = 0;

            maxMana = 1;
            currentMana = 1;
            maxEnemyMana = 0;
            currentEnemyMana = 0;

            startTurn = false;
        }
        if (random == 1)
        {
            isYourTurn = false;
            yourTurn = 0;
            yourOpponentTurn = 1;
            maxMana = 0;
            currentMana = 0;
            maxEnemyMana = 1;
            currentEnemyMana = 1;
        }
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        if (isYourTurn == true && seconds > 0)
        {
            seconds--;
            StartCoroutine(Timer());
        }
    }
    IEnumerator EnemyTimer()
    {
        yield return new WaitForSeconds(1);
        if (isYourTurn == false && seconds > 0)
        {
            seconds--;
            StartCoroutine(EnemyTimer());
        }
    }

    IEnumerator Protection()
    {
        yield return new WaitForSeconds(3f);
        protectStart = false;
    }
}
