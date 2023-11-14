using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field1 : MonoBehaviour
{
    public static GameObject MinimaxZone;

    public static GameObject KbZone;

    public bool aeNhaNguyenEffect = false;

    public bool aeNhaNguyenEffectAI = false;
    // Start is called before the first frame update
    private void Start()
    {
        MinimaxZone = GameObject.Find("Zone");
        KbZone = GameObject.Find("EnemyZone");
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnSystem1.isYourTurn)
        {
            if (CardsInZone.eHowMany1 > 0)
                killChargeAI();
        }
        else
        {
            if (CardsInZone.howMany1 > 0)
                killChargePlayer();
        }

        if (checkAENhaNguyen() && aeNhaNguyenEffect == false)
        {
            foreach (Transform child in KbZone.transform)
            {
                child.GetComponent<AI1CardToHand>().isTarget = true;
                if (child.GetComponent<AI1CardToHand>().isTarget)
                {
                    child.GetComponent<AI1CardToHand>().hurted += 3;
                    child.GetComponent<AI1CardToHand>().isTarget = false;
                }
            }
            aeNhaNguyenEffect = true;
        }

        if (checkAENhaNguyenAI() && aeNhaNguyenEffectAI == false)
        {
            foreach (Transform child in MinimaxZone.transform)
            {
                child.GetComponent<AI2CardToHand>().isTarget = true;
                if (child.GetComponent<AI2CardToHand>().isTarget)
                {
                    child.GetComponent<AI2CardToHand>().hurted += 3;
                    child.GetComponent<AI2CardToHand>().isTarget = false;
                }
            }
            aeNhaNguyenEffectAI = true;
        }
    }

    public void killChargePlayer()
    {
        foreach (Transform child in MinimaxZone.transform)
        {
            if (child.GetComponent<AI2CardToHand>() != null && child.GetComponent<AI2CardToHand>().id == 4)
            {
                child.GetComponent<AI2CardToHand>().hurted = child.GetComponent<AI2CardToHand>().blood;
            }
        }
    }
    
    public void killChargeAI()
    {
        foreach (Transform child in KbZone.transform)
        {
            if (child.GetComponent<AI1CardToHand>() != null && child.GetComponent<AI1CardToHand>().id == 4)
            {
                child.GetComponent<AI1CardToHand>().hurted = child.GetComponent<AI1CardToHand>().blood;
            }
        }
    }

    public static bool checkChargePlayer()
    {
        foreach (Transform child in MinimaxZone.transform)
        {
            if (child.GetComponent<AI2CardToHand>() != null && child.GetComponent<AI2CardToHand>().id == 4)
            {
                return true;
            }
        }
        return false;
    }
    public static bool checkChargeAI()
    {
        foreach (Transform child in KbZone.transform)
        {
            if (child.GetComponent<AI1CardToHand>() != null && child.GetComponent<AI1CardToHand>().id == 4)
            {
                return true;
            }
        }
        return false;
    }

    public static bool checkPresentPlayer(int id)
    {
        foreach (Transform child in MinimaxZone.transform)
        {
            if (child.GetComponent<AI2CardToHand>() != null && child.GetComponent<AI2CardToHand>().id == id)
                return true;
        }
        return false;
    }
    
    public static bool checkPresentAI(int id)
    {
        foreach (Transform child in KbZone.transform)
        {
            if (child.GetComponent<AI1CardToHand>() != null && child.GetComponent<AI1CardToHand>().id == id)
                return true;
        }
        return false;
    }

    public bool checkAENhaNguyen()
    {
        if (checkPresentPlayer(11) && checkPresentPlayer(16) && checkPresentPlayer(21))
            return true;
        return false;
    }
    public bool checkAENhaNguyenAI()
    {
        if (checkPresentAI(11) && checkPresentAI(16) && checkPresentAI(21))
            return true;
        return false;
    }

    public static bool checkNTN()
    {
        if (checkPresentPlayer(17))
            return true;
        return false;
    }
    public static bool checkNTNAI()
    {
        if (checkPresentAI(17))
            return true;
        return false;
    }
}

